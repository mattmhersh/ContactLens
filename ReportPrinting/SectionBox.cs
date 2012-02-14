// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/

using System;
using System.Drawing;
using System.Diagnostics;


namespace ReportPrinting
{
	/// <summary>
	/// SectionBox is a simple rectangular section that
	/// represents a box.  It tries to follow the CSS box-style
	/// conventions as much as possible.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Although SectionBox contains one section of contents, it isn't stricly a
	/// container in the class hierarchy, since it doesn't inherit from SectionContainer.
	/// However, it does contain one section which can be assigned to it.  
	/// And the ReportBuilder class, when it supports the SectionBox, will treat it 
	/// as a container by assigning a layered or linear container to the box.
	/// </para>
	/// <para>
    /// The box follows many of the properties from the W3C box model.  
    /// I recommend visiting the following pages to learn more:
	/// <a href="http://www.w3.org/TR/REC-CSS2/box.html">
	/// http://www.w3.org/TR/REC-CSS2/box.html </a> and
	/// <a href="http://www.w3.org/TR/REC-CSS2/visuren.html#positioning-scheme">
	/// http://www.w3.org/TR/REC-CSS2/visuren.html#positioning-scheme</a>
	/// for more details.</para>
	/// <para>
    /// The margins are set the same as for any other report section. 
    /// The margins are always clear (unpainted). The borders are set 
    /// by specifying a <see cref="System.Drawing.Pen"/> object to use for each border. 
    /// The pen object specifies color and width.  The padding is specified in inches,
    /// again for each side independently.
	/// </para>
	/// <para>
    /// If a background brush is set (using the System.Drawing.Brushes), 
    /// it will paint the entire area inside the border (including the padding).
	/// </para>
	/// <para>
	/// The width and height can each be set independently, or not at all. 
	/// If the width is not specified, then the width will size to that of 
	/// the contents. If the width is explictly set using the Width property, 
	/// that width includes content, padding, border, and margins. 
	/// The width can also be set as a percentage of the parent 
	/// (using the WidthPercent property). 
	/// The same is true for the Height and HeightPercent properties.
	/// </para>
	/// <para>
	/// An offset can be set which moves the box in relation to the parent
	/// and normal flow. This should normally be used in LayeredLayout, as 
	/// the results in LinearLayout haven't been adequately tested.
	/// If offset is not specified, then the box is a normal box, 
	/// laid out according to the normal flow.
	/// </para>
	/// <para>
	/// The goal is that if an offset is specified, then 
    /// the box's position is calculated according to the
    /// normal flow (this is called the position in normal flow).
    /// Then the box is offset relative to its normal position.
    /// When a box B is relatively positioned, the position 
    /// of the following box is calculated as though B
    /// were not offset.  This would be the case in a linear layout.
	/// If the box is in a layered layout (LayeredSections container), then there
	/// is no "flow" and the box is simply positioned by offset + margins.
	/// </para>
	/// </remarks>
	public class SectionBox : SectionContainer
	{

        float width;
        float height;
        float widthPercent;
        float heightPercent;
        
        /// <summary>
        /// The four pens used to draw a border - plus
        /// this class has methods which assist in the drawing
        /// of the border
        /// </summary>
        BorderPens border = new BorderPens ();

        // width in inches
        float paddingTop;
        float paddingRight;
        float paddingBottom;
        float paddingLeft;

        //ReportSection content;
        Brush background;

        // positioning in inches
        float offsetTop;
        //float offsetRight;
        //float offsetBottom;
        float offsetLeft;

        Bounds borderBounds;
        Bounds paddingBounds;
        Bounds contentBounds;


        #region "Properties"

        /// <summary>
        /// Gets or sets the width for the box.
        /// This includes margins, borders, padding, and content.
        /// </summary>
        public float Width
        {
            get { return this.width; }
            set { this.width = value; }
        }

        /// <summary>
        /// Gets or sets the height for the box.
        /// This includes margins, borders, padding, and content.
        /// </summary>
        public float Height
        {
            get { return this.height; }
            set { this.height = value; }
        }

        /// <summary>
        /// Gets or sets the width for the box as a percent of the parent.
        /// This includes margins, borders, padding, and content.
        /// Note: In a horizontal layout, this will be the remaining percent
        /// on the page, not quite what we'd really want,
        /// but this is more to be used in LayeredLayout
        /// </summary>
        public float WidthPercent
        {
            get { return this.widthPercent; }
            set 
            { 
                if (value >= 0 && value <= 100)
                {
                    this.widthPercent = value; 
                }
                else
                {
                    throw new ArgumentException ("WidthPercent must be between 0 and 100, inclusive");
                }
            }
        }

