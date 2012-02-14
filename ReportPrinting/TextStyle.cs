// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/

using System;
using System.Drawing;

namespace ReportPrinting
{
	/// <summary>
	/// Allows styles and fonts to be added to text.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class that allows styles and fonts to be added to text,
	/// and also to allow default styles to be used if not explicitly set.
	/// </para>
	/// <para>
	/// All styles (except for the static TextStyle.Normal)
	/// have another style that is their "default" style.  Until a
	/// property is set (like bold, underline, size, font family, etc),
	/// a TextStyle object always uses the value of the property from
	/// its default style, or parent.
	/// </para>
	/// <para>
	/// Normal is defined as:
	/// <code>
    ///     FontFamily = FontFamily.GenericSansSerif
	///     Size = 10.0f
	///     Brush = Brushes.Black
	///     BackgroundBrush = null
	///     StringAlignment = StringAlignment.Near
	///     0 margins on top, near, far.  0.15 margin on bottom.
    /// </code>
    /// </para>
    /// <para>
    /// A new style defined as:
    /// <code>
    ///     TextStyle paragraphStyle = new TextStyle(Normal);
    ///     paragraphStyle.Bold = true;
    /// </code>
    /// will have all the same properties as TextStyle.Normal, except
    /// it will be bold.
    /// </para>
    /// <para>
    /// A later change to Normal such as:
    /// <code>
    ///     TextStyle.Normal.Size += 1.0f
    /// </code>
    /// will have the effect of increasing the size of both styles
    /// (Normal and paragraphStyle).
    /// </para>
    /// </remarks>
    public class TextStyle
    {

        /********************************************************
         * Static fields representing some preset styles
         * Updating these styles will affect the entire application.
         */

        /// <summary>
        /// Static constructor
        /// </summary>
        static TextStyle()
        {
            Normal = new TextStyle(null);
            Heading1 = new TextStyle(Normal);
            Heading2 = new TextStyle(Normal);
            Heading3 = new TextStyle(Normal);
            PageHeader = new TextStyle(Normal);
            PageFooter = new TextStyle(Normal);
            TableHeader = new TextStyle(Normal);
            TableRow = new TextStyle(Normal);
            BoldStyle = new TextStyle(Normal);
			UnderlineStyle = new TextStyle(Normal);
			ItalicStyle = new TextStyle(Normal);
            ResetStyles();
        }


        /// <summary>
        /// The Normal TextStyle which serves as the default TextStyle
        /// for most text fields.  It also serves as the default 
        /// style behind all other styles (ultimately, there may be a 
        /// chain, but Normal is at the top of all chains).
        /// Changing a property of Normal will affect all other
        /// styles that haven't explictly overriden that property.
        /// </summary>
        public static readonly TextStyle Normal;
        /// <summary>
        /// A TextStyle intended for use by Titles or Section Headings
        /// </summary>
        public static readonly TextStyle Heading1;
        /// <summary>
        /// A TextStyle intended for use in Section Headings
        /// </summary>
        public static readonly TextStyle Heading2;
        /// <summary>
        /// A TextStyle intended for use in Section Headings
        /// </summary>
        public static readonly TextStyle Heading3;
        /// <summary>
        /// The TextStyle used by the PageHeader.  
        /// It defaults to Normal, but centered.
        /// </summary>
        public static readonly TextStyle PageHeader;
        /// <summary>
        /// The TextStyle used by the PageFooter.  
        /// It defaults to Normal, but centered.
        /// </summary>
        public static readonly TextStyle PageFooter;
        /// <summary>
        /// The TextStyle used by the header row of a table.
        /// It defaults to Normal + Bold + MarginBottom=0
        /// </summary>
        public static readonly TextStyle TableHeader;
        /// <summary>
        /// The TextStyle used by the rows of data of a table.
        /// It defaults to Normal + MarginBottom=0
        /// </summary>
        public static readonly TextStyle TableRow;
        /// <summary>
		/// Normal text but bold
		/// </summary>
		public static readonly TextStyle BoldStyle;
		/// <summary>
		/// Normal text but underlined
		/// </summary>
		public static readonly TextStyle UnderlineStyle;
		/// <summary>
		/// Normal text but italicized
		/// </summary>
		public static readonly TextStyle ItalicStyle;


