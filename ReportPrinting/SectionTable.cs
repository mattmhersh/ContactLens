// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/

using System;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Data;
using System.Diagnostics;

namespace ReportPrinting
{
	/// <summary>
	/// A ReportSection that represents the printing of a table 
	/// from a DataView.
	/// </summary>
	/// <remarks>
	/// <para>
    /// SectionTable contains a DataView and
    /// zero or more IDataColumns that that correspond 
    /// to the columns of data to be printed from the DataView.
    /// </para>
    /// <para>
    /// A header row can be printed at the top of the table
    /// (and every new page) based on the HeaderText for
    /// each IDataColumn object (see the RepeatHeaderRow and
    /// SuppressHeaderRow properties). A summary row can be 
    /// printed at the bottom of the table (see ShowSummaryRow 
    /// property of this class and the SummaryRowText property and
    /// FormatSummaryRow event of ReportDataColumn).
    /// </para>
	/// <para>
	/// The width of the table is defined as the sum of the widths of all columns
	/// (plus the width of any borders). There are several ways to set the width
	/// of the table on the printed page.
	/// </para>
	/// <list type="bullet">
	/// <item><description>
	/// Set the property Width or MaxWidth for each ReportDataColumn (if
	/// no auto-sizing is used, these are identical).
	/// </description></item>
	/// <item><description>
	/// Set the property MaxWidth for each ReportDataColumn and also set 
	/// one or both of the properties SizeWidthToHeader and SizeWidthToContents to true
	/// for the columns. Then, the each column will be sized to the appropriate width
	/// base on the contents.
	/// </description></item>
	/// <item><description>
	/// Finally, the width of the entire table can be rescaled to a certain percentage
	/// of the page (using the PercentWidth property of SectionTable). To do this,
	/// first use one of the above methods for setting the widths of each column. Then
	/// set PercentWidth (e.g 100 for 100% of the usable page width) and each column
	/// will be scaled to make the total table fit that width.
	/// </description></item>
	/// </list>
	/// <para>
	/// Notes: UseFullWidth and UseFullHeight doen't affect the actual table 
	/// being printed (but do affect the placement of other sections).
	/// </para>
	/// <para>
    /// Margins and HorizontalAlignment are specified the same
    /// as for any other ReportSection. <b>VerticalAlignment is
    /// not implemented.</b>
    /// </para>
    /// <para>
    /// Setting borders also deserves a note. There are 6 borders that can be
    /// set for the entire SectionTable: the four lines around the outside 
    /// Top, Left, Right, and Bottom, 
    /// the line under the header row, and the line between every other row. 
    /// Each column also has a border that can be set for its right-side (the vertical
    /// lines between columns).
    /// Each of these are specified with a .Net pen, which has a width and a color.
    /// </para>
    /// </remarks>
	public class SectionTable : ReportPrinting.ReportSection
	{
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSource">A DataView to use as the source of data</param>
		public SectionTable(DataView dataSource)
		{
            this.DataSource = dataSource;
			this.RowFilter = dataSource.RowFilter;
			this.borderPens = new BorderPens();
            this.headerTextStyle = new TextStyle (TextStyle.TableHeader);
            this.detailRowTextStyle = new TextStyle (TextStyle.TableRow);
            this.alternatingRowTextStyle = new TextStyle (this.detailRowTextStyle);
			this.rowHeights = new ArrayList (dataSource.Count);
		}

        /// <summary>
        /// The row number used to indicate the HeaderRow
        /// </summary>
        const int HeaderRowNumber = -1;

        /// <summary>
        /// The current/next row to be printed.
        /// </summary>
        protected int rowIndex;

        /// <summary>
        /// The number of data rows that will fit on the next call to print.
        /// </summary>
		protected int dataRowsFit;

		/// <summary>
		/// The minimum number of data rows that must fit. 
		/// This is a reduced requirement from the KeepTogether section property.
		/// </summary>
		int minDataRowsFit;

		/// <summary>
		/// The height of the table on this page
		/// </summary>
		float tableHeightForPage = 0f;

        /// <summary>
        /// Heights of each row that will be printed next...
        /// </summary>
        ArrayList rowHeights;
		float headerRowHeight = 0f;
		//float summaryRowHeight = 0f;

		bool repeatHeaderRow;
		bool suppressHeaderRow;
		bool showSummaryRow;

