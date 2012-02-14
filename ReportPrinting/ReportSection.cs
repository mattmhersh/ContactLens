// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/

using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Diagnostics;

namespace ReportPrinting
{
	/// <summary>
	/// ReportSection represents a section of a report.
	/// </summary>
	/// <remarks>
	/// <para>
	/// There are several subclasses of ReportSection, one for text,
	/// one for a grid of data, one for just a simple horizontal line
    /// <seealso cref="ReportPrinting.SectionText"/>
    /// <seealso cref="ReportPrinting.SectionTable"/>
    /// </para>
    /// <para>
    /// For the developer trying to inherit from ReportSection, there are
    /// several things to note.
    /// </para>
    /// <para>
    /// First, override the following functions: 
    ///     DoBeginPrint, DoCalcSize, and DoPrint
    /// The order that these are called by the framework is more or less
    /// summarized as follows:
    /// </para> 
    /// <code>
    ///     DoBeginPrint()    // called once before all other functions are called.
    ///     do {
    ///        DoCalcSize()   // called once before each call to DoPrint
    ///        if (Fits)
    ///           DoPrint()   // if it Fits, DoPrint()
    ///     } while (Continued)
    /// </code>    
    /// <para>
    /// But there are many more complicated scenarios.  For example,
    /// if bounds change between the calls DoCalcSize and DoPrint, the following
    /// may occur:
    /// </para>
    /// <code>
    ///     DoBeginPrint()    // called once before all other functions are called.
    ///     do {
    ///        DoCalcSize()   // called once before each call to DoPrint
    ///        if (Fits) {
    ///           BoundsChanged()   // indicates that bounds have changed
    ///           if (!sized) {
    ///              DoCalcSize()  // if BoundsChanged calls ResetSize, 
    ///           }                // then DoCalcSize will be called again
    ///           DoPrint()        // if it Fits, DoPrint()
    ///        }
    ///     } while (Continued)
    /// </code> 
    /// <para>
    /// </para>   
	/// </remarks>
    public abstract class ReportSection
    {
        /// <summary>
        /// Default constructor, does nothing
        /// </summary>
        public ReportSection()
        {
        }

        /************
         * Locals
         */
        HorizontalAlignment horizontalAlignment;
        VerticalAlignment verticalAlignment;
        bool useFullHeight;
        bool useFullWidth;
        float marginLeft;
        float marginRight;
        float marginTop;
        float marginBottom;
        float maxWidth;
        float maxHeight;
        bool keepTogether;

        /// <summary>
        /// Indicates that this section requires actual space.
        /// The only sections that would specificaly set this to
        /// false would be something like the SectionBreak.
        /// </summary>
        protected bool requiresNonEmptyBounds = true;

        /// <summary>
        /// The content size required for the next call to print
        /// (this is the area inside the margins).
        /// </summary>
        SizeF requiredSize;

        /// <summary>
        /// The size that will be used on the following call to print
        /// (including margins and/or UseFullHeight / UseFullWidth)
        /// </summary>
        SizeF size;

        /// <summary>
        /// True if the next call to print will not complete the section.
        /// False if the next call to print will finish the entire section.
        /// </summary>
        bool continued;

        /// <summary>
        /// True if the section fits in the bounds given, 
        /// false otherwise.
        /// </summary>
        bool fits;

        /// <summary>
        /// Indicates that CalcSize has been run and
        /// the size / continued values are good.
        /// </summary>
        bool sized;

        /// <summary>
        /// Indicates that BeginPrint has been called
        /// </summary>
        bool startedPrinting;

        /// <summary>
        /// The bounds used to size with.
        /// </summary>
        Bounds sizingBounds;


        #region "Properties"

