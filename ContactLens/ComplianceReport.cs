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
    public class ComplianceReport : IReportMaker
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
            builder.AddPageHeader(String.Empty, "Scheduled Lens Replenishment Compliance Report", "Date: " + DateTime.Now.ToShortDateString());
            //builder.AddPageFooter("Page %p of %tp" , HorizontalAlignment.Right);
            //builder.AddText("Patients to be notified for scheduled lens replenishments during the month of: " + DateTime.Now.AddMonths(1).ToString("MMMM") + " " + DateTime.Now.AddMonths(1).Year + ".");
            builder.AddHorizontalLine();
            builder.DefaultTablePen = reportDocument.ThinPen;
            builder.AddTable(dv, true, 100);
            builder.CurrentSection.UseFullWidth = true;
            var headerRow = new TextStyle(TextStyle.TableHeader) { StringAlignment = StringAlignment.Center };

            //builder.AddColumn("PatientName", "Name", 1.4f, false, false);
            //builder.CurrentColumn.HeaderTextStyle = headerRow;
            //builder.AddColumn("PatientAddress", "Address", 1.1f, false, false);
            //builder.CurrentColumn.HeaderTextStyle = headerRow;
            //builder.AddColumn("RpS", "RpS", .6f, false, false);
            //builder.CurrentColumn.HeaderTextStyle = headerRow;
            //builder.AddColumn("NextSchdReplacement", "Next Schd Repl", .9f, false, false);
            //builder.CurrentColumn.HeaderTextStyle = headerRow;
            //builder.AddColumn("Eye", "Eye", .3f, false, false);
            //builder.CurrentColumn.HeaderTextStyle = headerRow;
            //builder.AddColumn("Manf", "Manufacturer", 1f, false, false);
            //builder.CurrentColumn.HeaderTextStyle = headerRow;
            //builder.AddColumn("Series", "Series", .7f, false, false);
            //builder.CurrentColumn.HeaderTextStyle = headerRow;
            //builder.AddColumn("BC", "BC", .3f, false, false);
            //builder.CurrentColumn.HeaderTextStyle = headerRow;
            //builder.AddColumn("DIA", "DIA", .3f, false, false);
            //builder.CurrentColumn.HeaderTextStyle = headerRow;
            //builder.AddColumn("Sphere", "Sphere", .5f, false, false);
            //builder.CurrentColumn.HeaderTextStyle = headerRow;
            //builder.AddColumn("CYL", "CYL", .3f, false, false);
            //builder.CurrentColumn.HeaderTextStyle = headerRow;
            //builder.AddColumn("Axis", "Axis", .3f, false, false);
            //builder.CurrentColumn.HeaderTextStyle = headerRow;
            //builder.AddColumn("Add", "Add", .3f, false, false);
            //builder.CurrentColumn.HeaderTextStyle = headerRow;
            //builder.AddColumn("Tint", "Tint", 1f, false, false);
            //builder.CurrentColumn.HeaderTextStyle = headerRow;
            //builder.AddColumn("Automail", "AM", .2f, false, false);
            //builder.CurrentColumn.HeaderTextStyle = headerRow;
            //builder.AddColumn("DoNotSend", "DS", .2f, false, false);
            //builder.CurrentColumn.HeaderTextStyle = headerRow;


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

                DateTime dtNewDate;
                DateTime.TryParse(strEndDate, out dtNewDate);

                StringBuilder myStringBuilder = new StringBuilder(3478);
                myStringBuilder.AppendFormat(@"SELECT     Soft_Contact_RX_2.patient_id, Soft_Contact_Lens_RX_1.right_or_left_cd, tbl.first_name, ");
                myStringBuilder.AppendFormat(@"tbl.last_name, address.address1, Replenishment.Category, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                      MIN(tbl.ReplenishmentDate) AS ReplenishmentDate, vendor.vendor_name1, ");
                myStringBuilder.AppendFormat(@"Soft_Contact_Lens_RX_1.lens_name, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                      Soft_Contact_Lens_RX_1.base_curve_mm, Soft_Contact_Lens_RX_1.diameter_mm, ");
                myStringBuilder.AppendFormat(@"Soft_Contact_Lens_RX_1.sphere_diopter, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                      Soft_Contact_Lens_RX_1.cylinder_diopter, Soft_Contact_Lens_RX_1.axis_deg, ");
                myStringBuilder.AppendFormat(@"Soft_Contact_Lens_RX_1.add_diopter, code.description, Replenishment.Automail, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                      Replenishment.DoNotSend, Replenishment.Notes, NULL AS ComplianceDate{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"FROM         patient INNER JOIN{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                      address ON patient.address_no = address.address_no INNER JOIN{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                          (SELECT     ReplenishmentSchedule.Rx_ID, ");
                myStringBuilder.AppendFormat(@"MAX(ReplenishmentSchedule.ReplenishmentDate) AS ReplenishmentDate, patient_1.last_name, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                                                   patient_1.first_name{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                            FROM          ReplenishmentSchedule INNER JOIN{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                                                   Soft_Contact_RX ON ReplenishmentSchedule.Rx_ID = ");
                myStringBuilder.AppendFormat(@"Soft_Contact_RX.rx_id INNER JOIN{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                                                   patient AS patient_1 ON ");
                myStringBuilder.AppendFormat(@"Soft_Contact_RX.patient_id = patient_1.patient_no CROSS JOIN{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                                                   Soft_Contact_Lens_RX{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                            GROUP BY ReplenishmentSchedule.ComplianceDate, ");
                myStringBuilder.AppendFormat(@"ReplenishmentSchedule.ReplenishmentDate, ReplenishmentSchedule.Rx_ID, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                                                   patient_1.last_name, patient_1.first_name{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                            HAVING      (ReplenishmentSchedule.ComplianceDate IS NULL) AND ");
                myStringBuilder.AppendFormat(@"(ReplenishmentSchedule.ReplenishmentDate <= '" + DBHelper.GetLastDayOfMonth(dtNewDate.AddMonths(-1)).ToShortDateString() + "' " + ") AND ");
                myStringBuilder.AppendFormat(@"(ReplenishmentSchedule.Rx_ID IN{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                                                       (SELECT     rx_id{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                                                         FROM          Soft_Contact_RX AS ");
                myStringBuilder.AppendFormat(@"Soft_Contact_RX_1))) AS tbl INNER JOIN{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                      Soft_Contact_RX AS Soft_Contact_RX_2 ON tbl.Rx_ID = Soft_Contact_RX_2.rx_id ");
                myStringBuilder.AppendFormat(@"INNER JOIN{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                      Soft_Contact_Lens_RX AS Soft_Contact_Lens_RX_1 ON Soft_Contact_RX_2.rx_id = ");
                myStringBuilder.AppendFormat(@"Soft_Contact_Lens_RX_1.rx_id ON {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                      patient.patient_no = Soft_Contact_RX_2.patient_id INNER JOIN{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                      Replenishment ON Soft_Contact_RX_2.rx_id = Replenishment.Rx_ID INNER JOIN{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                      vendor ON Soft_Contact_Lens_RX_1.lens_brand_cd = vendor.vendor_no  LEFT OUTER JOIN{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                      code ON Soft_Contact_Lens_RX_1.tint_cd = code.code{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"GROUP BY tbl.Rx_ID, tbl.last_name, tbl.first_name, Soft_Contact_RX_2.patient_id, ");
                myStringBuilder.AppendFormat(@"Soft_Contact_Lens_RX_1.right_or_left_cd, address.address1, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                      Replenishment.Category, Replenishment.Automail, Replenishment.DoNotSend, ");
                myStringBuilder.AppendFormat(@"vendor.vendor_name1, Soft_Contact_Lens_RX_1.lens_name, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                      Soft_Contact_Lens_RX_1.base_curve_mm, Soft_Contact_Lens_RX_1.diameter_mm, ");
                myStringBuilder.AppendFormat(@"Soft_Contact_Lens_RX_1.sphere_diopter, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                      Soft_Contact_Lens_RX_1.cylinder_diopter, Soft_Contact_Lens_RX_1.axis_deg, ");
                myStringBuilder.AppendFormat(@"Soft_Contact_Lens_RX_1.add_diopter, Replenishment.Notes, {0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"                      code.description{0}", Environment.NewLine);
                myStringBuilder.AppendFormat(@"ORDER BY last_name, first_name, ReplenishmentDate ASC, right_or_left_cd ASC");


                var dataAdapter = new SqlDataAdapter(myStringBuilder.ToString(), conn);

                var table = new DataTable
                {
                    Locale = System.Globalization.CultureInfo.InvariantCulture
                };
                dataAdapter.Fill(table);

                for (int i = 0; i <= table.Rows.Count-1; i++)
                  {
                    DataRow currentRow = table.Rows[i];
                    DataRow nextRow = table.Rows[i+1];
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
            catch {}

            var dv = dt.DefaultView;

            return dv;

        }

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

        private static DataTable RemoveDuplicateRows(DataTable dTable, string colName)
        {
            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();

            foreach (DataRow drow in dTable.Rows)
            {
                if (hTable.Contains(drow[colName]))
                    duplicateList.Add(drow);
                else
                    hTable.Add(drow[colName], string.Empty);
            }

            foreach (DataRow dRow in duplicateList)
                dTable.Rows.Remove(dRow);

            return dTable;
        }

        #endregion

        #region Private Methods


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
                case "True":
                    strResult = "X";
                    break;
                case "False":
                    strResult = "";
                    break;
                case "0":
                    strResult = "";
                    break;
                case "1":
                    strResult = "X";
                    break;
            }
            return strResult;
        }

        #endregion

    }
}
