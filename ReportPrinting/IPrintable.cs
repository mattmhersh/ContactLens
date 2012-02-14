// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/csharp/articles/Printing/

using System;
using System.Drawing;
using System.Drawing.Printing;

namespace ReportPrinting
{
	/// <summary>
	/// NOT USED YET!
	/// An interface for printable objects.  To be printed, you must
	/// implement this interface
	/// </summary>
	public interface IPrintable
	{

        /// <summary>
        /// Gets the size that will be used on the following call to print
        /// (including margins and/or UseFullHeight / UseFullWidth)
        /// </summary>
        SizeF Size
        {
            get;
        }

        /// <summary>
        /// Gets the actual required size for content 
        /// that will be used on the following call to print
        /// (excluding margins and/or UseFullHeight / UseFullWidth)
        /// </summary>
        SizeF RequiredSize
        {
            get;
        }

        /// <summary>
        /// Gets the boolean flag for if this ReportSection needs
        /// to be continued.
        /// </summary>
        bool Continued
        {
            get;
        }

        /// <summary>
        /// Gets the boolean flag for if this ReportSection 
        /// fits within the bounds at all.
        /// </summary>
        bool Fits
        {
            get;
        }


        /// <summary>
        /// This method is used to perform any required initialization.
        /// </summary>
        /// <param name="g">Graphics object to print on.</param>
        void BeginPrint (
            Graphics g
            );

        /// <summary>
        /// Resets the size values.  This enables
        /// the CalcSize method to be called again.
        /// </summary>
        void ResetSize();


        /// <summary>
        /// Method called to Calculate the size required for
        /// the next Print.  Calling this method should set the
        /// values for RequiredSize, Continued and Fits
        /// </summary>
        /// <param name="reportDocument">The parent ReportDocument that is printing.</param>
        /// <param name="g">Graphics object to print on.</param>
        /// <param name="bounds">Bounds of the area to print within.</param>
        void CalcSize (
            ReportDocument reportDocument,
            Graphics g,
            Bounds bounds);

        /// <summary>
        /// Method called to Print this object.
        /// </summary>
        /// <param name="reportDocument">The parent ReportDocument that is printing.</param>
        /// <param name="g">Graphics object to print on.</param>
        /// <param name="bounds">Bounds of the area to print within.</param>
        void Print (
            ReportDocument reportDocument,
            Graphics g,
            Bounds bounds);
	}
}
