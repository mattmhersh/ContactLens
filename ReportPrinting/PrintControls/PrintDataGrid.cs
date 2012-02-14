// This code was contributed by a user.
// Note: Requires some cleanup, and ideally, should be integrated better into
// the "ReportBuilder" class.

using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Printing;
using System.Diagnostics;
using ReportPrinting;

namespace ReportPrinting
{
	/// <summary>
	/// Prints a DataGrid View using the ReportBuilder Class Processing its data
	/// from the dataGrid DataSource, DataMember, DataTableGridSettings etc,
	/// Optionally using PrinterSetup, and/or PrintPreview Dialog's.
	///	</summary>
	public class PrintDataGrid : IReportMaker
	{
		private ReportDocument		reportDocument;
		private DataGrid			dataGrid;
		private PrintPreviewDialog	printPreviewDialog;
		private PrintDialog			printDialog;
		private PrinterSettings		printerSettings;
		private PageSettings		pageSettings;
		private ReportBuilder		reportBuilder;
		private float				maxColumnSize;
		private bool				printSpecificationPage;
		/// <summary>
		/// Gets or Sets  the report Document settings
		/// </summary>
		public ReportDocument ReportDocument
		{
			get {return reportDocument;}
			set {reportDocument = value;}
		}
		/// <summary>
		/// Gets the DataGrid
		/// </summary>
		public DataGrid DataGrid
		{
			set {dataGrid = value;}
		}
		/// <summary>
		/// Gets or Sets the PrintPreviewDialog
		/// </summary>
		public PrintPreviewDialog PrintPreviewDialog
		{
			get {return printPreviewDialog;}
			set {printPreviewDialog = value;}
		}
		/// <summary>
		/// Gets or Sets the PrintDialog
		/// </summary>
		public PrintDialog PrintDialog
		{
			get {return printDialog;}
			set {printDialog = value;}
		}
		/// <summary>
		/// Gets or Sets the PrinterSettings Object
		/// </summary>
		public PrinterSettings PrinterSettings
		{
			get {return printerSettings;}
			set {printerSettings = value;}
		}
		/// <summary>
		/// Gets or Sets the PageSettings Object
		/// </summary>
		public PageSettings PageSettings
		{
			get {return pageSettings;}
			set {pageSettings = value;}
		}
		/// <summary>
		/// Gets or Sets the PageSettings Object
		/// </summary>
		public ReportBuilder ReportBuilder
		{
			get {return reportBuilder;}
			set {reportBuilder = value;}
		}
		/// <summary>
		/// If true the first page will print showing the specification for the DataGrid,
		/// Default is false
		/// </summary>
		public bool PrintSpecificationPage 	{set {printSpecificationPage = value;} 
}
		/// <summary>
		/// Set the maximum size in inches for any single column
		/// </summary>
		public float MaxColumnSize 	{set {maxColumnSize = value;} }
		/// <summary>
		/// Default Constructor
		/// </summary>
		public PrintDataGrid()
		{
			SetDefaults();
		}
		/// <summary>
		///  Constructor with the DataGrid
		/// </summary>
		/// <param name="dataGrid"></param>
		public PrintDataGrid(DataGrid dataGrid)
		{
			this.dataGrid = dataGrid;
			SetDefaults();
		}
		/// <summary>
		/// Constructor with the DataGrid, options to automatically run any or all of
		/// PrintSetup dialog, PrintPreview dialog and Printing.
		/// </summary>
		public PrintDataGrid(DataGrid dataGrid, bool printerSetup, bool printPreview, bool print)
		{
			this.dataGrid = dataGrid;
			SetDefaults();

			if (printerSetup)
			{
				if(!PrinterSetup())
					return;
			}

			if (printPreview)
			{
				PrintPreview();
				return;
			}
			if (print)
			{
				Print();
			}
		}
		/// <summary>
		/// Set up all the Default Values
		/// </summary>
		private void SetDefaults()
		{
			//Default Page Settings
			pageSettings = new PageSettings();
			pageSettings.Margins.Top		= 50;
			pageSettings.Margins.Bottom		= 50;
			pageSettings.Margins.Left		= 50;
			pageSettings.Margins.Right		= 50;
			pageSettings.Landscape			= true;

			//Defaults for the Report Document
			reportDocument = new ReportDocument();
			reportDocument.ReportMaker=this;
			reportDocument.DefaultPageSettings=pageSettings;

			//Defaults for Printer Settings
			printerSettings = new PrinterSettings();
			printerSettings.MinimumPage		= 1;
			printerSettings.FromPage		= 1;
			printerSettings.ToPage			= 1;

			//Defaults for the Print Dialog
			printDialog = new PrintDialog();
			printDialog.AllowSomePages					= true;
			printDialog.AllowSelection					= true;
			printDialog.AllowPrintToFile				= true;
			printDialog.PrinterSettings					= printerSettings;

			//Defaults for the PrintPreview Dialog
			printPreviewDialog = new PrintPreviewDialog();
			printPreviewDialog.WindowState = FormWindowState.Maximized; //FullScreen

			//Defaults for this PrintDataGrid Class
			printSpecificationPage=false;

			//The maximum column size
			maxColumnSize = 5.0f;

			//Reset Default TextStyles
			ResetTextStyles(false);

			// Now use a builder to setup everything else
			reportBuilder			= new ReportBuilder(reportDocument);
			reportBuilder.MaxHeaderRowHeight	= 0.5f;
			reportBuilder.MaxDetailRowHeight	= 1.0f;

			//Determines Gridlines
			reportBuilder.DefaultTablePen		= reportDocument.ThinPen;
		}
		/// <summary>
		/// This will reset the TextStyles and Optionally set up the Defaults
		/// </summary>
		/// <param name="SkipSettingDefaults"></param>
		public void ResetTextStyles(bool SkipSettingDefaults)
		{
			TextStyle.ResetStyles();
			if (!SkipSettingDefaults)
			{
				// Setup the document's settings
				reportDocument.DefaultPageSettings= pageSettings;

				// Setup global TextStyles
				TextStyle.Heading1.Brush				= Brushes.Black;
				TextStyle.Heading1.SizeDelta			= 4.0f;
				TextStyle.Heading1.Underline			= true;
				TextStyle.TableHeader.Brush				= Brushes.Black;
				TextStyle.Normal.Size					= 9.0f;
				TextStyle.PageFooter.StringAlignment	= StringAlignment.Far;

				// add space at the right edge of columns, this will
				// keep them spaced apart a little nicer
				TextStyle.TableHeader.MarginFar			= 0.05f;
				TextStyle.TableRow.MarginFar			= 0.05f;
				TextStyle.TableHeader.MarginNear		= 0.05f;
				TextStyle.TableRow.MarginNear			= 0.05f;
			}
		}
		/// <summary>
		/// Print setup for the DataGrid
		/// </summary>
		/// <returns></returns>
		public bool PrinterSetup()
		{
			printDialog.PrinterSettings = printerSettings;
			printDialog.Document=reportDocument;
			if(printDialog.ShowDialog() == DialogResult.OK)
			{
				return true;
			}
			return false;
		}
		/// <summary>
		/// Creates a PrintPreviewDialog for the DataGrid
		/// </summary>
		public void PrintPreview()
		{
			printPreviewDialog.Document = reportDocument;
			printPreviewDialog.ShowDialog();
		}
		/// <summary>
		/// Prints the DataGrid
		/// </summary>
		public void Print()
		{
			reportDocument.Print();
		}