        DataView dataSource;
		string rowFilter;
		float minHeaderRowHeight = 0F;
        float minDetailRowHeight = 0F;
        float maxHeaderRowHeight = 8F; // inches default
        float maxDetailRowHeight = 8F; // inches default

        TextStyle headerTextStyle;
        TextStyle detailRowTextStyle;
        TextStyle alternatingRowTextStyle;

        /// <summary>
        /// Size of header
        /// </summary>
        bool headerSizeInit;

        /************
         * Pens used for lines
         */
        BorderPens borderPens;
        Pen innerPenHeaderBottom;
        Pen innerPenRow;

        float percentWidth;
		bool  allowMultiPageWidth;
		ArrayList infoForPages;
		int currentHorizPage;


		SizeF HeaderSize
		{
			get { return new SizeF (GetPageInfo().Width, this.headerRowHeight); }
		}

		/// <summary>
		/// Gets current page info
		/// </summary>
		PageInfo GetPageInfo ()
		{
			return (PageInfo) this.infoForPages[this.currentHorizPage];
		}




        #region "Properties and fields"

        /// <summary>
        /// OBSOLETE - RENAMED RepeatHeaderRow, see also SuppressHeaderRow
        /// A header row should be printed at the top of
        /// the table on every page.
        /// If false, the headerow is only printed on the first page.
        /// </summary>
        [Obsolete("For clarity, this property has been renamed RepeatHeaderRow.")]
        public bool ShowHeaderRow
        {
            get { return this.repeatHeaderRow; }
            set { this.repeatHeaderRow = value; }
        }

		/// <summary>
		/// Gets or sets the flag to repeat the header row at the 
		/// toop of the table of every page.
		/// If false, then just the first page has a header.
		/// </summary>
		public bool RepeatHeaderRow
		{
			get { return this.repeatHeaderRow; }
			set { this.repeatHeaderRow = value; }
		}

		/// <summary>
		/// Gets or sets the flag to suppress headers.
		/// If true, no header row is printed, and RepeatHeaderRow is ignored.
		/// </summary>
		public bool SuppressHeaderRow
		{
			get { return this.suppressHeaderRow; }
			set { this.suppressHeaderRow = value; }
		}

		/// <summary>
		/// Gets or sets a flag to show a summary row at the end of a table.
		/// If set to true, be sure to catch the FormatSummaryRow event of
		/// every DataColumn in order to format the summary row.
		/// </summary>
		public bool ShowSummaryRow
		{
			get { return this.showSummaryRow; }
			set { this.showSummaryRow = value; }
		}


        /// <summary>
        /// The DataView which represents the data to be shown
        /// in this section.
        /// </summary>
        public DataView DataSource
        {
            get { return this.dataSource; }
            set { this.dataSource = value; }
        }

		/// <summary>
		/// The RowFilter which represents needs to be applied to this DataView
		/// </summary>
		public string RowFilter
		{
			get { return this.rowFilter; }
			set { this.rowFilter = value; }
		}

		/// <summary>
        /// Gets the number of detail rows (rows in the DataView object)
        /// plus the summary row, if enabled.
        /// Equals 0 if there is no DataSource
        /// </summary>
        public int TotalRows
        {
            get
            {
                if (DataSource == null)
                {
                    return 0;
                }
                else
                {
					if (this.ShowSummaryRow)
					{
						return DataSource.Count + 1;
					}
					else
					{
						return DataSource.Count;
					}
                }
            }
        }

		/// <summary>
		/// The minimum number of detail rows that must fit. Default is zero.
		/// </summary>
		public int MinDataRowsFit
		{
			get { return minDataRowsFit; }
			set { minDataRowsFit = value; }
		}
        
        /// <summary>
        /// Used to determine the maximum size of a detail row
        /// any larger and it will be clipped at this size, possibly losing information
        /// </summary>
        public float MaxDetailRowHeight
        {
            get { return this.maxDetailRowHeight; }
            set { this.maxDetailRowHeight = value; }
        }

        /// <summary>
        /// Used to determine the minimum size of a detail row
        /// Even if the row is smaller, it will add empty space before
        /// the next row is printed
        /// </summary>
        public float MinDetailRowHeight
        {
            get { return this.minDetailRowHeight; }
            set { this.minDetailRowHeight = value; }
        }

        /// <summary>
        /// Used to determine the maximum size of a header row
        /// any larger and it will be clipped at this size, possibly losing information
        /// </summary>
        public float MaxHeaderRowHeight
        {
            get { return this.maxHeaderRowHeight; }
            set { this.maxHeaderRowHeight = value; }
        }

