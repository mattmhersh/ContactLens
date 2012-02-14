// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/

using System;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Drawing;

namespace ReportPrinting
{
	/// <summary>
	/// This class assists with the building of a ReportDocument
	/// </summary>
	/// <remarks>
	/// <para>
	/// ReportBuilder assists with the building of a report.
	/// When it is constructed, you must provide the 
	/// <see cref="ReportPrinting.ReportDocument"/> to be built.
	/// </para>
    /// <para>
    /// Some summaries of important objects:
    /// </para>
    /// <para>
    /// In page header / footer text, the following strings have special meanings.
    /// <list type="table">
    /// <listheader><term>String</term><description>Description</description></listheader>
    /// <item><term>%p</term><description>Page Number</description></item>
    /// </list>
    /// </para>
	/// </remarks>
	public class ReportBuilder
	{

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reportDocument">The document to be built</param>
        public ReportBuilder (ReportDocument reportDocument)
        {
            this.currentDocument = reportDocument;
            this.containers = new Stack();
            this.formatStrings = new Hashtable();
            AddFormatExpression(typeof(DateTime), DefaultDateTimeFormatString);
            float scale = reportDocument.GetScale();
            this.maxDetailRowHeight = 8f * scale;
            this.maxHeaderRowHeight = 8f * scale;
            this.horizLineMargins = 0.1f * scale;
            this.headerFooterMargins = horizLineMargins * 2;
            this.defaultTablePen = reportDocument.ThinPen;
        }

        const string DefaultDateTimeFormatString = "d";

        Stack containers;

        readonly Hashtable formatStrings;
        ReportDocument currentDocument;
        ReportSection currentSection;
        ReportDataColumn currentColumn;
        float maxDetailRowHeight;
        float minDetailRowHeight;
        float maxHeaderRowHeight;
        float minHeaderRowHeight;
        bool documentMode;
        bool columnLayoutMode;
        Pen defaultTablePen;
        float horizLineMargins;

        /// <summary>
        /// headerFooterMargins indicates the default bottom margin for the header,
        /// and the top margin for the page footer if there is no line.
        /// If a line is added, then the margin is made equal to the
        /// total height of line (pen width + 2 * horizLineMargins.
        /// </summary>
        float headerFooterMargins;
        HorizontalAlignment defaultColumnAlignment;


        #region "Settings, Properties, etc"


        /// <summary>
        /// Gets the current document being built
        /// </summary>
        public ReportDocument CurrentDocument
        {
            get { return this.currentDocument; }
        }

        /// <summary>
        /// Gets the current SectionContainer
        /// </summary>
        public SectionContainer CurrentContainer
        {
            get 
            {
                if (this.containers.Count == 0) 
                {
                    return null;
                }
                return this.containers.Peek() as SectionContainer; 
            }
        }

        /// <summary>
        /// Gets the last section added throught AddSection()
        /// </summary>
        public ReportSection CurrentSection
        {
            get { return this.currentSection; }
        }

        /// <summary>
        /// Gets the last section if it's a table
        /// </summary>
        public SectionTable Table
        {
            get { return CurrentSection as SectionTable; }
        }

        /// <summary>
        /// Gets the last section if it's a line
        /// </summary>
        public SectionLine Line
        {
            get { return CurrentSection as SectionLine; }
        }

        /// <summary>
        /// Gets the last section if it's a box
        /// </summary>
        public SectionBox Box
        {
            get { return CurrentContainer as SectionBox; }
        }



