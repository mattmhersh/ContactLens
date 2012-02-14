// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/

using System;
using System.Drawing;


namespace ReportPrinting
{
	/// <summary>
	/// A struct defined by a position (top-left corner)
	/// and a limit (bottom-right corner)
	/// </summary>
	public struct Bounds
	{
        /// <summary>
        /// Creates a Bounds struct with a top-left position
        /// and a bottom-right limit.
        /// </summary>
        /// <param name="position">A PointF for the top-left "position" of the bounds</param>
        /// <param name="limit">A PointF for the bottom-right "end position" of the bounds</param>
		public Bounds(PointF position, PointF limit)
		{
            this.Position = position;
            this.Limit = limit;
		}

        /// <summary>
        /// Creates a Bounds struct with a top-left position
        /// and a bottom-right limit.
        /// </summary>
        /// <param name="posX">X coordinate of position</param>
        /// <param name="posY">Y coordinate of position</param>
        /// <param name="limitX">X coordinate of limit</param>
        /// <param name="limitY">Y coordinate of limit</param>
        public Bounds (float posX, float posY, float limitX, float limitY)
            : this(new PointF(posX, posY), new PointF(limitX, limitY))
        {
        }

        /// <summary>
        /// Creates a Bounds struct from a rectangle
        /// </summary>
        /// <param name="rectangle">Rectangle forming the boundaries
        /// of this bounds object</param>
        public Bounds (RectangleF rectangle)
            : this (rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom)
        {
        }

        /**************************************
         * If these are made into properties, a separate
         * X and Y will have to be made for Position
         * and Limit since they are value-types
         */ 
        
        /// <summary>
        /// The top-left position of this bounds
        /// </summary>
        public PointF Position;
        /// <summary>
        /// The bottom-right limit of this bounds.
        /// </summary>
        public PointF Limit;

        #region "Operator overloads"

        /// <summary>
        /// Compares that the values of Position and Limit are identical
        /// between two bounds structs
        /// </summary>
        /// <param name="left">Bounds object on the left side of operator</param>
        /// <param name="right">Bounds object on the right side of operator</param>
        /// <returns>True if the two bounds have the same position and limit values</returns>
        public static bool operator == (Bounds left, Bounds right)
        {
            return left.Equals(right);
            //return ((this.Position == right.Position) && (this.Limit == right.Limit));
        }

        /// <summary>
        /// Compares that the values of Position and Limit are different
        /// between two bounds structs
        /// </summary>
        /// <param name="left">Bounds object on the left side of operator</param>
        /// <param name="right">Bounds object on the right side of operator</param>
        /// <returns>True if either of the two bounds have
        /// different position or limit values</returns>
        public static bool operator != (Bounds left, Bounds right)
        {
            return !left.Equals(right);
            //return ((left.Position != right.Position) || (left.Limit != right.Limit));
        }

        /// <summary>
        /// Equals method
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>True iff obj is a Bounds struct and has the same Position and Limit</returns>
        public override bool Equals (object obj)
        {
            if (!(obj is Bounds))
            {
                return false;
            }
            Bounds bounds = ((Bounds) obj);
            return (PointFsEqual(this.Position, bounds.Position) && PointFsEqual(this.Limit, bounds.Limit));
        }

		private bool PointFsEqual(PointF A, PointF B)
		{
			return (CoordEqual(A.X, B.X) && CoordEqual(A.Y, B.Y));
		}

		private bool CoordEqual(float A, float B)
		{
			if (A > B)
				return ((A - B) < FudgeFactor);
			else
				return ((B - A) < FudgeFactor);
		}

        /// <summary>
        /// Returns a hash code as the XOR of Position's and Limit's hashcodes
        /// </summary>
        /// <returns>An integer</returns>
        public override int GetHashCode ()
        {
            return base.GetHashCode();
            //return this.Position.GetHashCode() ^ this.Limit.GetHashCode();
        }
        
        #endregion


        /// <summary>
        /// Override of Object.ToString()
        /// </summary>
        /// <returns>String representing this object's value</returns>
        public override string ToString()
        {
            return this.Position.ToString() + ", " + this.Limit.ToString();
        }

        /// <summary>
        /// Gets the full height of the bounds
        /// </summary>
        public float Height
        {
            get { return this.Limit.Y - this.Position.Y; }
        }
        /// <summary>
        /// Gets the full width of the bounds
        /// </summary>
        public float Width
        {
            get { return this.Limit.X - this.Position.X; }
        }

        /// <summary>
        /// Indicates the bounds has zero space inside.
        /// </summary>
        /// <returns>True if the bounds width or height
        /// is less than or equal to 0.</returns>
        public bool IsEmpty ()
        {
            return ((this.Width <= FudgeFactor) || (this.Height <= FudgeFactor));
        }

