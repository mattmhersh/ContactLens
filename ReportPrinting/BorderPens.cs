// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/

using System;
using System.Drawing;
using System.Diagnostics;

namespace ReportPrinting
{

    /// <summary>
    /// BorderPens stores four pens, one to draw each side of a box
    /// or border.  It has functions that will draw that
    /// border, given a rectangle for the outer bounds of the border.
    /// </summary>
    public class BorderPens
    {
        /// <summary>
        /// Pen for the top border
        /// </summary>
        public Pen Top;
        /// <summary>
        /// Pen for the right border
        /// </summary>
        public Pen Right;
        /// <summary>
        /// Pen for the bottom border
        /// </summary>
        public Pen Bottom;
        /// <summary>
        /// Pen for the left border
        /// </summary>
        public Pen Left;

        #region "Properties"
        /// <summary>
        /// Gets the width of the top side
        /// </summary>
        public float TopWidth
        {
            get 
            { 
                float width = 0;
                if (Top != null) width = Top.Width;
                return width;
            }
        }

        /// <summary>
        /// Gets the width of the right side
        /// </summary>
        public float RightWidth
        {
            get 
            { 
                float width = 0;
                if (Right != null) width = Right.Width;
                return width;
            }
        }

        /// <summary>
        /// Gets the width of the bottom side
        /// </summary>
        public float BottomWidth
        {
            get 
            { 
                float width = 0;
                if (Bottom != null) width = Bottom.Width;
                return width;
            }
        }

        /// <summary>
        /// Gets the width of the left side
        /// </summary>
        public float LeftWidth
        {
            get 
            { 
                float width = 0;
                if (Left != null) width = Left.Width;
                return width;
            }
        }

        #endregion


        #region "Sizing functions"

        /// <summary>
        /// Returns a SizeF that is big enough for content plus border
        /// </summary>
        /// <param name="size">Size for the content</param>
        /// <returns>SizeF that is bigger than size by all borders</returns>
        public SizeF AddBorderSize (SizeF size)
        {
            if (Top != null)
            {
                size.Height += Top.Width;
            }
            if (Right != null)
            {
                size.Width += Right.Width;
            }
            if (Bottom != null)
            {
                size.Height += Bottom.Width;
            }
            if (Left != null)
            {
                size.Width += Left.Width;
            }
            return size;
        }

        /// <summary>
        /// Gets an inner bounds based on the widths of each pen
        /// </summary>
        /// <param name="bounds">Bounds inside the border</param>
        /// <returns>Bounds that takes the border into accound</returns>
        public Bounds GetInnerBounds (Bounds bounds)
        {
            if (Top != null)
            {
                bounds.Position.Y += Top.Width;
            }
            if (Right != null)
            {
                bounds.Limit.X -= Right.Width;
            }
            if (Bottom != null)
            {
                bounds.Limit.Y -= Bottom.Width;
            }
            if (Left != null)
            {
                bounds.Position.X += Left.Width;
            }
            return bounds;
        }

        #endregion


        #region "Drawing functions"

        /// <summary>
        /// Draws just the top side of the box
        /// </summary>
        /// <param name="g">Graphics object to draw on</param>
        /// <param name="rect">Outer rectangle of the border</param>
        public void DrawTop (Graphics g, RectangleF rect)
        {
            DrawLine (g, Top, rect, RectSide.Top);
        }

        /// <summary>
        /// Draws just the right side of the box
        /// </summary>
        /// <param name="g">Graphics object to draw on</param>
        /// <param name="rect">Outer rectangle of the border</param>
        public void DrawRight (Graphics g, RectangleF rect)
        {
            DrawLine (g, Right, rect, RectSide.Right);
        }

        /// <summary>
        /// Draws just the bottom side of the box
        /// </summary>
        /// <param name="g">Graphics object to draw on</param>
        /// <param name="rect">Outer rectangle of the border</param>
        public void DrawBottom (Graphics g, RectangleF rect)
        {
            DrawLine (g, Bottom, rect, RectSide.Bottom);
        }

        /// <summary>
        /// Draws just the left side of the box
        /// </summary>
        /// <param name="g">Graphics object to draw on</param>
        /// <param name="rect">Outer rectangle of the border</param>
        public void DrawLeft (Graphics g, RectangleF rect)
        {
            DrawLine (g, Left, rect, RectSide.Left);
        }

        /// <summary>
        /// Draws a border inside the given rectangle
        /// </summary>
        /// <param name="g">Graphics object to draw on</param>
        /// <param name="rect">Outer rectangle of the border</param>
        public void DrawBorder (Graphics g, RectangleF rect)
        {
            DrawLine (g, Top, rect, RectSide.Top);
            DrawLine (g, Right, rect, RectSide.Right);
            DrawLine (g, Bottom, rect, RectSide.Bottom);
            DrawLine (g, Left, rect, RectSide.Left);
        }

        /// <summary>
        /// Draws a border inside the given bounds
        /// </summary>
        /// <param name="g">Graphics object to print on</param>
        /// <param name="bounds">Outer bounds of the border</param>
        public void DrawBorder (Graphics g, Bounds bounds)
        {
            RectangleF rect = bounds.GetRectangleF();
            DrawBorder (g, rect);
        }

        #endregion


        #region "Private functions"
        /// <summary>
        /// The sides of a rectangle
        /// </summary>
        enum RectSide
        {
            Top = 0, 
            Right = 1, 
            Bottom = 2, 
            Left = 3
        }

        void DrawLine (Graphics g, Pen pen, 
            RectangleF rect, RectSide side)
        {
            float x1 = 0, y1 = 0, x2 = 0, y2 = 0;
            if (pen != null)
            {
                switch (side)
                {
                    case RectSide.Top:
                        x1 = rect.Left;
                        x2 = rect.Right;
                        y1 = y2 = rect.Top + pen.Width / 2;
                        break;
                    case RectSide.Right:
                        x1 = x2 = rect.Right - pen.Width / 2;
                        y1 = rect.Top;
                        y2 = rect.Bottom;
                        break;
                    case RectSide.Bottom:
                        x1 = rect.Left;
                        x2 = rect.Right;
                        y1 = y2 = rect.Bottom - pen.Width / 2;
                        break;
                    case RectSide.Left:
                        x1 = x2 = rect.Left + pen.Width / 2;
                        y1 = rect.Top;
                        y2 = rect.Bottom;
                        break;
                }
                g.DrawLine (pen, x1, y1, x2, y2);
            }
        }

        #endregion


	}
}