        /// <summary>
        /// Gets the last column added through AddColumn()
        /// </summary>
        public ReportDataColumn CurrentColumn
        {
            get { return this.currentColumn; }
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
        /// Gets or sets the max page header height of the current document.
        /// </summary>
        public float MaxPageHeaderHeight
        {
            get { return CurrentDocument.PageHeaderMaxHeight; }
            set { CurrentDocument.PageHeaderMaxHeight = value; }
        }

        /// <summary>
        /// Gets or sets the max page footer height of the current document.
        /// </summary>
        public float MaxPageFooterHeight
        {
            get { return CurrentDocument.PageFooterMaxHeight; }
            set { CurrentDocument.PageFooterMaxHeight = value; }
        }

        /// <summary>
        /// Adds a default format expression for a specific Type.
        /// </summary>
        /// <param name="type">The type which should use the given format string.</param>
        /// <param name="formatString">A formatString to use for the given type. 
        /// See the Format method of <see cref="System.String"/> </param>
        /// <remarks>
        /// These default format expressions are copied into a column's
        /// FormatExpression when the column is created if the type matches.
        /// </remarks>
        /// <example>
        /// The following entry is added by default for DateTime objects.
        /// Unless a new string is added for that Type (or the default is
        /// removed) then all columns of DateTime type will be formated
        /// with the string "{0:d}"
        /// <code>
        /// string DefaultDateTimeFormatString = "{0:d}";
        /// AddFormatExpression(typeof(DateTime), DefaultDateTimeFormatString);
        /// </code>
        /// </example>
        public void AddFormatExpression (Type type, string formatString)
        {
            this.formatStrings[type] = formatString;
        }

        /// <summary>
        /// Removes the default format expression for a given type
        /// </summary>
        /// <param name="type">The Type whose's default format expression should be removed</param>
        public void ClearFormatExpression (Type type)
        {
            if (this.formatStrings.ContainsKey(type))
            {
                this.formatStrings.Remove(type);
            }
        }

        /// <summary>
        /// Gets or sets the default top and bottom margins used
        /// for horizontal lines
        /// </summary>
        public float HorizLineMargins
        {
            get { return this.horizLineMargins; }
            set { this.horizLineMargins = value; }
        }

        /// <summary>
        /// Gets or sets the default HorizontalAlignment for columns
        /// </summary>
        public HorizontalAlignment DefaultColumnAlignment
        {
            get { return this.defaultColumnAlignment; }
            set { this.defaultColumnAlignment = value; }
        }

        #endregion


        #region "Containers or Layouts"

		/// <summary>
		/// This adds the provided SectionContainer to the ReportDocument
		/// and makes it the default container for new sections added to the report.
		/// </summary>
		/// <param name="container">The SectionContainter to add</param>
		/// <returns>The SectionContainter started</returns>
        public SectionContainer StartContainer (
            SectionContainer container)
        {
            if (this.CurrentDocument.Body == null)
            {
                this.CurrentDocument.Body = container;
            }
            else if (this.CurrentContainer != null)
            {
                this.CurrentContainer.AddSection(container);
            }
            this.containers.Push(container);
            return container;
        }

		/// <summary>
		/// Finish and close the last container opened
		/// </summary>
		/// <returns>The containter finished</returns>
        public SectionContainer FinishContainer ()
        {
            return (SectionContainer) this.containers.Pop();
        }

		/// <summary>
		/// Start a linear layout (that is, create a container
		/// from LinearSections, and add future sections to this
		/// container).
		/// </summary>
		/// <param name="direction">The Direction that the sub-sections
		/// of this linear section are laid out.  (Generally vertical)</param>
		/// <returns>The new LinearSections object started by this method</returns>
        public LinearSections StartLinearLayout (Direction direction)
        {
            LinearSections container = new LinearSections();
            container.Direction = direction;
            switch (direction)
            {
                case Direction.Horizontal:
                    container.UseFullHeight = true;
                    break;
                case Direction.Vertical:
                    container.UseFullWidth = true;
                    break;
            }
            StartContainer(container);
            return container;
        }

		/// <summary>
		/// Finish and close the last container opened, hopefully it
		/// is a container started with StartLinearLayout 
		/// </summary>
		/// <returns>The containter closed (as a LinearSections object)</returns>
		public LinearSections FinishLinearLayout ()
        {
            return (LinearSections) FinishContainer();
        }

        /// <summary>
        /// Start a layered layout (that is, create a container
        /// from LayeredSections, and add future sections to this
        /// container) that expands to use the full width and 
        /// height of the parent section
        /// </summary>
        /// <returns>The new LayeredSections object started by this method</returns>
        public LayeredSections StartLayeredLayout ()
        {
            return StartLayeredLayout (true, true);
        }

        /// <summary>
		/// Start a layered layout (that is, create a container
		/// from LayeredSections, and add future sections to this
		/// container).
		/// </summary>
        /// <param name="useFullWidth">Indicates that this layered layout will expand
        /// to full width within the parent layout</param>
        /// <param name="useFullHeight">Indicates that this layered layout will expand
        /// to full height within the parent layout</param>
        /// <returns>The new LayeredSections object started by this method</returns>
        public LayeredSections StartLayeredLayout (bool useFullWidth, bool useFullHeight)
        {
            LayeredSections container = new LayeredSections(useFullWidth, useFullHeight);
            StartContainer(container);
            return container;
        }

		/// <summary>
		/// Finish and close the last container opened, hopefully it
		/// is a container started with StartLinearLayout 
		/// </summary>
		/// <returns>The containter closed (as a LinearSections object)</returns>
        public LayeredSections FinishLayeredLayout ()
        {
            return (LayeredSections) FinishContainer();
        }

		/// <summary>
		/// This is an unsupported function.  Avoid using it...
		/// Starts a linear horizontal layout within a parent vertical layout,
		/// which causes sections to first be layed out across a page as rows, then 
		/// down the page at the end of each row.
		/// </summary>
		/// <remarks>
		/// This DocumentLayout should be avoided.  The simple LinearSection
		/// with Vertical layout can handle most scenarios.  However, some more advanced
		/// setups may require this.
		/// </remarks>
		public void StartDocumentLayout()
		{
#if _DEBUG
            Debug.WriteLine ("Avoid using the document layout - unsupported...");
#endif
			Debug.Assert(!this.documentMode, "Already in document layout.  May not nest these.");
			LinearRepeatableSections repSections = new LinearRepeatableSections (Direction.Vertical);
			this.StartContainer (repSections);
            this.StartLinearLayout(Direction.Horizontal);
			this.documentMode = true;
		}

		/// <summary>
		/// Finishes a document layout (really, it just finishes two
		/// LinearSections containers).
		/// </summary>
		public void FinishDocumentLayout()
		{
			Debug.Assert(this.documentMode, "Not in document layout.  This could present problems trying to FinishDocumentLayout()");
			this.FinishContainer();
			this.FinishContainer();
			this.documentMode = false;
		}


        /// <summary>
        /// Starts a layout of columns.
        /// </summary>
        /// <param name="columnWidth">The width of each column, in inches</param>
        /// <param name="spaceBetween">The distance between columns, in inches</param>
        /// <param name="lineDivider">Pen to use as a line divider</param>
        public void StartColumnLayout (float columnWidth, float spaceBetween,  Pen lineDivider)
        {
            this.columnLayoutMode = true;
            LinearRepeatableSections colSections = 
                new LinearRepeatableSections(Direction.Horizontal);
            this.StartContainer (colSections);
            if (lineDivider != null)
            {
                SectionLine line = new SectionLine (Direction.Vertical, lineDivider);
				if (spaceBetween < lineDivider.Width)
					spaceBetween = lineDivider.Width;
                line.MarginLeft = (spaceBetween - lineDivider.Width) / 2;
                line.MarginRight = (spaceBetween - lineDivider.Width) / 2;
                colSections.Divider = line;
            }
            else
            {
                colSections.SkipAmount = spaceBetween;
            }
            
            LinearSections sections;
            sections = this.StartLinearLayout(Direction.Vertical);
            sections.MaxWidth = columnWidth;
            sections.UseFullWidth = true;
        }

        /// <summary>
        /// Start a layout of columns by specifying the number of columns.
        /// </summary>
        /// <param name="numberOfColumns">Number of columns to fit on the page.</param>
        /// <param name="spaceBetween">The distance between columns, in inches</param>
        /// <param name="lineDivider">A pen to use as a line divider</param>
        public void StartColumnLayout (int numberOfColumns, float spaceBetween, Pen lineDivider)
        {
            float width = this.CurrentDocument.DefaultPageSettings.Bounds.Width;
            width -= this.CurrentDocument.DefaultPageSettings.Margins.Left;
            width -= this.CurrentDocument.DefaultPageSettings.Margins.Right;
            width *= this.CurrentDocument.GetScale() / 100f;
            width -= (numberOfColumns - 1) * spaceBetween;
            Debug.Assert(width > 0, "Too many columns, too wide of margins, or some other problem.  Columns will not print.");
            width /= numberOfColumns;
            StartColumnLayout (width, spaceBetween, lineDivider);
        }

        /// <summary>
        /// Start a layout of columns by specifying the number of columns.
        /// </summary>
        /// <param name="numberOfColumns">Number of columns to fit on the page.</param>
        /// <param name="spaceBetween">The distance between columns, in inches</param>
        public void StartColumnLayout (int numberOfColumns, float spaceBetween)
        {
            StartColumnLayout (numberOfColumns, spaceBetween, null);
        }

        /// <summary>
        /// Ends the layout in columns.
        /// </summary>
        public void FinishColumnLayout()
        {
            Debug.Assert (this.columnLayoutMode, "Not in column layout mode - finishing makes no sense.");
            this.FinishContainer(); // the vertical layout
            this.FinishContainer(); // the horizontal layout
        }


        /****************************************
         * Box "layout"
         */

        /// <summary>
        /// Starts a box section that can be used as a container for one section
        /// </summary>
        /// <returns>SectionBox added</returns>
        public SectionBox StartBox ()
        {
            SectionBox box = new SectionBox();
            StartContainer (box);
            return box;
        }

        /// <summary>
        /// Starts a box section that can be used as a container for one section
        /// </summary>
        /// <param name="margins">Margins to use on all sides</param>
        /// <param name="borders">Border pen to use on all sides</param>
        /// <param name="padding">Padding to use on all sides</param>
        /// <param name="background">Brush to use for the background</param>
        /// <returns>SectionBox added</returns>
        public SectionBox StartBox (float margins, Pen borders, float padding, Brush background)
        {
            return StartBox (margins, borders, padding, background, 0, 0);
        }

        /// <summary>
        /// Starts a box section that can be used as a container for one section
        /// </summary>
        /// <param name="margins">Margins to use on all sides</param>
        /// <param name="borders">Border pen to use on all sides</param>
        /// <param name="padding">Padding to use on all sides</param>
        /// <param name="background">Brush to use for the background</param>
        /// <param name="width">Width of the box, 0 to size to contents</param>
        /// <param name="height">Height of the box, 0 to size to contents</param>
        /// <returns>SectionBox added</returns>
        public SectionBox StartBox (float margins, Pen borders, float padding, Brush background,
            float width, float height)
        {
            SectionBox box = StartBox ();
            box.Margin = margins;
            box.Border = borders;
            box.Padding = padding;
            box.Background = background;
            box.Width = width;
            box.Height = height;
            return box;
        }


        /// <summary>
        /// Stops the layout into a box.  This must be called after no more than
        /// one additional section is put into the box.
        /// </summary>
        /// <returns>SectionBox finished</returns>
        public SectionBox FinishBox ()
        {
            return (SectionBox) FinishContainer();
        }


        #endregion


        #region "Basic Sections"

        /// <summary>
        /// Adds any ReportSection to the document.
        /// This method is intended for adding non-container sections, such as text,
        /// images, tables. See StartContainer method for beginning a section that
        /// is a container of other sections.
        /// </summary>
        /// <param name="section">ReportSection to add</param>
        /// <returns>The ReportSection added</returns>
        public ReportSection AddSection (ReportSection section)
        {
            if (this.CurrentContainer == null)
            {
                throw new ReportBuilderException("No SectionContainer defined to add section: " + section.ToString());
            }
            this.CurrentContainer.AddSection(section);
            this.currentSection = section;
            if (this.documentMode)
            {
                section.VerticalAlignment = VerticalAlignment.Bottom;
            }
            return section;
        }

        /// <summary>
        /// Adds a section to the ReportDocument
        /// consisting of a text field with a given TextStyle
        /// </summary>
        /// <param name="text">Text for this section</param>
        /// <param name="textStyle">Text style to use for this section</param>
        /// <returns>ReportSection just created</returns>
        public SectionText AddText (string text, TextStyle textStyle)
        {
            SectionText section = new SectionText(text, textStyle);
			if (this.documentMode)
			{
				section.SingleLineMode = true;
			}

            AddSection(section);
            return section;
        }

		/// <summary>
		/// Adds a section to the ReportDocument
		/// consisting of a text field.
		/// </summary>
		/// <param name="text">Text for this section</param>
		/// <returns>ReportSection just created</returns>
		public SectionText AddText (string text)
		{
			return AddText (text, TextStyle.Normal);
		}


        /// <summary>
        /// Adds a page break at this point in the report
        /// </summary>
        /// <returns>SectionBreak added</returns>
        public SectionBreak AddPageBreak ()
        {
            return (SectionBreak) AddSection (new SectionBreak ());
        }

        /// <summary>
        /// Adds a page break at this point in the report
        /// </summary>
        /// <param name="newOrientation">The PageOrientation (Portrait/Landscape) to be used
        /// by pages after the break</param>
        /// <returns>SectionBreak added</returns>
        public SectionBreak AddPageBreak (PageOrientation newOrientation)
        {
            SectionBreak sectBreak = new SectionBreak(true);
            sectBreak.NewPageOrientation = newOrientation;
            AddSection (sectBreak);
            return sectBreak;
        }

        /// <summary>
        /// Adds a column break at this point in the report
        /// </summary>
        /// <returns>SectionBreak added</returns>
        public SectionBreak AddColumnBreak ()
        {
            return (SectionBreak) AddSection (new SectionBreak (false));
        }

//        /// <summary>
//        /// Adds a line break at this point in the report
//        /// </summary>
//        /// <returns>SectionBreak added</returns>
//        public SectionBreak AddLineBreak ()
//        {
//            return (SectionBreak) AddSection (new SectionBreak (false));
//        }


        /// <summary>
        /// Adds a horizontal line
        /// </summary>
        /// <param name="pen">Pen to use for the line</param>
        /// <returns>SectionLine added</returns>
        public SectionLine AddHorizontalLine (Pen pen)
        {
            SectionLine sectionLine = new SectionLine (Direction.Horizontal, pen);
            sectionLine.MarginTop = HorizLineMargins;
            sectionLine.MarginBottom = HorizLineMargins;
            AddSection (sectionLine);
            return sectionLine;
        }

        /// <summary>
        /// Adds a horizontal line using the document's NormalPen
        /// </summary>
        /// <returns>SectionLine added</returns>
        public SectionLine AddHorizontalLine ()
        {
            return AddHorizontalLine (CurrentDocument.NormalPen);
        }


        #endregion


        #region "Tables and Columns"


        /// <summary>
        /// Gets or sets the default pen used for table grid lines
        /// </summary>
        public Pen DefaultTablePen
        {
            get { return this.defaultTablePen; }
            set { this.defaultTablePen = value; }
        }

        /// <summary>
        /// Adds a data section with no default columns.
        /// Use the AddColumn() method to add those.
        /// </summary>
        /// <param name="dataSource">DataView for the source of data</param>
        /// <param name="repeatHeaderRow">Indicates a header row should be printed at the top of every page, 
        /// instead of just the first.  Use Table.SuppressHeaderRow to turn off all headers</param>
        /// <param name="tablePercentWidth">The width of the table as a percentage of the parent.</param>
        /// <returns>ReportSection just created</returns>
        /// <remarks>
        /// <para>
        /// If a non-zero tablePercentWidth is specified, then columns will be
        /// widened or narrowed proportionately, based on the desired width of
        /// each column. The maxColumnWidth is no longer guaranteed to be the max column width.
        /// Here is the algorithm:
        /// </para>
        /// <list type="number">
        /// <item><description>Set each column width to maxColumnWidth</description></item>
        /// <item><description>If sizeWidthToHeader and/or sizeWidthToContents are set, then
        /// reduce each column's width down to that necessary for the header and/or contents.</description></item>
        /// <item><description>Finally, take the combined widths of all columns and stretch them to
        /// make the table have the tablePercentWidth.</description></item>
        /// </list>
        /// </remarks>
        public SectionTable AddTable (
            DataView dataSource, bool repeatHeaderRow, float tablePercentWidth
            )
        {
            SectionTable section = new SectionTable(dataSource);
            section.RepeatHeaderRow = repeatHeaderRow;
            section.MaxDetailRowHeight = this.MaxDetailRowHeight;
            section.MinDetailRowHeight = this.MinDetailRowHeight;
            section.MaxHeaderRowHeight = this.MaxHeaderRowHeight;
            section.MinHeaderRowHeight = this.MinHeaderRowHeight;
            section.InnerPenHeaderBottom = this.DefaultTablePen;
            section.InnerPenRow = this.DefaultTablePen;
            section.OuterPens = this.DefaultTablePen;
            section.PercentWidth = tablePercentWidth;

            AddSection(section);
            return section;
        }

        
        /// <summary>
        /// Adds a data section with no default columns.
        /// Use the AddColumn() method to add those.
        /// </summary>
        /// <param name="dataSource">DataView for the source of data</param>
		/// <param name="repeatHeaderRow">Indicates a header row should be printed at the top of every page, 
		/// instead of just the first.  Use Table.SuppressHeaderRow to turn off all headers</param>
		/// <returns>ReportSection just created</returns>
        public SectionTable AddTable (
            DataView dataSource, bool repeatHeaderRow
            )
        {
            return AddTable (dataSource, repeatHeaderRow, 0);
        }

        /// <summary>
        /// Adds all columns from the DataSource to the current section
        /// </summary>
        /// <param name="maxWidth">Maximum width for the columns</param>
        /// <param name="sizeWidthToHeader">Size the columns based on the header</param>
        /// <param name="sizeWidthToContents">Size the columns based on the data contents</param>
        public void AddAllColumns(float maxWidth,
            bool sizeWidthToHeader, bool sizeWidthToContents)
        {
            if (Table == null)
            {
                throw new ReportBuilderException ("Table must first be added with AddTable");
            }
            foreach (DataColumn col in Table.DataSource.Table.Columns)
            {
                AddColumn (col, col.ColumnName, maxWidth,
                    sizeWidthToHeader, sizeWidthToContents);
                
            }
        }

		/// <summary>
		/// Flag that, when true, replaces a boolean column with an
		/// image of checks / no checks. If false, it simply uses a
		/// text column with the words true/false.
		/// </summary>
        public bool UseImageColumnForBool = true;


        /*****************
         * Function that actually creates the column
         */

        /// <summary>
        /// Adds a single column to the current section (last section added).
        /// </summary>
        /// <param name="col">Column in the data source</param>
        /// <param name="headerText">Text to display in the header row(s)</param>
        /// <param name="maxWidth">Maximum width of the column</param>
        /// <param name="sizeWidthToHeader">Size the column width based on the header</param>
        /// <param name="sizeWidthToContents">Size the column width based on the data contents</param>
        /// <param name="horizontalAlignment">Specifies the horizontal alignment of the
        /// contents of this column</param>
        /// <returns>ReportDataColumn just added</returns>
        public ReportDataColumn AddColumn (DataColumn col, string headerText,
            float maxWidth, bool sizeWidthToHeader, bool sizeWidthToContents,
            HorizontalAlignment horizontalAlignment)
        {
            if (Table == null)
            {
                throw new ReportBuilderException ("Table must first be added with AddTable");
            }
            this.currentColumn = null;
            if (UseImageColumnForBool && col.DataType == typeof(bool))
            {
                this.currentColumn = new ReportBoolColumn(col.ColumnName, maxWidth);
            }
            else
            {
                this.currentColumn = new ReportDataColumn(col.ColumnName, maxWidth);
            }
            Table.AddColumn(this.currentColumn);

            this.currentColumn.HeaderRowText = headerText;
            this.currentColumn.SizeWidthToHeader = sizeWidthToHeader;
            this.currentColumn.SizeWidthToContents = sizeWidthToContents;

            // check for default format string and add it
            if (this.formatStrings.ContainsKey(col.DataType))
            {
                this.currentColumn.FormatExpression = (string) this.formatStrings[col.DataType];
            }
            this.currentColumn.RightPen = this.DefaultTablePen;

            this.currentColumn.HeaderTextStyle.StringAlignment = 
                SectionText.ConvertAlign (horizontalAlignment);
            this.currentColumn.DetailRowTextStyle.StringAlignment = 
                SectionText.ConvertAlign (horizontalAlignment);
            this.currentColumn.AlternatingRowTextStyle.StringAlignment = 
                SectionText.ConvertAlign (horizontalAlignment);
			this.currentColumn.SummaryRowTextStyle.StringAlignment = 
				SectionText.ConvertAlign (horizontalAlignment);

            return this.currentColumn;
        }

        /******************************************
         * Overloads with DataColumn
         */
        
        /// <summary>
        /// Adds a single column to the current section (last section added).
        /// </summary>
        /// <param name="col">Column in the data source</param>
        /// <param name="headerText">Text to display in the header row(s)</param>
        /// <param name="maxWidth">Maximum width of the column</param>
        /// <param name="sizeWidthToHeader">Size the column width based on the header</param>
        /// <param name="sizeWidthToContents">Size the column width based on the data contents</param>
        /// <returns>ReportDataColumn just added</returns>
        public ReportDataColumn AddColumn (DataColumn col, string headerText,
            float maxWidth, bool sizeWidthToHeader, bool sizeWidthToContents)
        {
            return AddColumn (col, headerText, maxWidth, sizeWidthToHeader, 
                sizeWidthToContents, DefaultColumnAlignment);
        }


        /// <summary>
        /// Adds a single column to the current section (last section added).
        /// </summary>
        /// <param name="col">Column in the data source</param>
        /// <param name="headerText">Text to display in the header row(s)</param>
        /// <param name="width">Maximum width of the column</param>
        /// <returns>ReportDataColumn just added</returns>
        public ReportDataColumn AddColumn (DataColumn col, string headerText,
            float width)
        {
            return AddColumn (col, headerText, width, false, false);
        }


        /********************
         * Overloads with name
         */

        /// <summary>
        /// Adds a single column to the current section (last section added).
        /// </summary>
        /// <param name="columnName">Column name in the data source</param>
        /// <param name="headerText">Text to display in the header row(s)</param>
        /// <param name="maxWidth">Maximum width of the column</param>
        /// <param name="sizeWidthToHeader">Size the column width based on the header</param>
        /// <param name="sizeWidthToContents">Size the column width based on the data contents</param>
        /// <param name="horizontalAlignment">Specifies the horizontal alignment of the
        /// contents of this column</param>
        /// <returns>ReportDataColumn just added</returns>
        public ReportDataColumn AddColumn (string columnName, string headerText,
            float maxWidth, bool sizeWidthToHeader, bool sizeWidthToContents,
            HorizontalAlignment horizontalAlignment)
        {
            if (Table == null)
            {
                throw new ReportBuilderException ("Table must first be added with AddTable");
            }
            DataColumn col = Table.DataSource.Table.Columns[columnName];
            if (col == null)
            {
                throw new ReportBuilderException ("There is no column named '" + columnName +
                    "' in table '" + Table.DataSource.Table.TableName + "'");
            }
            return AddColumn (col, headerText, maxWidth, 
                sizeWidthToHeader, sizeWidthToContents, horizontalAlignment);
        }



        /// <summary>
        /// Adds a single column to the current section (last section added).
        /// </summary>
        /// <param name="columnName">Column name in the data source</param>
        /// <param name="headerText">Text to display in the header row(s)</param>
        /// <param name="width">Maximum width of the column</param>
        /// <param name="sizeWidthToHeader">Size the column width based on the header</param>
        /// <param name="sizeWidthToContents">Size the column width based on the data contents</param>
        /// <returns>ReportDataColumn just added</returns>
        public ReportDataColumn AddColumn (string columnName, string headerText,
            float width, bool sizeWidthToHeader, bool sizeWidthToContents)
        {
            return AddColumn (columnName, headerText, width, sizeWidthToHeader, sizeWidthToContents,
                DefaultColumnAlignment);
        }


        /// <summary>
        /// Adds a single column to the current section (last section added).
        /// </summary>
        /// <param name="columnName">Column name in the data source</param>
        /// <param name="headerText">Text to display in the header row(s)</param>
        /// <param name="width">Width of the column</param>
        /// <returns>ReportDataColumn just added</returns>
        public ReportDataColumn AddColumn (string columnName, string headerText,
            float width)
        {
            return AddColumn (columnName, headerText, width, false, false);
        }


        #endregion


        #region "Print a datagrid"

        /// <summary>
        /// Defines the horizontal scale used to convert from
        /// DataGrid pixels (on the screen) to inches on the page.
        /// </summary>
        public float DataGridToPrinterHScale = 75;

        void DgApplyDefaultStyles (SectionTable table, float textSize)
        {
            table.HeaderTextStyle.Size = textSize;
            table.DetailRowTextStyle.Size = textSize;

            // add space around columns...
            table.HeaderTextStyle.MarginFar     = 0.05f * CurrentDocument.GetScale();
            table.HeaderTextStyle.MarginNear    = 0.05f * CurrentDocument.GetScale();
            table.DetailRowTextStyle.MarginFar  = 0.05f * CurrentDocument.GetScale();
            table.DetailRowTextStyle.MarginNear = 0.05f * CurrentDocument.GetScale();

        }

        void DgAddColumns (SectionTable table, System.Windows.Forms.DataGridTableStyle tableStyle)
        {
            foreach (System.Windows.Forms.DataGridColumnStyle columnStyle
                         in tableStyle.GridColumnStyles)
            {
                float width = columnStyle.Width / DataGridToPrinterHScale * CurrentDocument.GetScale();
                ReportDataColumn col = AddColumn(columnStyle.MappingName,
                    columnStyle.HeaderText,
                    width,
                    true,
                    true,
                    (ReportPrinting.HorizontalAlignment)columnStyle.Alignment);
                col.NullValueString = columnStyle.NullText;

                System.Windows.Forms.DataGridTextBoxColumn dgtbc = 
                    columnStyle as System.Windows.Forms.DataGridTextBoxColumn;
                if (dgtbc != null)
                {
                    col.FormatExpression = dgtbc.Format;
                }

            }
        }

        void DgApplyTableStyles (SectionTable table, System.Windows.Forms.DataGrid dataGrid)
        {
            // use grid defaults
            table.HeaderTextStyle.SetFromFont (dataGrid.HeaderFont);
            table.HeaderTextStyle.Brush = new SolidBrush (dataGrid.HeaderForeColor);
            table.HeaderTextStyle.BackgroundBrush = new SolidBrush (dataGrid.HeaderBackColor);
            table.DetailRowTextStyle.Brush = new SolidBrush (dataGrid.ForeColor);
            table.DetailRowTextStyle.BackgroundBrush = new SolidBrush (dataGrid.BackColor);
            table.AlternatingRowTextStyle.BackgroundBrush = new SolidBrush (dataGrid.AlternatingBackColor);
        }

        void DgApplyTableStyles (SectionTable table, System.Windows.Forms.DataGridTableStyle tableStyle)
        {
            // use tableStyle properties
            table.HeaderTextStyle.SetFromFont (tableStyle.HeaderFont);
            table.HeaderTextStyle.Brush = new SolidBrush (tableStyle.HeaderForeColor);
            table.HeaderTextStyle.BackgroundBrush = new SolidBrush (tableStyle.HeaderBackColor);
            table.DetailRowTextStyle.Brush = new SolidBrush (tableStyle.ForeColor);
            table.DetailRowTextStyle.BackgroundBrush = new SolidBrush (tableStyle.BackColor);
            table.AlternatingRowTextStyle.BackgroundBrush = new SolidBrush (tableStyle.AlternatingBackColor);
        }

        Pen DgGridPen (SectionTable table, System.Windows.Forms.DataGrid dataGrid)
        {
            Pen pen = null;
            if (dataGrid.GridLineStyle == System.Windows.Forms.DataGridLineStyle.Solid)
            {
                pen = new Pen (dataGrid.GridLineColor, CurrentDocument.ThinPen.Width);
                table.OuterPens = pen;
                table.InnerPenHeaderBottom = pen;
                table.InnerPenRow = pen;
            }
            return pen;
        }
        
        Pen DgGridPen (SectionTable table, System.Windows.Forms.DataGridTableStyle tableStyle)
        {
            Pen pen = null;
            if (tableStyle.GridLineStyle == System.Windows.Forms.DataGridLineStyle.Solid)
            {
                pen = new Pen (tableStyle.GridLineColor, CurrentDocument.ThinPen.Width);
                table.OuterPens = pen;
                table.InnerPenHeaderBottom = pen;
                table.InnerPenRow = pen;
            }
            return pen;
        }

        /// <summary>
        /// Adds a table to the report, based on the formatting for a DataGrid
        /// </summary>
        /// <param name="dataGrid">DataGrid to use for data and formatting</param>
        /// <param name="textSize">Font size for text (in points)</param>
        /// <param name="includeFormatting">Include fonts and colors from data grid
        /// (textSize parameter is then ignored)</param>
        /// <returns>SectionTable added to the report</returns>
        public SectionTable AddDataGrid (System.Windows.Forms.DataGrid dataGrid, 
            float textSize, bool includeFormatting)
        {

            if (dataGrid == null)
            {
                throw new ArgumentNullException ("dataGrid");
            }
            System.Windows.Forms.CurrencyManager currencyManager = 
                (System.Windows.Forms.CurrencyManager)dataGrid.BindingContext
                [dataGrid.DataSource,dataGrid.DataMember];

            //This is the DataView that we are going to fill up...
            DataView dataView = (DataView)currencyManager.List;

            // Add table, and apply default styles
            SectionTable table = AddTable (dataView, true);
            DgApplyDefaultStyles (table, textSize);

            // Get the tableStyle from the datagrid
            System.Windows.Forms.DataGridTableStyle tableStyle = null;
            foreach (System.Windows.Forms.DataGridTableStyle ts in dataGrid.TableStyles)
            {
                if (ts.MappingName == dataView.Table.TableName)
                {
                    tableStyle = ts;
                }
            }

            // Apply formatting
            Pen previousDefaultPen = DefaultTablePen;
            if (includeFormatting)
            {
                table.DetailRowTextStyle.SetFromFont (dataGrid.Font); // use dataGrid font
                if (tableStyle == null)
                {
                    DefaultTablePen = DgGridPen (table, dataGrid);
                    DgApplyTableStyles (table, dataGrid);
                }
                else
                {
                    DefaultTablePen = DgGridPen (table, tableStyle);
                    DgApplyTableStyles (table, tableStyle);
                }
            }

            // Finally add columns
            if (tableStyle == null)
            {
                float width = dataGrid.PreferredColumnWidth / DataGridToPrinterHScale;
                AddAllColumns (width, true, true);
            }
            else
            {
                DgAddColumns (table, tableStyle);
            }

            // restore default table pen
            DefaultTablePen = previousDefaultPen;
            return table;
        }


        #endregion


        #region "Header and Footer sections"


        /// <summary>
        /// Gets a LayeredSections container for the document's PageHeader.
        /// If ReportDocument's PageHeader is a valid LayeredSections
        /// object, then using this property returns that object.
        /// If ReportDocument's PageHeader is null or any other
        /// object, then using this property creates a new LayeredSection
        /// and replaces ReportDocument's PageHeader with the new section.
        /// </summary>
        public LayeredSections PageHeader
        {
            get
            {
                LayeredSections section = this.CurrentDocument.PageHeader as LayeredSections;
                if (section == null)
                {
                    section = new LayeredSections (true, false);
                    section.MarginBottom = headerFooterMargins;
                    this.CurrentDocument.PageHeader = section;
                }
                return section;
            }
        }

        /// <summary>
        /// Gets a LayeredSections container for the document's PageFooter.
        /// If ReportDocument's PageFooter is a valid LayeredSections
        /// object, then using this property returns that object.
        /// If ReportDocument's PageFooter is null or any other
        /// object, then using this property creates a new LayeredSection
        /// and replaces ReportDocument's PageFooter with the new section.
        /// </summary>
        public LayeredSections PageFooter
        {
            get
            {
                LayeredSections section = this.CurrentDocument.PageFooter as LayeredSections;
                if (section == null)
                {
                    section = new LayeredSections (true, false);
                    section.MarginTop = headerFooterMargins;
                    section.VerticalAlignment = VerticalAlignment.Bottom;
                    this.CurrentDocument.PageFooter = section;
                }
                return section;
            }
        }

        /// <summary>
        /// Gets a RepeatableTextSection with the given parameters.
        /// </summary>
        /// <param name="firstPageText">A string to use for the first page</param>
        /// <param name="evenPageText">A string to use for even pages</param>
        /// <param name="oddPageText">A string to use for odd pages</param>
        /// <param name="textStyle">The <see cref="ReportPrinting.TextStyle"/> for the text.</param>
        /// <param name="hAlign">The <see cref="ReportPrinting.HorizontalAlignment"/>
        /// for the text.</param>
        /// <returns>A RepeatableTextSection</returns>
        /// <remarks>
        /// <para>
        /// Specifing null for the firstPageText will result in oddPageText
        /// being used instead.  The blank string "" will suppress printing
        /// of a header.
        /// </para>
        /// This method is often used with the PageHeader and PageFooter
        /// properties as follows:
        /// <code>
        ///    builder.PageHeader.AddSection(builder.GetRepeatable(
        ///        "Text for first page",
        ///        "Text for odd pages",
        ///        "Text for even pages",
        ///        TextStyle.PageHeader,
        ///        StringAlignment.Center));
        /// </code>
        /// But it can be used anywhere you need to get a RepeateableTextSection.
        /// </remarks>
        public RepeatableTextSection GetRepeatable (string firstPageText, 
            string evenPageText, string oddPageText, 
            TextStyle textStyle, HorizontalAlignment hAlign)
        {
            RepeatableTextSection section = new RepeatableTextSection (
                oddPageText, textStyle);
            
            section.TextFirstPage = firstPageText;
            section.TextEvenPage = evenPageText;
            section.TextOddPage = oddPageText; 
            section.UseFullWidth = true;
            section.HorizontalAlignment = hAlign;
            return section;
        }


        /// <summary>
        /// Adds a page header to the report document with three sections of text
        /// and optionally a separate first page text
        /// </summary>
        /// <param name="leftText">
        /// Text to be displayed on the left side of the header
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="centerText">
        /// Text to be displayed in the center of the header
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="rightText">
        /// Text to be displayed on the right side of the header
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        public void AddPageHeader (
            string leftText,
            string centerText,
            string rightText
            )
        {
            AddPageHeader(leftText, centerText, rightText,
                leftText, centerText, rightText);
        }

        /// <summary>
        /// Adds a page header to the report document with three sections of text
        /// and optionally a separate first page text
        /// </summary>
        /// <param name="leftTextFirstPage">
        /// Text to be displayed on the left side of the header on the first page.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="centerTextFirstPage">
        /// Text to be displayed in the center of the header on the first page.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="rightTextFirstPage">
        /// Text to be displayed on the right side of the header on the first page.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="leftText">
        /// Text to be displayed on the left side of the header
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="centerText">
        /// Text to be displayed in the center of the header
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="rightText">
        /// Text to be displayed on the right side of the header
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        public void AddPageHeader (
            string leftTextFirstPage,
            string centerTextFirstPage,
            string rightTextFirstPage,
            string leftText,
            string centerText,
            string rightText
            )
        {
            AddPageHeader(
                leftTextFirstPage, centerTextFirstPage, rightTextFirstPage,
                leftText, centerText, rightText,
                leftText, centerText, rightText);
        }

        /// <summary>
        /// Adds a page header to the report document with three sections of text
        /// and optionally a separate first page text
        /// </summary>
        /// <param name="leftTextFirstPage">
        /// Text to be displayed on the left side of the header on the first page.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="centerTextFirstPage">
        /// Text to be displayed in the center of the header on the first page.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="rightTextFirstPage">
        /// Text to be displayed on the right side of the header on the first page.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="leftTextEvenPages">
        /// Text to be displayed on the left side of the header on even pages.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="centerTextEvenPages">
        /// Text to be displayed in the center of the header on even pages.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="rightTextEvenPages">
        /// Text to be displayed on the right side of the header on even pages.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="leftTextOddPages">
        /// Text to be displayed on the left side of the header on odd pages.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="centerTextOddPages">
        /// Text to be displayed in the center of the header on odd pages.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="rightTextOddPages">
        /// Text to be displayed on the right side of the header on odd pages.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        public void AddPageHeader (
            string leftTextFirstPage,
            string centerTextFirstPage,
            string rightTextFirstPage,
            string leftTextEvenPages,
            string centerTextEvenPages,
            string rightTextEvenPages,
            string leftTextOddPages,
            string centerTextOddPages,
            string rightTextOddPages
            )
        {
            AddPageHeader (leftTextFirstPage, leftTextEvenPages, leftTextOddPages, 
                 HorizontalAlignment.Left);
            AddPageHeader (centerTextFirstPage, centerTextEvenPages, centerTextOddPages, 
                 HorizontalAlignment.Center);
            AddPageHeader (rightTextFirstPage, rightTextEvenPages, rightTextOddPages, 
                 HorizontalAlignment.Right);
        }