        /// <summary>
        /// Gets the full size of the bounds (from position to limit)
        /// </summary>
        /// <returns>A sizeF struct representing the width and height
        /// of this bounds</returns>
        public SizeF GetSizeF()
        {
            return new SizeF(this.Width, this.Height); 
        }

        
        /// <summary>
        /// Gets a RectangleF representing the full bounds
        /// </summary>
        /// <returns>A RectangleF struct representing the full bounds</returns>
        public RectangleF GetRectangleF()
        {
            return new RectangleF (this.Position, this.GetSizeF());
        }

        /// <summary>
        /// Gets a rectangleF within the client area of the bounds with a given size.
        /// That is, the rectangle will be inside the margins of the bounds.
        /// If both width and height are maximized, it is the same as RectangleF
        /// </summary>
        /// <param name="size">size of the rectangle</param>
        /// <param name="hAlign">Alignment of the rectangle horizontally</param>
        /// <param name="vAlign">Alignment of the rectangle vertically</param>
        ///<returns>A RectangleF</returns>
        public RectangleF GetRectangleF (SizeF size,
            HorizontalAlignment hAlign, VerticalAlignment vAlign)
        {
            float width = size.Width;
            float height = size.Height;
            float posX = 0;
            float posY = 0;
            switch (hAlign)
            {
                case HorizontalAlignment.Left:
                    posX = this.Position.X;
                    break;
                case HorizontalAlignment.Right:
                    posX = this.Limit.X - width;
                    break;
                case HorizontalAlignment.Center:
                    posX = this.Position.X + (this.Width - width) / 2;
                    break;
            }
            switch (vAlign)
            {
                case VerticalAlignment.Top:
                    posY = this.Position.Y;
                    break;
                case VerticalAlignment.Bottom:
                    posY = this.Limit.Y - height;
                    break;
                case VerticalAlignment.Middle:
                    posY = this.Position.Y + (this.Height - height) / 2;
                    break;
            }
            return Check(new RectangleF(posX, posY, width, height));
        }


        /// <summary>
        /// Checks that a Rectangle fits within bounds, and returns one that does
        /// </summary>
        /// <param name="rect">A rectangle struct</param>
        /// <returns>A rectangle struct identical to that provided,
        /// unless the right side or bottom side go past limit.
        /// It is assumed that the "position" is fine.</returns>
        private RectangleF Check (RectangleF rect)
        {
            // check it's in the bounds
            if (rect.Right > this.Limit.X)
            {
                rect.Width -= (rect.Right - this.Limit.X);
            }
            if (rect.Bottom > this.Limit.Y)
            {
                rect.Height -= (rect.Bottom - this.Limit.Y); 
            }
            return rect;
        }

        /// <summary>
        /// Const used in comparing floats to overcome inprecision.
        /// </summary>
        public const float FudgeFactor = 0.001f;

        /// <summary>
        /// Check if a given size fits in the bounds
        /// </summary>
        /// <param name="size">The required size to check for</param>
        /// <returns>True if the provided size is less than or equal to
        /// the current bounds in both width and height.
        /// </returns>
        public bool SizeFits (SizeF size)
        {
            float h = size.Height - this.Height;
            float w = size.Width - this.Width;
            return !(h > FudgeFactor || w > FudgeFactor);
        }

        /// <summary>
        /// Gets the bounds large enough for the given size
        /// </summary>
        /// <param name="size">A size for the new bounds</param>
        /// <returns>A bounds with given size, positioned at "position"</returns>
        public Bounds GetBounds (SizeF size)
        {
            PointF limit = this.Position;
            limit.X += size.Width;
            limit.Y += size.Height;
            return new Bounds(this.Position, limit);
        }

        /// <summary>
        /// Gets a bounds within the client area of the bounds with a given size.
        /// That is, the new bounds will be inside the margins of the bounds.
        /// If both width and height are maximized, it is the same as the
        /// original bounds
        /// </summary>
        /// <param name="size">size of the rectangle</param>
        /// <param name="hAlign">Alignment of the rectangle horizontally</param>
        /// <param name="vAlign">Alignment of the rectangle vertically</param>
        ///<returns>A RectangleF</returns>
        public Bounds GetBounds (SizeF size,
            HorizontalAlignment hAlign, VerticalAlignment vAlign)
        {
            return new Bounds (GetRectangleF (size, hAlign, vAlign));
        }

        /// <summary>
        /// Gets a Bounds struct representing the bounds inside the margins
        /// </summary>
        /// <param name="marginLeft">Left side margin, inches</param>
        /// <param name="marginRight">Right side margin, inches</param>
        /// <param name="marginTop">Top margin, inches</param>
        /// <param name="marginBottom">Bottom margin, inches</param>
        /// <returns>A bounds representing this bounds, less the margins</returns>
        public Bounds GetBounds (
            float marginTop, float marginRight,
            float marginBottom, float marginLeft)
        {
            PointF pt1 = this.Position;
            PointF pt2 = this.Limit;
            pt1.X += marginLeft;
            pt1.Y += marginTop;
            pt2.X -= marginRight;
            pt2.Y -= marginBottom;
            return new Bounds (pt1, pt2);
        }

	}

}
