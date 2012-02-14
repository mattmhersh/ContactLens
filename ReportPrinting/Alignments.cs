// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/

using System;

namespace ReportPrinting
{

	/// <summary>
	/// Describes the vertical alignment of an object.
	/// </summary>
	public enum VerticalAlignment
	{
		/// <summary>
		/// Position text at the top
		/// </summary>
		Top,
		/// <summary>
		/// Position text vertically centered
		/// </summary>
		Middle,
		/// <summary>
		/// Position text at the bottom
		/// </summary>
		Bottom
	}


	/// <summary>
	/// Describes the horizontal alignment of an object.
	/// </summary>
	public enum HorizontalAlignment
	{
		/// <summary>
		/// Aligned to the left side of the page
		/// </summary>
		Left = System.Windows.Forms.HorizontalAlignment.Left,
		/// <summary>
		/// Centered horizontally
		/// </summary>
		Center = System.Windows.Forms.HorizontalAlignment.Center,
		/// <summary>
		/// Aligned to the right side of the page
		/// </summary>
		Right = System.Windows.Forms.HorizontalAlignment.Right,
	}

}