		/// <summary>
		/// This prints out the contents of a datagrid using  ReportBuilder
		/// </summary>
		/// <param name="reportDocument"></param>
		public void MakeDocument(ReportDocument reportDocument, string strFontSize, string strStartDate, string strEndDate)
		{
			if (dataGrid == null)
				return;

			// We may need a DataSet and Data Table depending on DataGrid source type
			DataTable dataTable				= new DataTable();
			DataSet   dataSet				= new DataSet();
			DataViewManager dataViewManager = new DataViewManager();
			DataView dataView				= new DataView();

			// We may need to create a DataView depending on the type of DataSouce that is
			// in the DataGrid
			bool dataViewExpected=true;
			//Depending on the Source and if there is a valid data memember we may need
			//to create a dataView, We actually will try and get the dataView later on
			//from the currency manager as this will let us show the datatable if we
			//have drilled down.
			switch (dataGrid.DataSource.GetType().ToString())
			{
				case "System.Data.DataViewManager":
				{
					//Check that a view is being shown, if no load views into a table
					if (dataGrid.DataMember == String.Empty)
					{
						dataViewExpected = false;
						//ok no Data View is active so print out he DataView
						dataTable = new DataTable("DataViewManager");
						DataColumn dataColumn
							= dataTable.Columns.Add("TableID",typeof(String));
						//Get the dataViewManger from the DataGrid source
						dataViewManager =  (DataViewManager)dataGrid.DataSource;
						//Add a dataRow to our little table for each DataView Setting
						foreach (DataViewSetting dvs in dataViewManager.DataViewSettings)
						{
							dataTable.Rows.Add(new string[]{dvs.Table.TableName});
						}
						//Now Create a DataView that the ReportPRinting can use to print
						dataView = new DataView(dataTable);
					}


					break;
				}
				case "System.Data.DataView":
				{
					dataView = (DataView)dataGrid.DataSource;
					break;
				}
				case "System.Data.DataTable":
				{
					dataView = ((DataTable)dataGrid.DataSource).DefaultView;
					break;
				}
				case "System.Data.DataSet":
				{    //If DataGrid uses a Data set than the DataTable is in DataMember
					if (dataGrid.DataMember == String.Empty)
					{
						dataViewExpected = false;

						//ok no Data View is active so print out tables in DataSet
						//by first creating a dataTable and loading the dataSet Table names
						//into it so we can create a dataView
						dataTable = new DataTable("DataSetTables");
						DataColumn dataColumn
							= dataTable.Columns.Add("TableID",typeof(String));
						//Get the DataSet from the DataGrid source
						dataSet =  (DataSet)dataGrid.DataSource;
						//Load the name of each table in the dataSet into our new table
						foreach (DataTable dt in dataSet.Tables)
						{
							dataTable.Rows.Add(new string[]{dt.TableName});
						}
						//Now Create a DataView that the ReportPRinting can use to print
						dataView = new DataView(dataTable);
					}

					break;
				}
			}
			// See if we can pickup the current view from the currency manager
			// This should also pickup if we are drilled down on any relations etc
			// This will be skipped where there was no dataView obtainable from the
			// dataGrid dataSource and DataMember
			CurrencyManager currencyManager;
			if (dataViewExpected)
			{
				//Currency Manager for the DataGrid

				currencyManager
					= (CurrencyManager)dataGrid.BindingContext
					[dataGrid.DataSource,dataGrid.DataMember];

				//This is the DataView that we are going to fill up...
				dataView = (DataView)currencyManager.List;

			}
			// Setup the document's settings
			reportDocument.DefaultPageSettings= pageSettings;

			reportBuilder.StartLinearLayout(Direction.Vertical);


			// Print out the actual Report Page

//			// %p is code for the page number
//			string pageStr = "-%p-";
//
//			string tableName=dataView.Table.TableName;
//
//			reportBuilder.AddPageHeader(
//				// First page
//				pageStr, tableName  , String.Empty,
//				// Right pages
//				pageStr,	tableName , String.Empty,
//				// Odd pages
//				String.Empty, tableName, pageStr);
//
//			reportBuilder.AddPageFooter (DateTime.Now.ToLongDateString(), ReportPrinting.HorizontalAlignment.Center);
//			//Now lets print out the Datagrid - First the Heading
			reportBuilder.AddText(dataGrid.CaptionText, TextStyle.Heading1);

			// We need to print any parent row info here
			// Check the dataGrid.DataMember and see if it is a data relation
			// If it is then get the first DataRow in the DataGrid and then
			// use its GetParentRows method, Each row should be checked to see
			// if there was a DataGridTableStyle set up for it
			// We have to work our way backwards up the data relation building strings that
			// need to be printed in reverse order to match the way the dataGrid displays
			if (dataGrid.ParentRowsVisible &&				//Are parents rows showing??
				dataViewExpected &&							//If no view then skip this
				dataGrid.DataMember.LastIndexOf(".")  > 0 ) //check Tablename.Relation
			{
				DataRowView dataRowView1= dataView[0];  //first get the DataRow View
				DataRow dataRow1 = dataRowView1.Row;    //Now get the DataRow for viewRow

				//break up the DataRelations string into its parts
				//[0] will be the original table,[1][..] will be relations
				//This need to be processed from last to first as the last one is
				//what is currently being displayed on the data grid
				string [] relations = dataGrid.DataMember.Split(new Char [] {'.'});

				//we will build an array of strings of parent data showing on the
				//datagrid that needs to be printed in reverse order
				//of the way they were built on the DataGrid in order
				//to replicate the drill down on the data grid.
				string[] parentText = new string[relations.Length - 1];

				//Go through each Relation from the last to first and get the parent rows
				//of the childRow using the data relations for that parent-child relation
				for (int r=relations.Length-1;r > 0;r--)
				{

					//If a child has multiple parent rows than we need to figure out which
					//is parent for this drill down. To get the information for each
					//parent row we are going to build a string with table & relations
					//which is the same as the dataGrid Builds automatically on drilldown
					//for the DataMember field which we will store in parentMember.
					//parentMember will then be used to get the correct currencyManager
					//which in turn will get the correct dataview,dataRowView and DataRow
					//IE TABLENAME.RELATION1.RELATION2 etc
					string parentMember = String.Empty;
					for (int i=0 ; i < r; i++)
					{
						parentMember  += relations[i];
						if (i < r-1)
							parentMember += "."; //Separate with periods except last
					}
					//Now that we have the parentMember we need to get the currency
					//manager for that parentmember which is holding the current
					//DataView from which we will get the
					currencyManager
						= (CurrencyManager)dataGrid.BindingContext
						[dataGrid.DataSource,parentMember];

					//This is the DataView that we are going to fill up...
					DataView parentDataView = (DataView)currencyManager.List;
					DataRowView parentDataRowView
						= (DataRowView)currencyManager.Current;  //first get the DataRow View

					DataRow parentRow = parentDataRowView.Row;

					//Start with the TableName:
					parentText[r-1] = parentRow.Table.TableName+":  ";

					//	Determine if there is DataGrid Table Style for the parent table
					// or do we just go through all the columns in the parent DataTable
					try
					{

						DataGridTableStyle tableStyle
							= dataGrid.TableStyles[parentRow.Table.TableName];
						//Go through the table style columns & build the parent text line
						foreach(DataGridColumnStyle columnStyle
									in tableStyle.GridColumnStyles)
						{
							parentText[r-1] +=  columnStyle.MappingName+": "
								+ parentRow[columnStyle.MappingName].ToString()+"   ";
						}
					}
					catch
					{
						//Go through the columns in the parentRow DataTable and built
						//the parent text line
						foreach(DataColumn dataColumn
									in parentRow.Table.Columns)
						{
							parentText[r-1] += dataColumn.ColumnName+": "
								+parentRow[dataColumn].ToString()+"   ";
						}
					}
				}
				//Now print out all the Parent Text array using the report builder

				for (int i=0; i < parentText.Length; i++)
				{
					reportBuilder.AddHorizontalLine ();
					reportBuilder.AddText(parentText[i], TextStyle.Normal);
				}
				reportBuilder.AddHorizontalLine ();
			}
			// Add dataView & all columns that are in the data grid
			reportBuilder.AddTable(dataView, true);

			// Now we have to determine if there was a DataGridTableStyle setup for this
			// DataGrid, The default will be to load from the DataView table columns
			bool loadFromDataView = true;

			//If there is a DataGridTableStyle - Add any columns showing in the grid..
			foreach (DataGridTableStyle tableStyle in dataGrid.TableStyles)
			{
				if(tableStyle.MappingName == dataView.Table.TableName)
				{
					loadFromDataView = false;
					foreach(DataGridColumnStyle columnStyle
								in tableStyle.GridColumnStyles)
					{
						reportBuilder.AddColumn(columnStyle.MappingName,
							columnStyle.HeaderText,
							(float)columnStyle.Width/75,  //Not sure if correct sizing
							true,
							true,
							(ReportPrinting.HorizontalAlignment)columnStyle.Alignment);

						DataGridTextBoxColumn textCol = columnStyle as DataGridTextBoxColumn;
						if (textCol != null)
						{
							Debug.WriteLine (textCol.Format);
							reportBuilder.CurrentColumn.FormatExpression = textCol.Format;
						}
					}
				}
			}
			//If this is still true than we have to load from the Table columns in the
			//dataView that the datagrid is using.
			//
			// IE there was NOT a valid DataGridTableStyle in the datagrid
			if (loadFromDataView)
			{
				reportBuilder.AddAllColumns (maxColumnSize, true, true);
			}
			reportBuilder.FinishLinearLayout();

		}

	}
}