        /// <summary>
        /// Gets or sets the height for the box as a percent of the parent
        /// This includes margins, borders, padding, and content.
        /// Note: In a vertical layout, this will be the remaining percent
        /// on the page, not quite what we'd really want.
        /// but this is more to be used in LayeredLayout
        /// </summary>
        public float HeightPercent
        {
            get { return this.heightPercent; }
            set 
            { 
                if (value >= 0 && value <= 100)
                {
                    this.heightPercent = value; 
                }
                else
                {
                    throw new ArgumentException ("HeightPercent must be between 0 and 100, inclusive");
                }
            }
        }

        /// <summary>
        /// Size the box's width to the contents
        /// </summary>
        bool SizeToContentsWidth
        {
            get { return ((WidthPercent == 0) && (Width == 0)); }
        }

        /// <summary>
        /// Size the box's height to the contents
        /// </summary>
        bool SizeToContentsHeight
        {
            get { return ((HeightPercent == 0) && (Height == 0)); }
        }

        /// <summary>
        /// Gets or sets the pen used for the top border
        /// </summary>
        public Pen BorderTop
        {
            get { return this.border.Top; }
            set { this.border.Top = value; }
        }

        /// <summary>
        /// Gets or sets the pen used for the right border
        /// </summary>
        public Pen BorderRight
        {
            get { return this.border.Right; }
            set { this.border.Right = value; }
        }

        /// <summary>
        /// Gets or sets the pen used for the bottom border
        /// </summary>
        public Pen BorderBottom
        {
            get { return this.border.Bottom; }
            set { this.border.Bottom = value; }
        }

        /// <summary>
        /// Gets or sets the pen used for the left border
        /// </summary>
        public Pen BorderLeft
        {
            get { return this.border.Left; }
            set { this.border.Left = value; }
        }

        /// <summary>
        /// Sets the pen used for all four sides of the border
        /// </summary>
        public Pen Border
        {
            set
            {
                BorderTop = value;
                BorderRight = value;
                BorderBottom = value;
                BorderLeft = value;
            }
        }

        /// <summary>
        /// Gets or sets the top padding in inches
        /// </summary>
        public float PaddingTop
        {
            get { return this.paddingTop; }
            set { this.paddingTop = value; }
        }

        /// <summary>
        /// Gets or sets the right padding in inches
        /// </summary>
        public float PaddingRight
        {
            get { return this.paddingRight; }
            set { this.paddingRight = value; }
        }

        /// <summary>
        /// Gets or sets the bottom padding in inches
        /// </summary>
        public float PaddingBottom
        {
            get { return this.paddingBottom; }
            set { this.paddingBottom = value; }
        }

        /// <summary>
        /// Gets or sets the left padding in inches
        /// </summary>
        public float PaddingLeft
        {
            get { return this.paddingLeft; }
            set { this.paddingLeft = value; }
        }

        /// <summary>
        /// Sets the padding on all four sides, in inches
        /// </summary>
        public float Padding
        {
            set
            {
                PaddingTop = value;
                PaddingRight = value; 
                PaddingBottom = value;
                PaddingLeft = value;
            }
        }

        /// <summary>
        /// Gets or sets the relative offset on the top side.
        /// </summary>
        public float OffsetTop
        {
            get { return this.offsetTop; }
            set { this.offsetTop = value; }
        }

        /// <summary>
        /// Gets or sets the relative offset on the left side.
        /// </summary>
        public float OffsetLeft
        {
            get { return this.offsetLeft; }
            set { this.offsetLeft = value; }
        }

//        /// <summary>
//        /// Gets or sets the content ReportSection
//        /// </summary>
//        public ReportSection Content
//        {
//            get { return this.content; }
//            set { this.content = value; }
//        }

        /// <summary>
        /// Gets or sets the background brush
        /// </summary>
        public Brush Background
        {
            get { return this.background; }
            set { this.background = value; }
        }


        #endregion


        #region "Private methods"

        /// <summary>
        /// Gets the BorderBounds based on the bounds inside
        /// the margins,
        /// using Width and Height, UseFullWidth and UseFullHeight, 
        /// and optinally the contentSize (if non-zero)
        /// </summary>
        Bounds GetBorderBounds (Bounds bounds, SizeF contentSize)
        {
            SizeF borderSize = bounds.GetSizeF();
            if (SizeToContentsWidth)
            {
                borderSize.Width = contentSize.Width + PaddingLeft + PaddingRight
                    + this.border.LeftWidth + this.border.RightWidth;
            }
            else
            {
                float widthToUse = Width;
                if (WidthPercent > 0)
                {
                    widthToUse = bounds.Width * (WidthPercent / 100);
                }
                borderSize.Width = widthToUse - MarginLeft - MarginRight;
            }

            if (SizeToContentsHeight)
            {
                borderSize.Height = contentSize.Height + PaddingTop + PaddingBottom
                    + this.border.LeftWidth + this.border.RightWidth;
            }
            else
            {
                float heightToUse = Height;
                if (HeightPercent > 0)
                {
                    heightToUse = bounds.Height * (HeightPercent / 100);
                }
                borderSize.Height = heightToUse - MarginTop - MarginBottom;
            }

            Bounds borderBounds =  bounds.GetBounds (borderSize, 
                this.HorizontalAlignment, this.VerticalAlignment); 

            return borderBounds;
        }