        /// <summary>
        /// Used to determine the minimum size of a header row
        /// Even if the row is smaller, it will add empty space before
        /// the next row is printed
        /// </summary>
        public float MinHeaderRowHeight
        {
            get { return this.minHeaderRowHeight; }
            set { this.minHeaderRowHeight = value; }
        }


        /// <summary>
        /// Gets or sets the pen used to draw the line at the top of the table
        /// </summary>
        public Pen OuterPenTop
        {
            get { return borderPens.Top; }
            set { borderPens.Top = value; }
        }
        /// <summary>
        /// Gets or sets the pen used to draw the line on the right side of the table
        /// </summary>
        public Pen OuterPenRight
        {
            get { return borderPens.Right; }
            set { borderPens.Right = value; }
        }
        /// <summary>
        /// Gets or sets the pen used to draw the line at the bottom of the table
        /// </summary>
        public Pen OuterPenBottom
        {
            get { return borderPens.Bottom; }
            set { borderPens.Bottom = value; }
        }
        /// <summary>
        /// Gets or sets the pen used to draw the line on the left side of the table
        /// </summary>
        public Pen OuterPenLeft
        {
            get { return borderPens.Left; }
            set { borderPens.Left = value; }
        }

        /// <summary>
        /// Sets the pens used on all outer sides (top, right, bottom, left)
        /// </summary>
        public Pen OuterPens
        {
            set
            {
                this.borderPens.Top = value;
                this.borderPens.Right = value;
                this.borderPens.Bottom = value;
                this.borderPens.Left = value;
            }
        }

        /// <summary>
        /// Gets or sets the pen used to draw the line under the header row
        /// </summary>
        public Pen InnerPenHeaderBottom
        {
            get { return innerPenHeaderBottom; }
            set { innerPenHeaderBottom = value; }
        }
        /// <summary>
        /// Gets or sets the pen used to draw the line under all other rows
        /// </summary>
        public Pen InnerPenRow
        {
            get { return innerPenRow; }
            set { innerPenRow = value; }
        }


        /// <summary>
        /// Gets or set the width of the table as a percentage of the 
        /// width of the parent section / container.
        /// Valid values are between 0 and 100, inclusive. 
        /// Default is 0, which means ignore this field.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If this value is 0, then the width of the table is generally
        /// the sum of widths of all columns.
        /// If this value is greater than 0, than the columns are all 
        /// re-sized so that the entire table fits the desired width.
        /// </para>
        /// <para>
        /// For example, consider a table with three columns of width 1 inch,
        /// 2 inches, and 1 inch to be printed in a "space" 6 inches wide.
        /// Setting PercentWidth to 0 will result in a table 4 inches (1 + 2 + 1) wide.
        /// Setting PercentWidth to 100 will result in a table 6 inches wide,
        /// with column widths 1.5, 3, and 1.5 inches.
        /// </para>
        /// </remarks>
        public float PercentWidth
        {
            get { return this.percentWidth; }
            set 
            {
                if (value >= 0 && value <= 100)
                {
                    this.percentWidth = value; 
                }
                else
                {
                    throw new ArgumentException ("PercentWidth must be between 0 and 100, inclusive");
                }
            }
        }

		/// <summary>
		/// Allow the table to span multiple pages in width - that is, not all
		/// columns are squeezed onto one page.
		/// </summary>
		public bool AllowMultiPageWidth
		{
			get { return this.allowMultiPageWidth; }
			set { this.allowMultiPageWidth = value; }
		}

		/// <summary>
        /// Gets the text style used for the header of this table.
        /// </summary>
        public TextStyle HeaderTextStyle
        {
            get { return this.headerTextStyle; }
        }

        /// <summary>
        /// Gets the text style used for the rows of this table.
        /// </summary>
        public TextStyle DetailRowTextStyle
        {
            get { return this.detailRowTextStyle; }
        }

        /// <summary>
        /// Gets the text style used for the alternating rows of this table.
        /// </summary>
        public TextStyle AlternatingRowTextStyle
        {
            get { return this.alternatingRowTextStyle; }
        }

        #endregion

        #region "Columns"
        /// <summary>
        /// The collection of columns used to format this SectionTable
        /// </summary>
        ArrayList columns = new ArrayList();

