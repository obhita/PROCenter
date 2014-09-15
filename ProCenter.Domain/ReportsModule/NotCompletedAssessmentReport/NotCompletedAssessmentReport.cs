// /*******************************************************************************
//  * Open Behavioral Health Information Technology Architecture (OBHITA.org)
//  * 
//  * Redistribution and use in source and binary forms, with or without
//  * modification, are permitted provided that the following conditions are met:
//  *     * Redistributions of source code must retain the above copyright
//  *       notice, this list of conditions and the following disclaimer.
//  *     * Redistributions in binary form must reproduce the above copyright
//  *       notice, this list of conditions and the following disclaimer in the
//  *       documentation and/or other materials provided with the distribution.
//  *     * Neither the name of the <organization> nor the
//  *       names of its contributors may be used to endorse or promote products
//  *       derived from this software without specific prior written permission.
//  * 
//  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  ******************************************************************************/

namespace ProCenter.Domain.ReportsModule.NotCompletedAssessmentReport
{
    #region Using Statements

    using System.Drawing;
    using System.Drawing.Printing;
    using System.IO;
    using System.Net;

    using DevExpress.XtraPrinting;
    using DevExpress.XtraPrinting.Native;
    using DevExpress.XtraReports.UI;

    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.ReportsModule.PatientScoreRangeReport;

    #endregion

    /// <summary>
    ///     The NotCompletedAssessmentReport class.
    /// </summary>
    public partial class NotCompletedAssessmentReport : XtraReport, IReport
    {
        private const int NameHeaderWidth = 125;

        private const int AgeHeaderWidth = 30;

        private const int AssessmentNameHeaderWidth = 300;

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotCompletedAssessmentReport" /> class.
        /// </summary>
        public NotCompletedAssessmentReport()
        {
            InitializeComponent ();
            BeforePrint += OnBeforePrint;
        }

        #endregion

        #region Methods

        private void BindData ()
        {
            var source = (DataSource as NotCompletedAssessmentDataCollection)[0] as NotCompletedAssessmentData;
            if ( source == null )
            {
                return;
            }
            var t = xrReportDataTable;
            var cellFont = new Font ( "Times New Roman", 12F );
            foreach ( var row in source.Data )
            {
                var tr = new XRTableRow ();
                var tdName = new XRTableCell { Text = row.PatientName, CanGrow = true, Font = cellFont };
                tdName.SizeF = new Size(NameHeaderWidth, tdName.Height);
                tr.Cells.Add ( tdName );
                var tdAge = new XRTableCell { Text = row.Age.ToString (), CanGrow = true, TextAlignment = TextAlignment.TopCenter, Font = cellFont };
                tdAge.SizeF = new Size(AgeHeaderWidth, tdAge.Height);
                tr.Cells.Add ( tdAge );
                var tdGender = new XRTableCell { Text = row.Gender, CanGrow = true, TextAlignment = TextAlignment.TopCenter, Font = cellFont };
                tr.Cells.Add ( tdGender );
                var tdAssessmentName = new XRTableCell { Text = row.AssessmentName, CanGrow = true, TextAlignment = TextAlignment.TopCenter, Font = cellFont };
                tdAssessmentName.SizeF = new Size(AssessmentNameHeaderWidth, tdAssessmentName.Height);
                tr.Cells.Add(tdAssessmentName);
                var tdAssessmentStatus = new XRTableCell { Text = row.AssessmentStatus, CanGrow = true, TextAlignment = TextAlignment.TopCenter, Font = cellFont };
                tr.Cells.Add(tdAssessmentStatus);

                var urlLeft = HttpContextAccessor.Url.Scheme + "://" + HttpContextAccessor.Url.Authority + "/Assessment/Edit/";
                var pb = new XRPictureBox
                {
                    ImageUrl = "/Images/open_16x12px.png",
                    NavigateUrl = urlLeft + row.AssessmentInstanceKey + "?patientKey=" + row.PatientKey,
                    Location = new Point(0, 0),
                    Target = "_blank",
                    TextAlignment = TextAlignment.TopCenter
                };
                var tdView = new XRTableCell { CanGrow = true, Font = cellFont, TextAlignment = TextAlignment.TopCenter };
                if ( row.AssessmentInstanceKey != null )
                {
                    tdView.Controls.Add(pb);
                }
                tr.Cells.Add(tdView);
                tr.Height = 60;

                t.Rows.Add ( tr );
            }
            CenterIcons ( t );
            t.Rows.Insert ( 0, GetHeaderRow ( source ) );
        }

