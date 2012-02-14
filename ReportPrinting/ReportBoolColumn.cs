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
	public class ReportBoolColumn : ReportImageColumn
	{

		const string DefaultTrueImageName = @"check3.png";

		/// <summary>
		/// Constructor of a true false column that creates a column
		/// using, be default, a check mark for true and no image for false values.
		/// </summary>
		/// <param name="field">The field within a dataview to use for the column.</param>
		/// <param name="maxWidth">The maximum width for this column</param>
		public ReportBoolColumn(string field, float maxWidth) : base (field, maxWidth)
		{
			Bitmap trueBmp = new Bitmap (GetType(), DefaultTrueImageName);
			trueBmp.MakeTransparent ();
            TrueImage = trueBmp;

		}

		/// <summary>
		/// The image to use when the value is true.
		/// </summary>
        public Image TrueImage;
		/// <summary>
		/// The image to use when the value is false.
		/// </summary>
        public Image FalseImage;


        /// <summary>
        /// Gets the image to draw based on the value in the DataRowView
        /// </summary>
        /// <param name="drv">DataRowView for the row currently being printed</param>
        /// <returns>An Image to draw</returns>
        protected override Image GetImage (DataRowView drv)
        {
            Image image;
            bool cellVal = (bool) drv[Field];
            if (cellVal)
            {
                image = TrueImage;
            }
            else
            {
                image = FalseImage;
            }
            return image;
        }



	}
}