        /// <summary>
        /// Add a column object to the list of columns
        /// Each column object should be a new instance of IDataColumn
        /// You can pass a single instance into AddColumn more than once, but
        /// you will just get the same column printed out multiple times.
        /// </summary>
        /// <param name="rc">The IDataColumn to add</param>
        /// <returns>The number of columns</returns>
        public virtual int AddColumn(IDataColumn rc)
        {
            rc.MaxHeaderRowHeight = this.MaxHeaderRowHeight;
            rc.MaxDetailRowHeight = this.MaxDetailRowHeight;
            rc.HeaderTextStyle = new TextStyle (this.HeaderTextStyle);
            rc.DetailRowTextStyle = new TextStyle (this.DetailRowTextStyle);
            rc.AlternatingRowTextStyle = new TextStyle (this.AlternatingRowTextStyle);
			rc.SummaryRowTextStyle = new TextStyle (this.DetailRowTextStyle);
			rc.SummaryRowTextStyle.Bold = true;
            return this.columns.Add(rc);
        }

        /// <summary>
        /// Removes a column from the section
        /// </summary>
        /// <param name="index">Index of the column to remove</param>
        public virtual void RemoveColumn(int index)
        {
            this.columns.RemoveAt(index);
        }

        /// <summary>
        /// Gets the column at the specified index
        /// </summary>
        /// <param name="index">Index of a column</param>
        /// <returns>A IDataColumn object</returns>
        public virtual IDataColumn GetColumn(int index)
        {
            return (IDataColumn) this.columns[index];
        }

        /// <summary>
        /// The number of columns in this section.
        /// </summary>
        public int ColumnCount
        {
            get { return this.columns.Count; }
        }

        /// <summary>
        /// Clears all the columns from this section.
        /// Printing a section with 0 columns is just fine,
        /// a little weird, but technically fine.
        /// </summary>
        public virtual void ClearColumns()
        {
            this.columns.Clear();
        }

        #endregion

        #region "Overrides from ReportSection"

        /// <summary>
        /// Setup for printing
        /// </summary>
        /// <param name="g">Graphics object</param>
        protected override void DoBeginPrint(Graphics g)
        {
			DataSource.RowFilter = RowFilter;
			if (this.SuppressHeaderRow || this.RepeatHeaderRow)
            {
                this.rowIndex = 0;
            }
            else // just print header row once...
            {
                this.rowIndex = HeaderRowNumber;
            }
            // setup column widths
            foreach (IDataColumn rCol in this.columns)
            {
                rCol.SizeColumn (g, this.DataSource);
            }

			this.currentHorizPage = 0;
			this.dataRowsFit = 0;
        }


        /// <summary>
        /// Called to calculate the size that this section requires on
        /// the next call to Print.  This method will be called exactly once
        /// prior to each call to Print.  It must update the values Size and
        /// Continued of the ReportSection base class.
        /// </summary>
        /// <param name="reportDocument">The parent ReportDocument that is printing.</param>
        /// <param name="g">Graphics object to print on.</param>
        /// <param name="bounds">Bounds of the area to print within.</param>
        /// <returns>SectionSizeValues of requiredSize, fits, and continues.</returns>
        protected override SectionSizeValues DoCalcSize (
            ReportDocument reportDocument,
            Graphics g,
            Bounds bounds
            )
        {
			DataSource.RowFilter = RowFilter;
			// Default values
            SectionSizeValues retvals = new SectionSizeValues();

            Bounds insideBorder = this.borderPens.GetInnerBounds (bounds);
            CalcHeaderSize (g, insideBorder);
            Bounds tableBounds = GetTableBounds (insideBorder);
			PointF originalPosition = tableBounds.Position;

            if (SizePrintHeader (g, ref tableBounds, true))
            {
				if (this.currentHorizPage == 0)
				{
					this.dataRowsFit = FindDataRowsFit (g, ref tableBounds);
					this.tableHeightForPage = tableBounds.Position.Y - originalPosition.Y;
				}
                if (this.TotalRows == 0) // special case of 0 rows
                {
                    retvals.Fits = true;
                }
                else if (this.dataRowsFit > 0)
                {
                    retvals.Fits = true;
					if (this.currentHorizPage < this.infoForPages.Count - 1)
					{
						retvals.Continued = true;
					}
                    else if (this.rowIndex + this.dataRowsFit < this.TotalRows)
                    {
                        retvals.Continued = true;
                    }
                }
            }

            retvals.RequiredSize = this.borderPens.AddBorderSize (
                new SizeF (GetPageInfo().Width, tableHeightForPage));
            
            return retvals;
        }



