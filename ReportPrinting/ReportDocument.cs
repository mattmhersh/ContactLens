// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/


// Awhile back, it was noticed that the nicely centered and layed-out reports
// that look great in PrintPreview are often off-center when actually printed.
// I added code that uses PInvoke to get to the actual printable section of the
// page - but it uses PInvoke and some other logic that I don't COMPLETELY understand.
// Seting this #define will bypass that logic and use the "normal" .Net method of
// getting margins (that doesn't quite work on the page correctly).
//#define BYPASS_PRINTABLE_AREA_LOGIC

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
    /// ReportDocument extends from <see cref="System.Drawing.Printing.PrintDocument"/>
    /// and is customized for printing reports from one or more tables of data. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// A ReportDocument is used just like <see cref="System.Drawing.Printing.PrintDocument"/>
    /// when used with other printing framework classes, such as <see cref="System.Windows.Forms.PrintDialog"/>
    /// or <see cref="System.Windows.Forms.PrintPreviewDialog"/>
    /// </para>
    /// <para>
    /// A ReportDocument object is the top level container for all the 
    /// sections that make up the report.  (This consists of a header, body, and footer.)
    /// </para>
    /// <para>
    /// The ReportDocument's main job is printing, which occurs when the
    /// Print() method is called of the base class.   The Print() method 
    /// iterates through all the ReportSections making up the document, \
    /// printing each one.
    /// </para>
    /// <para>
    /// The strategy design pattern is employed for formatting the report.
    /// An object implementing <see cref="ReportPrinting.IReportMaker"/>
    /// may be associated with the ReportDocument. This IReportMaker 
    /// object is application specific and knows how to create a report
    /// based on application state and user settings. This object would be
    /// responsible for creating sections, associating DataViews, 
    /// and applying any required styles through use of the 
    /// <see cref="ReportPrinting.TextStyle"/> class.  It will generally
    /// use the <see cref="ReportPrinting.ReportBuilder"/> class to
    /// assist with the complexity of building a report.
    /// </para>
    /// </remarks>
    public class ReportDocument : System.Drawing.Printing.PrintDocument
    {



        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ReportDocument()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            this.resetAfterPrint = true;
            DocumentUnit = GraphicsUnit.Inch;
            ResetPens();
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
            components = new System.ComponentModel.Container();
        }
        #endregion


        IReportMaker reportMaker;
        float pageHeaderMaxHeight;
        float pageFooterMaxHeight;
        ReportSection pageHeader;
        ReportSection pageFooter;
        ReportSection body;
        private int currentPage;
        int totalPages;
        bool pagesWereCounted;
        bool countPages;
        bool resetAfterPrint;
		bool forPreview;
		bool noPrint;
		Pen normalPen;
        Pen thinPen;
        Pen thickPen;
        GraphicsUnit documentUnit;
        Hashtable pageOrientations;
        string strStartDate;
        string strEndDate;
        string strFontSize;

        #region "Properties"

        public string StartDate
        {
            get { return this.strStartDate; }
            set { this.strStartDate = value; }
        }

        public string EndDate
        {
            get { return this.strEndDate; }
            set { this.strEndDate = value; }
        }

        public string FontSize
        {
            get { return this.strFontSize; }
            set { this.strFontSize = value; }
        }

        /// <summary>
        /// An object that will setup this report before printing.
        /// (Strategy pattern).
        /// </summary>
        [DefaultValue(null)]
        public IReportMaker ReportMaker
        {
            get { return this.reportMaker; }
            set { this.reportMaker = value; }
        }

        /// <summary>
        /// Used to define the size of the page header
        /// The header will stop at or before this distance from the top margin of the page
        /// </summary>
        [Description("The maximum height allowed for the page header."), DefaultValue(0f)]
        public float PageHeaderMaxHeight
        {
            get { return this.pageHeaderMaxHeight; }
            set { this.pageHeaderMaxHeight = value; }
        }

        /// <summary>
        /// Used to define the size of the page footer
        /// The footer will start at this distance from the bottom margin of the page
        /// </summary>
        [Description("The maximum height allowed for the page footer."), DefaultValue(0f)]
        public float PageFooterMaxHeight
        {
            get { return this.pageFooterMaxHeight; }
            set { this.pageFooterMaxHeight = value; }
        }

        /// <summary>
        /// Returns the current page number
        /// </summary>
        /// <returns>Integer for the current page number</returns>
        [Browsable(false)]
        public int GetCurrentPage()
        {
            return currentPage;
        }

        /// <summary>
        /// Gets the total number of pages in the document.
        /// Only returns non-zero if CountPages is true.
        /// </summary>
        /// <returns>Total number of pages</returns>
        [Browsable(false)]
        public int TotalPages
        {
            get { return this.totalPages; }
        }

		/// <summary>
		/// Gets or sets the flag
		/// indicating that a first pass should be done to count the pages.
		/// This allows 'Page # of #' using the TotalPages property.
		/// </summary>
		[DefaultValue(false)]
		public bool CountPages
		{
			get { return this.countPages; }
			set { this.countPages = value; }
		}

		/// <summary>
        /// Gets or sets the flag
        /// indicating all sections are cleared after printing.
        /// This allows the next print of a different document to assume
        /// a clear document, and releas memory.
        /// </summary>
        [DefaultValue(true)]
        public bool ResetAfterPrint
        {
            get { return this.resetAfterPrint; }
            set { this.resetAfterPrint = value; }
        }

		/// <summary>
		/// Gets or sets the flag
		/// indicating whether this document is being previewed.
		/// This allows the 'hard' margins to be put back to improve display.
		/// </summary>
		[DefaultValue(false)]
		public bool ForPreview
		{
			get { return this.forPreview; }
			set { this.forPreview = value; }
		}

		/// <summary>
		/// Gets the flag
		/// indicating whether the current page is to be printed.
		/// This stops any Graphics being produced when skipping pages, or counting pages.
		/// </summary>
		[DefaultValue(false)]
		public bool NoPrint
		{
			get { return this.noPrint; }
		}

		/// <summary>
        /// The ReportSection responsible for printing the page header.
        /// </summary>
        [Browsable(false)]
        public ReportSection PageHeader
        {
            get { return this.pageHeader; }
            set { this.pageHeader = value; }
        }

        /// <summary>
        /// The ReportSection reponsible for printing the page footer.
        /// </summary>
        [Browsable(false)]
        public ReportSection PageFooter
        {
            get { return this.pageFooter; }
            set { this.pageFooter = value; }
        }

        /// <summary>
        /// The ReportSection responsible for printing the page body.
        /// </summary>
        [Browsable(false)]
        public ReportSection Body
        {
            get { return this.body; }
            set { this.body = value; }
        }

        /// <summary>
        /// Gets the pen for normal styled lines
        /// </summary>
        [Browsable(false)]
        public Pen NormalPen
        {
            get { return this.normalPen; }
        }
        /// <summary>
        /// Gets the pen for thin lines
        /// </summary>
        [Browsable(false)]
        public Pen ThinPen
        {
            get { return this.thinPen; }
        }
        /// <summary>
        /// Gets the pen for thick lines
        /// </summary>
        [Browsable(false)]
        public Pen ThickPen
        {
            get { return this.thickPen; }
        }

        /// <summary>
        /// Gets or sets the units used for the entire document.  All
        /// margins, widths and heights should be expressed in this unit.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Most development work has been done in inches. 
        /// Millimeter and DocumentUnit have also been tested.
        /// Some notes on specifiying units other than inches:
        /// The GraphicsUnit is one of the first things that
        /// should be setup - even before a ReportBuilder is created.
        /// </para>
        /// <para>
        /// Default pen widths in ReportDocument will be
        /// changed by setting this property.
        /// </para>
        /// <para>
        /// These are the supported document units, and the expected
        /// size of that unit in inches.
        /// </para>
        /// <code>
        /// GraphicsUnit.Inch:       scale = 1f;
        /// GraphicsUnit.Millimeter: scale = 25.4f;
        /// GraphicsUnit.Display:    scale = 75f;
        /// GraphicsUnit.Document:   scale = 300f;
        /// GraphicsUnit.Point:      scale = 72f;
        /// </code>
        /// <para>
        /// Other units are not supported and will cause an exception.
        /// </para>
        /// </remarks>
        [DefaultValue(GraphicsUnit.Inch)]
        public virtual GraphicsUnit DocumentUnit
        {
            get { return this.documentUnit; }
            set 
            { 
                this.documentUnit = value; 
                ResetPens();
            }
        }
        #endregion

        /// <summary>
        /// Sets the orientation to either landscape or portrait from the
        /// specified page onwards (or until the next page with a SetPageOrientation is set).
        /// </summary>
        /// <param name="page">Number of the page to set the orientation</param>
        /// <param name="orient">Orientation for the page</param>
        public virtual void SetPageOrientation (int page, PageOrientation orient)
        {
            this.pageOrientations[page] = orient;
        }

        /// <summary>
        /// Removes a specified orientation for a page (and pages onward) and resets
        /// to either the default orientation, or the last specified orientation.
        /// </summary>
        /// <param name="page">Number of the page to clear the orientation</param>
        public virtual void ClearPageOrientation (int page)
        {
            if (this.pageOrientations.ContainsKey(page))
            {
                this.pageOrientations.Remove (page);
            }
        }



        /// <summary>
        /// Resets pens back to default values
        /// Black, and .08", .03" and .01" for thick, normal, and thin
        /// (assuming inches as the scale - these are scaled if units are changed)
        /// </summary>
        public virtual void ResetPens()
        {
            float scale = GetScale();
            thinPen = new Pen (Color.Black, 0.01f * scale);
            normalPen = new Pen (Color.Black, 0.03f * scale);
            thickPen = new Pen (Color.Black, 0.08f * scale);
        }


        /// <summary>
        /// Reset the row and page count before printing
        /// </summary>
        /// <param name="e">PrintEventArgs</param>
        protected override void OnBeginPrint(System.Drawing.Printing.PrintEventArgs e)
        {
            this.pagesWereCounted = false;
            this.totalPages = 0;
            reset();
        }

        void reset()
        {
            this.pageOrientations = new Hashtable();
            if (this.ReportMaker != null)
            {
				if(this.countPages)
				{
					if(!this.pagesWereCounted)
						this.ReportMaker.MakeDocument(this, strFontSize, strStartDate, strEndDate);
				}
				else
				{
                    this.ReportMaker.MakeDocument(this, strFontSize, strStartDate, strEndDate);
				}
            }
            if (this.Body != null)
            {
                this.Body.Reset();
            }
            if (this.PageFooter != null)
            {
                this.PageFooter.Reset();
            }
            if (this.PageHeader != null)
            {
                this.PageHeader.Reset();
            }

            this.currentPage = 0;
            //this.bodyBeginPrintCalled = false;
        }

        /// <summary>
        /// This is used to scale page margins (and other quantities)
        /// from inches to the GraphicsUnit specified.
        /// </summary>
        /// <returns>A floating point number which can be multiplied by
        /// a unit in inches to get a unit in the current DocumentUnit.
        /// </returns>
        public virtual float GetScale()
        {
            float scale = 1f;
            switch (DocumentUnit)
            {
                case GraphicsUnit.Inch:
                    scale = 1f;
                    break;
                case GraphicsUnit.Millimeter:
                    scale = 25.4f;
                    break;
                case GraphicsUnit.Display:
                    scale = 75f;
                    break;
                case GraphicsUnit.Document:
                    scale = 300f;
                    break;
                case GraphicsUnit.Point:
                    scale = 72f;
                    break;
                default:
                    throw new ApplicationException ("GraphicsUnit not supported: " + DocumentUnit);
            }
            return scale;
        }

        /// <summary>
        /// The actual method to print a page
        /// </summary>
        /// <param name="e">PrintPageEventArgs</param>
        /// <returns>True if there are more pages</returns>
        protected virtual bool PrintAPage (PrintPageEventArgs e)
        {
			Graphics g = e.Graphics;
			g.PageUnit = DocumentUnit;

			// Define page bounds (margins are always in inches)
			float scale = GetScale() / 100;
#if BYPASS_PRINTABLE_AREA_LOGIC
			// Bypass "tricky" page margins that take into account printer limits
			float leftMargin = e.MarginBounds.Left * scale;
			float topMargin = e.MarginBounds.Top * scale;
			float rightMargin = e.MarginBounds.Right * scale;
			float bottomMargin = e.MarginBounds.Bottom * scale;
			Bounds pageBounds = new Bounds(leftMargin, topMargin, rightMargin, bottomMargin);
// May need this next few lines ...
//			if (e.PageSettings.Landscape)
//			{
//				pageBounds = new Bounds(topMargin, leftMargin, bottomMargin, rightMargin);
//			} 
#else
			IntPtr hDc = e.Graphics.GetHdc();
			PrinterMarginInfo mi = new PrinterMarginInfo(hDc.ToInt32(), e.PageSettings.Landscape);
			e.Graphics.ReleaseHdc(hDc);
			Bounds pageBounds = mi.GetBounds(e.PageBounds, e.MarginBounds, scale, true, forPreview);
#endif
            // Header
            if (this.PageHeader != null)
            {
                Bounds headerBounds = pageBounds;
                if (this.PageHeaderMaxHeight > 0)
                {
                    headerBounds.Limit.Y = headerBounds.Position.Y + this.PageHeaderMaxHeight;
                }
                this.PageHeader.Print(this, g, headerBounds);
                pageBounds.Position.Y += this.PageHeader.Size.Height;
            }
           
            // Footer
            if (this.PageFooter != null)
            {
                Bounds footerBounds = pageBounds;
                if (this.PageFooterMaxHeight > 0)
                {
                    footerBounds.Position.Y = footerBounds.Limit.Y - this.PageFooterMaxHeight;
                }
                this.PageFooter.CalcSize (this, g, footerBounds);
                footerBounds = footerBounds.GetBounds (this.PageFooter.Size,
                    this.PageFooter.HorizontalAlignment, this.PageFooter.VerticalAlignment);
                this.PageFooter.Print (this, g, footerBounds);
                pageBounds.Limit.Y -= this.PageFooter.Size.Height;
            }

            // Body
            if (this.Body != null)
            {
                this.Body.Print(this, g, pageBounds);
                e.HasMorePages = this.Body.Continued;
            }
            else
            {
                e.HasMorePages = false;
            }
            return e.HasMorePages;
        } // OnPrintPage

        /// <summary>
        /// Overrided OnPrintPage from PrintDocument. 
        /// This method, on first call, may count the pages.
        /// Then it will simple call PrintAPage on every
        /// call.
        /// </summary>
        /// <param name="e">PrintPageEventArgs</param>
        protected override void OnPrintPage (PrintPageEventArgs e)
        {
			if (this.countPages && !this.pagesWereCounted)
			{
				this.pagesWereCounted = true;
				this.totalPages = 1;
				this.noPrint = true;
				try
				{
					while (PrintAPage(e))
					{
						this.totalPages++;
						this.currentPage++;
						SetOrientation(e, this.currentPage + 1);
					}
				}
				finally
				{
					this.noPrint = false;
					this.reset();
				}
			}

			this.currentPage++; // preincrement, so the first page is page 1
			
//			// Size pages before the range start
			while (this.currentPage < this.PrinterSettings.FromPage)
			{
				this.noPrint = true;
				PrintAPage(e);
				this.currentPage++;
			}
			this.noPrint = false;

			PrintAPage(e);
			
			SetOrientation (e, this.currentPage + 1);
			
			if (this.PrinterSettings.ToPage > 0 && this.currentPage >= this.PrinterSettings.ToPage)
				e.HasMorePages = false;
        }

		/// <summary>
		/// Set the page orientation
		/// </summary>
		/// <param name="e"></param>
		/// <param name="page">the page number to orientate</param>
        protected virtual void SetOrientation (PrintPageEventArgs e, int page)
        {
            // Setup pageSettings for next page
            if (this.pageOrientations.ContainsKey(page))
            {
                PageOrientation orient = (PageOrientation)this.pageOrientations[page];
                switch (orient)
                {
                    case PageOrientation.Portrait:
                        e.PageSettings.Landscape = false;
                        break;
                    case PageOrientation.Landscape:
                        e.PageSettings.Landscape = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Called at the end of printing.  If ResetAfterPrint is set (default is true)
        /// then all text sections will be released after printing.
        /// </summary>
        /// <param name="e">PrintEventArgs</param>
        protected override void OnEndPrint(System.Drawing.Printing.PrintEventArgs e)
        {
            if (this.ResetAfterPrint)
            {
                // reset stuff
                this.PageHeader = null;
                this.PageFooter = null;
                this.Body = null;
                this.PageHeaderMaxHeight = 0F;
                this.PageFooterMaxHeight = 0F;
				this.forPreview = false;
				this.PrinterSettings.FromPage = 0;
				this.PrinterSettings.ToPage = 0;

                // TODO: Get rid of this:
                DocumentUnit = GraphicsUnit.Inch;
            }
        }

    } // class


    /// <summary>
    /// Defines the orientation for a given page
    /// </summary>
    public enum PageOrientation
    {
        /// <summary>
        /// The page should be printed in Portrait layout (vertically)
        /// </summary>
        Portrait = 0,
        /// <summary>
        /// The page should be printed in Landscape layout (horizontally)
        /// </summary>
        Landscape = 1
    }

}