        /// <summary>
        /// Resets all predefined styles to default values
        /// </summary>
        public static void ResetStyles()
        {
            Normal.FontFamily = FontFamily.GenericSansSerif;
            Normal.Size = 10.0f;
            Normal.Bold = false;
            Normal.Italic = false;
            Normal.Underline = false;
            Normal.Brush = Brushes.Black;
            Normal.BackgroundBrush = null;
            Normal.StringAlignment = StringAlignment.Near;
            Normal.MarginTop = 0f;
            Normal.MarginBottom = 0f;
            Normal.MarginNear = 0f;
            Normal.MarginFar = 0f;
            Normal.VerticalAlignment = VerticalAlignment.Top;

            Heading1.ResetToDefault();
            Heading1.Bold = true;
            Heading1.SizeDelta = 2.0f;

            Heading2.ResetToDefault();
            Heading2.Bold = true;
            Heading2.SizeDelta = 1.0f;

            Heading3.ResetToDefault();
            Heading3.Italic = true;

            PageHeader.ResetToDefault();
            PageHeader.StringAlignment = StringAlignment.Center;
            PageHeader.VerticalAlignment = VerticalAlignment.Top;

            PageFooter.ResetToDefault();
            PageFooter.StringAlignment = StringAlignment.Center;
            PageFooter.VerticalAlignment = VerticalAlignment.Bottom;

            TableHeader.ResetToDefault();
            TableHeader.Bold = true;

            TableRow.ResetToDefault();

			BoldStyle.Bold = true;
			UnderlineStyle.Underline = true;
			ItalicStyle.Italic = true;
        }
  



        /********************************************
         * Instance specific information
         */

        /// <summary>
        /// Public constructor that is used to create a new TextStyle.
        /// A new TextStyle is always based on a pre-existing style.
        /// TextStyle.Normal is the top-most style that everything is
        /// based from.
        /// </summary>
        /// <param name="defaultStyle">A default <see cref="ReportPrinting.TextStyle"/>
        /// for the new style to be based on.</param>
        public TextStyle(TextStyle defaultStyle)
        {
            this.defaultStyle = defaultStyle;
        }

        /// <summary>
        /// Resets this style back to exactly the same as its
        /// default style.  It has no affect on Normal
        /// </summary>
        public void ResetToDefault()
        {
            // just unset every option
            if (this.defaultStyle != null)
            {
                boldSet = false;
                italicSet = false;
                underlineSet = false;
                sizeSet = false;
                sizeDelta = 0f;
                fontFamilySet = false;
                brushSet = false;
                backgroundBrushSet = false;
                stringAlignmentSet = false;
                verticalAlignmentSet = false;
                marginNearSet = false;
                marginFarSet = false;
                marginTopSet = false;
                marginBottomSet = false;
            }
        }


        /***
         * Instance variables
         *    when the ...Set version of any variable is true, 
         *    then the local variable will be returned via the
         *    public properties.
         * 
         *    when the ...Set version of any variable is false,
         *    (the default) then the public property will return
         *    the value from the corresponding property of the
         *    this.defaultStyle object.
         */

        private TextStyle defaultStyle;

        private bool bold;
        private bool boldSet = false;
        private bool italic;
        private bool italicSet = false;
        private bool underline;
        private bool underlineSet = false;

        private float size;
        private bool sizeSet = false;
        private float sizeDelta = 0f;

        private FontFamily fontFamily;
        private bool fontFamilySet = false;
        private Brush brush;
        private bool brushSet = false;

        private Brush backgroundBrush;
        private bool backgroundBrushSet = false;

        private StringAlignment stringAlignment;
        private bool stringAlignmentSet = false;

