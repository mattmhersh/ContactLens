using System.ComponentModel;

namespace ContactLens
{
    partial class FrmReplenishmentSchedule
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblPatientID = new System.Windows.Forms.Label();
            this.btBack = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btDeleteReplenishmentDates = new System.Windows.Forms.Button();
            this.btAddReplenishmentDates = new System.Windows.Forms.Button();
            this.cbDoNotSend = new System.Windows.Forms.CheckBox();
            this.cmbAutomail = new System.Windows.Forms.ComboBox();
            this.lblAutoMail = new System.Windows.Forms.Label();
            this.dgDates = new System.Windows.Forms.DataGridView();
            this.ReplenishmentDates = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComplianceDates = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Delqt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtInitialDispensingDate = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cbReplenishmentNoticeCategory = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtLTint = new System.Windows.Forms.TextBox();
            this.lblTint = new System.Windows.Forms.Label();
            this.txtRTint = new System.Windows.Forms.TextBox();
            this.txtRxNotes = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtLLensType = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtLSphere = new System.Windows.Forms.TextBox();
            this.txtRSphere = new System.Windows.Forms.TextBox();
            this.txtLAdd = new System.Windows.Forms.TextBox();
            this.txtLAxis = new System.Windows.Forms.TextBox();
            this.txtLCyl = new System.Windows.Forms.TextBox();
            this.txtLDia = new System.Windows.Forms.TextBox();
            this.txtLBC = new System.Windows.Forms.TextBox();
            this.txtLSeries = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtRAdd = new System.Windows.Forms.TextBox();
            this.txtRAxis = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRCyl = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRDia = new System.Windows.Forms.TextBox();
            this.txtRBC = new System.Windows.Forms.TextBox();
            this.txtRSeries = new System.Windows.Forms.TextBox();
            this.txtRLensType = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDates)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(12, 9);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(222, 13);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Scheduled Lens Replenishment Dates";
            // 
            // lblPatientID
            // 
            this.lblPatientID.AutoSize = true;
            this.lblPatientID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatientID.Location = new System.Drawing.Point(251, 9);
            this.lblPatientID.Name = "lblPatientID";
            this.lblPatientID.Size = new System.Drawing.Size(55, 13);
            this.lblPatientID.TabIndex = 1;
            this.lblPatientID.Text = "Patient: ";
            // 
            // btBack
            // 
            this.btBack.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btBack.Location = new System.Drawing.Point(27, 583);
            this.btBack.Name = "btBack";
            this.btBack.Size = new System.Drawing.Size(75, 23);
            this.btBack.TabIndex = 31;
            this.btBack.Text = "&Back";
            this.btBack.UseVisualStyleBackColor = true;
            this.btBack.Click += new System.EventHandler(this.btBack_Click);
            // 
            // btExit
            // 
            this.btExit.Location = new System.Drawing.Point(189, 583);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 32;
            this.btExit.Text = "E&xit";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(108, 583);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 23);
            this.btSave.TabIndex = 36;
            this.btSave.Text = "Save";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btDeleteReplenishmentDates);
            this.groupBox1.Controls.Add(this.btAddReplenishmentDates);
            this.groupBox1.Controls.Add(this.cbDoNotSend);
            this.groupBox1.Controls.Add(this.cmbAutomail);
            this.groupBox1.Controls.Add(this.lblAutoMail);
            this.groupBox1.Controls.Add(this.dgDates);
            this.groupBox1.Controls.Add(this.txtInitialDispensingDate);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.cbReplenishmentNoticeCategory);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Location = new System.Drawing.Point(15, 247);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(733, 330);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Replenishment Dates";
            // 
            // btDeleteReplenishmentDates
            // 
            this.btDeleteReplenishmentDates.Location = new System.Drawing.Point(411, 79);
            this.btDeleteReplenishmentDates.Name = "btDeleteReplenishmentDates";
            this.btDeleteReplenishmentDates.Size = new System.Drawing.Size(184, 23);
            this.btDeleteReplenishmentDates.TabIndex = 59;
            this.btDeleteReplenishmentDates.Text = "Delete Replenishment Schedule";
            this.btDeleteReplenishmentDates.UseVisualStyleBackColor = true;
            this.btDeleteReplenishmentDates.Click += new System.EventHandler(this.BtDeleteReplenishmentDatesClick);
            // 
            // btAddReplenishmentDates
            // 
            this.btAddReplenishmentDates.Location = new System.Drawing.Point(411, 46);
            this.btAddReplenishmentDates.Name = "btAddReplenishmentDates";
            this.btAddReplenishmentDates.Size = new System.Drawing.Size(154, 23);
            this.btAddReplenishmentDates.TabIndex = 58;
            this.btAddReplenishmentDates.Text = "Add Replenishment Dates";
            this.btAddReplenishmentDates.UseVisualStyleBackColor = true;
            this.btAddReplenishmentDates.Visible = false;
            this.btAddReplenishmentDates.Click += new System.EventHandler(this.btAddReplenishmentDates_Click);
            // 
            // cbDoNotSend
            // 
            this.cbDoNotSend.AutoSize = true;
            this.cbDoNotSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDoNotSend.Location = new System.Drawing.Point(411, 22);
            this.cbDoNotSend.Name = "cbDoNotSend";
            this.cbDoNotSend.Size = new System.Drawing.Size(99, 17);
            this.cbDoNotSend.TabIndex = 57;
            this.cbDoNotSend.Text = "Do Not Send";
            this.cbDoNotSend.UseVisualStyleBackColor = true;
            // 
            // cmbAutomail
            // 
            this.cmbAutomail.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAutomail.FormattingEnabled = true;
            this.cmbAutomail.Items.AddRange(new object[] {
            "",
            "A - Automail",
            "F - Automail - Different Address",
            "N - Replenishment Notes for Automail"});
            this.cmbAutomail.Location = new System.Drawing.Point(202, 86);
            this.cmbAutomail.Name = "cmbAutomail";
            this.cmbAutomail.Size = new System.Drawing.Size(180, 21);
            this.cmbAutomail.TabIndex = 56;
            // 
            // lblAutoMail
            // 
            this.lblAutoMail.AutoSize = true;
            this.lblAutoMail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAutoMail.Location = new System.Drawing.Point(7, 89);
            this.lblAutoMail.Name = "lblAutoMail";
            this.lblAutoMail.Size = new System.Drawing.Size(59, 13);
            this.lblAutoMail.TabIndex = 55;
            this.lblAutoMail.Text = "Automail:";
            // 
            // dgDates
            // 
            this.dgDates.CausesValidation = false;
            this.dgDates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ReplenishmentDates,
            this.ComplianceDates,
            this.Delqt});
            this.dgDates.Location = new System.Drawing.Point(10, 113);
            this.dgDates.Name = "dgDates";
            this.dgDates.Size = new System.Drawing.Size(672, 208);
            this.dgDates.TabIndex = 54;
            this.dgDates.VirtualMode = true;
            this.dgDates.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgDates_CellBeginEdit);
            this.dgDates.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgDates_CellEndEdit);
            this.dgDates.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgDates_DataError);
            this.dgDates.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgDates_CellContentClick);
            // 
            // ReplenishmentDates
            // 
            this.ReplenishmentDates.DataPropertyName = "Replenishment Date";
            this.ReplenishmentDates.HeaderText = "Replenishment Dates";
            this.ReplenishmentDates.Name = "ReplenishmentDates";
            this.ReplenishmentDates.Width = 200;
            // 
            // ComplianceDates
            // 
            this.ComplianceDates.DataPropertyName = "Compliance Date";
            this.ComplianceDates.HeaderText = "Compliance Dates";
            this.ComplianceDates.Name = "ComplianceDates";
            this.ComplianceDates.Width = 200;
            // 
            // Delqt
            // 
            this.Delqt.DataPropertyName = "Delqt";
            this.Delqt.HeaderText = "Delqt";
            this.Delqt.Name = "Delqt";
            this.Delqt.ReadOnly = true;
            // 
            // txtInitialDispensingDate
            // 
            this.txtInitialDispensingDate.BackColor = System.Drawing.Color.White;
            this.txtInitialDispensingDate.ForeColor = System.Drawing.Color.Black;
            this.txtInitialDispensingDate.Location = new System.Drawing.Point(202, 53);
            this.txtInitialDispensingDate.Name = "txtInitialDispensingDate";
            this.txtInitialDispensingDate.Size = new System.Drawing.Size(121, 20);
            this.txtInitialDispensingDate.TabIndex = 51;
            this.txtInitialDispensingDate.GotFocus += new System.EventHandler(this.txtInitialDispensingDate_GotFocus);
            this.txtInitialDispensingDate.LostFocus += new System.EventHandler(this.txtInitialDispensingDate_LostFocus);
            this.txtInitialDispensingDate.Validating += new System.ComponentModel.CancelEventHandler(this.txtInitialDispensingDate_Validating);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(7, 56);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(139, 13);
            this.label14.TabIndex = 50;
            this.label14.Text = "Initial Dispensing Date:";
            // 
            // cbReplenishmentNoticeCategory
            // 
            this.cbReplenishmentNoticeCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReplenishmentNoticeCategory.FormattingEnabled = true;
            this.cbReplenishmentNoticeCategory.Items.AddRange(new object[] {
            "",
            "12 Weeks",
            "24 Weeks",
            "1 Month",
            "3 Months",
            "6 Months",
            "1 Year"});
            this.cbReplenishmentNoticeCategory.Location = new System.Drawing.Point(202, 22);
            this.cbReplenishmentNoticeCategory.Name = "cbReplenishmentNoticeCategory";
            this.cbReplenishmentNoticeCategory.Size = new System.Drawing.Size(121, 21);
            this.cbReplenishmentNoticeCategory.TabIndex = 47;
            this.cbReplenishmentNoticeCategory.SelectedIndexChanged += new System.EventHandler(this.cbReplenishmentNoticeCategory_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(7, 25);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(189, 13);
            this.label12.TabIndex = 46;
            this.label12.Text = "Replenishment Notice Category:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.txtLTint);
            this.groupBox2.Controls.Add(this.lblTint);
            this.groupBox2.Controls.Add(this.txtRTint);
            this.groupBox2.Controls.Add(this.txtRxNotes);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.txtLLensType);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.txtLSphere);
            this.groupBox2.Controls.Add(this.txtRSphere);
            this.groupBox2.Controls.Add(this.txtLAdd);
            this.groupBox2.Controls.Add(this.txtLAxis);
            this.groupBox2.Controls.Add(this.txtLCyl);
            this.groupBox2.Controls.Add(this.txtLDia);
            this.groupBox2.Controls.Add(this.txtLBC);
            this.groupBox2.Controls.Add(this.txtLSeries);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtRAdd);
            this.groupBox2.Controls.Add(this.txtRAxis);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtRCyl);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtRDia);
            this.groupBox2.Controls.Add(this.txtRBC);
            this.groupBox2.Controls.Add(this.txtRSeries);
            this.groupBox2.Controls.Add(this.txtRLensType);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(15, 36);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(897, 205);
            this.groupBox2.TabIndex = 47;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Patient Lens Data";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(7, 152);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(24, 13);
            this.label10.TabIndex = 70;
            this.label10.Text = "OS";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(7, 123);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(25, 13);
            this.label13.TabIndex = 69;
            this.label13.Text = "OD";
            // 
            // txtLTint
            // 
            this.txtLTint.BackColor = System.Drawing.Color.White;
            this.txtLTint.ForeColor = System.Drawing.Color.Black;
            this.txtLTint.Location = new System.Drawing.Point(38, 149);
            this.txtLTint.Name = "txtLTint";
            this.txtLTint.ReadOnly = true;
            this.txtLTint.Size = new System.Drawing.Size(450, 20);
            this.txtLTint.TabIndex = 68;
            // 
            // lblTint
            // 
            this.lblTint.AutoSize = true;
            this.lblTint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTint.Location = new System.Drawing.Point(37, 104);
            this.lblTint.Name = "lblTint";
            this.lblTint.Size = new System.Drawing.Size(29, 13);
            this.lblTint.TabIndex = 67;
            this.lblTint.Text = "Tint";
            // 
            // txtRTint
            // 
            this.txtRTint.BackColor = System.Drawing.Color.White;
            this.txtRTint.ForeColor = System.Drawing.Color.Black;
            this.txtRTint.Location = new System.Drawing.Point(38, 120);
            this.txtRTint.Name = "txtRTint";
            this.txtRTint.ReadOnly = true;
            this.txtRTint.Size = new System.Drawing.Size(450, 20);
            this.txtRTint.TabIndex = 66;
            // 
            // txtRxNotes
            // 
            this.txtRxNotes.BackColor = System.Drawing.Color.White;
            this.txtRxNotes.ForeColor = System.Drawing.Color.Black;
            this.txtRxNotes.Location = new System.Drawing.Point(494, 120);
            this.txtRxNotes.Multiline = true;
            this.txtRxNotes.Name = "txtRxNotes";
            this.txtRxNotes.Size = new System.Drawing.Size(393, 79);
            this.txtRxNotes.TabIndex = 65;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(491, 104);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(127, 13);
            this.label15.TabIndex = 64;
            this.label15.Text = "Replenishment Notes";
            // 
            // txtLLensType
            // 
            this.txtLLensType.BackColor = System.Drawing.Color.White;
            this.txtLLensType.ForeColor = System.Drawing.Color.Black;
            this.txtLLensType.Location = new System.Drawing.Point(10, 72);
            this.txtLLensType.Name = "txtLLensType";
            this.txtLLensType.ReadOnly = true;
            this.txtLLensType.Size = new System.Drawing.Size(100, 20);
            this.txtLLensType.TabIndex = 61;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(467, 26);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 13);
            this.label11.TabIndex = 60;
            this.label11.Text = "Sphere";
            // 
            // txtLSphere
            // 
            this.txtLSphere.BackColor = System.Drawing.Color.White;
            this.txtLSphere.ForeColor = System.Drawing.Color.Black;
            this.txtLSphere.Location = new System.Drawing.Point(470, 70);
            this.txtLSphere.Name = "txtLSphere";
            this.txtLSphere.ReadOnly = true;
            this.txtLSphere.Size = new System.Drawing.Size(100, 20);
            this.txtLSphere.TabIndex = 59;
            // 
            // txtRSphere
            // 
            this.txtRSphere.BackColor = System.Drawing.Color.White;
            this.txtRSphere.ForeColor = System.Drawing.Color.Black;
            this.txtRSphere.Location = new System.Drawing.Point(469, 42);
            this.txtRSphere.Name = "txtRSphere";
            this.txtRSphere.ReadOnly = true;
            this.txtRSphere.Size = new System.Drawing.Size(100, 20);
            this.txtRSphere.TabIndex = 58;
            // 
            // txtLAdd
            // 
            this.txtLAdd.BackColor = System.Drawing.Color.White;
            this.txtLAdd.ForeColor = System.Drawing.Color.Black;
            this.txtLAdd.Location = new System.Drawing.Point(787, 71);
            this.txtLAdd.Name = "txtLAdd";
            this.txtLAdd.ReadOnly = true;
            this.txtLAdd.Size = new System.Drawing.Size(100, 20);
            this.txtLAdd.TabIndex = 57;
            // 
            // txtLAxis
            // 
            this.txtLAxis.BackColor = System.Drawing.Color.White;
            this.txtLAxis.ForeColor = System.Drawing.Color.Black;
            this.txtLAxis.Location = new System.Drawing.Point(681, 71);
            this.txtLAxis.Name = "txtLAxis";
            this.txtLAxis.ReadOnly = true;
            this.txtLAxis.Size = new System.Drawing.Size(100, 20);
            this.txtLAxis.TabIndex = 56;
            // 
            // txtLCyl
            // 
            this.txtLCyl.BackColor = System.Drawing.Color.White;
            this.txtLCyl.ForeColor = System.Drawing.Color.Black;
            this.txtLCyl.Location = new System.Drawing.Point(575, 71);
            this.txtLCyl.Name = "txtLCyl";
            this.txtLCyl.ReadOnly = true;
            this.txtLCyl.Size = new System.Drawing.Size(100, 20);
            this.txtLCyl.TabIndex = 55;
            // 
            // txtLDia
            // 
            this.txtLDia.BackColor = System.Drawing.Color.White;
            this.txtLDia.ForeColor = System.Drawing.Color.Black;
            this.txtLDia.Location = new System.Drawing.Point(363, 71);
            this.txtLDia.Name = "txtLDia";
            this.txtLDia.ReadOnly = true;
            this.txtLDia.Size = new System.Drawing.Size(100, 20);
            this.txtLDia.TabIndex = 54;
            // 
            // txtLBC
            // 
            this.txtLBC.BackColor = System.Drawing.Color.White;
            this.txtLBC.ForeColor = System.Drawing.Color.Black;
            this.txtLBC.Location = new System.Drawing.Point(257, 72);
            this.txtLBC.Name = "txtLBC";
            this.txtLBC.ReadOnly = true;
            this.txtLBC.Size = new System.Drawing.Size(100, 20);
            this.txtLBC.TabIndex = 53;
            // 
            // txtLSeries
            // 
            this.txtLSeries.BackColor = System.Drawing.Color.White;
            this.txtLSeries.ForeColor = System.Drawing.Color.Black;
            this.txtLSeries.Location = new System.Drawing.Point(148, 71);
            this.txtLSeries.Name = "txtLSeries";
            this.txtLSeries.ReadOnly = true;
            this.txtLSeries.Size = new System.Drawing.Size(100, 20);
            this.txtLSeries.TabIndex = 52;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(119, 78);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(24, 13);
            this.label9.TabIndex = 51;
            this.label9.Text = "OS";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(119, 49);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 13);
            this.label8.TabIndex = 50;
            this.label8.Text = "OD";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(784, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 49;
            this.label7.Text = "Add";
            // 
            // txtRAdd
            // 
            this.txtRAdd.BackColor = System.Drawing.Color.White;
            this.txtRAdd.ForeColor = System.Drawing.Color.Black;
            this.txtRAdd.Location = new System.Drawing.Point(787, 42);
            this.txtRAdd.Name = "txtRAdd";
            this.txtRAdd.ReadOnly = true;
            this.txtRAdd.Size = new System.Drawing.Size(100, 20);
            this.txtRAdd.TabIndex = 48;
            // 
            // txtRAxis
            // 
            this.txtRAxis.BackColor = System.Drawing.Color.White;
            this.txtRAxis.ForeColor = System.Drawing.Color.Black;
            this.txtRAxis.Location = new System.Drawing.Point(681, 42);
            this.txtRAxis.Name = "txtRAxis";
            this.txtRAxis.ReadOnly = true;
            this.txtRAxis.Size = new System.Drawing.Size(100, 20);
            this.txtRAxis.TabIndex = 47;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(678, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 13);
            this.label6.TabIndex = 46;
            this.label6.Text = "Axis";
            // 
            // txtRCyl
            // 
            this.txtRCyl.BackColor = System.Drawing.Color.White;
            this.txtRCyl.ForeColor = System.Drawing.Color.Black;
            this.txtRCyl.Location = new System.Drawing.Point(575, 42);
            this.txtRCyl.Name = "txtRCyl";
            this.txtRCyl.ReadOnly = true;
            this.txtRCyl.Size = new System.Drawing.Size(100, 20);
            this.txtRCyl.TabIndex = 45;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(572, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 44;
            this.label5.Text = "Cyl.";
            // 
            // txtRDia
            // 
            this.txtRDia.BackColor = System.Drawing.Color.White;
            this.txtRDia.ForeColor = System.Drawing.Color.Black;
            this.txtRDia.Location = new System.Drawing.Point(363, 42);
            this.txtRDia.Name = "txtRDia";
            this.txtRDia.ReadOnly = true;
            this.txtRDia.Size = new System.Drawing.Size(100, 20);
            this.txtRDia.TabIndex = 43;
            // 
            // txtRBC
            // 
            this.txtRBC.BackColor = System.Drawing.Color.White;
            this.txtRBC.ForeColor = System.Drawing.Color.Black;
            this.txtRBC.Location = new System.Drawing.Point(257, 43);
            this.txtRBC.Name = "txtRBC";
            this.txtRBC.ReadOnly = true;
            this.txtRBC.Size = new System.Drawing.Size(100, 20);
            this.txtRBC.TabIndex = 42;
            // 
            // txtRSeries
            // 
            this.txtRSeries.BackColor = System.Drawing.Color.White;
            this.txtRSeries.ForeColor = System.Drawing.Color.Black;
            this.txtRSeries.Location = new System.Drawing.Point(148, 42);
            this.txtRSeries.Name = "txtRSeries";
            this.txtRSeries.ReadOnly = true;
            this.txtRSeries.Size = new System.Drawing.Size(100, 20);
            this.txtRSeries.TabIndex = 41;
            // 
            // txtRLensType
            // 
            this.txtRLensType.BackColor = System.Drawing.Color.White;
            this.txtRLensType.ForeColor = System.Drawing.Color.Black;
            this.txtRLensType.Location = new System.Drawing.Point(10, 42);
            this.txtRLensType.Name = "txtRLensType";
            this.txtRLensType.ReadOnly = true;
            this.txtRLensType.Size = new System.Drawing.Size(100, 20);
            this.txtRLensType.TabIndex = 40;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(360, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 39;
            this.label4.Text = "Dia.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(254, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 38;
            this.label3.Text = "B.C.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(145, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 37;
            this.label2.Text = "Lens Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 36;
            this.label1.Text = "Manufacturer";
            // 
            // FrmReplenishmentSchedule
            // 
            this.AcceptButton = this.btSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btBack;
            this.ClientSize = new System.Drawing.Size(920, 644);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.btExit);
            this.Controls.Add(this.btBack);
            this.Controls.Add(this.lblPatientID);
            this.Controls.Add(this.lblHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmReplenishmentSchedule";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Replenishment Schedule";
            this.Load += new System.EventHandler(this.FrmReplenishmentScheduleLoad);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDates)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblPatientID;
        private System.Windows.Forms.Button btBack;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgDates;
        private System.Windows.Forms.TextBox txtInitialDispensingDate;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cbReplenishmentNoticeCategory;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtLSphere;
        private System.Windows.Forms.TextBox txtRSphere;
        private System.Windows.Forms.TextBox txtLAdd;
        private System.Windows.Forms.TextBox txtLAxis;
        private System.Windows.Forms.TextBox txtLCyl;
        private System.Windows.Forms.TextBox txtLDia;
        private System.Windows.Forms.TextBox txtLBC;
        private System.Windows.Forms.TextBox txtLSeries;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtRAdd;
        private System.Windows.Forms.TextBox txtRAxis;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRCyl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRDia;
        private System.Windows.Forms.TextBox txtRBC;
        private System.Windows.Forms.TextBox txtRSeries;
        private System.Windows.Forms.TextBox txtRLensType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLLensType;
        private System.Windows.Forms.ComboBox cmbAutomail;
        private System.Windows.Forms.Label lblAutoMail;
        private System.Windows.Forms.CheckBox cbDoNotSend;
        private System.Windows.Forms.TextBox txtLTint;
        private System.Windows.Forms.Label lblTint;
        private System.Windows.Forms.TextBox txtRTint;
        private System.Windows.Forms.TextBox txtRxNotes;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btAddReplenishmentDates;
        private System.Windows.Forms.Button btDeleteReplenishmentDates;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReplenishmentDates;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComplianceDates;
        private System.Windows.Forms.DataGridViewTextBoxColumn Delqt;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
    }
}