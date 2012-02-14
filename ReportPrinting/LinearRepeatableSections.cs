// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/

using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.Diagnostics;

namespace ReportPrinting
{
	/// <summary>
	/// Similar to LinearSections but used for situations where
	/// a "child" section should be repeated on a page.  This is
	/// useful for columns, document structure, etc.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Ths section is very similar to LinearSections.  Two things
	/// are noticeably different.</para>
	/// <para>
	/// 1. A sub-section may be repeated within a page.  This is
	/// ideal for making columns on a page (where this section
	/// is setup horizontal, and the only sub-section is a 
	/// Vertical LinearSection).  It also works for making a document
	/// structure where this section is setup vertical, and the
	/// child section is a linear section going horizontal, to make
	/// lines of text across a page.  A normal linear section will
	/// go to the next page between calls to a subsection needing to
	/// be repeated.
	/// </para>
	/// <para>
	/// 2. This section does not have the ability to size itself
	/// ahead of time.  Therefore, it should generally be used by itself
	/// in another section (or as part of a layered section). However,
	/// if MaxWidth/Height is set along with UseFullWidth/Height, then
	/// the size can be more accurately predicted.
	/// </para>
	/// </remarks>
	public class LinearRepeatableSections : LinearSections
	{
        /// <summary>
        /// Constructor to make a vertical layout
        /// </summary>
		public LinearRepeatableSections()
		{
            this.Direction = Direction.Vertical;
		}

        /// <summary>
        /// Constructor that sets the direction as specified
        /// </summary>
        /// <param name="direction">Direction this linear
        /// layout order the child sections.</param>
        public LinearRepeatableSections (Direction direction)
        {
            this.Direction = direction;
        }
    
        ReportSection divider;
        bool showDividerFirst;

        /// <summary>
        /// Gets or sets the ReportSection to use as a divider.
        /// </summary>
        public ReportSection Divider
        {
            get { return this.divider; }
            set { this.divider = value; }
        }

        /// <summary>
        /// Gets or sets the flag that the divder is shown as the
        /// first element on a page.  The default (false) is that
        /// the divider is only used to divide elements 
        /// </summary>
        public bool ShowDividerFirst
        {
            get { return this.showDividerFirst; }
            set { this.showDividerFirst = value; }
        }

        /// <summary>
        /// Called to calculate the size that this section requires on
        /// the next call to Print.
        /// Simply returns the full size (up to MaxWidth and MaxHeight
        /// </summary>
        /// <param name="reportDocument">The parent ReportDocument that is printing.</param>
        /// <param name="g">Graphics object to print on.</param>
        /// <param name="bounds">Bounds of the area to print within.</param>
        /// <returns>size is the full size of the bounds given,
        /// fits is always true, and continued is always false from this
        /// routine.  Note that DoPrint may change the values of all these.
        /// </returns>
        protected override SectionSizeValues DoCalcSize (
            ReportDocument reportDocument,
            Graphics g,
            Bounds bounds
            )
        {
            SectionSizeValues retvals = new SectionSizeValues();

            // assume worst-case size...
            retvals.RequiredSize = bounds.GetSizeF();
            retvals.Fits = true;

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
            SizeF mySize = new SizeF (0,0);

            if (ShowDividerFirst && (Divider != null))
            {
                divider.Print (reportDocument, g, bounds);
                AdvancePointers (divider.Size, ref bounds, ref mySize);
            }

            // size first
            SectionSizeValues oneCall = SizePrintLine (reportDocument, g, bounds, true, false);
            bool fits = oneCall.Fits;
            while (oneCall.Fits)
            {
                Bounds printBounds = bounds.GetBounds (oneCall.RequiredSize);
                SizePrintLine (reportDocument, g, printBounds, false, true); // print
                AdvancePointers (oneCall.RequiredSize, ref bounds, ref mySize);
                // if this section is not continued, quit now
				// or if this was the last column/row on this page, quit now
				if (!oneCall.Continued || bounds.IsEmpty())
				{
					break;
				}
				oneCall = SizePrintLine (reportDocument, g, bounds, true, false); // size
				if ( oneCall.Fits && Divider != null)
				{
					divider.Print (reportDocument, g, bounds);
					AdvancePointers (divider.Size, ref bounds, ref mySize);
				}
            }
            SetSize (mySize, bounds);
            SetFits (fits);
            SetContinued (oneCall.Continued);
        }

	}
}
