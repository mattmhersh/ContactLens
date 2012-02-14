using System;
using System.Configuration;
using System.Drawing;
using System.Data;
using ReportPrinting;
using System.Drawing.Printing;
using System.Text;
using System.Data.SqlClient;
using System.Collections;

namespace ContactLens
{
    /// <summary>
    /// Sets up a basic report showing different formats of text.
    /// </summary>
    public class AutomailReport : IReportMaker
    {

        #region Public Methods

        public void MakeDocument(ReportDocument reportDocument, string strFontSize, string strStartDate, string strEndDate)
        {
            TextStyle.ResetStyles();

            float fFontSize = 10;
            float.TryParse(strFontSize, out fFontSize);
            
            TextStyle.Normal.Size = 10;            
            TextStyle.Heading1.Size = 16;
            TextStyle.Heading1.Bold = true;
            TextStyle.TableHeader.StringAlignment = StringAlignment.Center;
            TextStyle.TableHeader.Size = fFontSize;
            TextStyle.TableHeader.Bold = true;
            TextStyle.TableRow.Size = fFontSize;
            TextStyle.TableRow.Bold = false;
            
            var builder = new ReportBuilder(reportDocument);            
            builder.StartLinearLayout(Direction.Vertical);

            var margins = new Margins(40, 30, 60, 0);
            builder.CurrentDocument.DefaultPageSettings.Margins = margins;

            var dv = GetDataView(strStartDate, strEndDate);
            //builder.AddPageHeader(String.Empty, "This is test 11 - Tables with lines", "page %p");
            builder.AddPageHeader(String.Empty, "Scheduled Lens Replenishment Automail Summary Report", "Date: " + DateTime.Now.ToShortDateString());
            //builder.AddPageFooter("Page %p of %tp" , HorizontalAlignment.Right);
            //builder.AddText("Patients to be notified for scheduled lens replenishments during the month of: " + DateTime.Now.AddMonths(1).ToString("MMMM") + " " + DateTime.Now.AddMonths(1).Year + ".");
            builder.AddText("Patients to be notified for scheduled lens replenishments beginning " + strStartDate + " and ending " + strEndDate + ".");
            builder.AddHorizontalLine();
            builder.DefaultTablePen = reportDocument.ThinPen;
            builder.AddTable(dv, true, 100);
            builder.CurrentSection.UseFullWidth = true;
            var headerRow = new TextStyle(TextStyle.TableHeader) {StringAlignment = StringAlignment.Center};

            builder.AddColumn("PatientName", "Patient", 1.1f, false, false);
            builder.CurrentColumn.HeaderTextStyle = headerRow;
            builder.AddColumn("RpS", "RpS", .7f, false, false);
            builder.CurrentColumn.HeaderTextStyle = headerRow;
            builder.AddColumn("NextSchdReplacement", "Next Schd Repl", 1.1f, false, false);
            builder.CurrentColumn.HeaderTextStyle = headerRow;
            builder.AddColumn("Eye", "Eye", .2f, false, false);
            builder.CurrentColumn.HeaderTextStyle = headerRow;
            builder.AddColumn("Manf", "Manufacturer", 1f, false, false);
            builder.CurrentColumn.HeaderTextStyle = headerRow;
            builder.AddColumn("Series", "Series", .7f, false, false);
            builder.CurrentColumn.HeaderTextStyle = headerRow;
            builder.AddColumn("BC", "BC", .6f, false, false);
            builder.CurrentColumn.HeaderTextStyle = headerRow;
            builder.AddColumn("DIA", "DIA", .5f, false, false);
            builder.CurrentColumn.HeaderTextStyle = headerRow;
            builder.AddColumn("Sphere", "Sphere", .6f, false, false);
            builder.CurrentColumn.HeaderTextStyle = headerRow;
            builder.AddColumn("CYL", "CYL", .7f, false, false);
            builder.CurrentColumn.HeaderTextStyle = headerRow;
            builder.AddColumn("Axis", "Axis", .5f, false, false);
            builder.CurrentColumn.HeaderTextStyle = headerRow;
            builder.AddColumn("Add", "Add", .6f, false, false);
            builder.CurrentColumn.HeaderTextStyle = headerRow;
            builder.AddColumn("Tint", "Tint", 1.2f, false, false);
            builder.CurrentColumn.HeaderTextStyle = headerRow;
            builder.AddColumn("Automail", "AM", .2f, false, false);
            builder.CurrentColumn.HeaderTextStyle = headerRow;
            builder.AddColumn("DoNotSend", "DS", .2f, false, false);
            builder.CurrentColumn.HeaderTextStyle = headerRow;
            builder.FinishLinearLayout();

        }

