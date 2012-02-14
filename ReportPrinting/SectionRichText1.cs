using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace ReportPrinting
{
	/// <summary>
	/// WORK IN PROGRESS: Prints the contents of a rich text box.
	/// </summary>
	/// <remarks>
	/// WORK IN PROGRESS...
    /// This code inspired by Mike Gold's Color Syntax Editor
    /// http://www.c-sharpcorner.com/Code/2003/June/ColorSyntaxEditor.asp
    /// <para>
    /// It's really slow.
    /// There are many things it doesn't do so well, like:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Different font sizes on the same line</description></item>
    /// <item><description>Some fonts don't seem to come through the rich text box property</description></item>
    /// <item><description>An entire word is formatted by the properties of the first letter</description></item>
    /// </list>
	/// </remarks>
    public class SectionRichText : ReportSection
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public SectionRichText()
        {
            wordSeparator = new Regex(@"\w", RegexOptions.IgnoreCase|RegexOptions.Compiled);
            //r = new Regex(@"\S", RegexOptions.IgnoreCase|RegexOptions.Compiled);

            // http://support.microsoft.com/default.aspx?scid=kb;EN-US;q307208
            stringFormat = new StringFormat (StringFormat.GenericTypographic);
        }

        RichTextBox richTextBox1;
        int lastCharIndex = 0;
        float verticalSpacing = 1f; // %times the font height
        Regex wordSeparator;
        StringFormat stringFormat;

        /// <summary>
        /// Gets or sets the RichTextBox to print...
        /// </summary>
        public RichTextBox RichTextBox
        {
            get { return this.richTextBox1; }
            set { this.richTextBox1 = value; }
        }




        #region "Methods to overload"


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
        protected override SectionSizeValues BoundsChanged (
            Bounds originalBounds,
            Bounds newBounds)
        {
//            SectionSizeValues retval = new SectionSizeValues();
//            retval.Fits = this.Fits;
//            retval.Continued = this.Continued;
//            retval.RequiredSize = this.RequiredSize;
//            return retval;
            return base.BoundsChanged (originalBounds, newBounds);
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
            this.lastCharIndex = 0;
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
            SectionSizeValues retval = new SectionSizeValues();
            retval.Fits = true;
            retval.RequiredSize = bounds.GetSizeF();
            //DrawEditControl(g, bounds, true);
            return retval;
        }

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
        protected override void DoPrint (
            ReportDocument reportDocument,
            Graphics g,
            Bounds bounds
            )
        {
            //g.TextRenderingHint = TextRenderingHint.AntiAlias;
            SectionSizeValues results = DrawEditControl(g, bounds, false);
            SetContinued (results.Continued);
            SetFits (results.Fits);
            SetSize (results.RequiredSize, bounds);
        }

        #endregion


        bool showchars = false;


        SectionSizeValues DrawEditControl(Graphics g, Bounds bounds, bool sizeOnly)
        {
            SectionSizeValues retval = new SectionSizeValues();
            retval.RequiredSize = new SizeF (0,0);

            // draw the control by selecting the first character
            // of each word, and getting its font / color
            float xPos = bounds.Position.X;
            float yPos = bounds.Position.Y;
            int c;
            int length = richTextBox1.Text.Length;
            for (c = this.lastCharIndex; c < length; c++)
            {
                richTextBox1.Select (c,1);
                char nextChar = richTextBox1.Text[c];
                Debug.WriteIf (showchars, nextChar);
                Color theColor = richTextBox1.SelectionColor;
                Font theFont = richTextBox1.SelectionFont;
                if (theFont == null)
                {
                    Debug.WriteLine ("Null font, using a default");
                    theFont = new Font (FontFamily.GenericSansSerif, 12);

                }
                float fontHeight = theFont.GetHeight(g);

                if (nextChar == '\n')
                {
                    retval.RequiredSize.Width = Math.Max (retval.RequiredSize.Width, xPos - bounds.Position.X);
                    xPos = bounds.Position.X;
                    yPos += (fontHeight * verticalSpacing);
                    if (yPos > bounds.Limit.Y)
                    {
                        break;
                    }
                }
                else if (nextChar == ' ')
                {
                   xPos += fontHeight / 2;
                }
                else if (nextChar == '\t')
                {
                    xPos += fontHeight * 2;
                }
                else
                {
                    // match the words for same font / color
                    Match m;

                    StringBuilder nextWord = new StringBuilder (30);
                    bool reduceAtEnd = false;
                    m = wordSeparator.Match(nextChar.ToString());
                    if (m.Success)
                    {
                        reduceAtEnd = true;
                    }
                    else
                    {
                        nextWord.Append (nextChar);
                    }


                    while (m.Success)
                    {
                        nextWord.Append (nextChar);
                        c++;
                        if (c >= length) break;
                        nextChar = richTextBox1.Text[c];
                        Debug.WriteIf (showchars, nextChar);
                        m = wordSeparator.Match (nextChar.ToString());
                    }
                    if (reduceAtEnd)
                    {
                        c--;
                    }
                    string word = nextWord.ToString();


                    PointF drawPoint = new PointF (xPos, yPos);
                    SizeF drawSize = g.MeasureString(word, 
                        theFont, drawPoint, stringFormat);
                    xPos += drawSize.Width;
                    if (xPos > bounds.Limit.X)
                    {
                        yPos += (fontHeight * verticalSpacing);
                        if (yPos > bounds.Limit.Y)
                        {
                            // need to undo changes to c
                            break;
                        }
                        retval.RequiredSize.Width = Math.Max (retval.RequiredSize.Width, xPos - bounds.Position.X);
                        xPos = bounds.Position.X + drawSize.Width;
                        drawPoint.X = bounds.Position.X;
                        drawPoint.Y = yPos;
                    }

                    if (!sizeOnly)
                    {
                        g.DrawString(word, theFont, 
                            new SolidBrush(theColor), drawPoint, stringFormat);
                    }
                }
            }

            retval.Fits = c > this.lastCharIndex;
            retval.Continued = c < richTextBox1.Text.Length;
            if (!sizeOnly) this.lastCharIndex = c;
            return retval;
        }


	}
}
