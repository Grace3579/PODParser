using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace POD
{
	/// <summary>
	/// POD Main Form.
	/// </summary>
	public class PODMainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu PODMainMenu;
		private System.Windows.Forms.ToolBar PODMainToolBar;
		private System.Windows.Forms.MenuItem ShipmentMenuItem;
		private System.Windows.Forms.MenuItem ImportShipmentMenuItem;
		private System.Windows.Forms.MenuItem ExportShipmentMenuItem;
		private System.Windows.Forms.MenuItem AddShipmentMenuItem;
		private System.Windows.Forms.MenuItem ShipmentSearchMenuItem;
		private System.Windows.Forms.MenuItem ExitMenuItem;
		private System.Windows.Forms.MenuItem ToolsMenuItem;
		private System.Windows.Forms.MenuItem ManagePhonesMenuItem;
		private System.Windows.Forms.MenuItem InstallPhoneAppMenuItem;
		private System.Windows.Forms.MenuItem HelpMenuItem;
		private System.Windows.Forms.MenuItem AboutPODMenuItem;
		private System.Windows.Forms.StatusBar StatusBarMain;
		private System.Windows.Forms.TreeView ShipmentsBuckets;
		private System.Windows.Forms.ListBox ShipmentDetails;
		private System.Windows.Forms.ListBox DeliveryDetails;
		private System.Windows.Forms.Button AllocateButton;
		private System.Windows.Forms.Button SendButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PODMainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			//For testing only
			//Add some items to the Shipments Bucket
			ShipmentsBuckets.Nodes.Add("UnAllocated Shipments");
			ShipmentsBuckets.Nodes.Add("Allocated Shipments");
			ShipmentsBuckets.Nodes.Add("Deliverred Shipments");
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.PODMainMenu = new System.Windows.Forms.MainMenu();
			this.ShipmentMenuItem = new System.Windows.Forms.MenuItem();
			this.AddShipmentMenuItem = new System.Windows.Forms.MenuItem();
			this.ImportShipmentMenuItem = new System.Windows.Forms.MenuItem();
			this.ExportShipmentMenuItem = new System.Windows.Forms.MenuItem();
			this.ShipmentSearchMenuItem = new System.Windows.Forms.MenuItem();
			this.ExitMenuItem = new System.Windows.Forms.MenuItem();
			this.ToolsMenuItem = new System.Windows.Forms.MenuItem();
			this.ManagePhonesMenuItem = new System.Windows.Forms.MenuItem();
			this.InstallPhoneAppMenuItem = new System.Windows.Forms.MenuItem();
			this.HelpMenuItem = new System.Windows.Forms.MenuItem();
			this.AboutPODMenuItem = new System.Windows.Forms.MenuItem();
			this.ShipmentsBuckets = new System.Windows.Forms.TreeView();
			this.ShipmentDetails = new System.Windows.Forms.ListBox();
			this.DeliveryDetails = new System.Windows.Forms.ListBox();
			this.PODMainToolBar = new System.Windows.Forms.ToolBar();
			this.StatusBarMain = new System.Windows.Forms.StatusBar();
			this.AllocateButton = new System.Windows.Forms.Button();
			this.SendButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// PODMainMenu
			// 
			this.PODMainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.ShipmentMenuItem,
																						this.ToolsMenuItem,
																						this.HelpMenuItem});
			// 
			// ShipmentMenuItem
			// 
			this.ShipmentMenuItem.Index = 0;
			this.ShipmentMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							 this.AddShipmentMenuItem,
																							 this.ImportShipmentMenuItem,
																							 this.ExportShipmentMenuItem,
																							 this.ShipmentSearchMenuItem,
																							 this.ExitMenuItem});
			this.ShipmentMenuItem.Text = "Shipments";
			// 
			// AddShipmentMenuItem
			// 
			this.AddShipmentMenuItem.Index = 0;
			this.AddShipmentMenuItem.Text = "Add";
			// 
			// ImportShipmentMenuItem
			// 
			this.ImportShipmentMenuItem.Index = 1;
			this.ImportShipmentMenuItem.Text = "Import";
			// 
			// ExportShipmentMenuItem
			// 
			this.ExportShipmentMenuItem.Index = 2;
			this.ExportShipmentMenuItem.Text = "Export";
			// 
			// ShipmentSearchMenuItem
			// 
			this.ShipmentSearchMenuItem.Index = 3;
			this.ShipmentSearchMenuItem.Text = "Search";
			// 
			// ExitMenuItem
			// 
			this.ExitMenuItem.Index = 4;
			this.ExitMenuItem.Text = "Exit";
			// 
			// ToolsMenuItem
			// 
			this.ToolsMenuItem.Index = 1;
			this.ToolsMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.ManagePhonesMenuItem,
																						  this.InstallPhoneAppMenuItem});
			this.ToolsMenuItem.Text = "Tools";
			// 
			// ManagePhonesMenuItem
			// 
			this.ManagePhonesMenuItem.Index = 0;
			this.ManagePhonesMenuItem.Text = "Manage Phones";
			this.ManagePhonesMenuItem.Click += new System.EventHandler(this.ManagePhonesMenuItem_Click);
			// 
			// InstallPhoneAppMenuItem
			// 
			this.InstallPhoneAppMenuItem.Index = 1;
			this.InstallPhoneAppMenuItem.Text = "Install Phone App";
			this.InstallPhoneAppMenuItem.Click += new System.EventHandler(this.InstallPhoneAppMenuItem_Click);
			// 
			// HelpMenuItem
			// 
			this.HelpMenuItem.Index = 2;
			this.HelpMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.AboutPODMenuItem});
			this.HelpMenuItem.Text = "Help";
			// 
			// AboutPODMenuItem
			// 
			this.AboutPODMenuItem.Index = 0;
			this.AboutPODMenuItem.Text = "About POD Manager";
			// 
			// ShipmentsBuckets
			// 
			this.ShipmentsBuckets.ImageIndex = -1;
			this.ShipmentsBuckets.Location = new System.Drawing.Point(5, 48);
			this.ShipmentsBuckets.Name = "ShipmentsBuckets";
			this.ShipmentsBuckets.SelectedImageIndex = -1;
			this.ShipmentsBuckets.Size = new System.Drawing.Size(144, 256);
			this.ShipmentsBuckets.TabIndex = 0;
			this.ShipmentsBuckets.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ShipmentsBuckets_AfterSelect);
			// 
			// ShipmentDetails
			// 
			this.ShipmentDetails.Location = new System.Drawing.Point(152, 48);
			this.ShipmentDetails.Name = "ShipmentDetails";
			this.ShipmentDetails.Size = new System.Drawing.Size(536, 95);
			this.ShipmentDetails.TabIndex = 1;
			this.ShipmentDetails.SelectedIndexChanged += new System.EventHandler(this.ShipmentDetails_SelectedIndexChanged);
			// 
			// DeliveryDetails
			// 
			this.DeliveryDetails.Location = new System.Drawing.Point(152, 160);
			this.DeliveryDetails.Name = "DeliveryDetails";
			this.DeliveryDetails.Size = new System.Drawing.Size(536, 147);
			this.DeliveryDetails.TabIndex = 2;
			// 
			// PODMainToolBar
			// 
			this.PODMainToolBar.DropDownArrows = true;
			this.PODMainToolBar.Location = new System.Drawing.Point(0, 0);
			this.PODMainToolBar.Name = "PODMainToolBar";
			this.PODMainToolBar.ShowToolTips = true;
			this.PODMainToolBar.Size = new System.Drawing.Size(704, 42);
			this.PODMainToolBar.TabIndex = 3;
			// 
			// StatusBarMain
			// 
			this.StatusBarMain.Location = new System.Drawing.Point(0, 371);
			this.StatusBarMain.Name = "StatusBarMain";
			this.StatusBarMain.Size = new System.Drawing.Size(704, 22);
			this.StatusBarMain.TabIndex = 4;
			this.StatusBarMain.Text = "Status";
			// 
			// AllocateButton
			// 
			this.AllocateButton.Location = new System.Drawing.Point(152, 328);
			this.AllocateButton.Name = "AllocateButton";
			this.AllocateButton.Size = new System.Drawing.Size(96, 23);
			this.AllocateButton.TabIndex = 5;
			this.AllocateButton.Text = "Allocate";
			// 
			// SendButton
			// 
			this.SendButton.Location = new System.Drawing.Point(272, 328);
			this.SendButton.Name = "SendButton";
			this.SendButton.Size = new System.Drawing.Size(104, 23);
			this.SendButton.TabIndex = 6;
			this.SendButton.Text = "Send Allocations";
			this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
			// 
			// PODMainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(704, 393);
			this.Controls.Add(this.SendButton);
			this.Controls.Add(this.AllocateButton);
			this.Controls.Add(this.StatusBarMain);
			this.Controls.Add(this.PODMainToolBar);
			this.Controls.Add(this.DeliveryDetails);
			this.Controls.Add(this.ShipmentDetails);
			this.Controls.Add(this.ShipmentsBuckets);
			this.Menu = this.PODMainMenu;
			this.Name = "PODMainForm";
			this.Text = "Shipment Delivery Manager";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new PODMainForm());
		}

		private void ManagePhones()
		{    
			// Create a new OpenFileDialog and display it.
			PhoneManager phoneMgr = new PhoneManager();
			phoneMgr.ShowDialog();
		}

		private void InstallPhoneAppMenuItem_Click(object sender, System.EventArgs e)
		{
		
		}

		private void ManagePhonesMenuItem_Click(object sender, System.EventArgs e)
		{
			ManagePhones();
		}

		private void ShipmentsBuckets_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			//clear all items in ShipmentDetails
			ShipmentDetails.Items.Clear();

			TreeNode selectedNode = ShipmentsBuckets.SelectedNode;
			if(selectedNode.Text == "UnAllocated Shipments")
			{
				ShipmentDetails.Items.Add("UnAllocated Shipment 1");
				ShipmentDetails.Items.Add("UnAllocated Shipment 2");
				ShipmentDetails.Items.Add("UnAllocated Shipment 3");
			}
			else if(selectedNode.Text == "Allocated Shipments")
			{
				ShipmentDetails.Items.Add("Allocated Shipment 1");
				ShipmentDetails.Items.Add("Allocated Shipment 2");
				ShipmentDetails.Items.Add("Allocated Shipment 3");
				ShipmentDetails.Items.Add("Allocated Shipment 4");
			}
			else if(selectedNode.Text == "Deliverred Shipments")
			{
				ShipmentDetails.Items.Add("Deliverred Shipment 1");
				ShipmentDetails.Items.Add("Deliverred Shipment 2");
				ShipmentDetails.Items.Add("Deliverred Shipment 3");
				ShipmentDetails.Items.Add("Deliverred Shipment 4");
				ShipmentDetails.Items.Add("Deliverred Shipment 5");
				ShipmentDetails.Items.Add("Deliverred Shipment 6");
			}
		}

		private void SendButton_Click(object sender, System.EventArgs e)
		{
		
		}

		private void ShipmentDetails_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			//If We are in the Deliverred Shipments bucket, get delivery details
			//first clear DeliveryDetails 
 			DeliveryDetails.Items.Clear();

			TreeNode selectedNode = ShipmentsBuckets.SelectedNode;
			if(selectedNode.Text == "Deliverred Shipments")
			{

				if (ShipmentDetails.SelectedItem.ToString() == "Deliverred Shipment 1")
				{
					DeliveryDetails.Items.Add("Deliverred Shipment 1 Deliverred on '20 Oct 2011");
				}
				else if (ShipmentDetails.SelectedItem.ToString() == "Deliverred Shipment 2")
				{
					DeliveryDetails.Items.Add("Deliverred Shipment 2 Deliverred on '19 Oct 2011");
				}
				if (ShipmentDetails.SelectedItem.ToString() == "Deliverred Shipment 3")
				{
					DeliveryDetails.Items.Add("Deliverred Shipment 3 Deliverred on '21 Oct 2011");
				}
			}

		}
	}
}