        /// <summary>
        /// If width / height is not specified, then it subtracts
        /// the border and padding bounds to create the returned bounds.
        /// If width / height is specified, then result is based
        /// on that width / height minux margins, border and padding.
        /// </summary>
        /// <param name="bounds">The bounds inside margins</param>
        /// <returns>The bounds outside the content area</returns>
        Bounds GetMaxContentBounds (Bounds bounds)
        {
            bounds.Position.X += border.LeftWidth + PaddingLeft;
            bounds.Position.Y += border.TopWidth + PaddingTop;
            if (Width > 0)
            {
                float contentWidth = Width 
                    - MarginLeft - MarginRight
                    - border.LeftWidth - border.RightWidth
                    - PaddingLeft - PaddingRight;
                bounds.Limit.X = bounds.Position.X + contentWidth;
            }
            else if (WidthPercent > 0)
            {
                float contentWidth = (bounds.Width * WidthPercent / 100)
                    - MarginLeft - MarginRight
                    - border.LeftWidth - border.RightWidth
                    - PaddingLeft - PaddingRight;
                bounds.Limit.X = bounds.Position.X + contentWidth;
            }
            else
            {
                bounds.Limit.X -= border.RightWidth + PaddingRight;
            }

            if (Height > 0)
            {
                float contentHeight = Height 
                    - MarginTop - MarginBottom
                    - border.TopWidth - border.BottomWidth
                    - PaddingTop - PaddingBottom;
                bounds.Limit.Y = bounds.Position.Y + contentHeight;
            }
            else if (HeightPercent > 0)
            {
                float contentHeight = (bounds.Height * HeightPercent / 100) 
                    - MarginTop - MarginBottom
                    - border.TopWidth - border.BottomWidth
                    - PaddingTop - PaddingBottom;
                bounds.Limit.Y = bounds.Position.Y + contentHeight;
            }
            else
            {
                bounds.Limit.Y -= border.BottomWidth + PaddingBottom;
            }
            return bounds;
        }

        #endregion


        #region "Override of SectionContainer"

        /// <summary>
        /// Add a section object to the list of sections
        /// Only one section may be added - generally another section container.
        /// </summary>
        /// <param name="section">The section info to add</param>
        /// <returns>The number of sections</returns>
        public override int AddSection (ReportSection section)
        {
            if (this.sections.Count > 0)
            {
                throw new ApplicationException ("Only one section may be directly added to box. " + 
					"Use another container like Layered to hold additional sections.");
            }
            return this.sections.Add(section);
        }

        #endregion


        #region "Override of ReportSection"

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
            // don't really do anything... (this is the base implementation)
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
            // Take offset into account...
            bounds.Position.X += OffsetLeft;
            bounds.Position.Y += OffsetTop;

            SectionSizeValues retval = new SectionSizeValues();
            // need to determine what to do with these values...
            retval.Fits = true;
            retval.Continued = false;

            SizeF contentSize = new SizeF (0,0);
            if (CurrentSection != null)
            {
                CurrentSection.CalcSize (reportDocument, g, GetMaxContentBounds(bounds));
                contentSize = CurrentSection.Size; // or could use RequiredSize?
            }

            this.borderBounds  = GetBorderBounds (bounds, contentSize);
            this.paddingBounds = border.GetInnerBounds (this.borderBounds);
            this.contentBounds = paddingBounds.GetBounds (PaddingTop, PaddingRight, 
                PaddingBottom, PaddingLeft);

            retval.RequiredSize = this.borderBounds.GetSizeF();
            return retval;
        }

        /// <summary>
        /// Called to actually print this section.  
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
				border.DrawBorder (g, this.borderBounds);
            if (Background != null)
            {
				if ( !reportDocument.NoPrint ) 
					g.FillRectangle (Background, this.paddingBounds.GetRectangleF());
            }
            if (CurrentSection != null)
            {
                CurrentSection.Print (reportDocument, g, this.contentBounds);
            }
        }

        #endregion

//        /// <summary>
//        /// Resets the size of the section, that is, it enforces
//        /// that a call to CalcSize() will actully have an effect,
//        /// and not just use a stored value.
//        /// </summary>
//        public override void ResetSize()
//        {
//            base.ResetSize();
//            if (Content != null)
//            {
//                Content.ResetSize();
//            }
//        }


	}
}
