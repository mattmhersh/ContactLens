// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/

using System;
using System.Data;

namespace ReportPrinting
{

    /// <summary>
    /// A delegate to handle column formatting.
    /// </summary>
	public delegate void FormatColumnHandler (object sender, FormatColumnEventArgs e);
	
	/// <summary>
	/// A delegate to handle the processing of data for sum and count
	/// </summary>
	public delegate void UpdateMathHandler (object sender, UpdateMathEventArgs e);

	/// <summary>
	/// A delegate to handle the formatting of a summary row.
	/// </summary>
	public delegate void FormatSummaryRowHandler (object sender, FormatSummaryRowEventArgs e);


	
    /// <summary>
    /// The Event Arguments used when a FormatColumn event is raised.
    /// </summary>
    public class FormatColumnEventArgs : EventArgs
    {
        object originalValue;
        string stringValue;
    
        /// <summary>
        /// The original value of the data to be printed in a column.
        /// </summary>
        public object OriginalValue
        {
            get { return this.originalValue; }
            set { this.originalValue = value; }
        }

        /// <summary>
        /// The formatted string that should be printed in place of the
        /// original column data.
        /// </summary>
        public string StringValue
        {
            get { return this.stringValue; }
            set { this.stringValue = value; }
        }

    } // class


	/// <summary>
	/// The Event Arguments used when a UpdateMath event is raised.
	/// </summary>
	public class UpdateMathEventArgs : EventArgs
	{
		object originalValue;
		DataRowView dataRowView;
		string stringRepresentation;
		double sum;
		double count;

		/// <summary>
		/// Constructor
		/// </summary>
		public UpdateMathEventArgs (DataRowView drv, object obj, string str)
		{
			this.originalValue = obj;
			this.dataRowView = drv;
			this.stringRepresentation = str;
		}
    
		/// <summary>
		/// The original value of the data to be printed in a column.
		/// </summary>
		public object OriginalValue
		{
			get { return this.originalValue; }
		}

		/// <summary>
		/// The DataRowView for the current row
		/// </summary>
		public DataRowView DataRowView
		{
			get { return this.dataRowView; }
		}

		/// <summary>
		///  The String Representation of the OriginalValue
		/// </summary>
		public string StringRepresentation
		{
			get { return this.stringRepresentation; }
		}

		/// <summary>
		/// The sum of values in the rows of this column. When the event is fired, this
		/// sum reflects the sum of all rows prior to the current row.
		/// After the event, it should reflect the sum of all rows up to
		/// and including the current row.
		/// </summary>
		public double Sum
		{
			get { return this.sum; }
			set { this.sum = value; }
		}

		/// <summary>
		/// The count (number) of rows. When the event is fired, this
		/// count reflects the number of rows prior to the current row.
		/// After the event, it should reflect the count of all rows up to
		/// and including the current row.
		/// </summary>
		public double Count
		{
			get { return this.count; }
			set { this.count = value; }
		}

	} // class


	/// <summary>
	/// The Event Arguments used when a FormatSummaryRow event is raised.
	/// </summary>
	public class FormatSummaryRowEventArgs : EventArgs
	{
		string field;
		string stringValue;
    
		/// <summary>
		/// The field name (column name) for the data source that this column represents.
		/// </summary>
		public string Field
		{
			get { return this.field; }
			set { this.field = value; }
		}

		/// <summary>
		/// The formatted string that should be printed in place of the
		/// original column data.
		/// </summary>
		public string StringValue
		{
			get { return this.stringValue; }
			set { this.stringValue = value; }
		}

	} // class


}