        /// <summary>
        /// Called to actually print this section.  
        /// The DoCalcSize method will be called exactly once prior to each
        /// call of DoPrint.
        /// It should obey the value or Size and Continued as set by
        /// DoCalcSize().
        /// </summary>
        /// <param name="reportDocument">The parent ReportDocument that is printing.</param>
        /// <param name="g">Graphics object to print on.</param>
        /// <param name="bounds">Bounds of the area to print within.</param>
        protected override void DoPrint (
            ReportDocument reportDocument,
            Graphics g,
            Bounds bounds
            )
        {
			DataSource.RowFilter = RowFilter;
			int origRowIndex = this.rowIndex;
            Bounds tableBounds = GetTableBounds (bounds, this.RequiredSize);
            Bounds insideBorders = borderPens.GetInnerBounds (tableBounds);
            Bounds printingBounds = insideBorders;
			if ( !reportDocument.NoPrint ) 
			{
				SizePrintHeader (g, ref printingBounds, false);
				PrintRows (g, ref printingBounds);
				// Draw lines last
				PrintAllRowLines (g,insideBorders, 
					(!this.SuppressHeaderRow && this.RepeatHeaderRow) );
				PrintAllColumnLines (g, insideBorders);
				this.borderPens.DrawBorder (g, tableBounds);
			}
			if (this.currentHorizPage < this.infoForPages.Count - 1)
			{
				this.currentHorizPage++;
				this.rowIndex = origRowIndex;
			}
			else
			{
				this.currentHorizPage = 0;
			}
        }

        #endregion

        #region "Private sizing and printing functions"

        /// <summary>
        /// Calculates the size of the header and put it in headerSize
        /// Each column has already run its sizing algorithm during
        /// the DoBeginPrint method of this class. Now, add
        /// up all widths to figure out the total width (and possibly
        /// Resize the columns to fit in the designated PercentWidth
        /// of the page).
        /// </summary>
        SizeF CalcHeaderSize (Graphics g, Bounds bounds)
        {
            if (!headerSizeInit)
            {
				float width = bounds.Width;
				if (PercentWidth != 0)
				{ 
					width *= PercentWidth / 100;
				}
                ResizeColumns (width);

				this.headerRowHeight = SizePrintRow(g, HeaderRowNumber, 
                    bounds.Position.X, bounds.Position.Y, 
                    bounds.Width, this.MaxHeaderRowHeight, true, true);
                headerSizeInit = true;
            }
            return new SizeF (GetPageInfo().Width, this.headerRowHeight);
        }

		/// <summary>
        /// Resizes columns based on the given width
        /// </summary>
        /// <param name="width">The width the table is allowed to use.</param>
        protected virtual void ResizeColumns (float width)
        {
			if (this.PercentWidth == 0)
			{
				SetupPageInfo (width, this.allowMultiPageWidth, true);
			}
			else
			{
				SetupPageInfo (width, this.allowMultiPageWidth, false);
				foreach (PageInfo info in infoForPages)
				{
					// find amount of "extra" width (negative if the columns are
					// too wide for the page)
					float extraWidth = width - info.Width;
					for (int colNumber = info.FirstColumn; colNumber <= info.LastColumn; colNumber++)
					{
						IDataColumn col = GetColumn(colNumber);
						col.Width += extraWidth * (col.Width / info.Width);
					}
					info.Width = width;
				}
			}
        }

		/// <summary>
		/// Sets up the array infoForPages that holds the number of columns
		/// on each page...
		/// </summary>
		/// <param name="width">The width the table is allowed to use.</param>
		/// <param name="allowMultiPageWidth">Allow columns to span multiple pages</param>
		/// <param name="limitFirstPage">Limit the columns on the first page to just those that fit</param>
		void SetupPageInfo (float width, bool allowMultiPageWidth, bool limitFirstPage)
		{
			infoForPages = new ArrayList();
			PageInfo pageInfo = new PageInfo();
			int colNumber = 0;
			IDataColumn col = GetColumn(colNumber);
			for (colNumber = 0; colNumber < this.ColumnCount; colNumber++)
			{
				pageInfo.NumberOfColumns++;
				pageInfo.Width += col.Width;
				int nextCol = colNumber + 1;
				if (nextCol < this.ColumnCount)
				{
					col = GetColumn(nextCol);
					if (pageInfo.Width + col.Width > width)
					{
						if (allowMultiPageWidth)
						{
							infoForPages.Add (pageInfo);
							pageInfo = new PageInfo();
							pageInfo.FirstColumn = nextCol;
						}
						else if (limitFirstPage)
						{
							break;
						}
					}
				}
			}
			infoForPages.Add (pageInfo); // add last page
		}
		