        /// <summary>
        /// Adds a header to the report document with one section of text.
        /// </summary>
        /// <param name="text">Text to be displayed</param>
        /// <param name="hAlign">Alignment of the text within the header</param>
        public void AddPageHeader (
            string text,
            HorizontalAlignment hAlign
            )
        {
            PageHeader.AddSection (GetRepeatable(text, text, text, 
                TextStyle.PageHeader, hAlign));
        }

        /// <summary>
        /// Adds a header to the report document with one section of text.
        /// </summary>
        /// <param name="textFirstPage">Text to be displayed on the first page.</param>
        /// <param name="textEvenPages">Text to be displayed on even pages.</param>
        /// <param name="textOddPages">Text to be dsiplayed on odd pages.</param>
        /// <param name="hAlign">Alignment of the text within the header</param>
        public void AddPageHeader (
            string textFirstPage,
            string textEvenPages,
            string textOddPages,
            HorizontalAlignment hAlign
            )
        {
            PageHeader.AddSection (GetRepeatable(textFirstPage, textEvenPages, textOddPages, 
                 TextStyle.PageHeader, hAlign));
        }



        /// <summary>
        /// Adds a page footer to the report document with three sections of text
        /// and optionally a separate first page text
        /// </summary>
        /// <param name="leftText">
        /// Text to be displayed on the left side of the footer
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="centerText">
        /// Text to be displayed in the center of the footer
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="rightText">
        /// Text to be displayed on the right side of the footer
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        public void AddPageFooter (
            string leftText,
            string centerText,
            string rightText
            )
        {
            AddPageFooter(
                leftText, centerText, rightText,
                leftText, centerText, rightText,
                leftText, centerText, rightText);
        }

