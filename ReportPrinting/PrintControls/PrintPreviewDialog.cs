using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace ReportPrinting.PrintControls
{
	/// <summary>
	/// Summary description for PrintPreviewDialog.
	/// </summary>
	[DesignTimeVisible(true), ToolboxItem(true), 
	Designer("System.ComponentModel.Design.ComponentDesigner, System.Design, " + 
	"Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), 
	DefaultProperty("Document")]
	public class PrintPreviewDialog : Form
	{
		// Events
		/// <summary>
		/// This event is triggered just prior to printing.
		/// </summary>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public event System.Drawing.Printing.PrintEventHandler Printing;
		/// <summary>
		/// This event is triggered just after printing.
		/// </summary>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public event EventHandler Printed;

		// Fields
		private new static readonly Size DefaultMinimumSize;
		private PrintPreviewControl previewControl;
		private ToolBarButton printButton;
		private ToolBarButton separator1;
		private ToolBarButton separator2;
		private ToolBarButton zoomButton;
		private ToolBarButton singlePage;
		private ToolBarButton twoPages;
		private ToolBarButton threePages;
		private ToolBarButton fourPages;
		private ToolBarButton sixPages;
		private Button closeButton;
		private NumericUpDown pageCounter;
		private Label pageLabel;
		private ToolBar toolBar1;
		private ImageList imageList;
		private MenuItem zoomMenu0;
		private MenuItem zoomMenu1;
		private MenuItem zoomMenu2;
		private MenuItem zoomMenu3;
		private MenuItem zoomMenu4;
		private MenuItem zoomMenu5;
		private MenuItem zoomMenu6;
		private MenuItem zoomMenu7;
		private MenuItem zoomMenu8;
		private ContextMenu menu;

//		[DefaultValue((string) null), SRDescription("PrintPreviewDocumentDescr"), SRCategory("CatBehavior")]
		/// <summary>
		/// The document to be printed
		/// </summary>
		[DefaultValue((string) null)]
		public PrintDocument Document
		{
			get
			{
				return this.previewControl.Document;
			}
			set
			{
				this.previewControl.Document = value;
			}
		}
 
		/// <summary>
		/// Called prior to printing.  It raises the Printing event.
		/// </summary>
		protected virtual bool onPrinting()
		{
			if (Printing == null)
				return true;
			else
			{
				PrintEventArgs e = new PrintEventArgs();
				Printing(this, e);
				return !e.Cancel;
			}
		}


		/// <summary>
		/// Called after printing.  It raises the Printed event.
		/// </summary>
		protected virtual void onPrinted()
		{
			if (Printed != null)
			{
				Printed(this, new EventArgs());
			}
		}

		System.Resources.ResourceManager resources;
		
		// Methods
		static PrintPreviewDialog()
		{
			PrintPreviewDialog.DefaultMinimumSize = new Size(0x177, 250);
		}

		/// <summary>
		/// The constructor
		/// </summary>
		public PrintPreviewDialog()
		{
			resources = new ResourceManager("System.Windows.Forms", System.Reflection.Assembly.GetAssembly(typeof(Form))); 
			this.menu = new ContextMenu();
			base.AutoScaleBaseSize = new Size(5, 13);
			this.InitForm();
			Bitmap bitmap1 = new Bitmap(typeof(System.Windows.Forms.PrintPreviewDialog), "PrintPreviewStrip.bmp");
			bitmap1.MakeTransparent();
			this.imageList.Images.AddStrip(bitmap1);
			this.MinimumSize = PrintPreviewDialog.DefaultMinimumSize;
		}

		private void InitForm()
		{
			// Create the controls
			this.previewControl = new PrintPreviewControl();
			this.previewControl.TabIndex = 1;
			this.previewControl.Size = new Size(0x318, 610);
			this.previewControl.Location = new Point(0, 0x2b);
			this.previewControl.Dock = DockStyle.Fill;
			this.previewControl.StartPageChanged += new EventHandler(this.previewControl_StartPageChanged);
			this.imageList = new ImageList();

			this.printButton = new ToolBarButton();
			this.printButton.ToolTipText = resources.GetString("PrintPreviewDialog_Print");
			this.printButton.ImageIndex = 0;
			this.zoomButton = new ToolBarButton();
			this.zoomButton.ToolTipText = resources.GetString("PrintPreviewDialog_Zoom");
			this.zoomButton.ImageIndex = 1;
			this.zoomButton.Style = ToolBarButtonStyle.DropDownButton;
			this.zoomButton.DropDownMenu = this.menu;
			this.separator1 = new ToolBarButton();
			this.separator1.Style = ToolBarButtonStyle.Separator;
			this.singlePage = new ToolBarButton();
			this.singlePage.ToolTipText = resources.GetString("PrintPreviewDialog_OnePage");
			this.singlePage.ImageIndex = 2;
			this.twoPages = new ToolBarButton();
			this.twoPages.ToolTipText = resources.GetString("PrintPreviewDialog_TwoPages");
			this.twoPages.ImageIndex = 3;
			this.threePages = new ToolBarButton();
			this.threePages.ToolTipText = resources.GetString("PrintPreviewDialog_ThreePages");
			this.threePages.ImageIndex = 4;
			this.fourPages = new ToolBarButton();
			this.fourPages.ToolTipText = resources.GetString("PrintPreviewDialog_FourPages");
			this.fourPages.ImageIndex = 5;
			this.sixPages = new ToolBarButton();
			this.sixPages.ToolTipText = resources.GetString("PrintPreviewDialog_SixPages");
			this.sixPages.ImageIndex = 6;
			this.separator2 = new ToolBarButton();
			this.separator2.Style = ToolBarButtonStyle.Separator;
			this.closeButton = new Button();
			this.closeButton.Location = new Point(0xc4, 2);
			this.closeButton.Size = new Size(50, 20);
			this.closeButton.TabIndex = 2;
			this.closeButton.FlatStyle = FlatStyle.Popup;
			this.closeButton.Text = resources.GetString("PrintPreviewDialog_Close");
			this.closeButton.Click += new EventHandler(this.closeButton_Click);
			this.pageLabel = new Label();
			this.pageLabel.Text = resources.GetString("PrintPreviewDialog_Page");
			this.pageLabel.TabStop = false;
			this.pageLabel.Location = new Point(510, 4);
			this.pageLabel.Size = new Size(50, 0x18);
			this.pageLabel.TextAlign = ContentAlignment.MiddleLeft;
			this.pageLabel.Dock = DockStyle.Right;
			this.pageCounter = new NumericUpDown();
			this.pageCounter.TabIndex = 1;
			this.pageCounter.Text = "1";
			this.pageCounter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.pageCounter.DecimalPlaces = 0;
			this.pageCounter.Minimum = new decimal(0);
			this.pageCounter.Maximum = new decimal(1000);
			this.pageCounter.ValueChanged += new EventHandler(this.UpdownMove);
			this.pageCounter.Size = new Size(0x40, 20);
			this.pageCounter.Dock = DockStyle.Right;
			this.pageCounter.Location = new Point(0x238, 0);
			this.toolBar1 = new ToolBar();
			this.toolBar1.ImageList = this.imageList;
			this.toolBar1.Dock = DockStyle.Top;
			this.toolBar1.Appearance = ToolBarAppearance.Flat;
			this.toolBar1.TabIndex = 3;
			this.toolBar1.Size = new Size(0x318, 0x2b);
			this.toolBar1.ShowToolTips = true;
			this.toolBar1.DropDownArrows = true;
			this.toolBar1.Buttons.AddRange(new ToolBarButton[] 
				{
					this.printButton, this.zoomButton, this.separator1, 
					this.singlePage, this.twoPages, this.threePages, this.fourPages, this.sixPages, this.separator2});
			this.toolBar1.Controls.Add(this.closeButton);
			this.toolBar1.Controls.Add(this.pageLabel);
			this.toolBar1.Controls.Add(this.pageCounter);
			this.toolBar1.ButtonClick += new ToolBarButtonClickEventHandler(this.ToolBarClick);

			this.zoomMenu0 = new MenuItem(resources.GetString("PrintPreviewDialog_ZoomAuto"), new EventHandler(this.ZoomAuto));
			this.zoomMenu1 = new MenuItem(resources.GetString("PrintPreviewDialog_Zoom500"), new EventHandler(this.Zoom500));
			this.zoomMenu2 = new MenuItem(resources.GetString("PrintPreviewDialog_Zoom200"), new EventHandler(this.Zoom250));
			this.zoomMenu3 = new MenuItem(resources.GetString("PrintPreviewDialog_Zoom150"), new EventHandler(this.Zoom150));
			this.zoomMenu4 = new MenuItem(resources.GetString("PrintPreviewDialog_Zoom100"), new EventHandler(this.Zoom100));
			this.zoomMenu5 = new MenuItem(resources.GetString("PrintPreviewDialog_Zoom75"), new EventHandler(this.Zoom75));
			this.zoomMenu6 = new MenuItem(resources.GetString("PrintPreviewDialog_Zoom50"), new EventHandler(this.Zoom50));
			this.zoomMenu7 = new MenuItem(resources.GetString("PrintPreviewDialog_Zoom25"), new EventHandler(this.Zoom25));
			this.zoomMenu8 = new MenuItem(resources.GetString("PrintPreviewDialog_Zoom10"), new EventHandler(this.Zoom10));
			this.zoomMenu0.Checked = true;
			this.menu.MenuItems.AddRange(new MenuItem[] 
				{
					this.zoomMenu0, this.zoomMenu1, this.zoomMenu2, this.zoomMenu3, this.zoomMenu4, 
					this.zoomMenu5, this.zoomMenu6, this.zoomMenu7, this.zoomMenu8 });

			this.Text = resources.GetString("PrintPreviewDialog_PrintPreview");
			this.MinimizeBox = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = SizeGripStyle.Hide;
			
			base.ClientSize = new Size(400, 300);
			base.Controls.Add(this.previewControl);
			base.Controls.Add(this.toolBar1);
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void CreateHandle()
		{
			if ((this.Document != null) && !this.Document.PrinterSettings.IsValid)
			{
				throw new InvalidPrinterException(this.Document.PrinterSettings);
			}
			base.CreateHandle();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			this.previewControl.InvalidatePreview();
		}

		private void previewControl_StartPageChanged(object sender, EventArgs e)
		{
			this.pageCounter.Value = (decimal) (this.previewControl.StartPage + 1);
		}

		internal bool ShouldSerializeMinimumSize()
		{
			return !this.MinimumSize.Equals(PrintPreviewDialog.DefaultMinimumSize);
		}

		//		internal override bool ShouldSerializeAutoScaleBaseSize()
		//		{
		//			return false;
		//		}

		//		internal override bool ShouldSerializeText()
//		{
//			return !this.Text.Equals(SR.GetString("PrintPreviewDialog_PrintPreview"));
//		}

		private void UpdownMove(object sender, EventArgs eventargs)
		{
			this.previewControl.StartPage = ((int) this.pageCounter.Value) - 1;
		}

		private void CheckZoomMenu(MenuItem toChecked)
		{
			foreach (MenuItem item1 in this.menu.MenuItems)
			{
				item1.Checked = toChecked == item1;
			}
		}

		private void ToolBarClick(object source, ToolBarButtonClickEventArgs eventargs)
		{
			if (eventargs.Button == this.printButton)
			{
				if (this.previewControl.Document != null)
				{
					if (onPrinting())
					{
						this.previewControl.Document.Print();
						onPrinted();
					}
				}
			}
			else if (eventargs.Button == this.zoomButton)
			{
				this.ZoomAuto(null, EventArgs.Empty);
			}
			else if (eventargs.Button == this.singlePage)
			{
				this.previewControl.Rows = 1;
				this.previewControl.Columns = 1;
			}
			else if (eventargs.Button == this.twoPages)
			{
				this.previewControl.Rows = 1;
				this.previewControl.Columns = 2;
			}
			else if (eventargs.Button == this.threePages)
			{
				this.previewControl.Rows = 1;
				this.previewControl.Columns = 3;
			}
			else if (eventargs.Button == this.fourPages)
			{
				this.previewControl.Rows = 2;
				this.previewControl.Columns = 2;
			}
			else if (eventargs.Button == this.sixPages)
			{
				this.previewControl.Rows = 2;
				this.previewControl.Columns = 3;
			}
		}

		private void ZoomAuto(object sender, EventArgs eventargs)
		{
			this.CheckZoomMenu(this.zoomMenu0);
			this.previewControl.AutoZoom = true;
		}

		private void Zoom500(object sender, EventArgs eventargs)
		{
			this.CheckZoomMenu(this.zoomMenu1);
			this.previewControl.Zoom = 5;
		}

		private void Zoom250(object sender, EventArgs eventargs)
		{
			this.CheckZoomMenu(this.zoomMenu2);
			this.previewControl.Zoom = 2.5;
		}

		private void Zoom150(object sender, EventArgs eventargs)
		{
			this.CheckZoomMenu(this.zoomMenu3);
			this.previewControl.Zoom = 1.5;
		}

		private void Zoom100(object sender, EventArgs eventargs)
		{
			this.CheckZoomMenu(this.zoomMenu4);
			this.previewControl.Zoom = 1;
		}

		private void Zoom75(object sender, EventArgs eventargs)
		{
			this.CheckZoomMenu(this.zoomMenu5);
			this.previewControl.Zoom = 0.75;
		}

		private void Zoom50(object sender, EventArgs eventargs)
		{
			this.CheckZoomMenu(this.zoomMenu6);
			this.previewControl.Zoom = 0.5;
		}

		private void Zoom25(object sender, EventArgs eventargs)
		{
			this.CheckZoomMenu(this.zoomMenu7);
			this.previewControl.Zoom = 0.25;
		}

		private void Zoom10(object sender, EventArgs eventargs)
		{
			this.CheckZoomMenu(this.zoomMenu8);
			this.previewControl.Zoom = 0.1;
		}

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PrintPreviewDialog
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Name = "PrintPreviewDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);

        }
	}
}