        public static DataView GetDataView(string strStartDate, string strEndDate)
        {
            var dt = new DataTable("Report");
            dt.Columns.Add("PatientName", typeof(string));
            //dt.Columns.Add("PatientAddress", typeof(string));
            dt.Columns.Add("RpS", typeof(string));
            dt.Columns.Add("NextSchdReplacement", typeof(string));
            dt.Columns.Add("Eye", typeof(string));
            dt.Columns.Add("Manf", typeof(string));
            dt.Columns.Add("Series", typeof(string));            
            dt.Columns.Add("BC", typeof(string));
            dt.Columns.Add("DIA", typeof(string));
            dt.Columns.Add("Sphere", typeof(string));
            dt.Columns.Add("CYL", typeof(string));
            dt.Columns.Add("Axis", typeof(string));
            dt.Columns.Add("Add", typeof(string));
            dt.Columns.Add("Tint", typeof(string));
            dt.Columns.Add("Automail", typeof(string));
            dt.Columns.Add("DoNotSend", typeof(string));

            try
            {
                DBHelper myHelper = new DBHelper();
                var conn = new SqlConnection(myHelper.OmdbConn);
                conn.Open();

                var myStringBuilder = new StringBuilder();
                myStringBuilder.AppendFormat(@"SELECT Soft_Contact_RX.patient_id,{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"Soft_Contact_Lens_RX.right_or_left_cd, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"patient.first_name, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"patient.last_name, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"address.address1, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"Replenishment.Category, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"ReplenishmentSchedule.ReplenishmentDate, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"vendor.vendor_name1, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"Soft_Contact_Lens_RX.lens_name, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"Soft_Contact_Lens_RX.base_curve_mm, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"Soft_Contact_Lens_RX.diameter_mm, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"Soft_Contact_Lens_RX.sphere_diopter, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"Soft_Contact_Lens_RX.cylinder_diopter, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"Soft_Contact_Lens_RX.axis_deg, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"Soft_Contact_Lens_RX.add_diopter, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"code.description, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"Replenishment.Automail, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"Replenishment.DoNotSend, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"Replenishment.Notes {0}, ", Environment.NewLine);
                myStringBuilder.AppendFormat(@"ReplenishmentSchedule.ComplianceDate {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"FROM ((((([dbo].[patient]  {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"INNER JOIN ([dbo].[Soft_Contact_RX]{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"INNER JOIN [dbo].[Soft_Contact_Lens_RX]{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"ON Soft_Contact_RX.rx_id = Soft_Contact_Lens_RX.rx_id) {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"ON patient.patient_no = Soft_Contact_RX.patient_id) {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"INNER JOIN [dbo].[address]{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"ON patient.address_no = address.address_no) {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"INNER JOIN [dbo].[vendor] {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"ON Soft_Contact_Lens_RX.lens_brand_cd = vendor.vendor_no) {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"LEFT JOIN [dbo].[code] ON Soft_Contact_Lens_RX.tint_cd = ");
                myStringBuilder.AppendFormat(@"code.code) {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"INNER JOIN [dbo].[Replenishment] {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"ON Soft_Contact_RX.rx_id = Replenishment.rx_id) {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"INNER JOIN [dbo].[ReplenishmentSchedule] {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"ON Soft_Contact_RX.rx_id = ReplenishmentSchedule.rx_id ");
                //myStringBuilder.AppendFormat(@"WHERE ReplenishmentSchedule.ReplenishmentDate Between '" + DBHelper.GetFirstDayOfMonth(DateTime.Now.AddMonths(1)).ToShortDateString() + "' And '" + DBHelper.GetLastDayOfMonth(DateTime.Now.AddMonths(1)).ToShortDateString() + "' ");
                myStringBuilder.AppendFormat(@"WHERE ReplenishmentSchedule.ReplenishmentDate Between '" + strStartDate + "' And '" + strEndDate + "' ");
                myStringBuilder.AppendFormat(@"AND LEN(Replenishment.Automail) > 0 ");
                myStringBuilder.AppendFormat(@"AND ReplenishmentSchedule.ComplianceDate IS NULL ");
                myStringBuilder.AppendFormat(@"ORDER BY patient.last_name, patient.first_name, ReplenishmentSchedule.ReplenishmentDate ASC, Soft_Contact_Lens_RX.right_or_left_cd ASC {0}", Environment.NewLine);

                var dataAdapter = new SqlDataAdapter(myStringBuilder.ToString(), conn);

                var table = new DataTable
                                {
                                    Locale = System.Globalization.CultureInfo.InvariantCulture
                                };
                dataAdapter.Fill(table);

                DataTable distinctTable = table.DefaultView.ToTable( /*distinct*/ true);

                table = distinctTable;

                for (int i = 0; i <= table.Rows.Count - 1; i++)
                {
                    DataRow currentRow = table.Rows[i];
                    DataRow nextRow = table.Rows[i + 1];
                    if (currentRow[0].ToString().Equals(nextRow[0].ToString()))
                    {
                        // Add Contact Lens 1
                        AddContactLens(ref dt, ref currentRow);

                        // Add Contact Lens 2
                        AddContactLens(ref dt, ref nextRow);

                        // Add Replenishment Notes
                        AddReplenishmentNotes(ref dt, ref currentRow);

                        // Add Blank Line
                        AddBlankLine(ref dt);

                        i++;
                    }
                    else
                    {
                        // Add Contact Lens 1
                        AddContactLens(ref dt, ref currentRow);

                        // Add Replenishment Notes
                        AddReplenishmentNotes(ref dt, ref currentRow);

                        // Add Blank Line
                        AddBlankLine(ref dt);
                    }
                }

            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message, ex);
            }

            var dv = dt.DefaultView;

            return dv;

        }

        #endregion

        #region Private Methods

        private static void AddContactLens(ref DataTable dt, ref DataRow dr)
        {
            string strAutomail = "";
            if (dr[16].ToString().Length > 0)
            {
                strAutomail = dr[16].ToString().Substring(0, 1);
            }
            dt.Rows.Add(new Object[] 
                                    { dr[2] + " " + dr[3] + " - " + dr[4].ToString(),                                        
                                        dr[5].ToString(), 
                                        DateTime.Parse(dr[6].ToString()).ToShortDateString(), 
                                        DecodeEye(dr[1].ToString()),
                                        dr[7].ToString(), 
                                        dr[8].ToString(),                                         
                                        dr[9].ToString(), 
                                        dr[10].ToString(), 
                                        dr[11].ToString(), 
                                        dr[12].ToString(), 
                                        dr[13].ToString(), 
                                        dr[14].ToString(), 
                                        dr[15].ToString(),
                                        strAutomail,
                                        DecodeBoolean(dr[17].ToString())});
        }

        private static void AddBlankLine(ref DataTable dt)
        {
            // Add Blank Line
            dt.Rows.Add(new Object[] 
                                { " ", 
                                    " ", 
                                    " ", 
                                    " ",
                                    " ", 
                                    " ",
                                    " ",
                                    " ",
                                    " ", 
                                    " ", 
                                    " ", 
                                    " ",
                                    " ",
                                    " ",
                                    " "});

        }

        private static void AddReplenishmentNotes(ref DataTable dt, ref DataRow dr)
        {
            var strReplenishmentNotes = dr[18].ToString();
            if (strReplenishmentNotes.Length == 0)
            {
                strReplenishmentNotes = " ";
            }
            // Add Replenishment Notes
            dt.Rows.Add(new Object[] 
                                { strReplenishmentNotes,  
                                    " ", 
                                    " ", 
                                    " ",
                                    " ", 
                                    " ",
                                    " ",
                                    " ",
                                    " ", 
                                    " ", 
                                    " ", 
                                    " ",
                                    " ",
                                    " ",
                                    " "});
        }

        private static string DecodeEye(string strEye)
        {
            var strResult = "";
            switch (strEye)
            {
                case "0":
                    strResult = "R";
                    break;
                case "1":
                    strResult = "L";
                    break;
            }
            return strResult;
        }

        private static string DecodeBoolean(string value)
        {
            var strResult = "";
            switch (value)
            {
                case "1":
                    strResult = "X";
                    break;
                case "0":
                    strResult = "";
                    break;
            }
            return strResult;
        }

        #endregion
    }
}
