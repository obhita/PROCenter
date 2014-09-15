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

namespace ProCenter.Domain.ReportsModule.PatientsWithSpecificResponseReport
{
    #region Using Statements

    using System;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.IO;
    using System.Linq;
    using System.Net;

    using DevExpress.XtraPrinting;
    using DevExpress.XtraPrinting.Native;
    using DevExpress.XtraReports.UI;

    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.ReportsModule.PatientScoreRangeReport;

    #endregion

    /// <summary>
    ///     The PatientsWithSpecificResponseReport class.
    /// </summary>
    public partial class PatientsWithSpecificResponseReport : XtraReport, IReport
    {
        private const int NameHeaderWidth = 125;

        private const int AgeHeaderWidth = 30;

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PatientsWithSpecificResponseReport" /> class.
        /// </summary>
        public PatientsWithSpecificResponseReport()
        {
            InitializeComponent ();
            BeforePrint += OnBeforePrint;
        }

        #endregion

        #region Methods

        private void BindData ()
        {
            var source = (DataSource as PatientsWithSpecificResponseDataCollection)[0] as PatientsWithSpecificResponseData;
            if ( source == null )
            {
                return;
            }
            var questionCount = 1;
            var t = xrReportDataTable;
            var itemDefinitionCode = string.Empty;
            var cellFont = new Font ( "Times New Roman", 12F );
            var rowCount = 0;
            foreach ( var row in source.Data )
            {
                if ( itemDefinitionCode != row.ItemDefinitionCode )
                {
                    var tdBlank = new XRTableCell { Text = " ", Multiline = true, CanGrow = true, SizeF = new Size ( 500, 60 ), Font = cellFont };
                    var trBlankBottom = new XRTableRow { HeightF = 60, CanGrow = true };
                    trBlankBottom.Cells.Add ( tdBlank );
                    trBlankBottom.Cells.Add ( tdBlank );
                    trBlankBottom.Cells.Add ( tdBlank );
                    t.Rows.Add(trBlankBottom);

                    //// break here on question
                    var trHeaderRowTable = AddQuestionResponse ( row, questionCount, source );
                    var parentCell = new XRTableCell { Tag = "TableParentCell", SizeF = new Size ( 880, trHeaderRowTable.Height ), CanGrow = true };
                    var trHeaderRow = new XRTableRow { CanGrow = true };
                    trHeaderRow.Cells.Add ( parentCell );
                    parentCell.Controls.Add ( trHeaderRowTable );
                    t.Rows.Add ( trHeaderRow );
                    questionCount += 1;
                    t.Rows.Add ( GetHeaderRow ( source ) );
                    rowCount = 0;
                }
                var backColor = Color.Transparent;
                if ( rowCount % 2 != 0 )
                {
                    backColor = Color.LightGray;
                }
                itemDefinitionCode = row.ItemDefinitionCode;
                var tr = new XRTableRow { BackColor = backColor };
                var tdName = new XRTableCell { Text = row.PatientName, CanGrow = true, Font = cellFont };
                tdName.SizeF = new Size(NameHeaderWidth, tdName.Height);
                tr.Cells.Add ( tdName );
                var tdAge = new XRTableCell { Text = row.Age.ToString (), CanGrow = true, TextAlignment = TextAlignment.TopCenter, Font = cellFont };
                tdAge.SizeF = new Size(AgeHeaderWidth, tdAge.Height);
                tr.Cells.Add ( tdAge );
                var tdGender = new XRTableCell { Text = row.Gender, CanGrow = true, TextAlignment = TextAlignment.TopCenter, Font = cellFont };
                tr.Cells.Add ( tdGender );
                var tdAssessmentDate = new XRTableCell { Text = row.AssessmentDate, CanGrow = true, TextAlignment = TextAlignment.TopCenter, Font = cellFont };
                tr.Cells.Add(tdAssessmentDate);
                var tdGivenResponse = new XRTableCell { Text = row.Response, CanGrow = true, TextAlignment = TextAlignment.TopCenter, Font = cellFont };
                tr.Cells.Add(tdGivenResponse);

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
                rowCount++;
            }
            CenterIcons ( t );
        }

