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
    /// Direction the sections are laid out
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// Each section is positioned below the previous
        /// </summary>
        Vertical = 0,
        /// <summary>
        /// Each section is positioned beside the previous
        /// </summary>
        Horizontal = 1
    }
    
    /// <summary>
	/// A simple section of one long row or column.
	/// It can optionally be made into many rows / columns by setting:
	/// RepeatOnPage true.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The LinearSections class is a subclass of SectionContainer,
	/// which is a subclass of ReportSection. Therefore, the LinearSections 
	/// can be thought of as "a printable section of a report." 
	/// However, it is also a container of one or more sections.
	/// </para>
	/// <para>
	/// As its name implies, it lays sections out linearly -- that is, 
	/// in a row or in a column. A property named Direction specifies 
	/// if this container will layout sections going down the page 
	/// (typical) or across the page (not as typical).
	/// </para>
    /// </remarks>
	public class LinearSections : ReportPrinting.SectionContainer
	{
        /// <summary>
        /// Constructor for a vertically laid-out section
        /// </summary>
		public LinearSections()
		{
            this.direction = Direction.Vertical;
		}

		/// <summary>
		/// Constructor specifying a direction
		/// </summary>
		/// <param name="direction">Direction for the section.</param>
		public LinearSections(Direction direction)
		{
			this.direction = direction;
		}
		
		Direction direction;
        float skipAmount;

        #region "Properties"

        /// <summary>
        /// Gets or sets the direction that sub-sections are laid out
        /// within this section.
		/// The default is vertical (meaning top to bottom).
		/// </summary>
        public Direction Direction
        {
            get { return this.direction; }
            set { this.direction = value; }
        }

        /// <summary>
        /// Gets or sets the amount of space to skip between
        /// sections
        /// </summary>
        public float SkipAmount
        {
            get { return this.skipAmount; }
            set { this.skipAmount = value; }
        }

        #endregion


        /// <summary>
        /// Setup for printing (do nothing)
        /// </summary>
        /// <param name="g">Graphics object</param>
        protected override void DoBeginPrint(Graphics g)
        {
            this.sectionIndex = 0;
        }


        /// <summary>
        /// Advances the pointers in bounds and requiredSize by an
        /// amount of size plus SkipAmount
        /// </summary>
        /// <param name="size">The size to increment bounds and requiredSize by</param>
        /// <param name="bounds">The bounds for printing of remaining sections.
        /// It is a struct passed by reference.</param>  
        /// <param name="requiredSize">The size required for the section or line
        /// It is a struct passed by reference.</param>
        protected void AdvancePointers (
            SizeF size,
            ref Bounds bounds,
            ref SizeF requiredSize
            )
        {
            // Set the bounds based on the size of the last section 
            // printed on this call
            switch (this.direction)
            {
                case Direction.Vertical:

                    bounds.Position.Y += size.Height;
                    requiredSize.Height += size.Height;
                    bounds.Position.Y += this.SkipAmount;
                    requiredSize.Height += this.SkipAmount;
                    requiredSize.Width = Math.Max (requiredSize.Width, size.Width);
                    break;
                case Direction.Horizontal:
                    bounds.Position.X += size.Width;
                    requiredSize.Width += size.Width;
                    bounds.Position.X += this.SkipAmount;
                    requiredSize.Width += this.SkipAmount;
                    requiredSize.Height = Math.Max (requiredSize.Height, size.Height);
                    break;
            }
        } // AdvancePointers


        /// <summary>
        /// Sizes all the sections that fit within a single row/column
        /// </summary>
        /// <param name="reportDocument">Parent ReportDocument</param>
        /// <param name="g">Graphics object for sizing / printing</param>
        /// <param name="bounds">The bounds the line must fit within</param>
        /// <param name="sizeOnly">Indicates that no printing should be done,
        /// just size the line</param>
        /// <param name="advanceSectionIndex">Advance the section index, 
        /// so that it's ready for the next line</param>
        /// <returns>Size and flags for if it fits and if it's continued</returns>
        protected SectionSizeValues SizePrintLine (
            ReportDocument reportDocument,
            Graphics g,
            Bounds bounds,
            bool sizeOnly,
            bool advanceSectionIndex
            )
        {
            SectionSizeValues retvals = new SectionSizeValues();
            retvals.Fits = false;

            int savedSectionIndex = this.sectionIndex;
//            Debug.WriteLine("\nEntering " + Direction + " section, sizeonly = " + sizeOnly);
//            Debug.WriteLine("   Bounds: " + bounds);

            // The following loop sizes all sections that fit on one line
            // If it runs out of room within the current bounds, it returns
            // and will continue where it left off on the next call.
            while (this.sectionIndex < this.SectionCount)
            {
                CurrentSection.CalcSize (reportDocument, g, bounds);
                if (CurrentSection.Fits)
                {
                    retvals.Fits = true;
                    if (!sizeOnly) CurrentSection.Print (reportDocument, g, bounds);
                    AdvancePointers(CurrentSection.Size, ref bounds, ref retvals.RequiredSize);
                    if (CurrentSection.Continued)
                    {
                        break;
                    }
                    else
                    {
                        this.sectionIndex++;
                    }
                }
                else // it doesn't fit
                {
                    // reset size since we didn't print but need the size to be
                    // refigured next time
                    CurrentSection.ResetSize(); 
                    break;
                }
            } // while

//            Debug.WriteLine("Leaving " + Direction + " section");
//            Debug.WriteLine("   Size: " + RequiredSize);

            retvals.Continued = this.sectionIndex < this.SectionCount;
			if (this.sectionIndex == savedSectionIndex && !retvals.Continued) retvals.Fits = true;
			if (!advanceSectionIndex) this.sectionIndex = savedSectionIndex;
            return retvals;
        } // SizePrintLine

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
            // get the size of the current line
            return SizePrintLine (reportDocument, g, bounds, true, false);
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
            if (!this.UseFullWidth)
            {
                bounds.Limit.X = bounds.Position.X + this.RequiredSize.Width;
            }
            if (!this.UseFullHeight)
            {
                bounds.Limit.Y = bounds.Position.Y + this.RequiredSize.Height;
            }
            
            
            
            SizePrintLine (reportDocument, g, bounds, false, true);
        }


  	}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            