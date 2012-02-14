// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/

using System;
using System.Drawing;
using System.Diagnostics;

namespace ReportPrinting
{
	/// <summary>
	/// A very simple ReportSection that prints a line.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A line can be created with a direction, a pen,
	/// and a length.  If no pen is specifed, the NormalLine
	/// pen from the current document will be used.
	/// If no length is specified, it is the full length,
	/// minus margins.
	/// </para>
	/// <para>
	/// The direction of the line (horizontal or vertical)
	/// is specifed with the Direction property.  Margins
	/// and alignment are specified the same as for other
	/// ReportSections.
	/// </para>
    /// </remarks>
    public class SectionLine : ReportSection
	{
        Pen pen;
		float length;
		Direction direction;
        // Following are set by SetLinePoints
        float y1;
        float y2;
        float x1; 
        float x2;

        /// <summary>
        /// Creates a report section line with default length and pen
        /// </summary>
        /// <param name="direction">Direction of the line (horizontal or vertical)</param>
        public SectionLine(Direction direction)
        {
            this.direction = direction;
        }

        /// <summary>
		/// Creates a report section line with default length
		/// </summary>
        /// <param name="direction">Direction of the line (horizontal or vertical)</param>
        /// <param name="pen">Pen to use to draw the line</param>
        public SectionLine(Direction direction, Pen pen)
		{
			this.direction = direction;
			this.pen = pen;
		}

		/// <summary>
		/// Creates a report section line
		/// </summary>
        /// <param name="direction">Direction of the line (horizontal or vertical)</param>
        /// <param name="pen">Pen to use to draw the line</param>
		/// <param name="length">Length of the line (in inches)</param>
		public SectionLine(Direction direction, Pen pen, float length)
		{
			this.direction = direction;
			this.pen = pen;
			this.length = length;
		}

        /// <summary>
        /// Creates a report section line with default pen
        /// </summary>
        /// <param name="direction">Direction of the line (horizontal or vertical)</param>
        /// <param name="length">Length of the line (in inches)</param>
        public SectionLine(Direction direction, float length)
        {
            this.direction = direction;
            this.length = length;
        }

        #region "Properties"

        /// <summary>
        /// Gets or sets the length of the line.
        /// </summary>
        public float Length
        {
            get { return this.length; }
            set { this.length = value; }
        }

        /// <summary>
        /// Gets or sets the Pen used to draw the line.
        /// </summary>
        public Pen Pen
        {
            get { return this.pen; }
            set { this.pen = value; }
        }

        /// <summary>
        /// Gets or sets the direction for the line.
        /// </summary>
        public Direction Direction
        {
            get { return this.direction; }
            set { this.direction = value; }
        }

        #endregion

        /// <summary>
        /// Sets the default pen to the NormalPen used by reportDocument
        /// </summary>
        /// <param name="reportDocument">Parent ReportDocument</param>
        protected virtual void SetDefaultPen (ReportDocument reportDocument)
        {
            if (this.pen == null)
            {
                this.pen = reportDocument.NormalPen;
            }
        }
        
        /// <summary>
        /// Does nothing
        /// </summary>
        protected override void DoBeginPrint(Graphics g)
        {
        }

        /// <summary>
        /// Sets the line points y1, y2, x1, and x2 based
        /// on length, direction, etc.
        /// </summary>
        /// <param name="bounds">The maximum bounds for the line</param>
        protected virtual void SetLinePoints (Bounds bounds)
        {
            float halfWidth = this.pen.Width / 2;
            if (this.direction == Direction.Horizontal)
            {
                switch (this.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        y1 = bounds.Position.Y + halfWidth;
                        break;
                    case VerticalAlignment.Middle:
                        y1 = (bounds.Position.Y + bounds.Limit.Y) / 2;
                        break;
                    case VerticalAlignment.Bottom:
                        y1 = bounds.Limit.Y - halfWidth;
                        break;
                }
                y2 = y1;

                if (this.length == 0)
                {
                    x1 = bounds.Position.X;
                    x2 = bounds.Limit.X;
                }
                else
                {
                    switch (this.HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            x1 = bounds.Position.X;
                            x2 = x1 + this.length;
                            break;
                        case HorizontalAlignment.Center:
                            x1 = bounds.Position.X + 
                                (bounds.Width - this.length) / 2;
                            x2 = x1 + this.length;
                            break;
                        case HorizontalAlignment.Right:
                            x2 = bounds.Limit.X;
                            x1 = x2 - this.length;
                            break;
                    }
                }
            }
            else  // Vertical line
            {
                switch (this.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        x1 = bounds.Position.X + halfWidth;
                        break;
                    case HorizontalAlignment.Center:
                        x1 = (bounds.Position.X + bounds.Limit.X) / 2;
                        break;
                    case HorizontalAlignment.Right:
                        x1 = bounds.Limit.X - halfWidth;
                        break;
                }
                x2 = x1;

                if (this.length == 0)
                {
                    y1 = bounds.Position.Y;
                    y2 = bounds.Limit.Y;
                }
                else
                {
                    switch (this.VerticalAlignment)
                    {
                        case VerticalAlignment.Top:
                            y1 = bounds.Position.Y;
                            y2 = y1 + this.length;
                            break;
                        case VerticalAlignment.Middle:
                            y1 = bounds.Position.Y + 
                                (bounds.Height - this.length) / 2;
                            y2 = y1 + this.length;
                            break;
                        case VerticalAlignment.Bottom:
                            y2 = bounds.Limit.Y;
                            y1 = y2 - this.length;
                            break;
                    }
                }
            }

            // range checking
            x1 = Math.Max (x1, bounds.Position.X);
            x2 = Math.Min (x2, bounds.Limit.X);
            y1 = Math.Max (y1, bounds.Position.Y);
            y2 = Math.Min (y2, bounds.Limit.Y);
        }

        /// <summary>
        /// Returns the sizeF for the current line
        /// </summary>
        SizeF GetSizeF()
        {
            float height = y2 - y1;
            float width = x2 - x1;
            switch (this.direction)
            {
                case Direction.Horizontal:
                    height = this.pen.Width;
                    break;
                case Direction.Vertical:
                    width = this.pen.Width;
                    break;
            }

            return new SizeF (width, height);
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
        protected override SectionSizeValues DoCalcSize (
            ReportDocument reportDocument,
            Graphics g,
            Bounds bounds
            )
        {

            SetDefaultPen (reportDocument);
            SetLinePoints (bounds);

            SectionSizeValues retvals = new SectionSizeValues();
            retvals.Fits = true;
            retvals.Continued = false;
            retvals.RequiredSize = GetSizeF();
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
			if ( !reportDocument.NoPrint ) 
				g.DrawLine(this.pen, x1, y1, x2, y2);
        }


        /// <summary>
        /// Re-computes the line based on changed bounds
        /// </summary>
        /// <param name="originalBounds">Original bounds used for size</param>
        /// <param name="newBounds">New bounds</param>
        /// <returns>SectionSizeValues struct</returns>
        protected override SectionSizeValues BoundsChanged (
            Bounds originalBounds,
            Bounds newBounds)
        {
            SetLinePoints (newBounds);

            SectionSizeValues retvals = new SectionSizeValues();
            retvals.Fits = true;
            retvals.Continued = false;
            retvals.RequiredSize = GetSizeF();
            return retvals;
        }


	}
}