        /// <summary>
        /// Adds a page footer to the report document with three sections of text
        /// and optionally a separate first page text
        /// </summary>
        /// <param name="leftTextFirstPage">
        /// Text to be displayed on the left side of the footer on the first page.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="centerTextFirstPage">
        /// Text to be displayed in the center of the footer on the first page.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="rightTextFirstPage">
        /// Text to be displayed on the right side of the footer on the first page.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="leftText">
        /// Text to be displayed on the left side of the footer
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="centerText">
        /// Text to be displayed in the center of the footer
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="rightText">
        /// Text to be displayed on the right side of the footer
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        public void AddPageFooter (
            string leftTextFirstPage,
            string centerTextFirstPage,
            string rightTextFirstPage,
            string leftText,
            string centerText,
            string rightText
            )
        {
            AddPageFooter(
                leftTextFirstPage, centerTextFirstPage, rightTextFirstPage,
                leftText, centerText, rightText, 
                leftText, centerText, rightText);
        }

        /// <summary>
        /// Adds a page footer to the report document with three sections of text
        /// and optionally a separate first page text
        /// </summary>
        /// <param name="leftTextFirstPage">
        /// Text to be displayed on the left side of the footer on the first page.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="centerTextFirstPage">
        /// Text to be displayed in the center of the footer on the first page.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="rightTextFirstPage">
        /// Text to be displayed on the right side of the footer on the first page.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="leftTextEvenPages">
        /// Text to be displayed on the left side of the footer on even pages.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="centerTextEvenPages">
        /// Text to be displayed in the center of the footer on even pages.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="rightTextEvenPages">
        /// Text to be displayed on the right side of the footer on even pages.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="leftTextOddPages">
        /// Text to be displayed on the left side of the footer on odd pages.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="centerTextOddPages">
        /// Text to be displayed in the center of the footer on odd pages.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        /// <param name="rightTextOddPages">
        /// Text to be displayed on the right side of the footer on odd pages.
        /// Specifying String.Empty will leave the section blank.
        /// </param>
        public void AddPageFooter (
            string leftTextFirstPage,
            string centerTextFirstPage,
            string rightTextFirstPage,
            string leftTextEvenPages,
            string centerTextEvenPages,
            string rightTextEvenPages,
            string leftTextOddPages,
            string centerTextOddPages,
            string rightTextOddPages
            )
        {
            AddPageFooter (leftTextFirstPage, leftTextEvenPages, leftTextOddPages, 
                HorizontalAlignment.Left);
            AddPageFooter (centerTextFirstPage, centerTextEvenPages, centerTextOddPages, 
                HorizontalAlignment.Center);
            AddPageFooter (rightTextFirstPage, rightTextEvenPages, rightTextOddPages, 
                HorizontalAlignment.Right);
        }


