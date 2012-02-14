using System;
using System.Drawing;

namespace ReportPrinting
{
	/// <summary>
	/// This is for use in Page Headers/Footers where a more
	/// complicated layout is needed. It inherits from LinearSections
	/// and only differs from it by always resetting its 
	/// SectionIndex back to zero, ready for the next page.
	/// Note that you must use RepeatableTextSections if you want 
	/// them to print on more than the first page. 
	/// </summary>
	public class PageLinearSections : LinearSections
	{
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
			base.DoPrint(reportDocument, g, bounds);
			this.sectionIndex = 0;
			Reset();
		}
	
	}
}