        private VerticalAlignment verticalAlignment;
        private bool verticalAlignmentSet = false;


        // margins
        private float marginNear;
        private float marginFar;
        private float marginTop;
        private float marginBottom;
        private bool marginNearSet = false;
        private bool marginFarSet = false;
        private bool marginTopSet = false;
        private bool marginBottomSet = false;

        #region "Properties"
        /// <summary>
        /// Gets or sets the flag for text is bold
        /// </summary>
        public bool Bold
        {
            get
            {
                if (this.boldSet)
                {
                    return this.bold;
                }
                else
                {
                    return this.defaultStyle.Bold;
                }
            }
            set
            {
                this.boldSet = true;
                this.bold = value;
            }
        }

        /// <summary>
        /// Gets or sets the flag for text is in italics
        /// </summary>
        public bool Italic
        {
            get
            {
                if (this.italicSet)
                {
                    return this.italic;
                }
                else
                {
                    return this.defaultStyle.Italic;
                }
            }
            set
            {
                this.italicSet = true;
                this.italic = value;
            }
        }

        /// <summary>
        /// Gets or sets the flag for text is underlined
        /// </summary>
        public bool Underline
        {
            get
            {
                if (this.underlineSet)
                {
                    return this.underline;
                }
                else
                {
                    return this.defaultStyle.Underline;
                }
            }
            set
            {
                this.underlineSet = true;
                this.underline = value;
            }
        }


        /// <summary>
        /// Gets or sets the the font size (in em's) of this style.
        /// </summary>
        public float Size
        {
            get
            {
                if (this.sizeSet)
                {
                    return this.size;
                }
                else
                {
                    return this.defaultStyle.Size + this.sizeDelta;
                }
            }
            set
            {
                this.sizeDelta = 0f;
                this.sizeSet = true;
                this.size = value;
            }
        }

