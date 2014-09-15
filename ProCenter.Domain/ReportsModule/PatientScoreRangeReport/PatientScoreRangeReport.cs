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

namespace ProCenter.Domain.ReportsModule.PatientScoreRangeReport
{
    #region Using Statements

    using System;
    using System.Drawing;
    using System.Drawing.Printing;

    using DevExpress.XtraPrinting;
    using DevExpress.XtraPrinting.Native;
    using DevExpress.XtraReports.UI;

    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>
    ///     The PatientScoreRangeReport class.
    /// </summary>
    public partial class PatientScoreRangeReport : XtraReport, IReport
    {
        private const int NameHeaderWidth = 135;

        private const int ScoreColumn = 3;

        private const int ChangeColumn = 4;

        private const double IconWidth = 8.5;

        private const string FirstTime = "First Time";

        private const string Higher = "Higher";

        private const string Lower = "Lower";

        private const string NoChange = "No Change";

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PatientScoreRangeReport" /> class.
        /// </summary>
        public PatientScoreRangeReport ()
        {
            InitializeComponent ();
            BeforePrint += OnBeforePrint;
        }

        #endregion

        #region Methods

        private void BindData ()
        {
            var source = ( DataSource as PatientScoreRangeDataCollection )[0] as PatientScoreRangeData;
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
                tr.Cells.Add ( tdAge );
                var tdGender = new XRTableCell { Text = row.Gender, CanGrow = true, TextAlignment = TextAlignment.TopCenter, Font = cellFont };
                tr.Cells.Add ( tdGender );
                var tdScore = new XRTableCell { Text = row.Score, CanGrow = true, TextAlignment = TextAlignment.TopCenter, Font = cellFont };
                tr.Cells.Add ( tdScore );

                var pbChange = new XRPictureBox ();
                if (row.Change == FirstTime)
                {
                    pbChange.ImageUrl = "/Images/initial_16x12px.png";
                }
                else if ( row.Change == Higher )
                {
                    pbChange.ImageUrl = "/Images/change_up_16x12px.png";
                }
                else if ( row.Change == Lower )
                {
                    pbChange.ImageUrl = "/Images/change_down_16x12px.png";
                }
                else if ( row.Change == NoChange )
                {
                    pbChange.ImageUrl = "/Images/no_change_16x12px.png";
                }
                var tdChangeImage = new XRTableCell { CanGrow = true, Font = cellFont };
                tdChangeImage.Controls.Add(pbChange);
                tr.Cells.Add ( tdChangeImage );

                var tdDate = new XRTableCell { Text = row.AssessmentDate, Font = cellFont };
                tr.Cells.Add ( tdDate );
                var urlLeft = HttpContextAccessor.Url.Scheme + "://" + HttpContextAccessor.Url.Authority + "/Assessment/Edit/";
                var pb = new XRPictureBox
                         {
                             ImageUrl = "/Images/open_16x12px.png",
                             NavigateUrl = urlLeft + row.AssessmentInstanceKey + "?patientKey=" + row.PatientKey,
                             Location = new Point ( 0, 0 ),
                             Target = "_blank",
                         };
                var tdView = new XRTableCell { CanGrow = true, Font = cellFont, TextAlignment = TextAlignment.TopCenter };
                tdView.Controls.Add ( pb );
                tr.Cells.Add ( tdView );
                tr.Height = 60;
                t.Rows.Add ( tr );
            }
            AdjustIcons ( t );
            CenterIcons(t);
            t.Rows.Insert ( 0, GetHeaderRow ( source ) );
        }

        private int GetScore ( string score )
        {
            int scoreInt;
            switch ( score )
            {
                case "True":
                    score = "1";
                    break;
                case "False":
                    score = "0";
                    break;
            }
            int.TryParse ( score, out scoreInt );
            return scoreInt;
        }

        private void AdjustIcons ( XRTable table )
        {
            var previousScore = "0";
            for ( var row = table.Rows.Count - 1; row >= 0; row-- )
            {
                var score = table.Rows[row].Cells[ScoreColumn].Text;
                XRPictureBox pb = null;
                foreach (var control in table.Rows[row].Cells[ChangeColumn].Controls)
                {
                    if ( control.GetType () == typeof(XRPictureBox) )
                    {
                        pb = ( (XRPictureBox)control );
                    }
                }
                if ( pb == null)
                {
                    continue;
                }

                if (pb.ImageUrl == "/Images/change_up_16x12px.png" && GetScore(previousScore) >= GetScore(score))
                {
                    pb.ImageUrl = "/Images/change_up_dots_16x12px.png";
                }
                else if (pb.ImageUrl == "/Images/change_down_16x12px.png" && GetScore(previousScore) <= GetScore(score))
                {
                    pb.ImageUrl = "/Images/change_down_dots_16x12px.png";
                }
                else if (pb.ImageUrl == "/Images/no_change_16x12px.png" && GetScore(previousScore) != GetScore(score))
                {
                    pb.ImageUrl = "/Images/no_change_dots_16x12px.png";
                }
                previousScore = score;
            }
        }

        private void CenterIcons (XRTable table)
        {
            foreach ( XRTableRow row in table.Rows )
            {
                foreach ( XRTableCell cell in row.Cells )
                {
                    foreach ( var control in cell.Controls )
                    {
                        if ( control.GetType () != typeof(XRPictureBox) )
                        {
                            continue;
                        }
                        var x = ((double)cell.Width / 2) - IconWidth;
                        ((XRPictureBox)control).LocationF = new PointF((float)x, 0);
                    }
                }
            }
        }

        private XRTableRow GetHeaderRow ( PatientScoreRangeData source )
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
                                  Font = headerFont
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
            var tdScoreHeader = new XRTableCell
                                {
                                    Text = source.HeaderScore,
                                    CanGrow = true,
                                    Borders = BorderSide.Bottom,
                                    BorderWidth = 3,
                                    BorderColor = Color.DarkGray,
                                    TextAlignment = TextAlignment.BottomCenter,
                                    Font = headerFont
                                };
            trHeader.Cells.Add ( tdScoreHeader );
            var tdChangeHeader = new XRTableCell
                                 {
                                     Text = source.HeaderChange,
                                     CanGrow = true,
                                     Borders = BorderSide.Bottom,
                                     BorderWidth = 3,
                                     BorderColor = Color.DarkGray,
                                     TextAlignment = TextAlignment.BottomCenter,
                                     Font = headerFont
                                 };
            trHeader.Cells.Add ( tdChangeHeader );
            var tdDateHeader = new XRTableCell
                               {
                                   Text = source.HeaderDate,
                                   CanGrow = true,
                                   Borders = BorderSide.Bottom,
                                   BorderWidth = 3,
                                   BorderColor = Color.DarkGray,
                                   TextAlignment = TextAlignment.BottomLeft,
                                   Font = headerFont
                               };
            trHeader.Cells.Add ( tdDateHeader );
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
            trHeader.Cells.Add(tdViewHeader);
            return trHeader;
        }

        private void OnBeforePrint ( object sender, PrintEventArgs printEventArgs )
        {
            BindData ();
        }

        #endregion
    }
}