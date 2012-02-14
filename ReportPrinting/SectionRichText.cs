// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/

using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Diagnostics;

namespace ReportPrinting
{
	/// <summary>
	/// WORK IN PROGRESS: Prints the contents of a rich text box.
	/// </summary>
	/// <remarks>
	/// WORK IN PROGRESS...
    /// This code inspired by Mike Gold's Color Syntax Editor
    /// Although this version looks nothing like his original...
    /// http://www.c-sharpcorner.com/Code/2003/June/ColorSyntaxEditor.asp
    /// <para>
    /// It's really slow.
    /// There are many things it doesn't do so well, like:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Word-wrapping isn't quite right (spaces may appear at the beginning of a line,
    /// and a period may be separated from the sentence it is ending.</description></item>
    /// <item><description>Some fonts don't seem to come through the rich text box property</description></item>
    /// </list>
	/// </remarks>
    public class SectionRichText : ReportSection
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public SectionRichText()
        {
            //r = new Regex(@"\S", RegexOptions.IgnoreCase|RegexOptions.Compiled);
            // http://support.microsoft.com/default.aspx?scid=kb;EN-US;q307208
        }

        RichTextBox richTextBox1;
        int charIndex;
        int printLineIndex;
        //float verticalSpacing = 1f; // %times the font height
        ArrayList linesToPrint;
        // true for debug only
        //bool showchars = false;
         

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
            this.charIndex = 0;
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
            this.linesToPrint = new ArrayList (100);
            SectionSizeValues retval = new SectionSizeValues();
            WordToPrint nextWord = null;
            int nextIndex;
            while (charIndex < this.richTextBox1.Text.Length)
            {
                LineToPrint line = LineToPrint.Get (charIndex, g, bounds.Position, 
                    bounds.Width, this.richTextBox1, ref nextWord, out nextIndex);
                if (line.Size.Height + bounds.Position.Y < bounds.Limit.Y)
                {
                    linesToPrint.Add (line);
                    charIndex = nextIndex;
                    retval.RequiredSize.Width = Math.Max (retval.RequiredSize.Width, line.Size.Width);
                    retval.RequiredSize.Height += line.Size.Height;
                    bounds.Position.Y += line.Size.Height;
                }
                else
                {
                    retval.Continued = true;
                    break;
                }
            }