        /// <summary>
        /// Adds a footer to the report document with one section of text.
        /// </summary>
        /// <param name="text">Text to be displayed</param>
        /// <param name="hAlign">Alignment of the text within the footer</param>
        public void AddPageFooter (
            string text,
            HorizontalAlignment hAlign
            )
        {
            PageFooter.AddSection (GetRepeatable(text, text, text,
                TextStyle.PageFooter, hAlign));
        }

        /// <summary>
        /// Adds a footer to the report document with one section of text.
        /// </summary>
        /// <param name="textFirstPage">Text to be displayed on the first page.</param>
        /// <param name="textEvenPages">Text to by displayed on even pages</param>
        /// <param name="textOddPages">Text to by displayed on odd pages</param>
        /// <param name="hAlign">Alignment of the text within the footer</param>
        public void AddPageFooter (
            string textFirstPage,
            string textEvenPages,
            string textOddPages,
            HorizontalAlignment hAlign
            )
        {
            PageFooter.AddSection (GetRepeatable(textFirstPage, textEvenPages, textOddPages, 
                 TextStyle.PageFooter, hAlign));
        }


        /// <summary>
        /// Adds a horizontal line to the bottom of the page header,
        /// to separate the header from the body.
        /// </summary>
        /// <remarks>
        /// In reality, the line goes into the bottom margin of the header's
        /// LayeredSections. This isn't completely desirable, but it's the
        /// easiest solution.
        /// </remarks>
        /// <returns>The SectionLine added.  Update the returned
        /// value to set pen, length, magins, etc.</returns>
        public SectionLine AddPageHeaderLine ()
        {
            SectionLine line = new SectionLine (Direction.Horizontal, CurrentDocument.NormalPen);
            line.VerticalAlignment = VerticalAlignment.Bottom;
            line.MarginBottom = -horizLineMargins;
            PageHeader.MarginBottom = 2 * horizLineMargins;
            PageHeader.AddSection (line);
            return line;
        }

