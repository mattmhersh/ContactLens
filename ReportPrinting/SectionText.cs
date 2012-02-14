// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/

using System;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

namespace ReportPrinting
{
    /// <summary>
    /// A section represeting just text.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class has two properties to be set.  First is Text which includes the
    /// string of text to be printed.  This could be one word, or many paragraphs.
    /// Second is TextStyle which defines the font, color, and margins for printing.
    /// </para>
    /// <para>
    /// The text will be aligned both horizontally and vertically, unless
    /// specifcally overriden by assigning the alignment with the
    /// ReportSection properties.
    /// </para>
    /// </remarks>
    public class SectionText : ReportPrinting.ReportSection
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">String of text to print</param>
        /// <param name="textStyle">TextStyle used to print the text</param>
        public SectionText(string text, TextStyle textStyle)
        {
            this.Text = text;
            this.TextStyle = textStyle;
        }

        string text;
        int charIndex;
        TextStyle textStyle;
        float minimumWidth = 0.001f;
        bool singleLineMode = false;
        bool hAlignmentSet = false;
        bool vAlignmentSet = false;
        bool marginLeftSet = false;
        bool marginRightSet = false;
        bool marginTopSet = false;
        bool marginBottomSet = false;

        RectangleF textLayout;
        string textToPrint;
        int charsFitted, linesFitted;
        Font textFont;

		/// <summary>
		/// Turns on certain run-time debugging information
		/// </summary>
#if _DEBUG
		public static bool debugEnabled = true;
#else
		public static bool debugEnabled = false;
#endif

