// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/

using System;
using System.Data;
using System.Drawing;

namespace ReportPrinting
{

    /// <summary>
    /// Interface used for a class that wants to be a DataColumn
    /// </summary>
    public interface IDataColumn
	{
        /// <summary>
        /// Gets or sets the width of the column
        /// </summary>
        float Width { get; set; }
        /// <summary>
        /// Gets or sets the max width of the column
        /// </summary>
        float MaxWidth { get; set; }
        /// <summary>
        /// Gets or sets the pen used to draw a line on the right side of the column
        /// </summary>
        Pen RightPen { get; set; }
        /// <summary>
        /// Gets or sets a flag to size the width of the column to the contents of the column
        /// </summary>
        bool SizeWidthToContents { get; set; }
        /// <summary>
        /// Gets or sets a flag to size the width of the column to the header of the column
        /// </summary>
        bool SizeWidthToHeader { get; set; }
        /// <summary>
        /// Gets or sets the max height of a header of this column (should be set only through the parent table.)
        /// </summary>
        float MaxHeaderRowHeight { get; set; }
        /// <summary>
        /// Gets or sets the max height of a row of this column  (should be set only through the parent table.)
        /// </summary>
        float MaxDetailRowHeight { get; set; }

        /// <summary>
        /// Gets or sets the TextStyle used for the header row.
        /// </summary>
        TextStyle HeaderTextStyle { get; set; }
        /// <summary>
        /// Gets or sets the TextStyle used for detail rows.
        /// </summary>
        TextStyle DetailRowTextStyle { get; set; }
        /// <summary>
        /// Gets or sets the TextStyle used for alternating rows.
        /// </summary>
        TextStyle AlternatingRowTextStyle { get; set; }
		/// <summary>
		/// Gets or sets the TextStyle used for a final summary row.
		/// </summary>
		TextStyle SummaryRowTextStyle { get; set; }


        /// <summary>
        /// Size the entire column, based on the properties
        /// SizeWidthToContents, SizeWidthToHeader, and anything else useful.
        /// </summary>
        /// <param name="g">Graphics used for measuring</param>
        /// <param name="dataSource">DataView of the actual data that will be printed</param>
        void SizeColumn (Graphics g, DataView dataSource);

        /// <summary>
        /// Draw the line down the right side of this column
        /// </summary>
        /// <param name="g">Graphics to draw on</param>
        /// <param name="x">X coordinate of the right edge of this column.
        /// Line will be drawn just to the left of this edge.</param>
        /// <param name="y">Y coordinate of the top of this column</param>
        /// <param name="height">Height of this column</param>
        void DrawRightLine (Graphics g, float x, float y, float height);
        

        /// <summary>
        /// Size or paint a cell in this column
        /// </summary>
        /// <param name="g">Graphics used for measuring or painting</param>
        /// <param name="headerRow">Cell painted should be a header row</param>
        /// <param name="alternatingRow">Cell painted is in an alternating row</param>
		/// <param name="summaryRow">True if this row is a summary row</param>
		/// <param name="drv">DataRowView used if this is not a header row</param>
        /// <param name="x">X coordinate of the left side of the cell</param>
        /// <param name="y">Y coordinate of the top of the cell</param>
        /// <param name="width">Width of the cell</param>
        /// <param name="height">Height of the cell (max height when in sizeonly mode)</param>
        /// <param name="sizeOnly">Only size the cell (don't paint) if true</param>
        /// <returns>Required size of the cell</returns>
        SizeF SizePaintCell (Graphics g, bool headerRow, bool alternatingRow, bool summaryRow, 
			DataRowView drv, float x, float y, float width, float height, bool sizeOnly);
	}

}