        /// <summary>
        /// Adds a horizontal line to the top of the page footer,
        /// to separate the footer from the body.
        /// </summary>
        /// <remarks>
        /// In reality, the line goes into the top margin of the footer's
        /// LayeredSections. This isn't completely desirable, but it's the
        /// easiest solution.
        /// </remarks>
        /// <returns>The SectionLine added.  Update the returned
        /// value to set pen, length, magins, etc.</returns>
        public SectionLine AddPageFooterLine ()
        {
            SectionLine line = new SectionLine (Direction.Horizontal, CurrentDocument.NormalPen);
            line.VerticalAlignment = VerticalAlignment.Top;
            // a negative margin puts the line into the margin of the surrounding box
            line.MarginTop = -horizLineMargins;
            PageFooter.AddSection (line);
            PageFooter.MarginTop = 2 * horizLineMargins;
            return line;
        }

        #endregion


        #region "Obsolete methods"

        /// <summary>
        /// OBSOLETE METHOD - use AddText instead
        /// </summary>
        [Obsolete("Use AddText in the future")]
        public SectionText AddTextSection (string text, TextStyle textStyle)
        {
            return AddText (text, textStyle);
        }

        /// <summary>
        /// OBSOLETE METHOD - use AddText instead
        /// </summary>
        [Obsolete("Use AddText in the future")]
        public SectionText AddTextSection (string text)
        {
            return AddText (text);
        }

