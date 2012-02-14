// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/

using System;

namespace ReportPrinting
{
    /// <summary>
    /// RepeatableTextSection is good for text that repeats 
    /// from page to page, such as a header or footer.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Unique text can be set for different pages using the
    /// following properties:
    /// <list type="table">
    /// <listheader><term>Property name</term><description>Description</description></listheader>
    /// <item><term>TextFirstPage</term><description>Text to display on first page</description></item>
    /// <item><term>TextOddPage</term><description>Text to display on all odd pages</description></item>
    /// <item><term>TextEvenPage</term><description>Text to display on all even pages</description></item>
    /// <item><term>Text</term><description>Default value used if one of the above is null</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// The following strings have special meanings.
    /// Each is substituted by the appropriate value
    /// if it appears within one of the above Text strings.
    /// <list type="table">
    /// <listheader><term>String</term><description>Description</description></listheader>
    /// <item><term>%p</term><description>Page Number</description></item>
	/// <item><term>%tp</term><description>Total Pages - this requires the 'CountPages' flag to be set</description></item>  
	/// </list>
    /// </para>
    /// </remarks>
    public class RepeatableTextSection : SectionText
	{
        /// <summary>
        /// Constructor requires text and style
        /// </summary>
        /// <param name="text">Text to be displayed </param>
        /// <param name="textStyle">TextStyle to use for printing the text</param>
		public RepeatableTextSection(string text, TextStyle textStyle)
            : base (text, textStyle)
		{
		}

        /// <summary>
        /// The text to use on the first page.
        /// Text will be used if TextFirstPage is null.
        /// </summary>
        public string TextFirstPage;
        /// <summary>
        /// The text to use on odd pages.
        /// Text will be used if TextOddPage is null.
        /// </summary>
        public string TextOddPage;
        /// <summary>
        /// The text to use on even pages.
        /// Text will be used if TextEvenPage is null.
        /// </summary>
        public string TextEvenPage;


        /// <summary>
        /// A function that should return the string to be printed on
        /// this call to Print()
        /// </summary>
        /// <param name="reportDocument">The parent ReportDocument</param>
        /// <returns>A string to be printed on this page</returns>
        protected override string GetText (ReportDocument reportDocument)
        {
            string text;
            int pageNumber = reportDocument.GetCurrentPage();
            if (pageNumber == 1)
            {
                text = TextFirstPage;
            }
            else if (pageNumber % 2 == 0)
            {
                text = TextEvenPage;
            }
            else
            {
                text = TextOddPage;
            }
            if (text == null)
            {
                if (this.Text == null)
                {
                    text = String.Empty;
                }
                else
                {
                    text = this.Text;
                }
            }
            text = text.Replace("%p", pageNumber.ToString());
			text = text.Replace("%tp", reportDocument.TotalPages.ToString());
			// TODO: Raise event for formatting text???

            return text;
        }

	}
}