        #region "Properties"
        /// <summary>
        /// Text to print
        /// </summary>
        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }


        /// <summary>
        /// The text style to use for text
        /// Defaults to TextStyle.Normal
        /// </summary>
        public TextStyle TextStyle
        {
            get
            {
                if (this.textStyle == null)
                {
                    return TextStyle.Normal;
                }
                else
                {
                    return this.textStyle;
                }
            }
            set { this.textStyle = value; }
        }

        /// <summary>
        /// Gets or sets minimum width that this section will try to print into.
        /// Default 1 inch
        /// </summary>
        public float MinimumWidth
        {
            get { return this.minimumWidth; }
            set { this.minimumWidth = value; }
        }

        /// <summary>
        /// Gets or sets the flag indicating that only a single line of text prints
        /// per call to print.
        /// This is used when in document layout.
        /// </summary>
        public bool SingleLineMode
        {
            get { return this.singleLineMode; }
            set { this.singleLineMode = value; }
        }

        /// <summary>
        /// Gets or sets the flag indicating that instead of using the alignments specified
        /// in the TextStyle, use the one specified in the ReportSection.
        /// </summary>
        public bool UseReportHAlignment
        {
            get { return this.hAlignmentSet; }
            set { this.hAlignmentSet = value; }
        }

        /// <summary>
        /// Indicates that instead of using the alignments specified
        /// in the TextStyle, use the one specified in the ReportSection.
        /// </summary>
        public bool UseReportVAlignment
        {
            get { return this.vAlignmentSet; }
            set { this.vAlignmentSet = value; }
        }


        /// <summary>
        /// Gets or sets the horizontal alignment for this section
        /// </summary>
        public override HorizontalAlignment HorizontalAlignment
        {
            get 
            { 
                if (this.hAlignmentSet)
                {
                    return base.HorizontalAlignment; 
                }
                else
                {
                    return ConvertAlign(this.TextStyle.StringAlignment);
                }
            }
            set 
            { 
                this.hAlignmentSet = true;
                base.HorizontalAlignment = value;
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment for this section
        /// </summary>
        public override VerticalAlignment VerticalAlignment
        {
            get 
            { 
                if (this.vAlignmentSet)
                {
                    return base.VerticalAlignment;
                }
                else
                {
                    return this.TextStyle.VerticalAlignment;
                }
            }
            set
            {
                this.vAlignmentSet = true;
                base.VerticalAlignment = value; 
            }
        }


        /// <summary>
        /// Gets or sets the margin on the left side.
        /// </summary>
        public override float MarginLeft
        {
            get 
            { 
                if (this.marginLeftSet)
                {
                    return base.MarginLeft;
                }
                else
                {
                    // TODO: Left is not always near, but it is in english
                    return this.TextStyle.MarginNear;
                }
            }
            set 
            { 
                this.marginLeftSet = true;
                base.MarginLeft = value; 
            }
        }

        /// <summary>
        /// Gets or sets the margin on the right side.
        /// </summary>
        public override float MarginRight
        {
            get 
            { 
                if (this.marginRightSet)
                {
                    return base.MarginRight;
                }
                else
                {
                    // TODO: Right is not always far, but it is in english
                    return this.TextStyle.MarginFar;
                }
            }
            set 
            { 
                this.marginRightSet = true;
                base.MarginRight = value; 
            }
        }
        
        /// <summary>
        /// Gets or sets the margin on the top.
        /// </summary>
        public override float MarginTop
        {
            get 
            { 
                if (this.marginTopSet)
                {
                    return base.MarginTop;
                }
                else
                {
                    return this.TextStyle.MarginTop;
                }
            }
            set 
            { 
                this.marginTopSet = true;
                base.MarginTop = value; 
            }
        }

        /// <summary>
        /// Gets or sets the margin on the bottom.
        /// </summary>
        public override float MarginBottom
        {
            get 
            { 
                if (this.marginBottomSet)
                {
                    return base.MarginBottom;
                }
                else
                {
                    return this.TextStyle.MarginBottom;
                }
            }
            set 
            { 
                this.marginBottomSet = true;
                base.MarginBottom = value; 
            }
        }
        #endregion

        #region "Public converter methods"
        /**********************************
         * These could be overriden to convert for right to left cultures.
         */

        /// <summary>
        /// This function is used to convert a StringAlignment
        /// (Near, Center, Far) into a HorizontalAlignment used
        /// for sections (Left, Center, Right).
        /// It assumes the culture is left to right.
        /// </summary>
        /// <param name="stringAlign">A StringAlignment for the TextStyle</param>
        /// <returns>A HorizontalAlignment for the Section</returns>
        public static HorizontalAlignment ConvertAlign(StringAlignment stringAlign)
        {
            HorizontalAlignment hAlign = HorizontalAlignment.Left;
            switch (stringAlign)
            {
                case StringAlignment.Near:
                    hAlign = HorizontalAlignment.Left;
                    break;
                case StringAlignment.Center:
                    hAlign = HorizontalAlignment.Center;
                    break;
                case StringAlignment.Far:
                    hAlign = HorizontalAlignment.Right;
                    break;
            }
            return hAlign;
        }

        /// <summary>
        /// This function is used to convert a HorizontalAlignment
        /// for sections (Left, Center, Right)
        /// into a StringAlignment (Near, Center, Far).
        /// It assumes the culture is left to right.
        /// </summary>
        /// <param name="stringAlign">A HorizontalAlignment</param>
        /// <returns>StringAlignment</returns>
        public static StringAlignment ConvertAlign(HorizontalAlignment stringAlign)
        {
            StringAlignment strAlign = StringAlignment.Near;
            switch (stringAlign)
            {
                case HorizontalAlignment.Left:
                    strAlign = StringAlignment.Near;
                    break;
                case HorizontalAlignment.Center:
                    strAlign = StringAlignment.Center;
                    break;
                case HorizontalAlignment.Right:
                    strAlign = StringAlignment.Far;
                    break;
            }
            return strAlign;
        }

        #endregion

        #region "Protected, virtual getters"

        /// <summary>
        /// A function that should return the string to be printed on
        /// this call to Print()
        /// </summary>
        /// <param name="reportDocument">The parent ReportDocument.  
        /// Can be used to overload this function with page specific verions, etc.</param>
        /// <returns>A string to be printed on this page</returns>
        protected virtual string GetText (ReportDocument reportDocument)
        {
            // TODO: Raise event for printing text...
            return this.Text.Substring(CharIndex);
        }

        /// <summary>
        /// Gets or sets the index of the next character to print
        /// </summary>
        protected virtual int CharIndex
        {
            get { return this.charIndex; }
            set { this.charIndex = value; }
        }

        /// <summary>
        /// Gets a StringFormat object based on the TextStyle
        /// and the HorizontalAlignment for the ReportSection.
        /// </summary>
        /// <returns>The StringFormat to use for this text section</returns>
        protected virtual StringFormat GetStringFormat()
        {
            StringFormat stringFormat = this.TextStyle.GetStringFormat();
            stringFormat.Alignment = ConvertAlign(this.HorizontalAlignment);
            return stringFormat;
        }

        #endregion

        #region "Override of ReportSection"

        /// <summary>
        /// Setup for printing (do nothing)
        /// </summary>
        /// <param name="g">Graphics object</param>
        protected override void DoBeginPrint(Graphics g)
        {
            this.CharIndex = 0;
        }

        /// <summary>
        /// Called to calculate the size that this section requires on
        /// the next call to Print.  This method will be called exactly once
        /// prior to each call to Print.  It must update the values Size and
        /// Continued of the ReportSection base class.
        /// </summary>
        /// <param name="reportDocument">Pparent ReportDocument that is printing.</param>
        /// <param name="g">Graphics object to print on.</param>
        /// <param name="bounds">Bounds of the area to print within.</param>
        /// <returns>SectionSizeValues</returns>
        protected override SectionSizeValues DoCalcSize (
            ReportDocument reportDocument,
            Graphics g,
            Bounds bounds
            )
        {
            SectionSizeValues retval = new SectionSizeValues();
            textFont = this.TextStyle.GetFont();
            textLayout = bounds.GetRectangleF();
            if (CheckTextLayout(g))
            {
                // Get a new string starting from where-ever we left off on the last page
                textToPrint = GetText(reportDocument);
                retval = SetTextSize (reportDocument, g, bounds);
            }
            else
            {
                retval.Fits = false;
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
			if ( !reportDocument.NoPrint ) 
			{
				// draw background and text
				if (this.TextStyle.BackgroundBrush != null)
				{
					RectangleF backgroundRect = textLayout;
					if (this.UseFullWidth)
					{
						backgroundRect.X = bounds.Position.X;
						backgroundRect.Width = bounds.Width;
					}
					if (this.UseFullHeight)
					{
						backgroundRect.Y = bounds.Position.Y;
						backgroundRect.Height = bounds.Height;
					}

					g.FillRectangle (this.TextStyle.BackgroundBrush, backgroundRect);
				}
				g.DrawString(textToPrint, textFont, this.TextStyle.Brush, textLayout, GetStringFormat());
				if (debugEnabled)
				{
					Console.WriteLine ("Draw string '" + textToPrint + "' at " + textLayout);
				}
			}

			// Increment the character pointer...
			this.CharIndex += charsFitted;
        }

        /// <summary>
        /// Notification that the bounds has changed between
        /// the size and the print.  
        /// Override this function to update anything based on the new location
        /// </summary>
        /// <param name="originalBounds">Bounds originally passed for sizing</param>
        /// <param name="newBounds">New bounds for printing</param>
        /// <returns>New required size</returns>
        protected override SectionSizeValues BoundsChanged (
            Bounds originalBounds,
            Bounds newBounds)
        {
            // Find the cases where a resizing is not-necessary.
            // For now, we can handle a change in the size of the
            // bounds as long as the placement doesn't change relative to
            // our "aligned" corner, and the size is still big enough.
            bool resize = true;
            int corner = GetOrigin();
            if (corner >= 0)
            {
                if (GetPoint(originalBounds, corner) == GetPoint(newBounds, corner))
                {
                    if (newBounds.SizeFits(this.RequiredSize))
                    {
                        resize = false;
                    }
                }
            }
            
            if (resize)
            {
                this.ResetSize();
            }
            return base.BoundsChanged (originalBounds, newBounds);
        }

        #endregion

        #region "Private methods"

        /// <summary>
        /// Checks that the textLayout rectangle is large enough
        /// Sets fits based on the results.
        /// </summary>
        /// <param name="g">Graphics object</param>
        /// <returns>the value of fits</returns>
        bool CheckTextLayout(Graphics g)
        {
            float fontHeight = textFont.GetHeight(g);
            bool fits = true;

            // Check that it's tall enough for at least one line
            // and wider than the minimum width.
            if (textLayout.Height < fontHeight || 
                textLayout.Width < this.MinimumWidth)
            {
                fits = false;
            }
            if (this.SingleLineMode)
            {
                // HACK: Fudge factor
                textLayout.Height = fontHeight * 1.5f;
            }
            return (fits);
        }

        /// <summary>
        /// Sets the TextLayout rectangle to the correct size
        /// Also sets size, itFits, and continued of the base class
        /// </summary>
        /// <param name="doc">The parent ReportDocument</param>
        /// <param name="g">Graphics object</param>
        /// <param name="bounds">Bounds to draw within</param>
        /// <returns>SectionSizeValues</returns>
        SectionSizeValues SetTextSize (ReportDocument doc, Graphics g, Bounds bounds)
        {
            SectionSizeValues retval = new SectionSizeValues();
            retval.Fits = true;

            // Find the height of the actual string + margins to be drawn
            SizeF requiredSize = g.MeasureString(textToPrint, textFont, 
                textLayout.Size, GetStringFormat(), out charsFitted, out linesFitted);

            if (charsFitted < textToPrint.Length)
            {
                // it doesn't all fit.
                if (this.KeepTogether)
                {
                    // try on the next page.  
                    // HACK: This is bad if the whole section doesn't
                    // ever fit on a page, since it will end up in an infinite loop.
                    retval.Fits = false;
                    charsFitted = 0;
                    linesFitted = 0;
                    return retval;
                }
                retval.Continued = true;
            } 
	
			if (this.HorizontalAlignment != HorizontalAlignment.Left)
				requiredSize.Width = textLayout.Size.Width;

            // Get a new rectangle aligned within the bounds and margins
            textLayout = bounds.GetRectangleF(requiredSize, 
                this.HorizontalAlignment, this.VerticalAlignment);
            retval.RequiredSize = textLayout.Size;

			if (debugEnabled)
			{
				Console.WriteLine ("Layout for string '" + textToPrint + "' is " + textLayout);
			}

            return retval;
        }


        /// <summary>
        /// Returns a number 0-3 indicating the corner acting as origin
        /// based the alignments (0 if Left-Top, 1 if Right-Top,
        /// 2 if Left-Bottom, 3 if Right-Bottom)
        /// or -1 if something is centered
        /// </summary>
        int GetOrigin()
        {
            int origin = 0;
            if ((this.HorizontalAlignment == HorizontalAlignment.Center) ||
                (this.VerticalAlignment == VerticalAlignment.Middle))
            {
                origin = -1;
            }
            else
            {
                if (this.HorizontalAlignment == HorizontalAlignment.Right)
                {
                    origin |= 1;
                }
                if (this.VerticalAlignment == VerticalAlignment.Bottom)
                {
                    origin |= 2;
                }
            }
            return origin;
        }


        /// <summary>
        /// Gets a Point from a bounds based on a given corner 0-3
        /// </summary>
        PointF GetPoint (Bounds bounds, int corner)
        {
            Debug.Assert (corner >= 0 && corner <= 3, "Illegal origin value.");
            float x = 0;
            float y = 0;
            if ((corner & 1) == 0)
            {
                x = bounds.Position.X;
            }
            else
            {
                x = bounds.Limit.X;
            }
            if ((corner & 2) == 0)
            {
                y = bounds.Position.Y;
            }
            else
            {
                y = bounds.Limit.Y;
            }
            return new PointF (x,y);
        }

        #endregion

    }
}