        private XRTable AddQuestionResponse(PatientsWithSpecificResponseDataObject data, int questionCount, PatientsWithSpecificResponseData source)
        {
            var cellFont = new Font("Times New Roman", 12F);
            var tdBlank = new XRTableCell { Text = " ", Multiline = true, CanGrow = true, SizeF = new Size(500, 60), Font = cellFont };
            var cellFontBold = new Font("Times New Roman", 12F, FontStyle.Bold);
            var tb = new XRTable { Tag = "TableBreak", CanGrow = true, WidthF = 880 };
            var trHeader = new XRTableRow();
            var trData = new XRTableRow();

            var trBlankTop = new XRTableRow { HeightF = 60, CanGrow = true };
            trBlankTop.Cells.Add(tdBlank);
            trBlankTop.Cells.Add(tdBlank);
            tb.Rows.Add(trBlankTop);
            
            var tdQuestionNumber = new XRTableCell { Text = source.HeaderQuestion + " " + questionCount, SizeF = new Size(500, 25), Font = cellFontBold };
            trHeader.Cells.Add(tdQuestionNumber);
            var tdSpecificResponseHeader = new XRTableCell { Text = source.HeaderSpecificResponseValue, SizeF = new Size(380, 25), Font = cellFontBold };
            trHeader.Cells.Add(tdSpecificResponseHeader);
            tb.Rows.Add ( trHeader );

            var tdQuestion = new XRTableCell { Text = data.Question, Multiline = true, CanGrow = true, SizeF = new Size(500, 25), Font = cellFont };
            trData.Cells.Add ( tdQuestion );
            var tdResponse = new XRTableCell
                             {
                                 Text = GetResponsesForHeader(source, data.ItemDefinitionCode), 
                                 Multiline = true, 
                                 CanGrow = true, 
                                 SizeF = new Size(380, 25), 
                                 Font = cellFont
                             };
            trData.Cells.Add ( tdResponse );
            tb.Rows.Add(trData);

            var trBlankBottom = new XRTableRow { HeightF = 60, CanGrow = true };
            trBlankBottom.Cells.Add(tdBlank);
            trBlankBottom.Cells.Add(tdBlank);
            tb.Rows.Add(trBlankBottom);
            return tb;
        }

        private string GetResponsesForHeader(PatientsWithSpecificResponseData source, string itemDefinitionCode)
        {
            var newLine = string.Empty;
            var returnStr = string.Empty;
            foreach ( var re in source.LocalizedQuestionResponses.Where ( a => a.ItemDefinitionCode == itemDefinitionCode ).SelectMany ( response => response.LocalizedResponses ) )
            {
                returnStr += newLine + re;
                newLine = Environment.NewLine;
            }
            return returnStr;
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

        private XRTableRow GetHeaderRow(PatientsWithSpecificResponseData source)
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
            var tdAssessmentDateHeader = new XRTableCell
                                {
                                    Text = source.HeaderAssessmentDate,
                                    CanGrow = true,
                                    Borders = BorderSide.Bottom,
                                    BorderWidth = 3,
                                    BorderColor = Color.DarkGray,
                                    TextAlignment = TextAlignment.BottomCenter,
                                    Font = headerFont
                                };
            trHeader.Cells.Add(tdAssessmentDateHeader);
            var tdGivenResponseHeader = new XRTableCell
                                {
                                    Text = source.HeaderGivenResponse,
                                    CanGrow = true,
                                    Borders = BorderSide.Bottom,
                                    BorderWidth = 3,
                                    BorderColor = Color.DarkGray,
                                    TextAlignment = TextAlignment.BottomCenter,
                                    Font = headerFont
                                };
            trHeader.Cells.Add(tdGivenResponseHeader);

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