        /// <summary>
        /// Gets the bounds used for the actual table, that is
        /// position is the top left corner of the table (after
        /// applying margins and alignments).  The width is based on
        /// this.headerSize and the border size.  The height is the full 
        /// height of the bounds.
        /// </summary>
        /// <param name="bounds">Maximum bounds allowed for the table</param>
        /// <returns>The bounds that the table is printed into.</returns>
        Bounds GetTableBounds (Bounds bounds)
        {
            // Find the correct x to center by getting a rectangle that holds the header row.
            SizeF size = this.borderPens.AddBorderSize (this.HeaderSize);
            RectangleF rect = bounds.GetRectangleF (size, 
                this.HorizontalAlignment, this.VerticalAlignment);
            
            // Create a new bounds with no margins at the correct location 
            // (centered left to right).
            return new Bounds (rect.Left, bounds.Position.Y, 
                rect.Right, bounds.Limit.Y);
        }

        /// <summary>
        /// Gets the bounds used for the actual table, that is
        /// position is the top left corner of the table (after
        /// applying margins and alignments).
        /// The width is based on
        /// this.headerSize and the border size.  The height is the full 
        /// height of the bounds.
        /// </summary>
        /// <param name="bounds">Maximum bounds allowed for the table</param>
        /// <param name="size">Size required for the table</param>
        /// <returns>The bounds that the table is printed into.</returns>
        Bounds GetTableBounds (Bounds bounds, SizeF size)
        {
            // size = this.borderPens.AddBorderSize (size);
            // Find the correct x and y to center
            RectangleF rect = bounds.GetRectangleF (size, 
                this.HorizontalAlignment, this.VerticalAlignment);
            
            // Create a new bounds with no margins at the correct location 
            // (centered left to right and top to bottom).
            return new Bounds (rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        /// <summary>
        /// Sizes or prints the header, if it is enabled
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bounds"></param>
        /// <param name="sizeOnly"></param>
        /// <returns>True if it fits, false if it doesn't</returns>
        bool SizePrintHeader (Graphics g, ref Bounds bounds, bool sizeOnly)
        {
            bool headerFits = true;
            if (!this.SuppressHeaderRow && this.RepeatHeaderRow)
            {
                // if it fits, print it
                if (bounds.SizeFits(this.HeaderSize))
                {
                    if (!sizeOnly)
                    {
                        SizePrintRow (g, HeaderRowNumber, bounds.Position.X, bounds.Position.Y,
                            this.GetPageInfo().Width, this.headerRowHeight, false, false);
                    }
                    bounds.Position.Y += this.headerRowHeight;
                }
                else
                {
                    headerFits = false;
                }
            }
            return headerFits;
        }

        /// <summary>
        /// Sizes rows and figures out how many will fit in the given bounds.
        /// </summary>
        /// <param name="g">Graphics object used for measuring</param>
        /// <param name="bounds">Maximum bounds allowed for table</param>
        /// <returns>The number of data rows that will fit</returns>
        int FindDataRowsFit (Graphics g, ref Bounds bounds)
        {
            int rowsThatFit = 0;
            int index = this.rowIndex;
            rowHeights = new ArrayList();
            // Find the rows that fit...
            while (index < this.TotalRows)
            {
                bool includeRowLine = index < TotalRows - 1;
                float rowHeight = SizePrintRow (g, index, bounds.Position.X, 
                    bounds.Position.Y,  bounds.Width, this.MaxDetailRowHeight,
                    true, includeRowLine);
                if (bounds.SizeFits(new SizeF (GetPageInfo().Width, rowHeight)))
                {
                    Debug.Assert (rowHeights.Count == rowsThatFit, "rowHeights.Count is not equal to index");
                    rowHeights.Add (rowHeight);
                    bounds.Position.Y += rowHeight;
                    index++;
                    rowsThatFit++;
                }
                else
                {
                    if (rowHeights.Count > 0)
                    {
                        // oops, it doesn't fit.. we'll resize 
                        // the last row without the line.
                        rowHeight = SizePrintRow (g, index - 1, bounds.Position.X, 
                            bounds.Position.Y,  bounds.Width, this.MaxDetailRowHeight,
                            true, false);
                        
                        bounds.Position.Y -= ((float)rowHeights[rowHeights.Count - 1]);
                        bounds.Position.Y += rowHeight;
                        rowHeights[rowHeights.Count - 1] = rowHeight;
                    }
                    break;
                }
            }
			if (minDataRowsFit != 0 && index < this.TotalRows)
			{
				if (rowsThatFit < minDataRowsFit)
					rowsThatFit = 0;
				else // make sure at least minDataRowsFit rows are left for the next page
				{
					int rowsLeft = this.TotalRows - index - ((this.ShowSummaryRow) ? 1 : 0);
					if (rowsLeft + rowsThatFit < (2 * minDataRowsFit))
						rowsThatFit = 0; // Force all onto next page
					else if(minDataRowsFit > rowsLeft)
						rowsThatFit -= (minDataRowsFit - rowsLeft); // Leave enough for next page.
				}
			}
			return rowsThatFit;
        }

 
        /// <summary>
        /// Prints rows of a table based on current rowIndex and dataRowsFit variables
        /// Also, bounds is incremented as each row is printed.
        /// </summary>
        /// <param name="g">Graphics object used for printing</param>
        /// <param name="bounds">Bounds allowed for table</param>
        /// <returns>True if at least one row fits</returns>
        void PrintRows (Graphics g, ref Bounds bounds)
        {
            // print all the rows that we already decided fit on the page...
            for (int rowCount = 0; rowCount < this.dataRowsFit; rowCount++, this.rowIndex++ )
            {
                float height = (float) rowHeights[rowCount];
                bool showLine = false; //rowCount != this.dataRowsFit - 1;
                SizePrintRow (g, this.rowIndex, bounds.Position.X, 
                    bounds.Position.Y, bounds.Width, height, false, showLine);
                bounds.Position.Y += height;

                Debug.Assert (bounds.Position.Y - bounds.Limit.Y < 0.005f, // TODO: Bad magic number
                    "Row doesn't really fit, but we thought it did. Delta: " + (bounds.Position.Y - bounds.Limit.Y));
            }
        }


        void PrintAllRowLines (Graphics g, Bounds bounds, bool includeHeader)
        {
            float x = bounds.Position.X;
            float y = bounds.Position.Y;
            float rowWidth = bounds.Width;
            if (includeHeader)
            {
                RowLine (g, x, y + this.headerRowHeight, rowWidth, true, false);
                y += this.headerRowHeight;
            }

            for (int rowCount = 0; rowCount < this.dataRowsFit - 1; rowCount++ )
            {
                float height = (float) rowHeights[rowCount];
                bool showLine = rowCount != this.dataRowsFit - 1;
                RowLine (g, x, y + height, rowWidth, false, false);
                y += height;
            }
        }

        void PrintAllColumnLines (Graphics g, Bounds bounds)
        {
			PageInfo info = GetPageInfo();
            float x = bounds.Position.X;
            float y = bounds.Position.Y;

            for (int colNumber = info.FirstColumn; colNumber <= info.LastColumn - 1; colNumber++)
            {
                IDataColumn col = GetColumn(colNumber);
                x += col.Width;
                col.DrawRightLine (g, x, y, bounds.Height);
            }

        }

		/// <summary>
		/// This is the main "worker" function for dealing with rows.
		/// Finds the size of a row, given the row number and the
		/// maximum height/width for the row.
		/// Prints the row if sizeOnly is not true.
		/// </summary>
		/// <param name="g">Graphics object to use for sizing</param>
		/// <param name="rowIndex">Index of the row within the DataView.
		/// Use the constant HeaderRowNumber to size the header row.</param>
		/// <param name="x">X position for the origin of the row.</param>
		/// <param name="y">Y position for the origin of the row.</param>
		/// <param name="maxWidth">The maximum width the row may consume.</param>
		/// <param name="maxHeight">The maximum height the row may consume.</param>
		/// <param name="sizeOnly">Only size, don't actually print</param>
		/// <param name="showLine">True to print a line under the row</param>
		/// <returns>A height for the row.</returns>
        float SizePrintRow (
            Graphics g, int rowIndex,
            float x, float y, float maxWidth, float maxHeight, bool sizeOnly, bool showLine)
        {
			bool isHeader = (rowIndex == HeaderRowNumber);
			bool isSummary = (rowIndex >= this.DataSource.Count);
			bool altRow = ((rowIndex % 2) != 0);
            float rowHeight = 0;
            float xPos = x;
			DataRowView drv = null;
            if (!isHeader && !isSummary)
            {
                drv = this.DataSource[rowIndex];
            }
			PageInfo info = GetPageInfo();
			int first = info.FirstColumn;
			int last = info.LastColumn;
			if (sizeOnly)
			{
				first = 0;
				last = this.ColumnCount - 1;
			}

			for (int colNumber = first; colNumber <= last; colNumber++)
			{
				IDataColumn col = GetColumn(colNumber);
				SizeF size = col.SizePaintCell (g, isHeader, altRow, isSummary, drv, xPos, y, col.Width, maxHeight, sizeOnly);
				rowHeight = Math.Max (rowHeight, GetValidHeight(size.Height, isHeader));
				xPos += col.Width;
			}

            if (showLine) // don't print a line for the last row...
            {
				//rowHeight += RowLine (g, x, y + rowHeight, rowWidth, isHeader, sizeOnly);
				rowHeight += RowLine (g, x, y + rowHeight, info.Width, isHeader, sizeOnly);
            }
            //return new SizeF (rowWidth, rowHeight);
			return rowHeight;
        }



 
        #endregion

        #region "Protected virtual getters"


        /// <summary>
        /// Checks that a height of a row is in the valid range.
        /// </summary>
        /// <param name="height">The desired height to use</param>
        /// <param name="isHeader">True if this is for a header row</param>
        /// <returns>height in the range min to max</returns>
        protected virtual float GetValidHeight (float height, bool isHeader)
        {
            float min, max;
            if (isHeader)
            {
                min = this.MinHeaderRowHeight;
                max = this.MaxHeaderRowHeight;
            }
            else
            {
                min = this.MinDetailRowHeight;
                max = this.MaxDetailRowHeight;
            }

            if (height < min)
            {
                return min;
            }
            else if (height > max)
            {
                return max;
            }
            else
            {
                return height;
            }
        }

        /// <summary>
        /// Prints or sizes a grid line that goes under a row
        /// </summary>
        /// <param name="g">Graphics object to draw on</param>
        /// <param name="x">X position of the line</param>
        /// <param name="y">Y position of the line</param>
        /// <param name="length">Length of the line</param>
        /// <param name="isHeader">The row is a header row</param>
        /// <param name="sizeOnly">SizeOnly - don't print if true</param>
        /// <returns>The width (height) of the line</returns>
        protected virtual float RowLine (Graphics g, float x, float y, float length, bool isHeader, bool sizeOnly)
        {
            float height = 0;
            Pen pen;
            if (isHeader)
            {
                pen = this.innerPenHeaderBottom;
            }
            else
            {
                pen = this.innerPenRow;
            }

            if (pen != null)
            {
                if (!sizeOnly)
                {
                    y -= pen.Width / 2;
                    g.DrawLine (pen, x, y, x + length, y);
                }
                height = pen.Width;
            }
            return height;
        }


        #endregion




        /// <summary>
        /// This class store data about the columns which will be printed on a page
        /// There will be one instance of this class for each page in width that the
        /// table consumes. That is, if columns spread across three pages wide, there
        /// will be three instances. All the pages used in "height" to display the
        /// rows of the table will share the same instances.
        /// It is used only within the class SectionTable.
        /// </summary>
        private class PageInfo
        {
            /// <summary>
            /// The index of the first column to be displayed on the page, 0 based.
            /// </summary>
            public int FirstColumn = 0;
            /// <summary>
            /// The number of columns to be displayed on the page.
            /// </summary>
            public int NumberOfColumns = 0;
            /// <summary>
            /// The horizontal dimension that this page requires, that is,
            /// the total width of all columns on the page.
            /// It should (I believe) include all borders to be displayed.
            /// </summary>
            public float Width = 0;
            /// <summary>
            /// The index of the last column to be displayed on the page.
            /// Returns FirstColumn + NumberOfColumns - 1
            /// </summary>
            public int LastColumn
            {
                get { return this.FirstColumn + this.NumberOfColumns - 1; }
            }
        }


	}

	
}
