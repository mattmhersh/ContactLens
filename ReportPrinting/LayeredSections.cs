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
	/// A section that layers all children sections.  That is,
	/// each sub-section is given the full bounds.
	/// It is used commonly for headers and footers.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The LayeredSections class is a subclass of SectionContainer, 
	/// which is a subclass of ReportSection. Therefore, the 
	/// LayeredSections can be thought of as "a printable section of a report." 
	/// It is also a container of one or more sections.
	/// </para>
	/// <para>
	/// The child sections of a LayeredSections object are all
	/// painted on top of one another (creating layers). 
	/// The first section added to a LayeredSections object is
	/// the bottom layer.  Subsequent ReportSection objects added 
	/// to the LayeredSections object will be shown on top of each other.
	/// </para>
	/// </remarks>
	public class LayeredSections : ReportPrinting.SectionContainer
	{
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="useFullWidth">Indicates this section will use the
        /// full width it is given</param>
        /// <param name="useFullHeight">Indicates this section will use the
        /// full height it is given</param>
		public LayeredSections(bool useFullWidth, bool useFullHeight)
		{
            this.UseFullWidth = useFullWidth;
            this.UseFullHeight = useFullHeight;
        }

        /// <summary>
        /// Setup for printing (do nothing)
        /// </summary>
        /// <param name="g">Graphics object</param>
        protected override void DoBeginPrint(Graphics g)
        {
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
        /// <returns>Size.Width and Size.Height are for the largest sub-section
        /// in each direction, fits is true if any section fits, and continued
        /// is true if any section is continued.</returns>
        protected override SectionSizeValues DoCalcSize (
            ReportDocument reportDocument,
            Graphics g,
            Bounds bounds
            )
        {
            SectionSizeValues retval = new SectionSizeValues();
            if (this.sections.Count == 0)
            {
                retval.Fits = true;
            }
            else
            {
                // TODO: get rid of enumerator, eventually hide sections
                foreach (ReportSection section in this.sections)
                {
                    section.CalcSize (reportDocument, g, bounds);
                    retval.RequiredSize.Height = Math.Max (
                        retval.RequiredSize.Height, section.Size.Height);
                    retval.RequiredSize.Width = Math.Max (
                        retval.RequiredSize.Width, section.Size.Width);
                    if (section.Continued)
                    {
                        retval.Continued = true;
                    }
                    if (section.Fits)
                    {
                        retval.Fits = true;
                    }
                }
            }
            return retval;
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
//            Debug.WriteLine ("LayeredSections DoPrint");
//            Debug.WriteLine("   Bounds: " + bounds.ToString());

            if (!this.UseFullHeight)
            {
                bounds.Limit.Y = bounds.Position.Y + this.RequiredSize.Height;
            }

            if (!this.UseFullWidth)
            {
                bounds.Limit.X = bounds.Position.X + this.RequiredSize.Width;
            }

            foreach (ReportSection section in this.sections)
            {
                section.Print (reportDocument, g, bounds);
                if (section.Continued)
                {
                    SetContinued (true);
                }
            }
        }

  	}
}
