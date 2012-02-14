namespace ContactLens
{
    partial class FrmFindPatient
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
            this.components = new System.ComponentModel.Container();
            this.groupCriteria = new System.Windows.Forms.GroupBox();
            this.txtChartNo = new System.Windows.Forms.TextBox();
            this.txtPatientNo = new System.Windows.Forms.TextBox();
            this.txtHomePhone = new System.Windows.Forms.TextBox();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.lblChartNo = new System.Windows.Forms.Label();
            this.lblPatientNo = new System.Windows.Forms.Label();
            this.lblHomePhone = new System.Windows.Forms.Label();
            this.lblCity = new System.Windows.Forms.Label();
            this.txtDOB = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtSSN = new System.Windows.Forms.TextBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.lblDOB = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblSSN = new System.Windows.Forms.Label();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.btFind = new System.Windows.Forms.Button();
            this.btSelect = new System.Windows.Forms.Button();
            this.btClear = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.dgResults = new System.Windows.Forms.DataGridView();
            this.pName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AddressCity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HomePhone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DOB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblPatientsFound = new System.Windows.Forms.Label();
            this.txtPatientsFound = new System.Windows.Forms.TextBox();
            this.dbBindSource = new System.Windows.Forms.BindingSource(this.components);
            this.btBack = new System.Windows.Forms.Button();
            this.groupCriteria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbBindSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupCriteria
            // 
            this.groupCriteria.Controls.Add(this.txtChartNo);
            this.groupCriteria.Controls.Add(this.txtPatientNo);
            this.groupCriteria.Controls.Add(this.txtHomePhone);
            this.groupCriteria.Controls.Add(this.txtCity);
            this.groupCriteria.Controls.Add(this.lblChartNo);
            this.groupCriteria.Controls.Add(this.lblPatientNo);
            this.groupCriteria.Controls.Add(this.lblHomePhone);
            this.groupCriteria.Controls.Add(this.lblCity);
            this.groupCriteria.Controls.Add(this.txtDOB);
            this.groupCriteria.Controls.Add(this.txtAddress);
            this.groupCriteria.Controls.Add(this.txtSSN);
            this.groupCriteria.Controls.Add(this.txtFirstName);
            this.groupCriteria.Controls.Add(this.txtLastName);
            this.groupCriteria.Controls.Add(this.lblDOB);
            this.groupCriteria.Controls.Add(this.lblAddress);
            this.groupCriteria.Controls.Add(this.lblSSN);
            this.groupCriteria.Controls.Add(this.lblFirstName);
            this.groupCriteria.Controls.Add(this.lblLastName);
            this.groupCriteria.Location = new System.Drawing.Point(12, 12);
            this.groupCriteria.Name = "groupCriteria";
            this.groupCriteria.Size = new System.Drawing.Size(724, 172);
            this.groupCriteria.TabIndex = 0;
            this.groupCriteria.TabStop = false;
            this.groupCriteria.Text = "Selection Criteria";
            // 
            // txtChartNo
            // 
            this.txtChartNo.Location = new System.Drawing.Point(376, 105);
            this.txtChartNo.Name = "txtChartNo";
            this.txtChartNo.Size = new System.Drawing.Size(140, 20);
            this.txtChartNo.TabIndex = 17;
            this.txtChartNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtChartNo_KeyDown);
            // 
            // txtPatientNo
            // 
            this.txtPatientNo.Location = new System.Drawing.Point(376, 74);
            this.txtPatientNo.Name = "txtPatientNo";
            this.txtPatientNo.Size = new System.Drawing.Size(140, 20);
            this.txtPatientNo.TabIndex = 16;
            this.txtPatientNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPatientNo_KeyDown);
            // 
            // txtHomePhone
            // 
            this.txtHomePhone.Location = new System.Drawing.Point(376, 45);
            this.txtHomePhone.Name = "txtHomePhone";
            this.txtHomePhone.Size = new System.Drawing.Size(140, 20);
            this.txtHomePhone.TabIndex = 15;
            this.txtHomePhone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtHomePhone_KeyDown);
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(376, 19);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(140, 20);
            this.txtCity.TabIndex = 14;
            this.txtCity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCity_KeyDown);
            // 
            // lblChartNo
            // 
            this.lblChartNo.AutoSize = true;
            this.lblChartNo.Location = new System.Drawing.Point(270, 112);
            this.lblChartNo.Name = "lblChartNo";
            this.lblChartNo.Size = new System.Drawing.Size(49, 13);
            this.lblChartNo.TabIndex = 13;
            this.lblChartNo.Text = "Chart No";
            // 
            // lblPatientNo
            // 
            this.lblPatientNo.AutoSize = true;
            this.lblPatientNo.Location = new System.Drawing.Point(270, 81);
            this.lblPatientNo.Name = "lblPatientNo";
            this.lblPatientNo.Size = new System.Drawing.Size(57, 13);
            this.lblPatientNo.TabIndex = 12;
            this.lblPatientNo.Text = "Patient No";
            // 
            // lblHomePhone
            // 
            this.lblHomePhone.AutoSize = true;
            this.lblHomePhone.Location = new System.Drawing.Point(270, 55);
            this.lblHomePhone.Name = "lblHomePhone";
            this.lblHomePhone.Size = new System.Drawing.Size(69, 13);
            this.lblHomePhone.TabIndex = 11;
            this.lblHomePhone.Text = "Home Phone";
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(270, 27);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(24, 13);
            this.lblCity.TabIndex = 10;
            this.lblCity.Text = "City";
            // 
            // txtDOB
            // 
            this.txtDOB.Location = new System.Drawing.Point(112, 135);
            this.txtDOB.Name = "txtDOB";
            this.txtDOB.Size = new System.Drawing.Size(140, 20);
            this.txtDOB.TabIndex = 9;
            this.txtDOB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDOB_KeyDown);
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(112, 105);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(140, 20);
            this.txtAddress.TabIndex = 8;
            this.txtAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAddress_KeyDown);
            // 
            // txtSSN
            // 
            this.txtSSN.Location = new System.Drawing.Point(112, 74);
            this.txtSSN.Name = "txtSSN";
            this.txtSSN.Size = new System.Drawing.Size(140, 20);
            this.txtSSN.TabIndex = 7;
            this.txtSSN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSSN_KeyDown);
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(112, 45);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(140, 20);
            this.txtFirstName.TabIndex = 6;
            this.txtFirstName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFirstName_KeyDown);
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(112, 19);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(140, 20);
            this.txtLastName.TabIndex = 5;
            this.txtLastName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLastName_KeyDown);
            // 
            // lblDOB
            // 
            this.lblDOB.AutoSize = true;
            this.lblDOB.Location = new System.Drawing.Point(6, 138);
            this.lblDOB.Name = "lblDOB";
            this.lblDOB.Size = new System.Drawing.Size(66, 13);
            this.lblDOB.TabIndex = 4;
            this.lblDOB.Text = "Date of Birth";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(6, 112);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(45, 13);
            this.lblAddress.TabIndex = 3;
            this.lblAddress.Text = "Address";
            // 
            // lblSSN
            // 
            this.lblSSN.AutoSize = true;
            this.lblSSN.Location = new System.Drawing.Point(6, 81);
            this.lblSSN.Name = "lblSSN";
            this.lblSSN.Size = new System.Drawing.Size(94, 13);
            this.lblSSN.TabIndex = 2;
            this.lblSSN.Text = "Social Security No";
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(6, 55);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(57, 13);
            this.lblFirstName.TabIndex = 1;
            this.lblFirstName.Text = "First Name";
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(6, 27);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(58, 13);
            this.lblLastName.TabIndex = 0;
            this.lblLastName.Text = "Last Name";
            // 
            // btFind
            // 
            this.btFind.Location = new System.Drawing.Point(213, 391);
            this.btFind.Name = "btFind";
            this.btFind.Size = new System.Drawing.Size(75, 23);
            this.btFind.TabIndex = 1;
            this.btFind.Text = "&Find";
            this.btFind.UseVisualStyleBackColor = true;
            this.btFind.Click += new System.EventHandler(this.btFind_Click);
            // 
            // btSelect
            // 
            this.btSelect.Location = new System.Drawing.Point(294, 391);
            this.btSelect.Name = "btSelect";
            this.btSelect.Size = new System.Drawing.Size(75, 23);
            this.btSelect.TabIndex = 2;
            this.btSelect.Text = "&Select";
            this.btSelect.UseVisualStyleBackColor = true;
            this.btSelect.Click += new System.EventHandler(this.btSelect_Click);
            // 
            // btClear
            // 
            this.btClear.Location = new System.Drawing.Point(375, 391);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(75, 23);
            this.btClear.TabIndex = 3;
            this.btClear.Text = "&Clear";
            this.btClear.UseVisualStyleBackColor = true;
            this.btClear.Click += new System.EventHandler(this.btClear_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(537, 391);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 4;
            this.btCancel.Text = "&Exit";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // dgResults
            // 
            this.dgResults.AllowUserToAddRows = false;
            this.dgResults.AllowUserToDeleteRows = false;
            this.dgResults.AllowUserToResizeRows = false;
            this.dgResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pName,
            this.AddressCity,
            this.HomePhone,
            this.SSN,
            this.DOB});
            this.dgResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgResults.Location = new System.Drawing.Point(13, 191);
            this.dgResults.MultiSelect = false;
            this.dgResults.Name = "dgResults";
            this.dgResults.ReadOnly = true;
            this.dgResults.RowHeadersVisible = false;
            this.dgResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgResults.Size = new System.Drawing.Size(723, 194);
            this.dgResults.TabIndex = 5;
            this.dgResults.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgResults_CellContentDoubleClick);
            this.dgResults.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgResults_CellContentDoubleClick);
            this.dgResults.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgResults_KeyDown);
            // 
            // pName
            // 
            this.pName.HeaderText = "Name";
            this.pName.Name = "pName";
            this.pName.ReadOnly = true;
            this.pName.Width = 150;
            // 
            // AddressCity
            // 
            this.AddressCity.HeaderText = "Address / City";
            this.AddressCity.Name = "AddressCity";
            this.AddressCity.ReadOnly = true;
            this.AddressCity.Width = 250;
            // 
            // HomePhone
            // 
            this.HomePhone.HeaderText = "Home Phone";
            this.HomePhone.Name = "HomePhone";
            this.HomePhone.ReadOnly = true;
            // 
            // SSN
            // 
            this.SSN.HeaderText = "SS No";
            this.SSN.Name = "SSN";
            this.SSN.ReadOnly = true;
            // 
            // DOB
            // 
            this.DOB.HeaderText = "DOB";
            this.DOB.Name = "DOB";
            this.DOB.ReadOnly = true;
            // 
            // lblPatientsFound
            // 
            this.lblPatientsFound.AutoSize = true;
            this.lblPatientsFound.Location = new System.Drawing.Point(13, 400);
            this.lblPatientsFound.Name = "lblPatientsFound";
            this.lblPatientsFound.Size = new System.Drawing.Size(81, 13);
            this.lblPatientsFound.TabIndex = 6;
            this.lblPatientsFound.Text = "Patients Found:";
            // 
            // txtPatientsFound
            // 
            this.txtPatientsFound.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtPatientsFound.Location = new System.Drawing.Point(100, 394);
            this.txtPatientsFound.Name = "txtPatientsFound";
            this.txtPatientsFound.ReadOnly = true;
            this.txtPatientsFound.Size = new System.Drawing.Size(100, 20);
            this.txtPatientsFound.TabIndex = 7;
            this.txtPatientsFound.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btBack
            // 
            this.btBack.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btBack.Location = new System.Drawing.Point(456, 391);
            this.btBack.Name = "btBack";
            this.btBack.Size = new System.Drawing.Size(75, 23);
            this.btBack.TabIndex = 8;
            this.btBack.Text = "&Back";
            this.btBack.UseVisualStyleBackColor = true;
            this.btBack.Click += new System.EventHandler(this.btBack_Click);
            // 
            // FrmFindPatient
            // 
            this.AcceptButton = this.btFind;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(748, 451);
            this.ControlBox = false;
            this.Controls.Add(this.btBack);
            this.Controls.Add(this.txtPatientsFound);
            this.Controls.Add(this.lblPatientsFound);
            this.Controls.Add(this.dgResults);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btClear);
            this.Controls.Add(this.btSelect);
            this.Controls.Add(this.btFind);
            this.Controls.Add(this.groupCriteria);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmFindPatient";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find Patient";
            this.Load += new System.EventHandler(this.FrmFindPatient_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmFindPatient_KeyDown);
            this.groupCriteria.ResumeLayout(false);
            this.groupCriteria.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbBindSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupCriteria;
        private System.Windows.Forms.Button btFind;
        private System.Windows.Forms.Button btSelect;
        private System.Windows.Forms.Button btClear;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.DataGridView dgResults;
        private System.Windows.Forms.Label lblPatientsFound;
        private System.Windows.Forms.TextBox txtPatientsFound;
        private System.Windows.Forms.TextBox txtDOB;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtSSN;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label lblDOB;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblSSN;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.TextBox txtChartNo;
        private System.Windows.Forms.TextBox txtPatientNo;
        private System.Windows.Forms.TextBox txtHomePhone;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.Label lblChartNo;
        private System.Windows.Forms.Label lblPatientNo;
        private System.Windows.Forms.Label lblHomePhone;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.BindingSource dbBindSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn pName;
        private System.Windows.Forms.DataGridViewTextBoxColumn AddressCity;
        private System.Windows.Forms.DataGridViewTextBoxColumn HomePhone;
        private System.Windows.Forms.DataGridViewTextBoxColumn SSN;
        private System.Windows.Forms.DataGridViewTextBoxColumn DOB;
        private System.Windows.Forms.Button btBack;
    }
}

