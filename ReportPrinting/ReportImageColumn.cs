// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/

using System;
using System.Drawing;
using System.Data;

namespace ReportPrinting
{
	/// <summary>
	/// A DataColumn that is used for true / false columns by
	/// showing an image for true and a different (or no) image for false.
	/// </summary>
	public class ReportImageColumn : ReportDataColumn
	{
		/// <summary>
		/// Constructor of a true false column that creates a column
		/// using, be default, a check mark for true and no image for false values.
		/// </summary>
		/// <param name="field">The field within a dataview to use for the column.</param>
		/// <param name="maxWidth">The maximum width for this column</param>
		public ReportImageColumn(string field, float maxWidth) : base (field, maxWidth)
		{

		}

		/// <summary>
		/// Height of the image.
		/// </summary>
		public float ImageWidth = 0.20f;
		/// <summary>
		/// Width of the image.
		/// </summary>
		public float ImageHeight = 0.20f;


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
		public override SizeF SizePaintCell
			( 
			Graphics g, bool headerRow, bool alternatingRow, bool summaryRow,
			DataRowView drv, float x, float y, float width,
			float height, bool sizeOnly)
		{
			if (headerRow || summaryRow)
			{
				return base.SizePaintCell (g, headerRow, alternatingRow, summaryRow, drv, x, y, width, height, sizeOnly);
			}

			// else it's a data row...
			// TODO: Update cell count and sum???
			SizeF cellSize = new SizeF(width, height);
			Image image = GetImage (drv);
			TextStyle textStyle = GetTextStyle (headerRow, alternatingRow, summaryRow);

			float sideMargins = textStyle.MarginNear + textStyle.MarginFar + RightPenWidth;
			float topBottomMargins = textStyle.MarginTop + textStyle.MarginBottom;
			Bounds bounds = new Bounds (x, y, x + width, y + height);
			Bounds innerBounds = bounds.GetBounds (textStyle.MarginTop, 
				textStyle.MarginFar + RightPenWidth, textStyle.MarginBottom, textStyle.MarginNear);
			SizeF maxSize = innerBounds.GetSizeF();

			if (sizeOnly)
			{
				if (image == null)
				{
					cellSize.Width = 0;
					cellSize.Height = 0;
				}
				else
				{
					cellSize.Width = ImageWidth;
					cellSize.Height = ImageHeight;
				}
			}
			else
			{
				// draw background
				if (textStyle.BackgroundBrush != null)
				{
					g.FillRectangle (textStyle.BackgroundBrush, bounds.GetRectangleF());
				}
				// draw image
				if (image != null)
				{
					RectangleF cellLayout = GetImageRect (innerBounds, image, textStyle);
					g.DrawImage (image, cellLayout);
				}
			}
			return cellSize;
		}


		/// <summary>
		/// Gets the image to draw based on the value in the DataRowView
		/// </summary>
		/// <param name="drv">DataRowView for the row currently being printed</param>
		/// <returns>An Image to draw</returns>
		protected virtual Image GetImage (DataRowView drv)
		{
			Image image;
			object cellVal = drv[Field];
			image = cellVal as Image;
			return image;
		}

		/// <summary>
		/// Gets a rectangle for the image
		/// </summary>
		RectangleF GetImageRect (Bounds bounds, Image image, TextStyle textStyle)
		{
			SizeF maxSize = bounds.GetSizeF();
			SizeF imgSize = new SizeF (ImageWidth, ImageHeight);

			return bounds.GetRectangleF (imgSize, 
				SectionText.ConvertAlign (textStyle.StringAlignment),
				textStyle.VerticalAlignment);
		}



	}
}