        /// <summary>
        /// OBSOLETE METHOD - use AddTable instead
        /// </summary>
        [Obsolete("Use AddTable in the future")]
        public SectionTable AddDataSection (
            DataView dataSource, bool showHeaderRow, float tablePercentWidth
            )
        {
            return AddTable (dataSource, showHeaderRow, tablePercentWidth);
        }

        /// <summary>
        /// OBSOLETE METHOD - use AddTable instead
        /// </summary>
        [Obsolete("Use AddTable in the future")]
        public SectionTable AddDataSection (
            DataView dataSource, bool showHeaderRow
            )
        {
            return AddTable (dataSource, showHeaderRow, 0);
        }

        /// <summary>
        /// OBSOLETE METHOD - use AddTable and AddColumn instead
        /// </summary>
        [Obsolete("Use separate AddTable and AddColumn or AddAllColumns in the future")]
        public SectionTable AddDataSection (
            DataView dataSource, bool showHeaderRow,
            float maxColumnWidth, bool sizeWidthToHeader, bool sizeWidthToContents
            )
        {
            SectionTable section = AddDataSection(dataSource, showHeaderRow, 0f);
            AddAllColumns (maxColumnWidth, sizeWidthToHeader, sizeWidthToContents);
            return section;
        }
        #endregion



	} // end class


    /// <summary>
    /// Exception thrown by the builder class when it is used incorrectly
    /// </summary>
    public class ReportBuilderException : ApplicationException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="msg">Message</param>
        public ReportBuilderException(string msg)
            : base(msg)
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="msg">Message</param>
        /// <param name="innerException">An inner exception</param>
        public ReportBuilderException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }
    }
}
