using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Samples.Windows.Forms.DataGridViewCustomColumn;
using Utilities;

namespace ContactLens
{
    public partial class FrmReplenishmentSchedule : Form
    {

        #region Public Variables

        public static FrmReplenishmentSchedule StaticVar;

        #endregion

        #region Public Methods

        public FrmReplenishmentSchedule()
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

        private void FrmReplenishmentSchedule_Closing(object sender, EventArgs e)
        {
            if (_IsExit)
            {
                Environment.Exit(0);
                this.Dispose();
            }
        }


        private void FrmReplenishmentScheduleLoad(object sender, EventArgs e)
        {
            try
            {
                lblPatientID.Text += GetPatientName(PatientId);
                RxId = Int32.Parse(SetCorrectRxId(PatientId));
                if (RxId == 0)
                {
                    return;
                }
                GetLensData();
                LoadValues();
                LoadReplenishmentSchedule();
            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not load Replenishment Schedule Form!");
                //throw new Exception(ex.Message, ex);
            }
        }

        private void RefreshReplenishmentSchedule()
        {
            SqlConnection conn = null;
            try
            {
                DBHelper myHelper = new DBHelper();
                conn = new SqlConnection(myHelper.OmdbConn);
                conn.Open();

                var _dataReplenishmentScheduleAdapter = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand
                    {
                        Connection = conn,
                        CommandText =
                            "SELECT ReplenishmentDate as [Replenishment Date], ComplianceDate as [Compliance Date], Delqt, rx_id, id  FROM REPLENISHMENTSCHEDULE WHERE RX_ID = " +
                            RxId
                    }
                };

                // Populate a new data table and bind it to the BindingSource.
                var ds = new DataSet();
                _dataReplenishmentScheduleAdapter.Fill(ds);
                
                if (ds != null)
                {
                    //set the DataGridView DataSource
                    dgDates.DataSource = ds;
                    dgDates.DataMember = "Table";
                    dgDates.Columns[0].ValueType = typeof(String);
                    dgDates.Columns[1].ValueType = typeof(String);
                    dgDates.Columns[2].ValueType = typeof(String);
                }

            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not go load Replenishment Schedule");
                //throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        private void LoadReplenishmentSchedule()
        {
            SqlConnection conn = null;
            try
            {
                DBHelper myHelper = new DBHelper();
                conn = new SqlConnection(myHelper.OmdbConn);
                conn.Open();

                var _dataReplenishmentScheduleAdapter = new SqlDataAdapter
                                                       {
                                                           SelectCommand = new SqlCommand
                                                                               {
                                                                                   Connection = conn,
                                                                                   CommandText =
                                                                                       "SELECT CONVERT(nvarchar(30), ReplenishmentDate, 101) as [Replenishment Date], CONVERT(nvarchar(30), ComplianceDate, 101) as [Compliance Date], CONVERT(nvarchar(30), Delqt) as Delqt, rx_id, id  FROM REPLENISHMENTSCHEDULE WHERE RX_ID = " +
                                                                                       RxId
                                                                               }
                                                       };

                // Populate a new data table and bind it to the BindingSource.
                var ds = new DataSet();
                _dataReplenishmentScheduleAdapter.Fill(ds);
                dgDates.Columns.Clear();
                dgDates.AutoGenerateColumns = false;

                // Create Columns
                CreateColumns();

                if (ds != null)
                {
                    //set the DataGridView DataSource
                    dgDates.DataSource = ds;
                    dgDates.DataMember = "Table";
                    dgDates.Columns[0].ValueType = typeof(String);
                    dgDates.Columns[1].ValueType = typeof(String);
                    dgDates.Columns[2].ValueType = typeof(String);
                }

            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not go load Replenishment Schedule");
                //throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        private void CreateColumns()
        {
            // Create Columns
            DataGridViewTextBoxColumn dgvtbc;
            MaskedTextBoxColumn mtbc;

            // Add Replenishment Date Column
            mtbc = new MaskedTextBoxColumn();
            mtbc.HeaderText = "Replenishment Date";
            mtbc.DataPropertyName = "Replenishment Date";
            mtbc.Mask = "00/00/0000";
            mtbc.Width = 200;
            mtbc.ValueType = typeof(String);
            mtbc.DefaultCellStyle.SelectionForeColor = Color.Black;
            mtbc.DefaultCellStyle.SelectionBackColor = Color.Yellow;
            mtbc.ReadOnly = false;
            dgDates.Columns.Add(mtbc);

            // Add Compliance Date Column
            mtbc = new MaskedTextBoxColumn();
            mtbc.HeaderText = "Compliance Date";
            mtbc.DataPropertyName = "Compliance Date";
            mtbc.Mask = "00/00/0000";
            mtbc.Width = 200;
            mtbc.ValueType = typeof(String);
            mtbc.IncludeLiterals = false;

            mtbc.DefaultCellStyle.SelectionForeColor = Color.Black;
            mtbc.DefaultCellStyle.SelectionBackColor = Color.Yellow;
            dgDates.Columns.Add(mtbc);

            dgvtbc = new DataGridViewTextBoxColumn();
            dgvtbc.HeaderText = "Delqt";
            dgvtbc.DataPropertyName = "Delqt";
            dgvtbc.Width = 100;
            dgvtbc.ValueType = typeof(string);
            dgvtbc.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvtbc.DefaultCellStyle.SelectionBackColor = Color.Yellow;
            dgvtbc.ReadOnly = true;
            dgDates.Columns.Add(dgvtbc);

            dgvtbc = new DataGridViewTextBoxColumn();
            dgvtbc.HeaderText = "ID";
            dgvtbc.DataPropertyName = "ID";
            dgvtbc.Width = 100;
            dgvtbc.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvtbc.DefaultCellStyle.SelectionBackColor = Color.Yellow;
            dgvtbc.ReadOnly = true;
            dgvtbc.Visible = false;
            dgDates.Columns.Add(dgvtbc);
        }

        private void SaveValues()
        {
            SqlConnection conn = null;
            try
            {
                Cursor = Cursors.WaitCursor;

                DBHelper myHelper = new DBHelper();
                conn = new SqlConnection(myHelper.OmdbConn);
                conn.Open();

                var dataAdapter = new SqlDataAdapter
                                      {
                                          SelectCommand = new SqlCommand {Connection = conn},
                                          InsertCommand = new SqlCommand {Connection = conn},
                                          UpdateCommand = new SqlCommand {Connection = conn}
                                      };

                // Setup Select Command
                dataAdapter.SelectCommand.CommandText = "SELECT * FROM REPLENISHMENT WHERE RX_ID = " + RxId;

                // Setup Update Command
                var intDoNotSend = 0;
                if (cbDoNotSend.Checked)
                {
                    intDoNotSend = 1;
                }

                // Setup Insert Command
                dataAdapter.InsertCommand.CommandText = "INSERT INTO REPLENISHMENT(RX_ID,NOTES,CATEGORY,AUTOMAIL,DONOTSEND,INITIALDISPENSINGDATE) VALUES(@RX_ID,@NOTES,@CATEGORY,@AUTOMAIL,@DONOTSEND,@INITIALDISPENSINGDATE)";
                dataAdapter.InsertCommand.Parameters.Add("@RX_ID", SqlDbType.Int).Value = RxId;
                dataAdapter.InsertCommand.Parameters.Add("@NOTES", SqlDbType.VarChar).Value = txtRxNotes.Text;
                dataAdapter.InsertCommand.Parameters.Add("@CATEGORY", SqlDbType.VarChar).Value = cbReplenishmentNoticeCategory.Text;
                dataAdapter.InsertCommand.Parameters.Add("@AUTOMAIL", SqlDbType.VarChar).Value = cmbAutomail.Text;
                dataAdapter.InsertCommand.Parameters.Add("@DONOTSEND", SqlDbType.Int).Value = intDoNotSend;

                DateTime dtInitialDispensingDate;
                if (DateTime.TryParse(txtInitialDispensingDate.Text, out dtInitialDispensingDate))
                {
                    dataAdapter.InsertCommand.Parameters.Add("@INITIALDISPENSINGDATE", SqlDbType.DateTime).Value =
                        DateTime.Parse(txtInitialDispensingDate.Text);
                }
                else
                {
                    dataAdapter.InsertCommand.Parameters.Add("@INITIALDISPENSINGDATE", SqlDbType.DateTime).Value =
                        DBNull.Value;
                }

                if (DateTime.TryParse(txtInitialDispensingDate.Text, out dtInitialDispensingDate))
                {
                    dataAdapter.UpdateCommand.CommandText = "UPDATE REPLENISHMENT SET NOTES = '" + txtRxNotes.Text +
                                                            "', CATEGORY = '" + cbReplenishmentNoticeCategory.Text +
                                                            "', AUTOMAIL = '" + cmbAutomail.Text + "', DONOTSEND = " +
                                                            intDoNotSend + ", INITIALDISPENSINGDATE = '" +
                                                            DateTime.Parse(txtInitialDispensingDate.Text) +
                                                            "' WHERE RX_ID = @RX_ID";
                }
                else
                {
                    dataAdapter.UpdateCommand.CommandText = "UPDATE REPLENISHMENT SET NOTES = '" + txtRxNotes.Text +
                                                            "', CATEGORY = '" + cbReplenishmentNoticeCategory.Text +
                                                            "', AUTOMAIL = '" + cmbAutomail.Text + "', DONOTSEND = " +
                                                            intDoNotSend + " WHERE RX_ID = @RX_ID";                    
                }

                dataAdapter.UpdateCommand.Parameters.Add("@RX_ID", SqlDbType.Int).Value = RxId;

                // Populate a new data table and bind it to the BindingSource.
                var table = new DataTable
                                {
                                    Locale = System.Globalization.CultureInfo.InvariantCulture
                                };
                dataAdapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    dataAdapter.UpdateCommand.ExecuteNonQuery();
                }
                else
                {
                    dataAdapter.InsertCommand.ExecuteNonQuery();
                }

                Cursor = Cursors.Arrow;

            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not save Replenishment Values!");
                //throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
           
        }

        private void SaveComplianceDates()
        {
            SqlConnection conn = null;
            try
            {
                Cursor = Cursors.WaitCursor;

                DBHelper myHelper = new DBHelper();
                conn = new SqlConnection(myHelper.OmdbConn);
                conn.Open();

           
                Cursor = Cursors.Arrow;

            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not save Replenishment Values!");
                //throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        private void ClearValues()
        {
            txtRxNotes.Text = "";
            cbReplenishmentNoticeCategory.Text = "";
            cmbAutomail.Text = "";
            cbDoNotSend.Checked = false;                        
            txtInitialDispensingDate.Text = "";
            cbReplenishmentNoticeCategory.SelectedIndex = -1;
            cmbAutomail.SelectedIndex = -1;
        }

        private void LoadValues()
        {
            SqlConnection conn = null;
            try
            {
                DBHelper myHelper = new DBHelper();
                conn = new SqlConnection(myHelper.OmdbConn);
                conn.Open();

                var dataAdapter = new SqlDataAdapter
                                      {
                                          SelectCommand = new SqlCommand
                                                              {
                                                                  Connection = conn,
                                                                  CommandText =
                                                                      "SELECT * FROM REPLENISHMENT WHERE RX_ID = " +
                                                                      RxId
                                                              }
                                      };

                // Setup Select Command

                // Populate a new data table and bind it to the BindingSource.
                var table = new DataTable
                                {
                                    Locale = System.Globalization.CultureInfo.InvariantCulture
                                };
                dataAdapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    txtRxNotes.Text = table.Rows[0]["NOTES"].ToString();
                    cbReplenishmentNoticeCategory.Text = table.Rows[0]["CATEGORY"].ToString();
                    cmbAutomail.Text = table.Rows[0]["AUTOMAIL"].ToString();
                    if (table.Rows[0]["DONOTSEND"].ToString() == "1")
                    {
                        cbDoNotSend.Checked = true;
                    }
                    DateTime dtInitialDispensingDate;
                    if (DateTime.TryParse(table.Rows[0]["INITIALDISPENSINGDATE"].ToString(), out dtInitialDispensingDate))
                    {
                        txtInitialDispensingDate.Text =
                            DateTime.Parse(table.Rows[0]["INITIALDISPENSINGDATE"].ToString()).ToShortDateString();
                    }
                }

            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not load values!");
                //throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        private void txtInitialDispensingDate_GotFocus(object sender, EventArgs e)
        {
            if (txtInitialDispensingDate.Text.Length == 0)
            {
                txtInitialDispensingDate.Text = DateTime.Now.ToShortDateString();
            }
        }

        private void txtInitialDispensingDate_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                DateTime dt;
                if(!DateTime.TryParse(txtInitialDispensingDate.Text, out dt))
                {
                    e.Cancel = true;
                    MessageBox.Show("The initial dispensing date must be a date!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private void btAddReplenishmentDates_Click(object sender, EventArgs e)
        {
            try
            {

                if (cbReplenishmentNoticeCategory.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a replenishment notice category!", "Missing Values");
                    return;                    
                }

                if (txtInitialDispensingDate.Text.Length == 0)
                {
                    MessageBox.Show("Please select an initial dispensing date!", "Missing Values");
                    return;
                }

                if (txtInitialDispensingDate.Text.Length > 0)
                {
                    DateTime dtInitialDispensingDate;
                    if (!DateTime.TryParse(txtInitialDispensingDate.Text, out dtInitialDispensingDate))
                    {
                        MessageBox.Show(
                            "You must enter a valid Initial Dispensing Date in the format: XX/XX/XXXX!",
                            "Error");
                        return;
                    }
                }

                // Add Replenishment Dates
                AddReplenishmentDates();

                // Load Replenishment Dates
                RefreshReplenishmentSchedule();
            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not add Replenishment Dates!");
                //throw new Exception(ex.Message, ex);
            }
        }

        private void AddReplenishmentDates()
        {
            try
            {
                DateTime dtInitialDispensingDate = DateTime.Parse(txtInitialDispensingDate.Text);
                switch (cbReplenishmentNoticeCategory.Text)
                {
                    case "12 Weeks":
                        dtInitialDispensingDate = dtInitialDispensingDate.AddDays(84);
                        for (int i = 0; i < 7; i++)
                        {
                            AddReplenishmentScheduleDate(dtInitialDispensingDate);
                            dtInitialDispensingDate = dtInitialDispensingDate.AddDays(84);
                        }
                        break;
                    case "24 Weeks":
                        dtInitialDispensingDate = dtInitialDispensingDate.AddDays(168);
                        for (int i = 0; i < 7; i++)
                        {
                            AddReplenishmentScheduleDate(dtInitialDispensingDate);
                            dtInitialDispensingDate = dtInitialDispensingDate.AddDays(168);
                        }
                        break;
                    case "1 Month":
                        dtInitialDispensingDate = dtInitialDispensingDate.AddMonths(1);
                        for (int i = 0; i < 7; i++)
                        {
                            AddReplenishmentScheduleDate(dtInitialDispensingDate);
                            dtInitialDispensingDate = dtInitialDispensingDate.AddMonths(1);
                        }
                        break;
                    case "3 Months":
                        dtInitialDispensingDate = dtInitialDispensingDate.AddMonths(3);
                        for (int i = 0; i < 7; i++)
                        {
                            AddReplenishmentScheduleDate(dtInitialDispensingDate);
                            dtInitialDispensingDate = dtInitialDispensingDate.AddMonths(3);
                        }
                        break;
                    case "6 Months":
                        dtInitialDispensingDate = dtInitialDispensingDate.AddMonths(6);
                        for (int i = 0; i < 7; i++)
                        {
                            AddReplenishmentScheduleDate(dtInitialDispensingDate);
                            dtInitialDispensingDate = dtInitialDispensingDate.AddMonths(6);
                        }
                        break;
                    case "1 Year":
                        dtInitialDispensingDate = dtInitialDispensingDate.AddYears(1);
                        for (int i = 0; i < 7; i++)
                        {
                            AddReplenishmentScheduleDate(dtInitialDispensingDate);
                            dtInitialDispensingDate = dtInitialDispensingDate.AddYears(1);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not add Replenishment Dates!");
                //throw new Exception(ex.Message, ex);
            }

        }

        private void AddReplenishmentDates(DateTime dtInitialDispensingDate)
        {
            try
            {
                switch (cbReplenishmentNoticeCategory.Text)
                {
                    case "12 Weeks":
                        dtInitialDispensingDate = dtInitialDispensingDate.AddDays(84);
                        for (int i = 0; i < 7; i++)
                        {
                            AddReplenishmentScheduleDate(dtInitialDispensingDate);
                            dtInitialDispensingDate = dtInitialDispensingDate.AddDays(84);
                        }
                        break;
                    case "24 Weeks":
                        dtInitialDispensingDate = dtInitialDispensingDate.AddDays(168);
                        for (int i = 0; i < 7; i++)
                        {
                            AddReplenishmentScheduleDate(dtInitialDispensingDate);
                            dtInitialDispensingDate = dtInitialDispensingDate.AddDays(168);
                        }
                        break;
                    case "1 Month":
                        dtInitialDispensingDate = dtInitialDispensingDate.AddMonths(1);
                        for (int i = 0; i < 7; i++)
                        {
                            AddReplenishmentScheduleDate(dtInitialDispensingDate);
                            dtInitialDispensingDate = dtInitialDispensingDate.AddMonths(1);
                        }
                        break;
                    case "3 Months":
                        dtInitialDispensingDate = dtInitialDispensingDate.AddMonths(3);
                        for (int i = 0; i < 7; i++)
                        {
                            AddReplenishmentScheduleDate(dtInitialDispensingDate);
                            dtInitialDispensingDate = dtInitialDispensingDate.AddMonths(3);
                        }
                        break;
                    case "6 Months":
                        dtInitialDispensingDate = dtInitialDispensingDate.AddMonths(6);
                        for (int i = 0; i < 7; i++)
                        {
                            AddReplenishmentScheduleDate(dtInitialDispensingDate);
                            dtInitialDispensingDate = dtInitialDispensingDate.AddMonths(6);
                        }
                        break;
                    case "1 Year":
                        dtInitialDispensingDate = dtInitialDispensingDate.AddYears(1);
                        for (int i = 0; i < 7; i++)
                        {
                            AddReplenishmentScheduleDate(dtInitialDispensingDate);
                            dtInitialDispensingDate = dtInitialDispensingDate.AddYears(1);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not add Replenishment Dates!");
                //throw new Exception(ex.Message, ex);
            }

        }

        private void AddReplenishmentScheduleDate(DateTime dtReplenishmentDate)
        {
            SqlConnection conn = null;
            try
            {
                DBHelper myHelper = new DBHelper();
                conn = new SqlConnection(myHelper.OmdbConn);
                conn.Open();

                var dataAdapter = new SqlDataAdapter
                {
                    InsertCommand = new SqlCommand
                    {
                        Connection = conn,
                        CommandText =
                            string.Format(
                            "INSERT INTO REPLENISHMENTSCHEDULE(REPLENISHMENTDATE, RX_ID) VALUES ('{0}', {1})",
                            dtReplenishmentDate.ToShortDateString(),
                            RxId)
                    }
                };

                // Setup Delete Command
                dataAdapter.InsertCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
            //    var logger = new ExceptionLogger();
            //    logger.AddLogger(new EventLogLogger());
            //    logger.LogException(ex, "Could not delete Replenishment Schedule!");
                //throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        private void UpdateComplianceDate(DateTime? dtComplianceDate, int ID)
        {
            SqlConnection conn = null;
            try
            {
                DBHelper myHelper = new DBHelper();
                conn = new SqlConnection(myHelper.OmdbConn);
                conn.Open();

                var dataAdapter = new SqlDataAdapter();
                SqlCommand UpdateCommand = new SqlCommand();
                dataAdapter.UpdateCommand = UpdateCommand;
                UpdateCommand.Connection = conn;

                if (dtComplianceDate == null)
                {
                    UpdateCommand.CommandText = "UPDATE REPLENISHMENTSCHEDULE SET COMPLIANCEDATE = NULL, DELQT = NULL WHERE ID = " + ID;
                }
                else
                {
                    UpdateCommand.CommandText =  
                            string.Format(
                            "UPDATE REPLENISHMENTSCHEDULE SET COMPLIANCEDATE = '{0}' WHERE ID = {1}",
                            dtComplianceDate,
                            ID);
                    
                }

                // Setup Delete Command
                dataAdapter.UpdateCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not update Replenishment Schedule!");
                //throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        private void UpdateComplianceDate(DateTime dtComplianceDate, int iDelqt, int ID)
        {
            SqlConnection conn = null;
            try
            {
                DBHelper myHelper = new DBHelper();
                conn = new SqlConnection(myHelper.OmdbConn);
                conn.Open();

                var dataAdapter = new SqlDataAdapter
                {
                    UpdateCommand = new SqlCommand
                    {
                        Connection = conn,
                        CommandText =
                            string.Format(
                            "UPDATE REPLENISHMENTSCHEDULE SET COMPLIANCEDATE = '{0}', DELQT = {1} WHERE ID = {2}",
                            dtComplianceDate.ToShortDateString(),
                            iDelqt,
                            ID)
                    }
                };

                // Setup Delete Command
                dataAdapter.UpdateCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not update Replenishment Schedule!");
                //throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        private void InsertComplianceDate(DateTime dtReplenishmentDate, DateTime dtComplianceDate, int ID)
        {
            SqlConnection conn = null;
            try
            {
                DBHelper myHelper = new DBHelper();
                conn = new SqlConnection(myHelper.OmdbConn);
                conn.Open();

                var dataAdapter = new SqlDataAdapter
                {
                    UpdateCommand = new SqlCommand
                    {
                        Connection = conn,
                        CommandText =
                            string.Format(
                            "INSERT INTO REPLENISHMENTSCHEDULE(RX_ID, REPLENISHMENTDATE, COMPLIANCEDATE) VALUES ({0}, '{1}', '{2}')",
                            ID,
                            dtReplenishmentDate.ToShortDateString(),
                            dtComplianceDate.ToShortDateString()
                            )
                    }
                };

                // Setup Delete Command
                dataAdapter.UpdateCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not update Replenishment Schedule!");
                //throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        private void InsertReplenishmentDate(DateTime dtReplenishmentDate, int ID)
        {
            SqlConnection conn = null;
            try
            {
                DBHelper myHelper = new DBHelper();
                conn = new SqlConnection(myHelper.OmdbConn);
                conn.Open();

                var dataAdapter = new SqlDataAdapter
                {
                    UpdateCommand = new SqlCommand
                    {
                        Connection = conn,
                        CommandText =
                            string.Format(
                            "INSERT INTO REPLENISHMENTSCHEDULE(RX_ID, REPLENISHMENTDATE) VALUES ({0}, '{1}')",
                            ID,
                            dtReplenishmentDate.ToShortDateString()
                            )
                    }
                };

                // Setup Delete Command
                dataAdapter.UpdateCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not update Replenishment Schedule!");
                //throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        private void UpdateReplenishmentDate(DateTime? dtReplenishmentDate, int ID)
        {
            SqlConnection conn = null;
            try
            {
                DBHelper myHelper = new DBHelper();
                conn = new SqlConnection(myHelper.OmdbConn);
                conn.Open();

                var dataAdapter = new SqlDataAdapter();
                SqlCommand UpdateCommand = new SqlCommand();
                dataAdapter.UpdateCommand = UpdateCommand;
                UpdateCommand.Connection = conn;

                if (dtReplenishmentDate == null)
                {
                    UpdateCommand.CommandText = "DELETE FROM REPLENISHMENTSCHEDULE WHERE ID = " + ID;
                }
                else
                {
                    UpdateCommand.CommandText =
                            string.Format(
                            "UPDATE REPLENISHMENTSCHEDULE SET REPLENISHMENTDATE = '{0}' WHERE ID = {1}",
                            dtReplenishmentDate,
                            ID);

                }

                // Setup Delete Command
                dataAdapter.UpdateCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not update Replenishment Schedule!");
                //throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        private void BtDeleteReplenishmentDatesClick(object sender, EventArgs e)
        {
            if (dgDates.Rows.Count == 1)
            {
                if (dgDates[0,0].Value == null)
                {
                    MessageBox.Show("There are no dates to delete!", "Error");
                    return;
                }
            }

            DialogResult dlgRes = MessageBox.Show(
                "Are you sure you want to delete the Replenishment Schedule?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (dlgRes == DialogResult.Yes)
            {
                DeleteReplenishmentSchedule();
                return;
            }
            return;
        }        
        
        private void DeleteReplenishmentSchedule()
        {
            SqlConnection conn = null;
            try
            {
                DBHelper myHelper = new DBHelper();
                conn = new SqlConnection(myHelper.OmdbConn);
                conn.Open();

                var dataAdapter = new SqlDataAdapter
                                                   {
                                                       DeleteCommand = new SqlCommand
                                                                           {
                                                                               Connection = conn,
                                                                               CommandText =
                                                                                   string.Format(
                                                                                   "DELETE FROM REPLENISHMENTSCHEDULE WHERE RX_ID = {0}",
                                                                                   RxId)
                                                                           }
                                                   };

                // Setup Delete Command
                dataAdapter.DeleteCommand.ExecuteNonQuery();

                // Delete Replenishment Info

                dataAdapter = new SqlDataAdapter
                {
                    DeleteCommand = new SqlCommand
                    {
                        Connection = conn,
                        CommandText =
                            string.Format(
                            "DELETE FROM Replenishment WHERE RX_ID = {0}",
                            RxId)
                    }
                };

                // Setup Delete Command
                dataAdapter.DeleteCommand.ExecuteNonQuery();

                // Clear Values
                ClearValues();

                RefreshReplenishmentSchedule();

            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not delete Replenishment Schedule!");
                //throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        private static string GetPatientName(int patientId)
        {
            var patientName = "";
            SqlConnection conn = null;
            try
            {
                DBHelper myHelper = new DBHelper();
                conn = new SqlConnection(myHelper.OmdbConn);
                conn.Open();

                var dataAdapter = new SqlDataAdapter(GetPatientSql(patientId), conn);
                var commandBuilder = new SqlCommandBuilder(dataAdapter);

                // Populate a new data table and bind it to the BindingSource.
                var table = new DataTable
                                {
                                    Locale = System.Globalization.CultureInfo.InvariantCulture
                                };
                dataAdapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    patientName = table.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not get Patient Name!");
                //throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return patientName;
        }

        private static string GetPatientSql(int patientId)
        {
            var strSql = new StringBuilder();
            try
            {
                strSql.Append("SELECT patient.first_name + ' ' + patient.last_name ");
                strSql.Append("FROM patient ");
                strSql.Append("WHERE patient.patient_no =" + patientId);
            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not get Patient SQL!");
            }
            return strSql.ToString();
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

        private void GetLensData()
        {

            SqlConnection conn = null;
            var sbSql = new StringBuilder(606);
            try
            {
                DBHelper myHelper = new DBHelper();
                conn = new SqlConnection(myHelper.OmdbConn);
                conn.Open();

                sbSql.AppendFormat(@"SELECT vendor.vendor_name1, Soft_Contact_Lens_RX.lens_name, Soft_Contact_Lens_RX.base_curve_mm, ");
                sbSql.AppendFormat(@"Soft_Contact_Lens_RX.diameter_mm, Soft_Contact_Lens_RX.sphere_diopter, ");
                sbSql.AppendFormat(@"Soft_Contact_Lens_RX.cylinder_diopter, Soft_Contact_Lens_RX.axis_deg, ");
                sbSql.AppendFormat(@"Soft_Contact_Lens_RX.add_diopter, code.description, Soft_Contact_RX.Rx_Notes, Soft_Contact_Lens_RX.right_or_left_cd {0}", Environment.NewLine);
                sbSql.AppendFormat(@"FROM ((Soft_Contact_Lens_RX INNER JOIN vendor ON Soft_Contact_Lens_RX.lens_brand_cd = ");
                sbSql.AppendFormat(@"vendor.vendor_no) LEFT JOIN code ON Soft_Contact_Lens_RX.tint_cd = code.code) INNER JOIN ");
                sbSql.AppendFormat(@"Soft_Contact_RX ON Soft_Contact_Lens_RX.rx_id = Soft_Contact_RX.rx_id{0}", Environment.NewLine);
                sbSql.Append(" WHERE Soft_Contact_Lens_RX.rx_id =" + RxId);

                var dataAdapter = new SqlDataAdapter(sbSql.ToString(), conn);
                var commandBuilder = new SqlCommandBuilder(dataAdapter);

                // Populate a new data table and bind it to the BindingSource.
                var table = new DataTable
                                {
                                    Locale = System.Globalization.CultureInfo.InvariantCulture
                                };
                dataAdapter.Fill(table);

                if (table.Rows.Count == 0)
                {
                    StringBuilder sbSQLNoVendor = new StringBuilder();
                    sbSQLNoVendor.AppendFormat(@"SELECT     '' as vendor_name1, Soft_Contact_Lens_RX.lens_name, Soft_Contact_Lens_RX.base_curve_mm, ");
                    sbSQLNoVendor.AppendFormat(@"Soft_Contact_Lens_RX.diameter_mm, {0}", Environment.NewLine);
                    sbSQLNoVendor.AppendFormat(@"                      Soft_Contact_Lens_RX.sphere_diopter, Soft_Contact_Lens_RX.cylinder_diopter, ");
                    sbSQLNoVendor.AppendFormat(@"Soft_Contact_Lens_RX.axis_deg, Soft_Contact_Lens_RX.add_diopter, {0}", Environment.NewLine);
                    sbSQLNoVendor.AppendFormat(@"                      code.description, Soft_Contact_RX.Rx_Notes{0}, Soft_Contact_Lens_RX.right_or_left_cd ", Environment.NewLine);
                    sbSQLNoVendor.AppendFormat(@"FROM         Soft_Contact_Lens_RX LEFT OUTER JOIN{0}", Environment.NewLine);
                    sbSQLNoVendor.AppendFormat(@"                      code ON Soft_Contact_Lens_RX.tint_cd = code.code INNER JOIN{0}", Environment.NewLine);
                    sbSQLNoVendor.AppendFormat(@"                      Soft_Contact_RX ON Soft_Contact_Lens_RX.rx_id = Soft_Contact_RX.rx_id{0}", Environment.NewLine);
                    sbSQLNoVendor.Append(" WHERE Soft_Contact_Lens_RX.rx_id =" + RxId);

                    var dataAdapterNoVendor = new SqlDataAdapter(sbSQLNoVendor.ToString(), conn);
                    var commandBuilderNoVendor = new SqlCommandBuilder(dataAdapterNoVendor);

                    // Populate a new data table and bind it to the BindingSource.
                    var tableNoVendor = new DataTable
                    {
                        Locale = System.Globalization.CultureInfo.InvariantCulture
                    };
                    dataAdapterNoVendor.Fill(tableNoVendor);

                    if (tableNoVendor.Rows.Count > 0)
                    {
                        txtRLensType.Text = tableNoVendor.Rows[0]["vendor_name1"].ToString();
                        txtRSeries.Text = tableNoVendor.Rows[0]["lens_name"].ToString();
                        txtRBC.Text = tableNoVendor.Rows[0]["base_curve_mm"].ToString();
                        txtRDia.Text = tableNoVendor.Rows[0]["diameter_mm"].ToString();
                        txtRSphere.Text = tableNoVendor.Rows[0]["sphere_diopter"].ToString();
                        txtRCyl.Text = tableNoVendor.Rows[0]["cylinder_diopter"].ToString();
                        txtRAxis.Text = tableNoVendor.Rows[0]["axis_deg"].ToString();
                        txtRAdd.Text = tableNoVendor.Rows[0]["add_diopter"].ToString();
                        txtRTint.Text = tableNoVendor.Rows[0]["description"].ToString();
                    }

                    if (tableNoVendor.Rows.Count > 1)
                    {
                        txtLLensType.Text = tableNoVendor.Rows[1]["vendor_name1"].ToString();
                        txtLSeries.Text = tableNoVendor.Rows[1]["lens_name"].ToString();
                        txtLBC.Text = tableNoVendor.Rows[1]["base_curve_mm"].ToString();
                        txtLDia.Text = tableNoVendor.Rows[1]["diameter_mm"].ToString();
                        txtLSphere.Text = tableNoVendor.Rows[1]["sphere_diopter"].ToString();
                        txtLCyl.Text = tableNoVendor.Rows[1]["cylinder_diopter"].ToString();
                        txtLAxis.Text = tableNoVendor.Rows[1]["axis_deg"].ToString();
                        txtLAdd.Text = tableNoVendor.Rows[1]["add_diopter"].ToString();
                        txtLTint.Text = tableNoVendor.Rows[1]["description"].ToString();
                    }
                }

                // Set Eye Data
                foreach (DataRow row in table.Rows)
                {
                    SetEye(row);
                }
                
            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not get Lens Data!");
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }           
        }

        private void SetEye(DataRow row)
        {
            switch (row["right_or_left_cd"].ToString())
            {
                case "0":
                    // Set Right Eye
                    txtRLensType.Text = row["vendor_name1"].ToString();                    
                    txtRSeries.Text = row["lens_name"].ToString();                    
                    txtRBC.Text = row["base_curve_mm"].ToString();                    
                    txtRDia.Text = row["diameter_mm"].ToString();                    
                    txtRSphere.Text = row["sphere_diopter"].ToString();                   
                    txtRCyl.Text = row["cylinder_diopter"].ToString();                    
                    txtRAxis.Text = row["axis_deg"].ToString();                    
                    txtRAdd.Text = row["add_diopter"].ToString();                    
                    txtRTint.Text = row["description"].ToString();  
                    break;
                case "1":
                    // Set Left Eye
                    txtLLensType.Text = row["vendor_name1"].ToString();
                    txtLSeries.Text = row["lens_name"].ToString();
                    txtLBC.Text = row["base_curve_mm"].ToString();
                    txtLDia.Text = row["diameter_mm"].ToString();
                    txtLSphere.Text = row["sphere_diopter"].ToString();
                    txtLCyl.Text = row["cylinder_diopter"].ToString();
                    txtLAxis.Text = row["axis_deg"].ToString();
                    txtLAdd.Text = row["add_diopter"].ToString();
                    txtLTint.Text = row["description"].ToString();
                    break;
            }
        }

        private void btBack_Click(object sender, EventArgs e)
        {
            // Go Back
            GoToFindPatient();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
            Environment.Exit(0);
            this.Dispose();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (cbReplenishmentNoticeCategory.SelectedIndex > -1)
            {
                if (txtInitialDispensingDate.Text.Length == 0)
                {
                    MessageBox.Show(
                        "You must enter an Initial Dispensing Date if you have selected a Replenishment Category!",
                        "Error");
                    return;
                }
            }

            if (txtInitialDispensingDate.Text.Length > 0)
            {
                DateTime dtInitialDispensingDate;
                if (!DateTime.TryParse(txtInitialDispensingDate.Text, out dtInitialDispensingDate))
                {
                    MessageBox.Show(
                        "You must enter a valid Initial Dispensing Date in the format: XX/XX/XXXX!",
                        "Error");
                    return;
                }
            }

            // Save Values
            SaveValues();

            // Go To Menu
            GoToFindPatient();
        }

        private void dgDates_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if (e.RowIndex > -1)
                {
                        // Set Compliance Date
                        if (dgDates[e.ColumnIndex, e.RowIndex].Value.ToString().Length == 0)
                        {
                            dgDates[e.ColumnIndex, e.RowIndex].Value = DateTime.Now.ToShortDateString();
                        }

                        // Set Replenishment Date
                        if (dgDates[0, e.RowIndex].Value.ToString().Length == 0)
                        {
                            DateTime dtReplenishment;
                            if (e.RowIndex > 0)
                            {
                                if (DateTime.TryParse(dgDates[e.ColumnIndex - 1, e.RowIndex - 1].Value.ToString(), out dtReplenishment))
                                {
                                    DateTime dtNextDate = GetNextReplenishmentDate(dtReplenishment);
                                    dgDates[e.ColumnIndex - 1, e.RowIndex].Value = dtNextDate.ToShortDateString();
                                }
                            }
                        }
                }
            }
            if (e.ColumnIndex == 0)
            {
                if (dgDates[e.ColumnIndex, e.RowIndex].Value.ToString().Length == 0)
                {
                    dgDates[e.ColumnIndex, e.RowIndex].Value = DateTime.Now.ToShortDateString();
                }
            }
        }

        private bool CheckDate(DateTime Date)
        {
            bool value = true;
            try
            {
                if (Date < new DateTime(1900, 1, 1))
                {
                    value = false;
                    MessageBox.Show("Date is invalid!");
                }
            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not edit Replenishment Schedule Dates manually!");
                //throw new Exception(ex.Message, ex);
            }
            return value;
        }

        private void dgDates_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Clear the row error in case the user presses ESC.   
            dgDates.Rows[e.RowIndex].ErrorText = String.Empty;
            try
            {

                if (e.ColumnIndex == 0)
                {
                    DateTime dateTime;

                    // Update Replenishment Date
                    if (DateTime.TryParse(dgDates[0, e.RowIndex].Value.ToString(), out dateTime) && dgDates[1, e.RowIndex].Value.ToString().Length == 0)
                    {
                        DateTime dtReplenishment = DateTime.Parse(dgDates[0, e.RowIndex].Value.ToString());
                        if (dgDates[3, e.RowIndex].Value.ToString().Length == 0)
                        {
                            if (dgDates[3, e.RowIndex - 1].Value.ToString().Length > 0)
                            {
                                InsertReplenishmentDate(dtReplenishment, RxId);
                            }
                        }
                        else
                        {
                            int ID = int.Parse(dgDates[3, e.RowIndex].Value.ToString());
                            UpdateReplenishmentDate(dtReplenishment, ID);
                        }
                    }
                    else
                    {
                        if (dgDates[0, e.RowIndex].Value.ToString() == "  /  /")
                        {
                            if (dgDates[3, e.RowIndex].Value.ToString().Length > 0)
                            {
                                dgDates.Rows[e.RowIndex].Cells[0].Value = String.Empty;
                                int ID = int.Parse(dgDates[3, e.RowIndex].Value.ToString());

                                UpdateReplenishmentDate(null, ID);
                                LoadReplenishmentSchedule();
                                //DataGridViewRow row = dgDates.SelectedRows[e.RowIndex];
                                //dgDates.Rows.Remove(dgDates.CurrentRow);
                            }
                        }
                    }
                }
                if (e.ColumnIndex == 1)
                {
                    DateTime dateTime;

                    // Update Compliance Date
                    if (DateTime.TryParse(dgDates[0, e.RowIndex].Value.ToString(), out dateTime) && DateTime.TryParse(dgDates[1, e.RowIndex].Value.ToString(), out dateTime))
                    {
                        // Set Delqt Date
                        DateTime dtReplenishment = DateTime.Parse(dgDates[0, e.RowIndex].Value.ToString());
                        DateTime dtCompliance = DateTime.Parse(dgDates[1, e.RowIndex].Value.ToString());

                        // Check Date
                        //if (!CheckDate(dtReplenishment))
                        //{
                        //    dgDates[0, e.RowIndex].Value = "";
                        //    dgDates.Rows[e.RowIndex].ErrorText = "Date is invalid!";
                        //    return;
                        //}

                        //if (!CheckDate(dtCompliance))
                        //{
                        //    dgDates[1, e.RowIndex].Value = "";
                        //    dgDates.Rows[e.RowIndex].ErrorText = "Date is invalid!";
                        //    return;
                        //}

                        int ID;

                        // Insert Compliance Date
                        if (dgDates[3, e.RowIndex].Value.ToString().Length == 0)
                        {
                            InsertComplianceDate(dtReplenishment, dtCompliance, RxId);
                            LoadReplenishmentSchedule();
                            ID = int.Parse(dgDates[3, e.RowIndex].Value.ToString());
                        }
                        else
                        {
                            ID = int.Parse(dgDates[3, e.RowIndex].Value.ToString());
                        }

                        // Do not update negative dates
                        if (dtCompliance.Subtract(dtReplenishment).Days >= 0)
                        {
                            dgDates[2, e.RowIndex].Value = dtCompliance.Subtract(dtReplenishment).Days.ToString();
                            dgDates.Rows[e.RowIndex].Cells[2].Value = dtCompliance.Subtract(dtReplenishment).Days.ToString();
                            UpdateComplianceDate(dtCompliance, dtCompliance.Subtract(dtReplenishment).Days, ID);
                        }

                        // Update Compliance Date
                        if (CheckDate(dtCompliance))
                        {
                            if (dtCompliance.Subtract(dtReplenishment).Days < 0)
                            {
                                UpdateComplianceDate(dtCompliance, ID);
                            }
                        }

                        // Update Replenishment Schedule
                        UpdateReplenishmentSchedule(dtCompliance, e.RowIndex);

                    }
                    if (dgDates[1, e.RowIndex].Value.ToString().Length == 0)
                    {
                        dgDates[2, e.RowIndex].Value = DBNull.Value;
                    }

                    if (dgDates.Rows[e.RowIndex].Cells[1].Value.ToString() == "  /  /")
                    {
                        //// Set Compliance Date to Null
                        //dgDates.Rows[e.RowIndex].Cells[1].Value = String.Empty;
                        //// Set Delqt Date to Null
                        //dgDates.Rows[e.RowIndex].Cells[2].Value = String.Empty;
                        //int ID = int.Parse(dgDates[3, e.RowIndex].Value.ToString());
                        //UpdateComplianceDate(null, ID);

                    

                        // Update Replenishment Date
                        if (DateTime.TryParse(dgDates[0, e.RowIndex].Value.ToString(), out dateTime) && dgDates[1, e.RowIndex].Value.ToString().Length == 0)
                        {
                            DateTime dtReplenishment = DateTime.Parse(dgDates[0, e.RowIndex].Value.ToString());
                            if (dgDates[3, e.RowIndex].Value.ToString().Length == 0)
                            {
                                if (dgDates[3, e.RowIndex - 1].Value.ToString().Length > 0)
                                {
                                    InsertReplenishmentDate(dtReplenishment, RxId);
                                }
                            }
                            else
                            {
                                int ID = int.Parse(dgDates[3, e.RowIndex].Value.ToString());
                                UpdateReplenishmentDate(dtReplenishment, ID);
                            }
                        }
                        else
                        {
                            if (dgDates[0, e.RowIndex].Value.ToString() == "  /  /")
                            {
                                if (dgDates[3, e.RowIndex].Value.ToString().Length > 0)
                                {
                                    dgDates.Rows[e.RowIndex].Cells[0].Value = String.Empty;
                                    int ID = int.Parse(dgDates[3, e.RowIndex].Value.ToString());

                                    UpdateReplenishmentDate(null, ID);
                                    LoadReplenishmentSchedule();
                                    //DataGridViewRow row = dgDates.SelectedRows[e.RowIndex];
                                    //dgDates.Rows.Remove(dgDates.CurrentRow);
                                }
                            }
                            else
                            {
                                if (dgDates[3, e.RowIndex].Value.ToString().Length > 0)
                                {                                    
                                    if (dgDates.Rows[e.RowIndex].Cells[0].Value.ToString().Length > 0)
                                    {
                                        int ID = int.Parse(dgDates[3, e.RowIndex].Value.ToString());
                                        DateTime dtReplenishment = DateTime.Parse(dgDates[0, e.RowIndex].Value.ToString());
                                        UpdateReplenishmentDate(dtReplenishment, ID);
                                        UpdateComplianceDate(null, ID);
                                        LoadReplenishmentSchedule();
                                    }
                                    else
                                    {
                                        dgDates.Rows[e.RowIndex].Cells[0].Value = String.Empty;
                                        int ID = int.Parse(dgDates[3, e.RowIndex].Value.ToString());

                                        UpdateReplenishmentDate(null, ID);
                                        LoadReplenishmentSchedule();
                                    }
                                    //DataGridViewRow row = dgDates.SelectedRows[e.RowIndex];
                                    //dgDates.Rows.Remove(dgDates.CurrentRow);
                                }
                                else
                                {
                                    DateTime dtReplenishment = DateTime.Parse(dgDates[0, e.RowIndex].Value.ToString());
                                    if (dgDates[3, e.RowIndex].Value.ToString().Length == 0)
                                    {
                                        if (dgDates[3, e.RowIndex - 1].Value.ToString().Length > 0)
                                        {
                                            InsertReplenishmentDate(dtReplenishment, RxId);
                                        }
                                    }
                                    else
                                    {
                                        int ID = int.Parse(dgDates[3, e.RowIndex].Value.ToString());
                                        UpdateReplenishmentDate(dtReplenishment, ID);
                                    }
                                }
                            }
                        }
                    }
                }

                // If last record after adding compliance date add more replenishment dates
                if (e.ColumnIndex == 1)
                {
                    if (dgDates.RowCount == e.RowIndex + 2)
                    {
                        DateTime dtReplenishment = DateTime.Parse(dgDates[0, e.RowIndex].Value.ToString());
                        // Add Replenishment Dates
                        AddReplenishmentDates(dtReplenishment);
                    }
                }

                LoadReplenishmentSchedule();
            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not edit Replenishment Schedule Dates manually!");
                //throw new Exception(ex.Message, ex);
            }
        }

        private DateTime GetNextReplenishmentDate(DateTime dtReplenishment)
        {            
            DateTime dtNewDate = DateTime.Now;
            switch (cbReplenishmentNoticeCategory.Text)
            {
                case "12 Weeks":
                    dtNewDate = dtReplenishment.AddDays(84);
                    break;
                case "24 Weeks":
                    dtNewDate = dtReplenishment.AddDays(168);
                    break;
                case "1 Month":
                    dtNewDate = dtReplenishment.AddMonths(1);
                    break;
                case "3 Months":
                    dtNewDate = dtReplenishment.AddMonths(3);
                    break;
                case "6 Months":
                    dtNewDate = dtReplenishment.AddMonths(6);
                    break;
                case "1 Year":
                    dtNewDate = dtReplenishment.AddYears(1);
                    break;
            }
            return dtNewDate; 
        }

        private void UpdateReplenishmentSchedule(DateTime dtCompliance, int rowIndex)
        {
            Cursor = Cursors.WaitCursor;
            DateTime dtNewDate;
            var iRowCount = dgDates.Rows.Count - rowIndex;
            if (cbReplenishmentNoticeCategory.Text.Length <= 0) return;
            switch (cbReplenishmentNoticeCategory.Text)
            {
                case "12 Weeks":
                    dtNewDate = dtCompliance.AddDays(84);
                    for (var i = 1; i < iRowCount - 1; i++)
                    {
                        dgDates[0, rowIndex + i].Value = dtNewDate.ToShortDateString();
                        UpdateReplenishmentDate(dtNewDate, int.Parse(dgDates[3, rowIndex + i].Value.ToString()));
                        dtNewDate = dtNewDate.AddDays(84);
                    }
                    break;
                case "24 Weeks":
                    dtNewDate = dtCompliance.AddDays(168);
                    for (var i = 1; i < iRowCount - 1; i++)
                    {
                        dgDates[0, rowIndex + i].Value = dtNewDate.ToShortDateString();
                        UpdateReplenishmentDate(dtNewDate, int.Parse(dgDates[3, rowIndex + i].Value.ToString()));
                        dtNewDate = dtNewDate.AddDays(168);
                    }
                    break;
                case "1 Month":
                    dtNewDate = dtCompliance.AddMonths(1);
                    for (var i = 1; i < iRowCount - 1; i++)
                    {
                        dgDates[0, rowIndex + i].Value = dtNewDate.ToShortDateString();
                        UpdateReplenishmentDate(dtNewDate, int.Parse(dgDates[3, rowIndex + i].Value.ToString()));
                        dtNewDate = dtNewDate.AddMonths(1);
                    }
                    break;
                case "3 Months":
                    dtNewDate = dtCompliance.AddMonths(3);
                    for (var i = 1; i < iRowCount - 1; i++)
                    {
                        dgDates[0, rowIndex + i].Value = dtNewDate.ToShortDateString();
                        UpdateReplenishmentDate(dtNewDate, int.Parse(dgDates[3, rowIndex + i].Value.ToString()));
                        dtNewDate = dtNewDate.AddMonths(3);
                    }
                    break;
                case "6 Months":
                    dtNewDate = dtCompliance.AddMonths(6);
                    for (var i = 1; i < iRowCount - 1; i++)
                    {
                        dgDates[0, rowIndex + i].Value = dtNewDate.ToShortDateString();
                        UpdateReplenishmentDate(dtNewDate, int.Parse(dgDates[3, rowIndex + i].Value.ToString()));
                        dtNewDate = dtNewDate.AddMonths(6);
                    }
                    break;
                case "1 Year":
                    dtNewDate = dtCompliance.AddYears(1);
                    for (var i = 1; i < iRowCount - 1; i++)
                    {
                        dgDates[0, rowIndex + i].Value = dtNewDate.ToShortDateString();
                        UpdateReplenishmentDate(dtNewDate, int.Parse(dgDates[3, rowIndex + i].Value.ToString()));
                        dtNewDate = dtNewDate.AddYears(1);
                    }
                    break;
            }
            Cursor = Cursors.Arrow;
        }

        //private void dgDates_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        //{
        //    DateTime dateTime;
        //    if (e.ColumnIndex == 0)
        //    {
        //        if (e.FormattedValue.ToString().Length > 0)
        //        {
        //            if (!DateTime.TryParse(e.FormattedValue.ToString(), out dateTime))
        //            {
        //                dgDates.Rows[e.RowIndex].ErrorText = "Replenishment Date must be a date!";
        //                e.Cancel = true;
        //            }
        //        }
        //    }
        //    if (e.ColumnIndex == 1)
        //    {
        //        if (e.FormattedValue.ToString().Length > 0)
        //        {
        //            if (e.FormattedValue.ToString() == "  /  /")
        //            {
        //                //DateTime dtEmtpy = new DateTime(1000,1,1);
        //                dgDates[e.ColumnIndex, e.RowIndex].Value = DateTime.Now.ToShortDateString();
        //                e.Cancel = false;
        //                return;
        //            }
        //            //if (!DateTime.TryParse(e.FormattedValue.ToString(), out dateTime))
        //            //{
        //            //    dgDates.Rows[e.RowIndex].ErrorText = "Compliance Date must be a date!";
        //            //    e.Cancel = true;
        //            //}
        //        }
        //    }
        //}

        private void GoToMenu()
        {
            StaticVar = this;
            Hide();
            var form2 = new FrmReplacementMenu();
            form2.Show();
        }

        private void GoToFindPatient()
        {
            StaticVar = this;
            Hide();
            this.Visible = false;
            this.IsExit = false;
            this.Close();
            Application.DoEvents();
            System.Threading.Thread.Sleep(400);
            var form2 = new FrmFindPatient();
            form2.Show();            
        }

        private void dgDates_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //if (e.Context == DataGridViewDataErrorContexts.Parsing)
            //{
            //    e.ThrowException = false;
            //}
            //DataGridView view = (DataGridView)sender;

            //view.Rows[e.RowIndex].ErrorText = "an error";
            //view.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "an error";
            //view.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
            //dgDates[e.ColumnIndex, e.RowIndex].Value = System.DBNull.Value;
            e.ThrowException = false;


        }

        private void txtInitialDispensingDate_LostFocus(object sender, EventArgs e)
        {
            try
            {

                if (cbReplenishmentNoticeCategory.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a replenishment notice category!", "Missing Values");
                    return;
                }

                if (txtInitialDispensingDate.Text.Length == 0)
                {
                    MessageBox.Show("Please select an initial dispensing date!", "Missing Values");
                    return;
                }

                if (txtInitialDispensingDate.Text.Length > 0)
                {
                    DateTime dtInitialDispensingDate;
                    if (!DateTime.TryParse(txtInitialDispensingDate.Text, out dtInitialDispensingDate))
                    {
                        MessageBox.Show(
                            "You must enter a valid Initial Dispensing Date in the format: XX/XX/XXXX!",
                            "Error");
                        return;
                    }
                }

                // Add Replenishment Dates
                AddReplenishmentDates();

                // Load Replenishment Dates
                RefreshReplenishmentSchedule();
            }
            catch (Exception ex)
            {
                //var logger = new ExceptionLogger();
                //logger.AddLogger(new EventLogLogger());
                //logger.LogException(ex, "Could not add Replenishment Dates!");
                //throw new Exception(ex.Message, ex);
            }
        }

        private void cbReplenishmentNoticeCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update Replenishment Schedule
            //UpdateReplenishmentSchedule(dtCompliance, e.RowIndex);

            int rowNum = 0;
            DateTime dtCompliance = DateTime.Now;
            try
            {
                if (dgDates.Rows.Count > 0)
                {
                    // Find the last Valid Compliance Date and Row Number
                    foreach (DataGridViewRow row in dgDates.Rows)
                    {
                        if (row.Cells[1] != null)
                        {
                            if (row.Cells[1].Value != null)
                            {
                                if (row.Cells[1].Value.ToString().Length > 0)
                                {
                                    dtCompliance = DateTime.Parse(row.Cells[1].Value.ToString());
                                    rowNum = row.Index;
                                }
                            }
                        }
                    }

                    // Update all replenishment dates after all compliant dates
                    foreach (DataGridViewRow row in dgDates.Rows)
                    {
                        if (row.Cells[0] != null)
                        {
                            if (row.Index > rowNum)
                            {
                                if (row.Cells[0].Value != null)
                                {
                                    if (row.Cells[0].Value.ToString().Length > 0)
                                    {
                                        ChangeReplenishmentDateCategory(dtCompliance, row.Index-1, dgDates.Rows.Count - row.Index +1);
                                        return;
                                    }
                                }
                            }
                        }
                    }


                }
            }
            catch (Exception ex) { }            

        }

        private void ChangeReplenishmentDateCategory(DateTime dtCompliance, int rowIndex, int iRowCount)
        {
            DateTime dtNewDate = DateTime.Now;
            try
            {
                if (cbReplenishmentNoticeCategory.Text.Length <= 0) return;
                switch (cbReplenishmentNoticeCategory.Text)
                {
                    case "12 Weeks":
                        dtNewDate = dtCompliance.AddDays(84);
                        for (var i = 1; i < iRowCount - 1; i++)
                        {
                            dgDates[0, rowIndex + i].Value = dtNewDate.ToShortDateString();
                            UpdateReplenishmentDate(dtNewDate, int.Parse(dgDates[3, rowIndex + i].Value.ToString()));
                            dtNewDate = dtNewDate.AddDays(84);
                        }
                        break;
                    case "24 Weeks":
                        dtNewDate = dtCompliance.AddDays(168);
                        for (var i = 1; i < iRowCount - 1; i++)
                        {
                            dgDates[0, rowIndex + i].Value = dtNewDate.ToShortDateString();
                            UpdateReplenishmentDate(dtNewDate, int.Parse(dgDates[3, rowIndex + i].Value.ToString()));
                            dtNewDate = dtNewDate.AddDays(168);
                        }
                        break;
                    case "1 Month":
                        dtNewDate = dtCompliance.AddMonths(1);
                        for (var i = 1; i < iRowCount - 1; i++)
                        {
                            dgDates[0, rowIndex + i].Value = dtNewDate.ToShortDateString();
                            UpdateReplenishmentDate(dtNewDate, int.Parse(dgDates[3, rowIndex + i].Value.ToString()));
                            dtNewDate = dtNewDate.AddMonths(1);
                        }
                        break;
                    case "3 Months":
                        dtNewDate = dtCompliance.AddMonths(3);
                        for (var i = 1; i < iRowCount - 1; i++)
                        {
                            dgDates[0, rowIndex + i].Value = dtNewDate.ToShortDateString();
                            UpdateReplenishmentDate(dtNewDate, int.Parse(dgDates[3, rowIndex + i].Value.ToString()));
                            dtNewDate = dtNewDate.AddMonths(3);
                        }
                        break;
                    case "6 Months":
                        dtNewDate = dtCompliance.AddMonths(6);
                        for (var i = 1; i < iRowCount - 1; i++)
                        {
                            dgDates[0, rowIndex + i].Value = dtNewDate.ToShortDateString();
                            UpdateReplenishmentDate(dtNewDate, int.Parse(dgDates[3, rowIndex + i].Value.ToString()));
                            dtNewDate = dtNewDate.AddMonths(6);
                        }
                        break;
                    case "1 Year":
                        dtNewDate = dtCompliance.AddYears(1);
                        for (var i = 1; i < iRowCount - 1; i++)
                        {
                            dgDates[0, rowIndex + i].Value = dtNewDate.ToShortDateString();
                            UpdateReplenishmentDate(dtNewDate, int.Parse(dgDates[3, rowIndex + i].Value.ToString()));
                            dtNewDate = dtNewDate.AddYears(1);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {            
            }
        }

        #endregion

        #region Public Methods

        public int PatientId { get; set; }

        public int RxId { get; set; }

        #endregion

        private void dgDates_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}
