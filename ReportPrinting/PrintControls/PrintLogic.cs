using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace ReportPrinting
{
	/// <summary>
	/// This control handles the logic required for printing.
	/// It doesn't provide much to the end user, it just simply
	/// wraps a PageSettings dialog, a PrintPreview dialog, and
	/// a PrintDialog so that you don't have to instantiate all three.
	/// </summary>
	/// <remarks>
	/// This class is used as the logic behind the PrintControl control
	/// and the PrintControlToolbar control.
	/// See the RichTextEdit sample for an example of using it.
	/// </remarks>
	[DefaultProperty("Document"), DefaultEvent("Printing")]
	public class PrintLogic : System.ComponentModel.Component
	{

        PrintDocument document;
        bool showStatusDialog = true;
        bool printInBackground = false;
        bool printInProgress = false;
		bool previewing = false;
		PageSetupDialog pageSetupDialog1 = new PageSetupDialog ();
        PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog ();
        PrintDialog printDialog1 = new PrintDialog ();


        #region "Properties"
        /// <summary>
        /// Gets or sets the PrintDocument used by the dialog.
        /// </summary>
        [Description("Gets or sets the PrintDocument used by the dialog.")]
        public PrintDocument Document
        {
            get { return this.document; }
            set { this.document = value; }
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
            get { return this.showStatusDialog; }
            set { this.showStatusDialog = value; }
        }


        /// <summary>
        /// Indicates that printing should be done in the background.
        /// Default is true.
        /// </summary>
        [Description("Indicates that printing should be done in the background."),
        DefaultValue(false)]
        public bool PrintInBackground
        {
            get { return this.printInBackground; }
            set { this.printInBackground = value; }
        }


        /// <summary>
        /// Indicates that a print job is currently in progress.
        /// </summary>
        [Browsable(false)]
        public bool PrintInProgress
        {
            get { return this.printInProgress; }
        }

		/// <summary>
		/// Indicates that a print job is about to be previewed.
		/// </summary>
		[Browsable(false)]
		public bool Previewing
		{
			get { return this.previewing; }
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

        /// <summary>
        /// Called prior to printing.  It raises the Printing event.
        /// </summary>
        protected virtual void onPrinting()
        {
            if (Printing != null)
            {
                Printing(this, new EventArgs());
            }
        }

        /// <summary>
        /// Called after printing.  It raises the Printed event.
        /// </summary>
        protected virtual void onPrinted()
        {
            if (Printed != null)
            {
                Printed(this, new EventArgs());
            }
        }

        #endregion

        /// <summary>
        /// Called for a PageSetup
        /// </summary>
        public virtual void PageSetup(object sender, System.EventArgs e)
        {
            if (!this.printInProgress)
            {
                this.pageSetupDialog1.Document = this.document;
                this.pageSetupDialog1.ShowDialog ();
            }
        }

        /// <summary>
        /// Call for a preview
        /// </summary>
        public virtual void Preview (object sender, System.EventArgs e)
        {
            if (!this.printInProgress)
            {
                this.printPreviewDialog1.Document = this.document;
				this.previewing = true;
				try
				{
					onPrinting();
				}
				finally
				{
					this.previewing = false;
				}
                this.printPreviewDialog1.ShowDialog();
            }
        }

        delegate void PrintInBackgroundDelegate();

        /// <summary>
        /// Call to print
        /// </summary>
        public virtual void Print (object sender, System.EventArgs e)
        {
            if (!this.printInProgress)
            {
                this.printDialog1.Document = this.document;
				this.printDialog1.AllowSomePages = true;
				DialogResult result = this.printDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    PrintController printController;
                    printController = new StandardPrintController();

                    if (this.ShowStatusDialog)
                    {
                        this.document.PrintController = new PrintControllerWithStatusDialog(
                            printController, "Please wait...");
                    }
                    else
                    {
                        this.document.PrintController = printController;
                    }

                    onPrinting();
                    if (this.PrintInBackground)
                    {
                        // disable buttons or else the user could get into trouble!
                        PrintInBackgroundDelegate d = new PrintInBackgroundDelegate(
                            BeginBackgroundPrint);
                        d.BeginInvoke(new AsyncCallback(PrintInBackgroundComplete), null);
                    }
                    else
                    {
                        this.printInProgress = true;
                        this.document.Print();
                        this.printInProgress = false;
                        onPrinted();
                    }

                }
            }
        }

        /// <summary>
        /// Start a background print job
        /// </summary>
        protected virtual void BeginBackgroundPrint()
        {
            this.printInProgress = true;
            this.document.Print();
        }

        /// <summary>
        /// A background print job is complete
        /// </summary>
        protected virtual void PrintInBackgroundComplete(IAsyncResult r)
        {
            this.printInProgress = false;
            onPrinted();
        }




	}
}