        private void CenterIcons(XRTable table)
        {
            const double IconWidth = 8.5;
            foreach (XRTableRow row in table.Rows)
            {
                foreach (XRTableCell cell in row.Cells)
                {
                    foreach (var control in cell.Controls)
                    {
                        if (control.GetType() != typeof(XRPictureBox))
                        {
                            continue;
                        }
                        var x = ((double)cell.Width / 2) - IconWidth;
                        ((XRPictureBox)control).LocationF = new PointF((float)x, 0);
                    }
                }
            }
        }

        private XRTableRow GetHeaderRow(NotCompletedAssessmentData source)
        {
            var headerFont = new Font ( "Times New Roman", 13F );
            var trHeader = new XRTableRow ();
            var tdNameHeader = new XRTableCell
                               {
                                   Text = source.HeaderName,
                                   CanGrow = true,
                                   Borders = BorderSide.Bottom,
                                   BorderWidth = 3,
                                   BorderColor = Color.DarkGray,
                                   TextAlignment = TextAlignment.BottomLeft,
                                   Font = headerFont,
                                   SizeF = new Size(NameHeaderWidth, Height),
                               };
            trHeader.Cells.Add ( tdNameHeader );
            var tdAgeHeader = new XRTableCell
                              {
                                  Text = source.HeaderAge,
                                  CanGrow = true,
                                  Borders = BorderSide.Bottom,
                                  BorderWidth = 3,
                                  BorderColor = Color.DarkGray,
                                  TextAlignment = TextAlignment.BottomCenter,
                                  Font = headerFont,
                                  SizeF = new Size(AgeHeaderWidth, Height),
                              };
            trHeader.Cells.Add ( tdAgeHeader );
            var tdGenderHeader = new XRTableCell
                                 {
                                     Text = source.HeaderGender,
                                     CanGrow = true,
                                     Borders = BorderSide.Bottom,
                                     BorderWidth = 3,
                                     BorderColor = Color.DarkGray,
                                     TextAlignment = TextAlignment.BottomCenter,
                                     Font = headerFont
                                 };
            trHeader.Cells.Add ( tdGenderHeader );
            var tdAssessmentNameHeader = new XRTableCell
                                {
                                    Text = source.HeaderAssessmentName,
                                    CanGrow = true,
                                    Borders = BorderSide.Bottom,
                                    BorderWidth = 3,
                                    BorderColor = Color.DarkGray,
                                    TextAlignment = TextAlignment.BottomCenter,
                                    Font = headerFont,
                                    SizeF = new Size(AssessmentNameHeaderWidth, Height),
                                };
            trHeader.Cells.Add(tdAssessmentNameHeader);
            var tdAssessmentStatusHeader = new XRTableCell
                               {
                                   Text = source.HeaderAssessmentStatus,
                                   CanGrow = true,
                                   Borders = BorderSide.Bottom,
                                   BorderWidth = 3,
                                   BorderColor = Color.DarkGray,
                                   TextAlignment = TextAlignment.BottomCenter,
                                   Font = headerFont
                               };
            trHeader.Cells.Add(tdAssessmentStatusHeader);
            var tdViewHeader = new XRTableCell
                               {
                                   Text = source.HeaderViewAssessment,
                                   CanGrow = true,
                                   Borders = BorderSide.Bottom,
                                   BorderWidth = 3,
                                   BorderColor = Color.DarkGray,
                                   TextAlignment = TextAlignment.BottomCenter,
                                   Font = headerFont
                               };
            trHeader.Cells.Add ( tdViewHeader );
            return trHeader;
        }

        private void OnBeforePrint ( object sender, PrintEventArgs printEventArgs )
        {
            BindData ();
        }

        #endregion
    }
}