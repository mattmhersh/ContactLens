using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Utilities;

namespace ContactLens
{
    public partial class FrmFindPatient : Form
    {
        #region Private Variables

        #endregion

        #region Public Variables

        public static FrmFindPatient StaticVar;

        public bool Delete { get; set; }

        #endregion

        #region Public Methods

        public FrmFindPatient()
        {
            InitializeComponent();
        }

        private bool _IsExit = true;
        public bool IsExit
        {
            get { return _IsExit; }
            set { _IsExit = value; }
        }

        #endregion

        #region Private Methods

        private void FrmFindPatient_Closing(object sender, EventArgs e)
        {
            if (_IsExit)
            {
                Environment.Exit(0);
                this.Dispose();
            }
        }

        private void FrmFindPatient_Load(object sender, EventArgs e)
        {

        }

        #region Catch F2 Key

        private void FrmFindPatient_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                FindPatient();
            }
        }

        private void txtLastName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                FindPatient();
            }
        }

        private void txtFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                FindPatient();
            }
        }

        private void txtSSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                FindPatient();
            }
        }

        private void txtAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                FindPatient();
            }
        }

        private void txtDOB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                FindPatient();
            }
        }

        private void txtCity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                FindPatient();
            }
        }

        private void txtHomePhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                FindPatient();
            }
        }

        private void txtPatientNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                FindPatient();
            }
        }

        private void txtChartNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                FindPatient();
            }
        }

        private void dgResults_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                FindPatient();
            }
        }

        #endregion

        private void FindPatient()
        {
            SqlConnection conn = null;
            try
            {

                Cursor = Cursors.WaitCursor;
                dgResults.DataSource = null;
                AddColumns();

                DBHelper myHelper = new DBHelper();
                conn = new SqlConnection(myHelper.OmdbConn);

                conn.Open();

                var dataAdapter = new SqlDataAdapter(GetSql(), conn);

                // Populate a new data table and bind it to the BindingSource.
                var table = new DataTable();
                dataAdapter.Fill(table);
                dbBindSource.DataSource = table;

                // you can make it grid readonly.
                dgResults.ReadOnly = true;

                // finally bind the data to the grid
                dgResults.DataSource = dbBindSource;

                txtPatientsFound.Text = table.Rows.Count.ToString();

                Cursor = Cursors.Arrow;

            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Find Patient Error!");
                //throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
                Close();
                Environment.Exit(0);
                this.Dispose();
        }

        private void btFind_Click(object sender, EventArgs e)
        {            

            SqlConnection conn = null;
            try
            {

                Cursor = Cursors.WaitCursor;
                dgResults.DataSource = null;
                AddColumns();

                DBHelper myHelper = new DBHelper();
                conn = new SqlConnection(myHelper.OmdbConn);         

                conn.Open();

                var dataAdapter = new SqlDataAdapter(GetSql(), conn);

                // Populate a new data table and bind it to the BindingSource.
                var table = new DataTable();
                dataAdapter.Fill(table);
                dbBindSource.DataSource = table;

                // you can make it grid readonly.
                dgResults.ReadOnly = true;

                // finally bind the data to the grid
                dgResults.DataSource = dbBindSource;

                txtPatientsFound.Text = table.Rows.Count.ToString();

                Cursor = Cursors.Arrow;

            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Find Patient Error!");
                //throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            try
            {
                dgResults.DataSource = null;
            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not clear results grid!");
                //throw new Exception(ex.Message, ex);
            }
        }

        private void btSelect_Click(object sender, EventArgs e)
        {
            try
            {


                if (dgResults.RowCount == 0)
                {
                    MessageBox.Show("Please select a Patient!");
                    return;
                }

                if (Delete)
                {
                    DialogResult dlgRes = MessageBox.Show(
                        "Are you sure you want to delete the Replenishment Schedule for: " +
                        dgResults.SelectedRows[0].Cells[1].Value + "?",
                        "Confirm Delete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if (dlgRes == DialogResult.Yes)
                    {
                        DeleteReplenishmentSchedule(Int32.Parse(dgResults.SelectedRows[0].Cells[0].Value.ToString()));
                        MessageBox.Show("Succesfully Deleted Replenishment Schedule!");
                        StaticVar = this;
                        Hide();
                        var form3 = new FrmReplacementMenu();
                        form3.Show();
                        return;
                    }
                    else
                    {
                        StaticVar = this;
                        Hide();
                        var form3 = new FrmReplacementMenu();
                        form3.Show();
                        return;
                    }
                }


                if (SetCorrectRxId(int.Parse(dgResults.SelectedRows[0].Cells[0].Value.ToString())) == "0")
                {
                    MessageBox.Show(
                        "Please add a Soft Contact Lens Order for: " + dgResults.SelectedRows[0].Cells[1].Value + "!",
                        "Add a Soft Contact Lens Order");
                    return;
                }

                StaticVar = this;
                Hide();
                var form2 = new FrmReplenishmentSchedule
                                {
                                    PatientId = Int32.Parse(dgResults.SelectedRows[0].Cells[0].Value.ToString())
                                };                
                form2.Show();
                this.Visible = false;
                this.IsExit = false;
                this.Close();
            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Error selecting patient!");
                //throw new Exception(ex.Message, ex);
            }
        }

        private static void DeleteReplenishmentSchedule(int patientId)
        {
            SqlConnection conn = null;
            try
            {
                DBHelper myHelper = new DBHelper();
                conn = new SqlConnection(myHelper.OmdbConn);
                conn.Open();

                var dataAdapter = new SqlDataAdapter { DeleteCommand = new SqlCommand { Connection = conn } };

                // Setup Insert Command
                var iRxId = Int32.Parse(SetCorrectRxId(patientId));
                dataAdapter.DeleteCommand.CommandText = string.Format("DELETE FROM REPLENISHMENTSCHEDULE WHERE RX_ID = {0}", iRxId);

                dataAdapter.DeleteCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not Delete Replenishment Schedule");
                //throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        private static string SetCorrectRxId(int patientId)
        {
            SqlConnection conn = null;
            var sbSql = new StringBuilder();
            var result = "";
            try
            {
                DBHelper myHelper = new DBHelper();
                conn = new SqlConnection(myHelper.OmdbConn);
                conn.Open();

                sbSql.Append("SELECT max(rx_id) ");
                sbSql.Append("FROM Soft_Contact_Rx ");
                sbSql.Append("WHERE patient_id =" + patientId);

                var dataAdapter = new SqlDataAdapter(sbSql.ToString(), conn);

                // Populate a new data table and bind it to the BindingSource.
                var table = new DataTable
                                {
                                    Locale = System.Globalization.CultureInfo.InvariantCulture
                                };
                dataAdapter.Fill(table);

                if (table.Rows[0][0] != null)
                {
                    result = table.Rows[0][0].ToString();
                }

                if (result.Length == 0)
                {
                    return "0";
                }

            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not set correct rx id!");
                //throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return result;
        }

        private void AddColumns()
        {
            try
            {
                dgResults.Columns.Clear();
                dgResults.AutoGenerateColumns = false;

                DataGridViewCell oCell = new DataGridViewTextBoxCell();

                var col = new DataGridViewColumn
                                             {
                                                 Name = "patient_no",
                                                 HeaderText = "patient_no",
                                                 DataPropertyName = "patient_no",
                                                 CellTemplate = oCell,
                                                 Visible = false
                                             };
                dgResults.Columns.Add(col);

                col = new DataGridViewColumn
                          {
                              Name = "name",
                              HeaderText = "Name",
                              DataPropertyName = "name",
                              Width = 150,
                              CellTemplate = oCell
                          };
                dgResults.Columns.Add(col);

                col = new DataGridViewColumn
                          {
                              Name = "addresscity",
                              HeaderText = "Address / City",
                              DataPropertyName = "addresscity",
                              CellTemplate = oCell,
                              Width = 250
                          };
                dgResults.Columns.Add(col);

                col = new DataGridViewColumn
                          {
                              Name = "homephone",
                              HeaderText = "Home Phone",
                              DataPropertyName = "homephone",
                              CellTemplate = oCell
                          };
                dgResults.Columns.Add(col);

                col = new DataGridViewColumn
                          {
                              Name = "ssn",
                              HeaderText = "SS No",
                              DataPropertyName = "ssn",
                              CellTemplate = oCell
                          };
                dgResults.Columns.Add(col);

                col = new DataGridViewColumn
                          {
                              Name = "birthdate",
                              HeaderText = "DOB",
                              DataPropertyName = "birthdate",
                              CellTemplate = oCell
                          };
                dgResults.Columns.Add(col);
            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not add columns to result table!");
                //throw new Exception(ex.Message, ex);
            }
        }

        private string GetSql()
        {
            var sbSql = new StringBuilder();
            try
            {
                sbSql.Append("Select distinct(patient.patient_no)");
                sbSql.Append(",patient.last_name + ', ' + patient.first_name as name");
                sbSql.Append(",address.address1 + ' / ' + address.city as addresscity");
                sbSql.Append(",address.phone1 as homephone");
                sbSql.Append(",patient.ss_no as ssn");
                sbSql.Append(",patient.birth_date as [birthdate], patient.last_name ");
                //sbSql.Append("FROM patient INNER JOIN address ");
                //sbSql.Append("ON patient.address_no = address.address_no WHERE patient.patient_no > 1 ");
                sbSql.Append(
                    "FROM (patient INNER JOIN address ON patient.address_no = address.address_no) INNER JOIN Soft_Contact_RX ON patient.patient_no = Soft_Contact_RX.patient_id WHERE patient.patient_no > 1 ");

                if (txtLastName.Text.Length > 0)
                {
                    sbSql.Append("and Lower(patient.last_name) like Lower('%" + txtLastName.Text.Replace("'", "''") + "%') ");
                }
                if (txtFirstName.Text.Length > 0)
                {
                    sbSql.Append("and Lower(patient.first_name) like Lower('%" + txtFirstName.Text.Replace("'", "''") + "%') ");
                }
                if (txtSSN.Text.Length > 0)
                {
                    sbSql.Append("and patient.ss_no like '%" + txtSSN.Text.Replace("'", "''") + "%' ");
                }
                if (txtAddress.Text.Length > 0)
                {
                    sbSql.Append("and Lower(address.address1) like Lower('%" + txtAddress.Text.Replace("'", "''") + "%') ");
                }
                if (txtDOB.Text.Length > 0)
                {
                    sbSql.Append("and patient.birth_date like '%" + txtDOB.Text.Replace("'", "''") + "%' ");
                }
                if (txtCity.Text.Length > 0)
                {
                    sbSql.Append("and Lower(address.city) like Lower('%" + txtCity.Text.Replace("'", "''") + "%') ");
                }
                if (txtHomePhone.Text.Length > 0)
                {
                    sbSql.Append("and address.phone1 like '%" + txtHomePhone.Text.Replace("'", "''") + "%' ");
                }
                if (txtPatientNo.Text.Length > 0)
                {
                    sbSql.Append("and patient.patient_no = '" + txtPatientNo.Text.Replace("'", "''") + "' ");
                }
                if (txtChartNo.Text.Length > 0)
                {
                    sbSql.Append("and patient.chart_no = '" + txtChartNo.Text.Replace("'", "''") + "' ");
                }
                sbSql.Append("ORDER BY patient.last_name ASC");
            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not get SQL for finding a patient!");
                //throw new Exception(ex.Message, ex);
            }
            return sbSql.ToString();
        }

        private void btBack_Click(object sender, EventArgs e)
        {
                StaticVar = this;
                Hide();
                this.Visible = false;
                this.IsExit = false;
                this.Close();
                var form2 = new FrmReplacementMenu();
                form2.Show();
        }

        private void dgResults_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {


                if (dgResults.RowCount == 0)
                {
                    MessageBox.Show("Please select a Patient!");
                    return;
                }

                if (Delete)
                {
                    DialogResult dlgRes = MessageBox.Show(
                        "Are you sure you want to delete the Replenishment Schedule for: " +
                        dgResults.SelectedRows[0].Cells[1].Value + "?",
                        "Confirm Delete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if (dlgRes == DialogResult.Yes)
                    {
                        DeleteReplenishmentSchedule(Int32.Parse(dgResults.SelectedRows[0].Cells[0].Value.ToString()));
                        MessageBox.Show("Succesfully Deleted Replenishment Schedule!");
                        StaticVar = this;
                        Hide();
                        var form3 = new FrmReplacementMenu();
                        form3.Show();
                        return;
                    }
                    else
                    {
                        StaticVar = this;
                        Hide();
                        var form3 = new FrmReplacementMenu();
                        form3.Show();
                        return;
                    }
                }


                if (SetCorrectRxId(int.Parse(dgResults.SelectedRows[0].Cells[0].Value.ToString())) == "0")
                {
                    MessageBox.Show(
                        "Please add a Soft Contact Lens Order for: " + dgResults.SelectedRows[0].Cells[1].Value + "!",
                        "Add a Soft Contact Lens Order");
                    return;
                }

                StaticVar = this;
                Hide();
                
                var form2 = new FrmReplenishmentSchedule
                {
                    PatientId = Int32.Parse(dgResults.SelectedRows[0].Cells[0].Value.ToString())
                };
                form2.Show();
                this.Visible = false;
                this.IsExit = false;
                this.Close();
            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Error selecting patient!");
                //throw new Exception(ex.Message, ex);
            }
        }

        #endregion
        
    }
}
