// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace ReportPrinting
{
    /// <summary>
    /// A simple control that wraps the functionality of the key buttons required
    /// for printing
    /// </summary>
    /// <remarks>
    /// <para>
    /// Some code for this class inspired by:
    /// Alex Calvo's article 
    /// "Preview and Print from Your Windows Forms App with the .NET Printing Namespace"
    /// http://msdn.microsoft.com/msdnmag/issues/03/02/PrintinginNET/default.aspx
    /// </para>
    /// <para>
    /// You can customize a few things with the following properties:
    /// 
    /// <list type="table">
    /// <item>
    /// <term>ShowStatusDialog</term>
    /// <description>
    /// The progress of the print job is shown in a status dialog.  Default is true.
    /// </description>
    /// </item>
    /// 
    /// <item>
    /// <term>PrintInBackground</term>
    /// <description>
    /// Indicates that printing should be done in the background.  
    /// Default is true.  
    /// (Note, if your application exits while printing is still in progress, 
    /// it will quit.  Make sure to monitor the PrintInProgress flag).
    /// </description>
    /// </item>
    /// 
    /// <item>
    /// <term>Printing</term>
    /// <description>
    ///	This event is raised prior to printing.  It allows user code 
    ///	to setup for printing.  (This is useful for dumping data from the 
    ///	GUI to a helper class, for instance).
    ///	</description>
    ///	</item>
    ///	</list>
    ///	</para>
    /// </remarks>
    /// 
    /// <example>
    /// To use the print control, place it on a form.
    /// Set the Document property to a valid PrintDocument
    /// (it doesn’t have to just be the ReportDocument).  That’s it!
    /// </example>
    [DefaultProperty("Document"), DefaultEvent("Printing")]
    public class PrintControl : System.Windows.Forms.UserControl
	{

        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnPageSetup;
        private System.Windows.Forms.ToolTip toolTip1;
        private ReportPrinting.PrintLogic printLogic1;
        private System.ComponentModel.IContainer components;


        /// <summary>
        /// This control is to be placed on a form that enables printing.
        /// It has four buttons: PageSetup, PrintPreview, Cancel and Ok.
        /// Ok will launch the normal PrintDialog for choosing a printer.
        /// </summary>
        /// <remarks>
        /// Simply instantiate this control on a form.
        /// Set the Document property to a valid PrintDocument.
        /// Optionally, subscribe to the event Printing to be notified
        /// of a print job before it is started.
        /// </remarks>
    	public PrintControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.btnPreview = new System.Windows.Forms.Button();
            this.printLogic1 = new ReportPrinting.PrintLogic();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnPageSetup = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(112, 8);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.TabIndex = 1;
            this.btnPreview.Text = "Pre&view";
            this.btnPreview.Click += new System.EventHandler(this.Preview);
            this.toolTip1.SetToolTip(this.btnPreview, "Preivew the print job.");
            // 
            // printLogic1
            // 
            this.printLogic1.Document = null;
            this.printLogic1.PrintInBackground = false;
            this.printLogic1.Printing += new System.EventHandler(this.printLogic1_Printing);
            this.printLogic1.Printed += new System.EventHandler(this.printLogic1_Printed);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(304, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.toolTip1.SetToolTip(this.btnCancel, "Quit this dialog without printing.");
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(208, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&Print";
            this.btnOK.Click += new System.EventHandler(this.Print);
            this.toolTip1.SetToolTip(this.btnOK, "Print the selected document.");
            // 
            // btnPageSetup
            // 
            this.btnPageSetup.Location = new System.Drawing.Point(8, 8);
            this.btnPageSetup.Name = "btnPageSetup";
            this.btnPageSetup.Size = new System.Drawing.Size(88, 23);
            this.btnPageSetup.TabIndex = 0;
            this.btnPageSetup.Text = "Page &Setup";
            this.btnPageSetup.Click += new System.EventHandler(this.PageSetup);
            this.toolTip1.SetToolTip(this.btnPageSetup, "Setup page and printer settings.");
            // 
            // PrintControl
            // 
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.btnPreview,
                                                                          this.btnCancel,
                                                                          this.btnOK,
                                                                          this.btnPageSetup});
            this.Name = "PrintControl";
            this.Size = new System.Drawing.Size(384, 40);
            this.ResumeLayout(false);

        }
		#endregion


        private bool hideOnPrint = true;


        #region "Properties"

        /// <summary>
        /// Gets or sets the PrintDocument used by the dialog.
        /// </summary>
        [Description("Gets or sets the PrintDocument used by the dialog.")]
        public PrintDocument Document
        {
            get { return this.printLogic1.Document; }
            set { this.printLogic1.Document = value; }
        }


        /// <summary>
        /// The progress of the print job is shown in a status dialog.
        /// Default is true.
        /// When true, the PrintControllerWithStatusDialog is used.  Otherwise,
        /// a StandardPrintController is used for printing.
        /// </summary>
        [Description("The progress of the print job is shown in a status dialog."),
            DefaultValue(true)]
        public bool ShowStatusDialog
        {
            get { return this.printLogic1.ShowStatusDialog; }
            set { this.printLogic1.ShowStatusDialog = value; }
        }


        /// <summary>
        /// Indicates that printing should be done in the background.
        /// Default is true.
        /// </summary>
        [Description("Indicates that printing should be done in the background."),
        DefaultValue(false)]
        public bool PrintInBackground
        {
            get { return this.printLogic1.PrintInBackground; }
            set { this.printLogic1.PrintInBackground = value; }
        }


        /// <summary>
        /// Indicates that a print job is currently in progress.
        /// </summary>
        [Browsable(false)]
        public bool PrintInProgress
        {
            get { return this.printLogic1.PrintInProgress; }
        }

        /// <summary>
        /// Gets or sets a flag indicating the parent of
        /// this control will be hidden when OK or Cancel
        /// is hit for this control.
        /// </summary>
        [Description("Indicates that the parent of this control will be hidden when Print is clicked."),
        DefaultValue(false)]
        public bool HideOnPrint
        {
            get { return this.hideOnPrint; }
            set { this.hideOnPrint = value; }
        }

        #endregion


        #region "Events"
        /// <summary>
        /// This event is raised prior to printing.  
        /// It allows user code to setup for the printing.
        /// Perhaps disable any Print buttons, etc.
        /// </summary>
        public event EventHandler Printing;

        /// <summary>
        /// This event is raised after printing.
        /// It allows user code to cleanup after printing.
        /// Perhaps enable any Print buttons, etc.
        /// </summary>
        public event EventHandler Printed;


        #endregion
		

        #region "Public printing methods"

        /// <summary>
        /// Called for a PageSetup
        /// </summary>
        public void PageSetup(object sender, System.EventArgs e)
        {
            this.printLogic1.PageSetup (sender, e);
        }

        /// <summary>
        /// Call for a preview
        /// </summary>
        public void Preview (object sender, System.EventArgs e)
        {
            this.printLogic1.Preview (sender, e);
        }

        /// <summary>
        /// Call to print
        /// </summary>
        public virtual void Print (object sender, System.EventArgs e)
        {
            this.printLogic1.Print (sender, e);
        }
        #endregion


        /// <summary>
        /// The Cancel button has been clicked.  
        /// (Standard button click event handler).
        /// Hides the parent form.
        /// </summary>
        public virtual void btnCancel_Click(object sender, System.EventArgs e)
        {
            if (this.hideOnPrint)
            {
                this.ParentForm.Hide();
            }
        }

        private void printLogic1_Printed(object sender, System.EventArgs e)
        {
            if (this.Printed != null)
            {
                this.Printed (sender, e);
            }
        }

        private void printLogic1_Printing(object sender, System.EventArgs e)
        {
            if (this.Printing != null)
            {
                this.Printing (sender, e);
            }
        }


	}
}
