// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/

using System;

namespace ReportPrinting
{
	/// <summary>
	/// An interface to be used in the Strategy design pattern.
	/// ReportDocument will call the single method MakeDocument()
	/// when it wants to be formatted and made into a complete document.
	/// </summary>
	public interface IReportMaker
	{
        /// <summary>
        /// This function is called prior to printing.
        /// The implementer of this function is passed a handle
        /// to a ReportDocument object that needs to be setup.
        /// </summary>
        /// <param name="reportDocument">Handle to the ReportDocument object</param>
        /// <remarks>
        /// It is desirable to call reportDocument.ClearSections() before
        /// adding new sections to the reportDocument.
        /// </remarks>
        void MakeDocument(ReportDocument reportDocument, string strFontSize, string strStartDate, string strEndDate);
	}
}
