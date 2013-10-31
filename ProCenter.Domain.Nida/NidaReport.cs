#region License Header
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
#endregion
namespace ProCenter.Domain.Nida
{
    #region

    using System.Linq;
    using CommonModule;
    using DevExpress.XtraReports.UI;

    #endregion

    public partial class NidaReport : XtraReport, IReport
    {
        public NidaReport()
        {
            InitializeComponent();
        }

        private void NidaReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var data = ((this.DataSource as NidaReportDataCollection)[0] as NidaReportData);
            if (data.FollowUpItems == null || !data.FollowUpItems.Any())
            {
                xrLabel17.Text = null;
                xrLabel17.Visible = false;
            }
            if (data.UseTreatmentHistoryItems == null || !data.UseTreatmentHistoryItems.Any())
            {
                xrLabel18.Text = null;
                xrLabel18.Visible = false;
            }
            if (data.PatientResourceItems == null || !data.PatientResourceItems.Any())
            {
                xrLabel19.Text = null;
                xrLabel19.Visible = false;
            }
            if (data.SummaryItems == null || !data.SummaryItems.Any())
            {
                xrLabel16.Text = null;
                xrLabel16.Visible = false;
            }
        }
    }
}