        /// <summary>
        /// Gets or sets the horizontal alignment for this section
        /// </summary>
        public virtual HorizontalAlignment HorizontalAlignment
        {
            get { return this.horizontalAlignment; }
            set 
            { 
                this.horizontalAlignment = value; 
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment for this section
        /// </summary>
        public virtual VerticalAlignment VerticalAlignment
        {
            get { return this.verticalAlignment; }
            set 
            { 
                this.verticalAlignment = value; 
            }
        }

        /// <summary>
        /// This section consumes the full height passed to it during print.
        /// Default is false.
        /// </summary>
        public virtual bool UseFullHeight
        {
            get { return this.useFullHeight; }
            set { this.useFullHeight = value; }
        }

        /// <summary>
        /// This section consumes the full width passed to it during print.
        /// Default is false.
        /// </summary>
        public virtual bool UseFullWidth
        {
            get { return this.useFullWidth; }
            set { this.useFullWidth = value; }
        }

        /// <summary>
        /// Gets or sets the margin on the left side.
        /// </summary>
        public virtual float MarginLeft
        {
            get { return this.marginLeft; }
            set { this.marginLeft = value; }
        }

        /// <summary>
        /// Gets or sets the margin on the right side.
        /// </summary>
        public virtual float MarginRight
        {
            get { return this.marginRight; }
            set { this.marginRight = value; }
        }
        
        /// <summary>
        /// Gets or sets the margin on the top.
        /// </summary>
        public virtual float MarginTop
        {
            get { return this.marginTop; }
            set { this.marginTop = value; }
        }

        /// <summary>
        /// Gets or sets the margin on the bottom.
        /// </summary>
        public virtual float MarginBottom
        {
            get { return this.marginBottom; }
            set { this.marginBottom = value; }
        }

        /// <summary>
        /// Sets the magins on all four sides
        /// </summary>
        public virtual float Margin
        {
            set
            {
                MarginTop = value;
                MarginRight = value;
                MarginBottom = value;
                MarginLeft = value;
            }
        }

        /// <summary>
        /// Gets or sets the MaxWidth this section will consume
        /// on a pass.
        /// </summary>
        /// <remarks>
        /// Default value (0) disables any max.
        /// </remarks>
        public float MaxWidth
        {
            get { return this.maxWidth; }
            set { this.maxWidth = value; }
        }

        /// <summary>
        /// Gets or sets the MaxHeight this section will consume
        /// on a pass.
        /// </summary>
        /// <remarks>
        /// Default value (0) disables any max.
        /// </remarks>
        public float MaxHeight
        {
            get { return this.maxHeight; }
            set { this.maxHeight = value; }
        }

        /// <summary>
        /// Keep this section together on one page
        /// </summary>
        public bool KeepTogether
        {
            get { return this.keepTogether; }
            set { this.keepTogether = value; }
        }




        /// <summary>
        /// Gets the size that will be used on the following call to print
        /// (including margins and/or UseFullHeight / UseFullWidth)
        /// </summary>
        internal protected SizeF Size
        {
            get 
            { 
                return this.size; 
            }
        }

        /// <summary>
        /// Gets the actual required size for content 
        /// that will be used on the following call to print
        /// (excluding margins and/or UseFullHeight / UseFullWidth)
        /// </summary>
        internal protected SizeF RequiredSize
        {
            get 
            { 
                return this.requiredSize; 
            }
        }

        /// <summary>
        /// Gets the boolean flag for if this ReportSection needs
        /// to be continued.
        /// </summary>
        internal protected bool Continued
        {
            get 
            { 
                return this.continued; 
            }
        }

        /// <summary>
        /// Gets the boolean flag for if this ReportSection 
        /// fits within the bounds at all.
        /// </summary>
        internal protected bool Fits
        {
            get 
            { 
                return this.fits; 
            }
        }

        #endregion


        #region "Public methods"
        /// <summary>
        /// This method is used to perform any required initialization.
        /// </summary>
        /// <param name="g">Graphics object to print on.</param>
        public void BeginPrint (
            Graphics g
            )
        {
            if (!this.startedPrinting)
            {
                this.DoBeginPrint(g);
                this.startedPrinting = true;
            }
        }

        /// <summary>
        /// Resets the entire section, useful at the very beginning
        /// of a print (before Graphics is even known)
        /// to reset startedPrinting.
        /// </summary>
        public virtual void Reset()
        {
            this.startedPrinting = false;
            this.sized = false;
            this.fits = false;
            this.continued = false;
        }

        /// <summary>
        /// Resets the size values.  This enables
        /// the CalcSize method to be called again.
        /// </summary>
        public virtual void ResetSize()
        {
            this.sized = false;
        }


        /// <summary>
        /// Method called to Calculate the size required for
        /// the next Print.  Calling this method initializes
        /// the values Size and Continued.  Once these values
        /// are initialized, further calls to CalcSize have
        /// no affect, unless ResetSize() is called.
        /// </summary>
        /// <param name="reportDocument">The parent ReportDocument that is printing.</param>
        /// <param name="g">Graphics object to print on.</param>
        /// <param name="bounds">Bounds of the area to print within.</param>
        public void CalcSize (
            ReportDocument reportDocument,
            Graphics g,
            Bounds bounds)
        {
            BeginPrint(g);
            if (this.requiresNonEmptyBounds && bounds.IsEmpty())
            {
                this.fits = false;
            }
            else if (!this.sized)
            {
                // two default values
                this.sizingBounds = LimitBounds (bounds);
                SectionSizeValues vals = DoCalcSize(reportDocument, g, this.sizingBounds);
                SetSize (vals.RequiredSize, bounds);
                if (this.keepTogether && vals.Continued)
                {
                    this.fits = false;
                }
                else
                {
                    this.fits = vals.Fits;
                }
                this.continued = vals.Continued;
                this.sized = true;
            }
        }

        /// <summary>
        /// Method called to Print this ReportSection.
        /// If CalcSize has not already been called, it will call it.
        /// </summary>
        /// <param name="reportDocument">The parent ReportDocument that is printing.</param>
        /// <param name="g">Graphics object to print on.</param>
        /// <param name="bounds">Bounds of the area to print within.</param>
        public void Print (
            ReportDocument reportDocument,
            Graphics g,
            Bounds bounds)
        {
            Bounds printingBounds = LimitBounds (bounds);
            if (this.sized && (printingBounds != this.sizingBounds))
            {
                SectionSizeValues vals = BoundsChanged (this.sizingBounds, printingBounds);
                SetSize (vals.RequiredSize, bounds);
                this.fits = vals.Fits;
                this.continued = vals.Continued;
            }

            CalcSize (reportDocument, g, bounds);
            if (this.fits)
            {
                DoPrint (reportDocument, g, printingBounds);
            }
            ResetSize ();
        }

        #endregion


        #region "Private methods"

        /// <summary>
        /// Limits a bounds down to MaxWidth and MaxHeight
        /// </summary>
        Bounds LimitBounds (Bounds bounds)
        {
            if ((this.MaxWidth > 0) && (bounds.Width > this.MaxWidth))
            {
                bounds.Limit.X = bounds.Position.X + this.MaxWidth;
            }
            if ((this.MaxHeight > 0) && (bounds.Height > this.MaxHeight))
            {
                bounds.Limit.Y = bounds.Position.Y + this.MaxHeight;
            }

            // Take margins into account
            bounds = bounds.GetBounds (this.MarginTop, 
                this.MarginRight, this.MarginBottom, this.MarginLeft);
            return bounds;
        }

        #endregion


        #region "Protected Methods"

        /// <summary>
        /// Sets the size variable based on the requiredSize provided.
        /// The requiredSize should not include margins since
        /// those are added by SetSize().  
        /// It also takes into account FullWidth and FullHeight and
        /// uses the values from bounds if required.
        /// </summary>
        /// <param name="requiredSize">The size required for the section</param>
        /// <param name="bounds">The full bounds allowed</param>
        protected virtual void SetSize (SizeF requiredSize, Bounds bounds)
        {
            this.requiredSize = requiredSize;
            this.size = new SizeF (0,0); // just to get rid of a compiler warning
            if (this.UseFullWidth)
            {
                this.size.Width = bounds.Width;
            }
            else
            {
                this.size.Width = requiredSize.Width + this.MarginLeft + this.MarginRight;
            }
            if (this.UseFullHeight)
            {
                this.size.Height = bounds.Height;
            }
            else
            {
                this.size.Height = requiredSize.Height + this.MarginTop + this.MarginBottom;
            }
            if (this.MaxWidth > 0)
            {
                this.size.Width  = Math.Min (this.size.Width,  this.MaxWidth);
            }
            if (this.MaxHeight > 0)
            {
                this.size.Height = Math.Min (this.size.Height, this.MaxHeight);
            }
        }


        /// <summary>
        /// Sets the value for fits, not recommended to use this,
        /// but in an emergency you may
        /// </summary>
        /// <param name="val">New value</param>
        protected void SetFits (bool val)
        {
            this.fits = val;
        }

        /// <summary>
        /// Sets the value for continued, not recommended to use this,
        /// but in an emergency you may
        /// </summary>
        /// <param name="val">New value</param>
        protected void SetContinued (bool val)
        {
            this.continued = val;
        }

        #endregion 


        #region "Methods to overload"

        /// <summary>
        /// A struct with values requied by DoCalcSize
        /// </summary>
        protected struct SectionSizeValues
        {
            /// <summary>
            /// The required size for the section.
            /// </summary>
            public SizeF RequiredSize;
            /// <summary>
            /// Whether or not the section fits in the bounds given.
            /// </summary>
            public bool Fits;
            /// <summary>
            /// Whether or not the section must be continued later.
            /// </summary>
            public bool Continued;
        }


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
        ///    return base.BoundsChanged (originalBounds, newBounds);
        /// </code>
        /// </remarks>
        protected virtual SectionSizeValues BoundsChanged (
            Bounds originalBounds,
            Bounds newBounds)
        {
            SectionSizeValues retval = new SectionSizeValues();
            retval.Fits = this.Fits;
            retval.Continued = this.Continued;
            retval.RequiredSize = this.RequiredSize;
            return retval;
        }


        /// <summary>
        /// This method is used to perform any required initialization.
        /// This method is called exactly once.
        /// This method is called prior to DoCalcSize and DoPrint.
        /// </summary>
        /// <param name="g">Graphics object to print on.</param>
        protected abstract void DoBeginPrint (
            Graphics g
            );

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
        protected abstract SectionSizeValues DoCalcSize (
            ReportDocument reportDocument,
            Graphics g,
            Bounds bounds
            );

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
        protected abstract void DoPrint (
            ReportDocument reportDocument,
            Graphics g,
            Bounds bounds
            );

        #endregion

        }
}
