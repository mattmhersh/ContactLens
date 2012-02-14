// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace ReportPrinting
{

	/// <summary>
	/// ReportSectionImage is a simple rectangular section that
	/// prints a provided image.
	/// </summary>
	public class SectionImage : ReportSection
	{
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="image">Image to print</param>
		public SectionImage (Image image)
		{
			// initially set transparency to 0 (meaning fully opaque)
			this.Transparency = 0;
			this.Image = image;
		}

        Image image;
        bool preserveAspectRatio = true;
        RectangleF imageRect;

        float opacity;


        #region "Properties"

        /// <summary>
        /// Gets or sets the image to draw
        /// </summary>
        public virtual Image Image
        {
            get { return this.image; }
            set { this.image = value; }
        }

        /// <summary>
        /// Gets or sets the flag to preserve aspect ratio
        /// Default is true.
        /// </summary>
        public bool PreserveAspectRatio
        {
            get { return this.preserveAspectRatio; }
            set { this.preserveAspectRatio = value; }
        }

        /// <summary>
        /// Sets the alpha blending of the image.  A value
        /// between 0 (opaque) and 100 (transparent).
        /// </summary>
        public float Transparency
        {
            set 
            { 
                if (value < 0 || value > 100)
                {
                    throw new ArgumentException ("Transparency must be between 0 and 100");
                }
                this.opacity = (100 - value) / 100;
            }
            get
            {
                return 100 - (this.opacity * 100);
            }
        }

        #endregion


        #region "Private methods"


        /// <summary>
        /// Gets a rectangle for the image
        /// </summary>
        RectangleF GetImageRect (Bounds bounds)
        {
            SizeF maxSize = bounds.GetSizeF();
            float scaleW = maxSize.Width / Image.Width;
            float scaleH = maxSize.Height / Image.Height;
            if (PreserveAspectRatio)
            {
                float scale = Math.Min(scaleW, scaleH);
                scaleW = scale;
                scaleH = scale;
            }
            float width = scaleW * Image.Width;
            float height = scaleH * Image.Height;
            SizeF imgSize = new SizeF (width, height);

            return bounds.GetRectangleF (imgSize, this.HorizontalAlignment, this.VerticalAlignment);
        }

        #endregion


        #region "protected override functions"

        /// <summary>
        /// This method is called after a size and before a print if
        /// the bounds have changed between the sizing and the printing.
        /// Override this function to update anything based on the new location
        /// </summary>
        /// <param name="originalBounds">Bounds originally passed for sizing</param>
        /// <param name="newBounds">New bounds for printing</param>
        /// <returns>SectionSizeValues for the new values of size, fits, continued</returns>
        /// <remarks>To simply have size recalled, implement the following:
        /// <code>
        ///    this.ResetSize();
        ///    return base.ChangeBounds (originalBounds, newBounds);
        /// </code>
        /// </remarks>
        protected override SectionSizeValues BoundsChanged (
            Bounds originalBounds,
            Bounds newBounds)
        {
            SectionSizeValues retval = new SectionSizeValues();
            this.imageRect = GetImageRect(newBounds);
            retval.RequiredSize = this.imageRect.Size;
            retval.Fits = newBounds.SizeFits(retval.RequiredSize);
            retval.Continued = false;
            return retval;
        }


        /// <summary>
        /// This method is used to perform any required initialization.
        /// This method is called exactly once.
        /// This method is called prior to DoCalcSize and DoPrint.
        /// </summary>
        /// <param name="g">Graphics object to print on.</param>
        protected override void DoBeginPrint (
            Graphics g
            )
        {
        }

        /// <summary>
        /// Called to calculate the size that this section requires on
        /// the next call to Print.  This method will be called once
        /// prior to each call to Print.  
        /// </summary>
        /// <param name="reportDocument">The parent ReportDocument that is printing.</param>
        /// <param name="g">Graphics object to print on.</param>
        /// <param name="bounds">Bounds of the area to print within.
        /// The bounds passed already takes the margins into account - so you cannot
        /// print or do anything within these margins.
        /// </param>
        /// <returns>The values for RequireSize, Fits and Continues for this section.</returns>
        protected override SectionSizeValues DoCalcSize (
            ReportDocument reportDocument,
            Graphics g,
            Bounds bounds
            )
        {
            SectionSizeValues retval = new SectionSizeValues();
            this.imageRect = GetImageRect(bounds);
            retval.RequiredSize = this.imageRect.Size;
            retval.Fits = bounds.SizeFits(retval.RequiredSize);
            retval.Continued = false;
            return retval;
        }

        /// <summary>
        /// Called to actually print this section.  
        /// The DoCalcSize method will be called once prior to each
        /// call of DoPrint.
        /// DoPrint is not called if DoCalcSize sets fits to false.
        /// It should obey the values of Size and Continued as set by
        /// DoCalcSize().
        /// </summary>
        /// <param name="reportDocument">The parent ReportDocument that is printing.</param>
        /// <param name="g">Graphics object to print on.</param>
        /// <param name="bounds">Bounds of the area to print within.
        /// These bounds already take the margins into account.
        /// </param>
        protected override void DoPrint (
            ReportDocument reportDocument,
            Graphics g,
            Bounds bounds
            )
        {
			if ( !reportDocument.NoPrint ) 
				DrawImage (g, this.image, this.imageRect);
        }

        void DrawImage(Graphics g, Image image, RectangleF destRect)
        {
            PointF[] destPoints = {
                new PointF (destRect.Left,  destRect.Top),
                new PointF (destRect.Right, destRect.Top),
                new PointF (destRect.Left,  destRect.Bottom)};

            ColorMatrix myColorMatrix = new ColorMatrix();
            myColorMatrix.Matrix00 = 1.00f; // Red
            myColorMatrix.Matrix11 = 1.00f; // Green
            myColorMatrix.Matrix22 = 1.00f; // Blue
            myColorMatrix.Matrix33 = this.opacity; // alpha
            myColorMatrix.Matrix44 = 1.00f;        // w
            ImageAttributes imgAttributes = new ImageAttributes();
            imgAttributes.SetColorMatrix (myColorMatrix,
                ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);

            g.DrawImage (image, destPoints, new Rectangle (0, 0, image.Width, image.Height),
                GraphicsUnit.Pixel, imgAttributes);
        }


        #endregion

	} // class


}