            retval.Fits = true;
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
			if ( !reportDocument.NoPrint ) 
			{
//			  g.TextRenderingHint = TextRenderingHint.AntiAlias;
//            while (this.printLineIndex < this.linesToPrint.Count)
//            {
//                LineToPrint line = this.linesToPrint[printLineIndex] as LineToPrint;
				foreach (LineToPrint line in linesToPrint) 
				{
					bounds.Position.Y += line.Size.Height;
					line.Print (g, bounds.Position);
					this.printLineIndex++;
				}
//            SectionSizeValues results = DrawEditControl(g, bounds, false);
//            SetContinued (results.Continued);
//            SetFits (results.Fits);
//            SetSize (results.RequiredSize, bounds);
			}
		}

        #endregion


        #region "Helper Classes"


        class CharToPrint
        {
            static Hashtable spaceSizes = new Hashtable();
            static SizeF GetSpaceSize (Graphics g, Font font)
            {
                if (spaceSizes.Contains (font))
                {
                    return (SizeF) spaceSizes[font];
                }
                else
                {
                    SizeF size1 = g.MeasureString ("||", font);
                    SizeF size2 = g.MeasureString ("| |", font);
                    return new SizeF (size2.Width - size1.Width, size2.Height);
                }
            }
            
            public static float WidthOfSpace = 0.25f;
            public static float SpacesInTab = 4;

            public char Text;
            public SizeF Size;
            public Font Font;
            public Color Color;

            public bool IsLineBreak
            {
                get { return (Text == '\n'); }
            }

            public static CharToPrint Get (int index, Graphics g, PointF pos, RichTextBox rtb)
            {
                CharToPrint nextChar = new CharToPrint();
                rtb.Select (index, 1);
                nextChar.Text = rtb.Text[index];
                //Debug.WriteIf (showchars, nextChar);
                nextChar.Font = rtb.SelectionFont;
                if (nextChar.Font == null)
                {
                    //Debug.WriteLine ("Null font, using a default");
                    nextChar.Font = new Font (FontFamily.GenericSansSerif, 12);
                }
                if (nextChar.Text == ' ')
                {
                    nextChar.Size = GetSpaceSize (g, nextChar.Font);
                }
                else if (nextChar.Text == '\t')
                {
                    nextChar.Size = GetSpaceSize (g, nextChar.Font);
                    nextChar.Size.Width *= SpacesInTab;
                }
                else
                {
                    nextChar.Color = rtb.SelectionColor;
                    nextChar.Size = g.MeasureString (nextChar.Text.ToString(), nextChar.Font, 
                        pos, StringFormat.GenericTypographic);
                }

                return nextChar;
            }

            public void Print (Graphics g, PointF bottomLeft)
            {
                //Console.Write (Text);
                if (Text == ' ' || Text == '\t')
                {
                    // do what when we have to print an underlined space or tab?
                }
                else
                {
                    PointF point = new PointF (bottomLeft.X, bottomLeft.Y - Size.Height);
                    g.DrawString(Text.ToString(), Font, new SolidBrush(Color), 
                        point, StringFormat.GenericTypographic);
                }
            }

        } // class CharToPrint

        /// <summary>
        /// Contians all the characters that fit in a word
        /// </summary>
        class WordToPrint
        {
            static Regex wordSeparator;
            static WordToPrint()
            {
                wordSeparator = new Regex(@"\w", RegexOptions.IgnoreCase|RegexOptions.Compiled);
            }

            WordToPrint ()
            {
                charsToPrint = new ArrayList (20);
                Size = new SizeF (0,0);
            }
            ArrayList charsToPrint;
            public SizeF Size;
            public int Length
            {
                get { return charsToPrint.Count; }
            }

            /// <summary>
            /// Appends a CharToPrint to the end of the word
            /// </summary>
            /// <param name="aChar">The CharToPrint</param>
            public void Append (CharToPrint aChar)
            {
                charsToPrint.Add (aChar);
                Size.Width += aChar.Size.Width;
                Size.Height = Math.Max (Size.Height, aChar.Size.Height);
            }
            /// <summary>
            /// Indexer to get CharToPrint
            /// </summary>
            public CharToPrint this [int index]
            {
                get { return charsToPrint[index] as CharToPrint; }
            }


            public bool IsLineBreak
            {
                get { return this[0].IsLineBreak; }
            }


            public static WordToPrint Get (int index, Graphics g, PointF pos, RichTextBox rtb)
            {
                WordToPrint nextWord = new WordToPrint();
                // match the words for same font / color
                Match m;
                CharToPrint nextChar = CharToPrint.Get (index, g, pos, rtb);
                m = wordSeparator.Match(nextChar.Text.ToString());
                if (m.Success)
                {
                    while (m.Success)
                    {
                        nextWord.Append (nextChar);
                        index++;
                        if (index >= rtb.Text.Length) break;
                        nextChar = CharToPrint.Get (index, g, pos, rtb);
                        m = wordSeparator.Match (nextChar.Text.ToString());
                    }
                    // TODO: do what with .,/: and other punctuation at the end of a word.
                }
                else
                {
                    nextWord.Append (nextChar);
                }
                return nextWord;
            }

            public void Print (Graphics g, PointF bottomLeft)
            {
                foreach (CharToPrint aChar in charsToPrint)
                {
                    aChar.Print (g, bottomLeft);
                    bottomLeft.X += aChar.Size.Width;
                }
            }

        } // class WordToPrint


        /// <summary>
        /// Contains all the words that fit in a line
        /// </summary>
        class LineToPrint
        {
            LineToPrint ()
            {
                wordsToPrint = new ArrayList (20);
                Size = new SizeF (0,0);
            }
            ArrayList wordsToPrint;
            public SizeF Size;
            public int NumberOfWords
            {
                get { return wordsToPrint.Count; }
            }

            public void Append (WordToPrint aWord)
            {
                wordsToPrint.Add (aWord);
                Size.Width += aWord.Size.Width;
                Size.Height = Math.Max (Size.Height, aWord.Size.Height);
            }

            /// <summary>
            /// Indexer to get WordToPrint
            /// </summary>
            public WordToPrint this [int index]
            {
                get { return wordsToPrint[index] as WordToPrint; }
            }

            public static LineToPrint Get (int index, Graphics g, PointF pos, 
                float width, RichTextBox rtb, ref WordToPrint nextWord, out int nextIndex)
            {
                LineToPrint line = new LineToPrint();
                if (nextWord != null)
                {
                    line.Append (nextWord);
                    pos.X += nextWord.Size.Width;
                }
                nextWord = null;
                int i = index;
                while (i < rtb.Text.Length)
                {
                    WordToPrint word = WordToPrint.Get (i, g, pos, rtb);
                    i += word.Length;
                    if (word.IsLineBreak)
                    {
                        // throw it away?
                        break;
                    }
                    else if (word.Size.Width + line.Size.Width < width)
                    {
                        line.Append (word);
                        pos.X += word.Size.Width;
                    }
                    else
                    {
                        nextWord = word;
                        break;
                    }
                }
                nextIndex = i;
                return line;
            }

            public void Print (Graphics g, PointF bottomLeft)
            {
                //Console.Write ("\nLine: ");
                foreach (WordToPrint word in wordsToPrint)
                {
                    word.Print (g, bottomLeft);
                    bottomLeft.X += word.Size.Width;
                }
            }
        } // class LineToPrint

        #endregion


        // The old way was about 2x faster, but still slow
        #region "Oldstuff"

