// This class contributed by Mike Bon
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
    /// for printing into a ToolBar
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
    ///	
    ///	</para>
    /// </remarks>
    /// 
    /// <example>
    /// To use the print control, place it on a form.
    /// Set the Document property to a valid PrintDocument
    /// (it doesn’t have to be a ReportDocument).  That’s it!
    /// </example>
    public class PrintControlToolBar : System.Windows.Forms.UserControl
	{

        private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ToolBar PrintToolBar;
		private System.Windows.Forms.ToolBarButton SetupButton;
		private System.Windows.Forms.ToolBarButton PreviewButton;
		private System.Windows.Forms.ToolBarButton PrintButton;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ToolBarButton CancelButton;
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
    	public PrintControlToolBar()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            ShowCancelButton = false;
            ShowTextLabels = true;
		}

        private bool hideOnPrint = false;


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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PrintControlToolBar));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.PrintToolBar = new System.Windows.Forms.ToolBar();
            this.SetupButton = new System.Windows.Forms.ToolBarButton();
            this.PreviewButton = new System.Windows.Forms.ToolBarButton();
            this.PrintButton = new System.Windows.Forms.ToolBarButton();
            this.CancelButton = new System.Windows.Forms.ToolBarButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.printLogic1 = new ReportPrinting.PrintLogic();
            this.SuspendLayout();
            // 
            // PrintToolBar
            // 
            this.PrintToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
                                                                                            this.SetupButton,
                                                                                            this.PreviewButton,
                                                                                            this.PrintButton,
                                                                                            this.CancelButton});
            this.PrintToolBar.Divider = false;
            this.PrintToolBar.DropDownArrows = true;
            this.PrintToolBar.ImageList = this.imageList1;
            this.PrintToolBar.Name = "PrintToolBar";
            this.PrintToolBar.ShowToolTips = true;
            this.PrintToolBar.Size = new System.Drawing.Size(210, 36);
            this.PrintToolBar.TabIndex = 4;
            this.PrintToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.PrintToolBar_ButtonClick);
            // 
            // SetupButton
            // 
            this.SetupButton.ImageIndex = 0;
            this.SetupButton.ToolTipText = "Printer Setup";
            // 
            // PreviewButton
            // 
            this.PreviewButton.ImageIndex = 1;
            this.PreviewButton.ToolTipText = "Print Preview";
            // 
            // PrintButton
            // 
            this.PrintButton.ImageIndex = 2;
            this.PrintButton.ToolTipText = "Print";
            // 
            // CancelButton
            // 
            this.CancelButton.ImageIndex = 3;
            this.CancelButton.ToolTipText = "Cancel Printing";
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 15);
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Silver;
            // 
            // printLogic1
            // 
            this.printLogic1.Document = null;
            this.printLogic1.Printing += new System.EventHandler(this.printLogic1_Printing);
            this.printLogic1.Printed += new System.EventHandler(this.printLogic1_Printed);
            // 
            // PrintControlToolBar
            // 
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.PrintToolBar});
            this.Name = "PrintControlToolBar";
            this.Size = new System.Drawing.Size(210, 40);
            this.ResumeLayout(false);

        }
		#endregion

        void SetLabels()
        {
            if (this.showText)
            {
                this.SetupButton.Text = "&Setup";
                this.PreviewButton.Text = "Pre&view";
                this.PrintButton.Text = "&Print";
                this.CancelButton.Text = "&Cancel";
            }
            else
            {
                this.SetupButton.Text = String.Empty;
                this.PreviewButton.Text = String.Empty;
                this.PrintButton.Text = String.Empty;
                this.CancelButton.Text = String.Empty;
            }
        }

        bool showText;
        
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


        /// <summary>
        /// Property to provide access to ToolBar 
        /// </summary>
        public ToolBar ToolBar
        {
            get { return PrintToolBar; }
        }

        /// <summary>
        /// Indicates that the Cancel Button should appear.
        /// </summary>
        [Description("The cancel button should be shown"),
        DefaultValue(false)]
        public bool ShowCancelButton
        {
            get 
            {
                return this.ToolBar.Buttons[3].Visible;
            }
            set 
            {
                this.ToolBar.Buttons[3].Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a flag to show the text labels
        /// </summary>
        [Description("The text labels in the toolbar should be shown"),
        DefaultValue(true)]
        public bool ShowTextLabels
        {
            get { return this.showText; }
            set 
            { 
                this.showText = value; 
                SetLabels ();
            }
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
		/// A button in the Tool Bar has been Pressed
		/// </summary>
		/// <param name="sender">The sender of the object</param>
		/// <param name="e">Event Args</param>
		private void PrintToolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			switch (PrintToolBar.Buttons.IndexOf(e.Button))
			{			
				case 0 :  //Page Setup
				{
					PageSetup (this, null);
					break;
				}
				case 1 :  //Preview
				{
                    Preview (this, null);
					break;
				}	
				case 2 :  //Print
				{
                    Print (this, null);
                    break;
				}				
				case 3 :  //Cancel
				{
					btnCancel_Click();
					break;
				}			
			}

		}	

        /// <summary>
        /// The Cancel button has been clicked.  (Standard button click event handler).
        /// Closes the parent form.
        /// </summary>
        public virtual void btnCancel_Click()
        {
            if (this.hideOnPrint)
            {
                this.ParentForm.Hide();
            }
        }

        private void printLogic1_Printing(object sender, System.EventArgs e)
        {
            if (this.Printing != null)
            {
                this.Printing (sender, e);
            }
        }

        private void printLogic1_Printed(object sender, System.EventArgs e)
        {
            if (this.Printed != null)
            {
                this.Printed (sender, e);
            }
        }


	}
}
