// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/

using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ReportPrinting
{

    /// <summary>
    /// ReportDataColumn provides the necessary information for
    /// formatting data for a column of a report.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For every column to be presented within a section of data,
    /// a new ReportDataColumn object is instantiated and added
    /// to the <see cref="ReportPrinting.ReportSection"/>.
    /// At a minimum, each column describes a
    /// source field (column) from the DataSource,
    /// and a maximum width for the column.
    /// </para>
    /// <para>
    /// The ReportDataColumn can also be setup with its own unique
    /// <see cref="ReportPrinting.TextStyle"/> for header and normal rows.
    /// Therefore, each column's data could be formatted differently.
    /// Use the TextStyle to setup font, color, and alignment (left, center, right)
    /// </para>
    /// <para>
    /// A summary row will be printed if ShowSummaryRow is true for the table.
    /// Be sure to either set SummaryRowText or create an event handler for
    /// FormatSummaryRow to set the correct text to be displayed.
    /// </para>
    /// </remarks>
    public class ReportDataColumn : IDataColumn
    {
        string      field;
        float       width;
        float       maxWidth;
        string      formatExpression;
        string      headerRowText;
		string      summaryRowText;
        TextStyle   headerTextStyle;
        TextStyle   detailRowTextStyle;
        TextStyle   alternatingRowTextStyle;
		TextStyle   summaryRowTextStyle;
        bool        sizeWidthToHeader;
        bool        sizeWidthToContents;
        Pen         rightPen;
        string      nullValueString = "<NULL>";
        float       maxDetailRowHeight;
        float       maxHeaderRowHeight;
		bool        displaySum;
		bool        displayAvg;
		bool        displayCount;
		double      sumOfValues;
		double      countOfValues;
		Hashtable   rowValues;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="field">The name of the field in the DataSource to be used in this column</param>
        /// <param name="maxWidth">The maxWidth for this column.</param>
        public ReportDataColumn (string field, float maxWidth)
        {
            Field = field;
            MaxWidth = maxWidth;
            HeaderRowText = field;
			this.sumOfValues = 0;
			this.countOfValues = 0;
			rowValues = new Hashtable ();
        }

        /// <summary>
        /// An event fired for every cell in the column to
        /// get a properly formatted string for the corresponding object
        /// in the databse.
        /// </summary>
		public event FormatColumnHandler FormatColumn;

		/// <summary>
		/// An event fired for every cell of the column to
		/// allow customization of how sum and count are calculated.
		/// </summary>
		public event UpdateMathHandler UpdateMath;

		/// <summary>
		/// An event fired to format the text for a summary row.
		/// </summary>
		public event FormatSummaryRowHandler FormatSummaryRow;


        #region "Properties part of IDataColumn"

        /// <summary>
        /// Gets or sets the width of the column.
        /// </summary>
        public float Width
        {
            get { return this.width; }
            set { this.width = value; }
        }

        /// <summary>
        /// Gets or sets the maximum width of the column.
        /// </summary>
        public float MaxWidth
        {
            get { return this.maxWidth; }
            set {this.maxWidth = value; }
        }

        /// <summary>
        /// Gets or sets a flag indicating the 
        /// column's width is sized to the header text
        /// The Width property, if non zero,
        /// will set the maximum width. 
        /// </summary>
        public bool SizeWidthToHeader
        {
            get { return this.sizeWidthToHeader; }
            set { this.sizeWidthToHeader = value; }
        }

        /// <summary>
        /// Gets or sets a flag indicating the 
        /// column's width is size to the contents of
        /// all cells (not including header text).  This adds
        /// a bit of processing time for long tables.
        /// The Width property, if non zero,
        /// will set the maximum width.
        /// </summary>
        public bool SizeWidthToContents
        {
            get { return this.sizeWidthToContents; }
            set { this.sizeWidthToContents = value; }
        }
              
        /// <summary>
        /// Gets or sets the pen used to draw the line for columns
        /// </summary>
        public Pen RightPen
        {
            get { return this.rightPen; }
            set { this.rightPen = value; }
        }


        // Uses explicit interface implementation to kindof hide these properties from
        // a user.

        /// <summary>
        /// Gets or sets the max height of a header of this column (should be set only through the parent table.)
        /// </summary>
        float IDataColumn.MaxHeaderRowHeight 
        {
            get { return this.maxHeaderRowHeight; }
            set { this.maxHeaderRowHeight = value; }
        }

        /// <summary>
        /// Gets or sets the max height of a row of this column  (should be set only through the parent table.)
        /// </summary>
        float IDataColumn.MaxDetailRowHeight 
        {
            get { return this.maxDetailRowHeight; }
            set { this.maxDetailRowHeight = value; }
        }

        /// <summary>
        /// Gets or sets the text style to use for text
        /// </summary>
        public TextStyle HeaderTextStyle
        {
            get { return this.headerTextStyle; }
            set { this.headerTextStyle = value; }
        }

        /// <summary>
        /// Gets or sets the text style to use for text
        /// </summary>
        public TextStyle DetailRowTextStyle
        {
            get { return this.detailRowTextStyle; }
            set { this.detailRowTextStyle = value; }
        }

        /// <summary>
        /// Gets or sets the text style to use for text in even rows
        /// </summary>
        public TextStyle AlternatingRowTextStyle
        {
            get { return this.alternatingRowTextStyle; }
            set { this.alternatingRowTextStyle = value; }
        }

		/// <summary>
		/// Gets or sets the text style to use for text in the summary row
		/// </summary>
		public TextStyle SummaryRowTextStyle
		{
			get { return this.summaryRowTextStyle; }
			set { this.summaryRowTextStyle = value; }
		}
		
        /// <summary>
        /// Gets or sets the <see cref="System.String"/> to display in the header row.
        /// The default value is the field name.
        /// </summary>
        public string HeaderRowText
        {
            get { return this.headerRowText; }
            set { this.headerRowText = value; }
        }

        #endregion


        #region "Public properties"


        /// <summary>
        /// Gets or sets the source field from the DataSource.  That is, this is
        /// the DataTable column name. 
        /// </summary>
        public string Field
        {
            get { return this.field; }
            set {this.field = value; }
        }

        string prefix = "{0:";
        string suffix = "}";
        /// <summary>
        /// Gets or sets the format expression to use for output formatting.
        /// </summary>
        public string FormatExpression
        {
            get 
            { 
                return this.formatExpression; 
            }
            set 
            { 
                if (value.StartsWith(prefix))
                {
                    Debug.WriteLine ("Deprecated use of FormatExpression.  In the future, omit the {0:}");
                    this.formatExpression = value;
                }
                else
                {
                    this.formatExpression = prefix + value + suffix;
                }
            }
        }


        /// <summary>
        /// Gets or sets the string used to represent null values in a cell.
        /// </summary>
        public string NullValueString
        {
            get { return this.nullValueString; }
            set { this.nullValueString = value; }
        }

		/// <summary>
		/// Gets or sets the string used to represent the summary row.
		/// Some special values can be used as expressions.
		/// The text actually printed may be changed in an event handler for
		/// FormatSummaryRow
		/// </summary>
		/// <remarks>
		/// Special values for SummaryRow include:
		/// <list type="table">
		/// <listheader>
		/// <term>Value</term>
		/// <description>Result in Table</description>
		/// </listheader>
		/// <item>
		/// <term>=[avg]</term>
		/// <description>Shows the mean (average) of all displayed values in the column.</description>
		/// </item>
		/// <item>
		/// <term>=[sum]</term>
		/// <description>Shows the sum of all displayed values in the column.</description>
		/// </item>
		/// <item>
		/// <term>=[count]</term>
		/// <description>Shows the number of items in the column (number of rows).</description>
		/// </item>
		/// </list>
		/// </remarks>
		public string SummaryRowText
		{
			get { return this.summaryRowText; }
			set 
			{
				this.summaryRowText = String.Empty;
				this.displayAvg = false;
				this.displayCount = false;
				this.displaySum = false;
				switch (value)
				{
					case "=[avg]" :
						this.displayAvg = true;
						break;
					case "=[sum]" :
						this.displaySum = true;
						break;
					case "=[count]" :
						this.displayCount = true;
						break;
					default :
						this.summaryRowText = value; 
						break;
				}
			}
		}

        #endregion


        #region "Implementation of IDataColumn public methods"

        /// <summary>
        /// Set the size of the column - must be called prior to printing
        /// or else width is never set.
        /// </summary>
        /// <param name="g">Graphics object</param>
        /// <param name="dataSource">DataSource for the cell contents</param>
        public virtual void  SizeColumn (
            Graphics g, DataView dataSource)
        {
            float maxHeight = this.maxHeaderRowHeight;
            float headerWidth = 0f;
            if (SizeWidthToHeader)
            {
                // size the header
                SizeF headerSize = SizePaintCell( 
                    g, true, false, false, null, 0, 0, this.MaxWidth, maxHeight, true);
                headerWidth = headerSize.Width;
            }

            maxHeight = this.maxDetailRowHeight;
            float contentWidth = 0f;
            if (SizeWidthToContents)
            {
                bool alternatingRow = false;
                foreach (DataRowView drv in dataSource)
                {
                    // size the rows
                    SizeF cellSize = SizePaintCell ( 
                        g, false, alternatingRow, false, drv, 0, 0, this.MaxWidth, maxHeight, true);
                    // use the new width if it is bigger
                    contentWidth = Math.Max(contentWidth, cellSize.Width);
                    alternatingRow = !alternatingRow;
                }
				// ToDo: size the summary row?

            }

            // find the maximum used width (if nonzero), and use it 
            // if it's less than the max
            float maxUsedWidth = Math.Max(headerWidth, contentWidth);
            if (maxUsedWidth > 0 && maxUsedWidth < this.maxWidth)
            {
                this.width = maxUsedWidth;
            }
            else
            {
                this.width = this.maxWidth;
            }
        }



        /// <summary>
        /// Paints or measures the object passed in according 
        /// to the formatting rules of this column.
        /// </summary>
        /// <param name="g">the graphics to paint the value onto</param>
        /// <param name="headerRow">True if this is a header row</param>
        /// <param name="alternatingRow">True if this row is an "alternating" row (even row most likely)</param>
        /// <param name="summaryRow">True if this row is a summary row</param>
        /// <param name="drv">DataRowView to grab the cell from</param>
        /// <param name="x">the x coordinate to start the paint</param>
        /// <param name="y">the y coordinate to start the paint</param>
        /// <param name="width">the width of the cell</param>
        /// <param name="height">The max height of this cell (when in sizeOnly mode)</param>
        /// <param name="sizeOnly">only calculate the sizes</param>
        /// <returns>A sizeF representing the measured size of the string + margins</returns>
        public virtual SizeF SizePaintCell
            ( 
            Graphics g, bool headerRow, bool alternatingRow, bool summaryRow,
			DataRowView drv, float x, float y, float width,
            float height, bool sizeOnly)
        {
            string text = GetString (headerRow, summaryRow, drv);
            TextStyle textStyle = GetTextStyle (headerRow, alternatingRow, summaryRow);
			if (summaryRow && drv != null)
			{
				if ( this.displayAvg )
					drv[this.Field] = this.sumOfValues / this.countOfValues;
				else if ( this.displayCount )
					drv[this.Field] = this.countOfValues;
				else if ( this.displaySum )
					drv[this.Field] = this.sumOfValues;
			}
			return SizePaintCell(g, text, textStyle, x, y, width, height, sizeOnly);
		}

		/// <summary>
		/// Paints or measures the object passed in according 
		/// to the formatting rules of this column.
		/// </summary>
		/// <param name="g">the graphics to paint the value onto</param>
		/// <param name="text">the text to paint</param>
		/// <param name="textStyle">the textStyle to use to paint the text</param>
		/// <param name="x">the x coordinate to start the paint</param>
		/// <param name="y">the y coordinate to start the paint</param>
		/// <param name="width">the width of the cell</param>
		/// <param name="height">The max height of this cell (when in sizeOnly mode)</param>
		/// <param name="sizeOnly">only calculate the sizes</param>
		/// <returns>A sizeF representing the measured size of the string + margins</returns>
		public virtual SizeF SizePaintCell( 
			Graphics g, string text, TextStyle textStyle,
			float x, float y, float width,
			float height, bool sizeOnly)
		{
			SizeF stringSize = new SizeF(width, height);
			Font font = textStyle.GetFont();
            StringFormat stringFormat = textStyle.GetStringFormat();

            float sideMargins = textStyle.MarginNear + textStyle.MarginFar + RightPenWidth;
            float topBottomMargins = textStyle.MarginTop + textStyle.MarginBottom;
            Bounds bounds = new Bounds (x, y, x + width, y + height);
            Bounds innerBounds = bounds.GetBounds (textStyle.MarginTop, 
                textStyle.MarginFar + RightPenWidth, textStyle.MarginBottom, textStyle.MarginNear);
            SizeF maxSize = innerBounds.GetSizeF();

            if (sizeOnly)
            {
                // Find the height of the actual string to be drawn
                stringSize = g.MeasureString(text, font, maxSize, stringFormat);
                stringSize.Width += sideMargins;
                stringSize.Height += topBottomMargins;
                // Don't go bigger than maxHeight
                stringSize.Height = Math.Min (stringSize.Height, height);
            }
            else
            {
                // draw background & text
                if (textStyle.BackgroundBrush != null)
                {
                    g.FillRectangle (textStyle.BackgroundBrush, bounds.GetRectangleF());
                }
                RectangleF textLayout = innerBounds.GetRectangleF (stringSize, 
                    SectionText.ConvertAlign (textStyle.StringAlignment), 
                    textStyle.VerticalAlignment);
                g.DrawString(text, font, textStyle.Brush, textLayout, stringFormat);
            }
            return stringSize;
        }


        /// <summary>
        /// Draws the line down the right side of the column
        /// </summary>
        /// <param name="g">Graphics object to drawn on</param>
        /// <param name="x">The right side of the column (line is drawn just to the left of the line)</param>
        /// <param name="y">Y position of the top of the line</param>
        /// <param name="height">The height of the line</param>
        public virtual void DrawRightLine (Graphics g, float x, float y, float height)
        {
            if (this.RightPen != null)
            {
                // Draw to the inside of the rectangle
                x -= this.RightPenWidth;
                g.DrawLine (RightPen, x, y, x, y + height);
            }
        }

        #endregion


        #region "Protected methods / properties"

        /// <summary>
        /// Gets the TextStyle to use for this column
        /// </summary>
        /// <param name="headerRow">True if this is a header row</param>
        /// <param name="alternatingRow">True if this is an alternating row</param>
        /// <param name="summaryRow">True if this is a summary row</param>
        /// <returns>The TextStyle to use</returns>
        protected virtual TextStyle GetTextStyle (bool headerRow, bool alternatingRow,
			bool summaryRow)
        {
            TextStyle style;
            if (headerRow)
            {
                style = this.HeaderTextStyle;
            }
            else
            {
                if (alternatingRow && this.AlternatingRowTextStyle != null)
                {
                    style = this.AlternatingRowTextStyle;
                }
                else
                {
                    style = this.DetailRowTextStyle;
                }

				if (summaryRow && this.SummaryRowTextStyle != null)
				{
					style = this.SummaryRowTextStyle;
				}
			}
            return style;
        }

        /// <summary>
        /// Gets the width of the pen, or 0 if null
        /// </summary>
        protected float RightPenWidth
        {
            get
            {
                float width = 0;
                if (RightPen != null)
                {
                    width = RightPen.Width;
                }
                return width;
            }
        }

        /// <summary>
        /// Gets a string for the information in this column based on
        /// the object passed.
        /// </summary>
        /// <param name="headerRow">True if the text should be the header text.</param>
        /// <param name="summaryRow">True if the text should reflect a summary row</param>
        /// <param name="drv">DataRowView of the current row being printed</param>
        /// <returns>A string to print in the report</returns>
        protected virtual string GetString (bool headerRow, bool summaryRow, DataRowView drv)
        {
            if (headerRow)
            {
                return this.headerRowText;
            }
			if (summaryRow)
			{
				return GetSummaryRowString();
			}

			string retval;
			if (this.rowValues.ContainsKey (drv))
			{
				retval = (string)this.rowValues[drv];
			}
			else
			{
				retval = GetDetailRowString (drv);
				this.rowValues.Add (drv, retval);
			}
			return retval;

        } // GetString

		/// <summary>
		/// Applies FormatExpression to an object to convert it to a string
		/// </summary>
		/// <param name="obj">Any object</param>
		/// <returns>A string</returns>
		protected virtual string ApplyFormat (object obj)
		{
			if (this.FormatExpression == null || this.FormatExpression == String.Empty)
			{
				return obj.ToString();
			}
			else
			{
				return String.Format(this.FormatExpression, obj);
			}

		}

		/// <summary>
		/// Gets a string for a detail row, given a DataRowView for the row
		/// </summary>
		/// <param name="drv">DataRowView for the given row</param>
		/// <returns>String to be printed to the report</returns>
		protected virtual string GetDetailRowString (DataRowView drv)
		{
			object obj = drv[this.Field];
			FormatColumnEventArgs e = new FormatColumnEventArgs();
			e.OriginalValue = obj;
			if (obj != null)
			{
				e.StringValue = ApplyFormat (obj);
			}
			else
			{
				e.StringValue = this.NullValueString;
			}
			// fire event
			if (this.FormatColumn != null)
			{
				this.FormatColumn (this, e);
			}

			UpdateMathForRow (drv, obj, e.StringValue);

			return e.StringValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="drv"></param>
		/// <param name="obj"></param>
		/// <param name="stringRepresentation"></param>
		protected virtual void UpdateMathForRow (DataRowView drv, object obj, string stringRepresentation)
		{
			if (UpdateMath == null)
			{
				// only calc sum if we need it (save on effort of conversions, etc)
				if (this.displayAvg || this.displaySum)
				{
					try
					{
//						this.sumOfValues += Convert.ToDouble (stringRepresentation);
						this.sumOfValues += Convert.ToDouble (obj);
					}
					catch (Exception ex)
					{
						Console.WriteLine (ex.Message);
					}
				}
				this.countOfValues++;
			}
			else
			{
				UpdateMathEventArgs e = new UpdateMathEventArgs (drv, obj, stringRepresentation);
				e.Count = this.countOfValues;
				e.Sum = this.sumOfValues;
				UpdateMath (this, e);
				this.sumOfValues = e.Sum;
				this.countOfValues = e.Count;
			}
		}

		/// <summary>
		/// Gets the string to use in the summary row by firing the FormatSummaryRow event.
		/// </summary>
		/// <returns>The string to use on the summary row.</returns>
		protected virtual string GetSummaryRowString ()
		{
			string text = this.SummaryRowText;
			if (this.displayCount)
			{
				text = this.countOfValues.ToString();
			}
			else if (this.displaySum)
			{
				text = ApplyFormat (this.sumOfValues);
			}
			else if (this.displayAvg)
			{
				double avg = this.sumOfValues / this.countOfValues;
				text = ApplyFormat (avg);
			}

			// Raise event for formatting
			FormatSummaryRowEventArgs e = new FormatSummaryRowEventArgs();
			e.Field = this.Field;
			e.StringValue = text;
			if (this.FormatSummaryRow != null)
			{
				this.FormatSummaryRow (this, e);
			}
			return e.StringValue;
		}


        #endregion

    } // class ColumnInfo

}