// ORIGINAL WAY:
//        SectionSizeValues DrawEditControl(Graphics g, Bounds bounds, bool sizeOnly)
//        {
//            SectionSizeValues retval = new SectionSizeValues();
//            retval.RequiredSize = new SizeF (0,0);
//
//            // draw the control by selecting the first character
//            // of each word, and getting its font / color
//            int c;
//            int length = richTextBox1.Text.Length;
//            for (c = this.lastCharIndex; c < length; c++)
//            {
//                richTextBox1.Select (c,1);
//                char nextChar = richTextBox1.Text[c];
//                Debug.WriteIf (showchars, nextChar);
//                Color theColor = richTextBox1.SelectionColor;
//                Font theFont = richTextBox1.SelectionFont;
//                if (theFont == null)
//                {
//                    Debug.WriteLine ("Null font, using a default");
//                    theFont = new Font (FontFamily.GenericSansSerif, 12);
//
//                }
//                float fontHeight = theFont.GetHeight(g);
//
//                if (nextChar == '\n')
//                {
//                    retval.RequiredSize.Width = Math.Max (retval.RequiredSize.Width, xPos - bounds.Position.X);
//                    xPos = bounds.Position.X;
//                    yPos += (fontHeight * verticalSpacing);
//                    if (yPos > bounds.Limit.Y)
//                    {
//                        break;
//                    }
//                }
//                else if (nextChar == ' ')
//                {
//                   xPos += fontHeight / 2;
//                }
//                else if (nextChar == '\t')
//                {
//                    xPos += fontHeight * 2;
//                }
//                else
//                {
//                    // match the words for same font / color
//                    Match m;
//
//                    StringBuilder nextWord = new StringBuilder (30);
//                    bool reduceAtEnd = false;
//                    m = wordSeparator.Match(nextChar.ToString());
//                    if (m.Success)
//                    {
//                        reduceAtEnd = true;
//                    }
//                    else
//                    {
//                        nextWord.Append (nextChar);
//                    }
//
//
//                    while (m.Success)
//                    {
//                        nextWord.Append (nextChar);
//                        c++;
//                        if (c >= length) break;
//                        nextChar = richTextBox1.Text[c];
//                        Debug.WriteIf (showchars, nextChar);
//                        m = wordSeparator.Match (nextChar.ToString());
//                    }
//                    if (reduceAtEnd)
//                    {
//                        c--;
//                    }
//                    string word = nextWord.ToString();
//
//
//                    PointF drawPoint = new PointF (xPos, yPos);
//                    SizeF drawSize = g.MeasureString(word, 
//                        theFont, drawPoint, stringFormat);
//                    xPos += drawSize.Width;
//                    if (xPos > bounds.Limit.X)
//                    {
//                        yPos += (fontHeight * verticalSpacing);
//                        if (yPos > bounds.Limit.Y)
//                        {
//                            // need to undo changes to c
//                            break;
//                        }
//                        retval.RequiredSize.Width = Math.Max (retval.RequiredSize.Width, xPos - bounds.Position.X);
//                        xPos = bounds.Position.X + drawSize.Width;
//                        drawPoint.X = bounds.Position.X;
//                        drawPoint.Y = yPos;
//                    }
//
//                    if (!sizeOnly)
//                    {
//                        g.DrawString(word, theFont, 
//                            new SolidBrush(theColor), drawPoint, stringFormat);
//                    }
//                }
//            }
//
//            retval.Fits = c > this.lastCharIndex;
//            retval.Continued = c < richTextBox1.Text.Length;
//            if (!sizeOnly) this.lastCharIndex = c;
//            return retval;
//        }

        #endregion

	}
}
