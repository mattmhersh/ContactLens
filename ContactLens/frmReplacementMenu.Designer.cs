namespace ContactLens
{
    partial class FrmReplacementMenu
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
            this.btExit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbDelete = new System.Windows.Forms.RadioButton();
            this.rbUpdate = new System.Windows.Forms.RadioButton();
            this.btSelect = new System.Windows.Forms.Button();
            this.btPrintAutomailReport = new System.Windows.Forms.Button();
            this.btPrintComplianceReport = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtReportEndDate = new System.Windows.Forms.TextBox();
            this.txtReportStartDate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtReportFontSize = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btExit
            // 
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Location = new System.Drawing.Point(366, 198);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 0;
            this.btExit.Text = "E&xit";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbDelete);
            this.groupBox1.Controls.Add(this.rbUpdate);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(429, 67);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Patient Scheduling";
            // 
            // rbDelete
            // 
            this.rbDelete.AutoSize = true;
            this.rbDelete.Location = new System.Drawing.Point(19, 42);
            this.rbDelete.Name = "rbDelete";
            this.rbDelete.Size = new System.Drawing.Size(212, 17);
            this.rbDelete.TabIndex = 6;
            this.rbDelete.Text = "Delete a Replenishment Date Schedule";
            this.rbDelete.UseVisualStyleBackColor = true;
            // 
            // rbUpdate
            // 
            this.rbUpdate.AutoSize = true;
            this.rbUpdate.Checked = true;
            this.rbUpdate.Location = new System.Drawing.Point(19, 19);
            this.rbUpdate.Name = "rbUpdate";
            this.rbUpdate.Size = new System.Drawing.Size(216, 17);
            this.rbUpdate.TabIndex = 4;
            this.rbUpdate.TabStop = true;
            this.rbUpdate.Text = "Update a Replenishment Date Schedule";
            this.rbUpdate.UseVisualStyleBackColor = true;
            // 
            // btSelect
            // 
            this.btSelect.Location = new System.Drawing.Point(12, 198);
            this.btSelect.Name = "btSelect";
            this.btSelect.Size = new System.Drawing.Size(75, 23);
            this.btSelect.TabIndex = 4;
            this.btSelect.Text = "&Select Item";
            this.btSelect.UseVisualStyleBackColor = true;
            this.btSelect.Click += new System.EventHandler(this.btSelect_Click);
            // 
            // btPrintAutomailReport
            // 
            this.btPrintAutomailReport.Location = new System.Drawing.Point(93, 198);
            this.btPrintAutomailReport.Name = "btPrintAutomailReport";
            this.btPrintAutomailReport.Size = new System.Drawing.Size(128, 23);
            this.btPrintAutomailReport.TabIndex = 5;
            this.btPrintAutomailReport.Text = "Print Automail Report";
            this.btPrintAutomailReport.UseVisualStyleBackColor = true;
            this.btPrintAutomailReport.Click += new System.EventHandler(this.btPrintAutomailReport_Click);
            // 
            // btPrintComplianceReport
            // 
            this.btPrintComplianceReport.Location = new System.Drawing.Point(227, 198);
            this.btPrintComplianceReport.Name = "btPrintComplianceReport";
            this.btPrintComplianceReport.Size = new System.Drawing.Size(133, 23);
            this.btPrintComplianceReport.TabIndex = 6;
            this.btPrintComplianceReport.Text = "Print Compliance Report";
            this.btPrintComplianceReport.UseVisualStyleBackColor = true;
            this.btPrintComplianceReport.Click += new System.EventHandler(this.btPrintComplianceReport_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtReportEndDate);
            this.groupBox2.Controls.Add(this.txtReportStartDate);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtReportFontSize);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 85);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(429, 107);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Report Settings";
            // 
            // txtReportEndDate
            // 
            this.txtReportEndDate.Location = new System.Drawing.Point(111, 71);
            this.txtReportEndDate.Name = "txtReportEndDate";
            this.txtReportEndDate.Size = new System.Drawing.Size(100, 20);
            this.txtReportEndDate.TabIndex = 6;
            // 
            // txtReportStartDate
            // 
            this.txtReportStartDate.Location = new System.Drawing.Point(111, 47);
            this.txtReportStartDate.Name = "txtReportStartDate";
            this.txtReportStartDate.Size = new System.Drawing.Size(100, 20);
            this.txtReportStartDate.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Report End Date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Report Start Date:";
            // 
            // txtReportFontSize
            // 
            this.txtReportFontSize.Location = new System.Drawing.Point(111, 24);
            this.txtReportFontSize.Name = "txtReportFontSize";
            this.txtReportFontSize.Size = new System.Drawing.Size(100, 20);
            this.txtReportFontSize.TabIndex = 2;
            this.txtReportFontSize.Text = "11";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Report Font Size:";
            // 
            // FrmReplacementMenu
            // 
            this.AcceptButton = this.btSelect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(467, 258);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btPrintComplianceReport);
            this.Controls.Add(this.btPrintAutomailReport);
            this.Controls.Add(this.btSelect);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmReplacementMenu";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Contact Lens Replenishment Menu";
            this.Load += new System.EventHandler(this.frmReplacementMenu_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbDelete;
        private System.Windows.Forms.RadioButton rbUpdate;
        private System.Windows.Forms.Button btSelect;
        private System.Windows.Forms.Button btPrintAutomailReport;
        private System.Windows.Forms.Button btPrintComplianceReport;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtReportFontSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtReportEndDate;
        private System.Windows.Forms.TextBox txtReportStartDate;
        private System.Windows.Forms.Label label3;
    }
}