        /// <summary>
        /// Gets or sets the font size (in points) based on a delta
        /// from the default font size.  A positive size makes it
        /// larger, negative makes it smaller.
        /// </summary>
        public float SizeDelta
        {
            get
            {
                return this.sizeDelta;
            }
            set
            {
                this.sizeDelta = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Drawing.FontFamily"/> 
        /// for the text.
        /// </summary>
        public FontFamily FontFamily
        {
            get
            {
                if (this.fontFamilySet)
                {
                    return this.fontFamily;
                }
                else
                {
                    return this.defaultStyle.FontFamily;
                }
            }
            set
            {
                this.fontFamilySet = true;
                this.fontFamily = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Drawing.Brush"/> 
        /// for text.
        /// </summary>
        public Brush Brush
        {
            get
            {
                if (this.brushSet)
                {
                    return this.brush;
                }
                else
                {
                    return this.defaultStyle.Brush;
                }
            }
            set
            {
                this.brushSet = true;
                this.brush = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Drawing.Brush"/> 
        /// for the background.
        /// </summary>
        public Brush BackgroundBrush
        {
            get
            {
                if (this.backgroundBrushSet)
                {
                    return this.backgroundBrush;
                }
                else
                {
                    return this.defaultStyle.BackgroundBrush;
                }
            }
            set
            {
                this.backgroundBrushSet = true;
                this.backgroundBrush = value;
            }
        }

        /// <summary>
        /// Gets or sets the margin on the near (left) side of this text field
        /// </summary>
        public float MarginNear
        {
            get
            {
                if (this.marginNearSet)
                {
                    return this.marginNear;
                }
                else
                {
                    return this.defaultStyle.MarginNear;
                }
            }
            set
            {
                this.marginNearSet = true;
                this.marginNear = value;
            }
        }

        /// <summary>
        /// Gets or sets the margin on the far (right) side of this text field
        /// </summary>
        public float MarginFar
        {
            get
            {
                if (this.marginFarSet)
                {
                    return this.marginFar;
                }
                else
                {
                    return this.defaultStyle.MarginFar;
                }
            }
            set
            {
                this.marginFarSet = true;
                this.marginFar = value;
            }
        }


        /// <summary>
        /// Gets or sets the margin on the top of this text field
        /// </summary>
        public float MarginTop
        {
            get
            {
                if (this.marginTopSet)
                {
                    return this.marginTop;
                }
                else
                {
                    return this.defaultStyle.MarginTop;
                }
            }
            set
            {
                this.marginTopSet = true;
                this.marginTop = value;
            }
        }

        /// <summary>
        /// Gets or sets the margin on the bottom side of this text field
        /// </summary>
        public float MarginBottom
        {
            get
            {
                if (this.marginBottomSet)
                {
                    return this.marginBottom;
                }
                else
                {
                    return this.defaultStyle.MarginBottom;
                }
            }
            set
            {
                this.marginBottomSet = true;
                this.marginBottom = value;
            }
        }

        /// <summary>
        /// Sets all margins (top, right, bottom, left)
        /// </summary>
        public float Margins
        {
            set
            {
                MarginTop = value;
                MarginFar = value;
                MarginBottom = value;
                MarginNear = value;
            }
        }

        /// <summary>
        /// Gets or sets the StringAlignment for this style
        /// </summary>
        public StringAlignment StringAlignment
        {
            get
            {
                if (this.stringAlignmentSet)
                {
                    return this.stringAlignment;
                }
                else
                {
                    return this.defaultStyle.stringAlignment;
                }
            }
            set
            {
                this.stringAlignmentSet = true;
                this.stringAlignment = value;
            }
        }

        /// <summary>
        /// Gets or sets the veritical alignment of the text
        /// </summary>
        public VerticalAlignment VerticalAlignment
        {
            get
            {
                if (this.verticalAlignmentSet)
                {
                    return this.verticalAlignment;
                }
                else
                {
                    return this.defaultStyle.VerticalAlignment;
                }
            }
            set
            {
                this.verticalAlignmentSet = true;
                this.verticalAlignment = value;
            }
        }

        #endregion


        /// <summary>
        /// Sets this textstyle's properties to match those of a given font
        /// </summary>
        /// <param name="font">Font to set properties from</param>
        public virtual void SetFromFont(Font font)
        {
            FontFamily = font.FontFamily;
            Bold = font.Bold;
            Italic = font.Italic;
            Underline = font.Underline;
            Size = font.Size;
        }

        /// <summary>
        /// Returns a new Font object representing the state of
        /// this TextStyle for Bold, Italic, Underline, FontFamily and Size
        /// </summary>
        /// <returns>a new <see cref="System.Drawing.Font"/> object</returns>
        public virtual Font GetFont()
        {
            FontStyle style = FontStyle.Regular;
            if (Bold)
            {
                style |= FontStyle.Bold;
            }
            if (Italic)
            {
                style |= FontStyle.Italic;
            }
            if (Underline)
            {
                style |= FontStyle.Underline;
            }
            return new Font(FontFamily, Size, style);
        }

        /// <summary>
        /// Not really supported, but you can change the defaultStringFormat...
        /// </summary>
        StringFormat defaultStringFormat = StringFormat.GenericDefault;
        /// <summary>
        /// Not really supported, but you can change the formatFlags for this textstyle
        /// </summary>
        StringFormatFlags defaultFormatFlags =
            StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
        /// <summary>
        /// Not really supported, but you can change the string trimming for this textstyle
        /// </summary>
        StringTrimming defaultStringTrimming = StringTrimming.Word;

        /// <summary>
        /// Returns a valid StringFormat object given the StringAlignment of
        /// this object.
        /// </summary>
        /// <returns>A new <see cref="System.Drawing.StringFormat"/> object</returns>
        public virtual StringFormat GetStringFormat()
        {
            StringFormat stringFormat = new StringFormat (defaultStringFormat);
            stringFormat.FormatFlags = defaultFormatFlags;
            stringFormat.Trimming = defaultStringTrimming;
            stringFormat.Alignment = this.StringAlignment;
            return (stringFormat);
        }

	} // class

} // namespace
