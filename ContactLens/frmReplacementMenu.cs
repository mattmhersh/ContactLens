using System;
using System.Windows.Forms;
using ReportPrinting;
using Utilities;

namespace ContactLens
{
    public partial class FrmReplacementMenu : Form
    {

        #region Public Variables

        public static FrmReplacementMenu StaticVar;
        
        #endregion

        public FrmReplacementMenu()
        {
            InitializeComponent();
        }

        private bool _IsExit = true;
        public bool IsExit
        {
            get { return _IsExit; }
            set { _IsExit = value; }            
        }

        #region Private Methods

        private void btExit_Click(object sender, EventArgs e)
        {

                Close();
                Environment.Exit(0);
                this.Dispose();

        }

        private void frmReplacementMenu_Load(object sender, EventArgs e)
        {
            //lblPatientID.Text += GetPatientName(PatientID);

            // Set Dates

            txtReportStartDate.Text = DBHelper.GetFirstDayOfMonth(DateTime.Now.AddMonths(1)).ToShortDateString();
            txtReportEndDate.Text = DBHelper.GetLastDayOfMonth(DateTime.Now.AddMonths(1)).ToShortDateString();

        }

        private void btSelect_Click(object sender, EventArgs e)
        {
            try
            {
                bool isChecked = false;
                foreach (Control ctl in groupBox1.Controls)
                {
                    if (ctl is RadioButton)
                    {
                        var aRadioButton = (RadioButton) ctl;
                        if (aRadioButton.Checked)
                        {
                            isChecked = true;
                        }
                    }
                }

                if (!isChecked)
                {
                    MessageBox.Show("Please select an option!");
                    return;
                }

                var form2 = new FrmFindPatient();
                foreach (Control ctl in groupBox1.Controls)
                {
                    if (ctl is RadioButton)
                    {
                        var aRadioButton = (RadioButton) ctl;
                        if (aRadioButton.Checked)
                        {
                            switch (aRadioButton.Name)
                            {
                                case "rbUpdate":
                                    StaticVar = this;
                                    this.Visible = false;
                                    Hide();
                                    form2.Show();
                                    break;
                                case "rbDelete":
                                    StaticVar = this;
                                    this.Visible = false;
                                    Hide();
                                    form2.Delete = true;
                                    form2.Show();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Menu selection failed!");
                //throw new Exception(ex.Message, ex);
            }
        }

        private void btPrintAutomailReport_Click(object sender, EventArgs e)
        {
            try
            {

                DateTime dtDate;
                int iFontSize;
                if (!DateTime.TryParse(txtReportStartDate.Text, out dtDate))
                {
                    MessageBox.Show("Report Start Date is invalid!");
                    return;
                }

                if (!DateTime.TryParse(txtReportEndDate.Text, out dtDate))
                {
                    MessageBox.Show("Report End Date is invalid!");
                    return;
                }
                
                if (!int.TryParse(txtReportFontSize.Text, out iFontSize))
                {
                    MessageBox.Show("Report Font Size is invalid!");
                    return;
                }

                var reportDocument1 = new ReportDocument {ReportMaker = new AutomailReport()};
                reportDocument1.StartDate = txtReportStartDate.Text;
                reportDocument1.EndDate = txtReportEndDate.Text;
                reportDocument1.FontSize = txtReportFontSize.Text;
                var printControl = new PrintControl {Document = reportDocument1};
                printControl.Preview(sender, e);
            }
            catch (Exception ex)
            {

                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not print Automail Report!");
                //throw new Exception(ex.Message, ex);
            }
        }

        private void btPrintComplianceReport_Click(object sender, EventArgs e)
        {
            try
            {

                DateTime dtDate;
                int iFontSize;
                if (!DateTime.TryParse(txtReportStartDate.Text, out dtDate))
                {
                    MessageBox.Show("Report Start Date is invalid!");
                    return;
                }

                if (!DateTime.TryParse(txtReportEndDate.Text, out dtDate))
                {
                    MessageBox.Show("Report End Date is invalid!");
                    return;
                }

                if (!int.TryParse(txtReportFontSize.Text, out iFontSize))
                {
                    MessageBox.Show("Report Font Size is invalid!");
                    return;
                }

                var reportDocument1 = new ReportDocument {ReportMaker = new ComplianceReport()};
                reportDocument1.StartDate = txtReportStartDate.Text;
                reportDocument1.EndDate = txtReportEndDate.Text;
                reportDocument1.FontSize = txtReportFontSize.Text;                
                var printControl = new PrintControl {Document = reportDocument1};
                printControl.Preview(sender, e);
            }
            catch (Exception ex)
            {

                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not print Compliance Report!");
                //throw new Exception(ex.Message, ex);
            }
        }

        #endregion

    }
}
