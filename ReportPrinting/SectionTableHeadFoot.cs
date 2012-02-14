using System;
using System.Drawing;
using System.Data;

namespace ReportPrinting
{
	/// <summary>
	/// Extends SectionTable to include optional Header and Footer sections.
	/// Only needed if KeepTogether or MinDataRowsFit is in use.
	/// Header will print on the same page as the first bit of the table.
	/// Footer will print on the same page as the last bit of the table.
	/// </summary>
	public class SectionTableHeadFoot : SectionTable
	{
		ReportSection tableHeader = null;
		ReportSection tableFooter = null;

		/// <summary>
		/// Returns the header section of this table
		/// </summary>
		public ReportSection TableHeader
		{
			get { return tableHeader; }
			set { tableHeader = value; }
		}
		
		/// <summary>
		/// Returns the footer section of this table
		/// </summary>
		public ReportSection TableFooter
		{
			get { return tableFooter; }
			set { tableFooter = value; }
		}
		

		/// <summary>
		/// The constructor
		/// </summary>
		/// <param name="dataSource"></param>
		public SectionTableHeadFoot(DataView dataSource) : base(dataSource)
		{}

		/// <summary>
		/// Called to calculate the size that this section requires on
		/// the next call to Print.  This method will be called exactly once
		/// prior to each call to Print.  It must update the values Size and
		/// Continued of the ReportSection base class.
		/// </summary>
		/// <param name="reportDocument">The parent ReportDocument that is printing.</param>
		/// <param name="g">Graphics object to print on.</param>
		/// <param name="bounds">Bounds of the area to print within.</param>
		/// <returns>SectionSizeValues of requiredSize, fits, and continues.</returns>
		protected override SectionSizeValues DoCalcSize (
			ReportDocument reportDocument,
			Graphics g,
			Bounds bounds
			)
		{
			// Default values
			SectionSizeValues retvals = new SectionSizeValues();

			DataSource.RowFilter = RowFilter;

			Bounds tableBounds = new Bounds(0, 0, 0, 0);
			if (TableHeader == null || rowIndex > 0)
				tableBounds = bounds;
			else
			{
				TableHeader.CalcSize(reportDocument, g, bounds);
				if ( TableHeader.Fits )
					tableBounds = new Bounds(
						bounds.Position.X, bounds.Position.Y + TableHeader.RequiredSize.Height,
						bounds.Limit.X, bounds.Limit.Y);
			} 
			if ( !tableBounds.IsEmpty() )
			{
				retvals = base.DoCalcSize(reportDocument, g, tableBounds);
				if ( retvals.Fits )
				{
					if (TableHeader != null && rowIndex <= 0)
						retvals.RequiredSize = new SizeF(retvals.RequiredSize.Width, 
							retvals.RequiredSize.Height + TableHeader.RequiredSize.Height);
					if ( !retvals.Continued && TableFooter != null)
					{
						Bounds footerBounds = new Bounds(
							tableBounds.Position.X, tableBounds.Position.Y + RequiredSize.Height,
							tableBounds.Limit.X, tableBounds.Limit.Y);
						TableFooter.CalcSize(reportDocument, g, footerBounds);
						if ( TableFooter.Fits )
							retvals.RequiredSize = new SizeF(retvals.RequiredSize.Width, 
								retvals.RequiredSize.Height + TableFooter.RequiredSize.Height);
						else
						{
							if (this.dataRowsFit >= (2 * this.MinDataRowsFit))
							{
								this.dataRowsFit -= this.MinDataRowsFit;
								retvals.Continued = true;
								// should adjust RequiredSize.
							} 
							else
							{
								retvals.Fits = false;
							}
						}
					}
				} 
			}
			return retvals;
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
			DataSource.RowFilter = RowFilter;

			int origRowIndex = this.rowIndex;
			Bounds origBounds = bounds;
			if (this.rowIndex <= 0 && TableHeader != null)
			{
				TableHeader.Print(reportDocument, g, bounds);
				bounds.Position.Y += TableHeader.RequiredSize.Height;
			}
			base.DoPrint(reportDocument, g, bounds);
			if ( !this.Continued && TableFooter != null)
			{
				bounds.Position.Y = origBounds.Position.Y + this.RequiredSize.Height - TableFooter.RequiredSize.Height;
				TableFooter.Print(reportDocument, g, bounds);
			}
		}

		/// <summary>
		/// Resets the entire section, useful at the very beginning
		/// of a print (before Graphics is even known)
		/// to reset startedPrinting.
		/// </summary>
		public override void Reset()
		{
			if ( this.TableHeader != null)
				this.TableHeader.Reset();
			base.Reset();
			if ( this.TableFooter != null)
				this.TableFooter.Reset();
		}

		/// <summary>
		/// Resets the size values.  This enables
		/// the CalcSize method to be called again.
		/// </summary>
		public override void ResetSize()
		{
			if ( this.TableHeader != null)
				this.TableHeader.ResetSize();
			base.ResetSize();
			if ( this.TableFooter != null)
				this.TableFooter.ResetSize();
		}
	};
}
