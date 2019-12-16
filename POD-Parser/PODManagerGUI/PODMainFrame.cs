using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using PODBusinessObjects;
using PODManager;

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
		private System.Windows.Forms.Button AssignUnassignButton;
        private System.Windows.Forms.Button SendButton;
        private IContainer components;
        private DataGridView ShipmentDataGridView;

        private PhoneManager phoneMgr;

        private static string connString = "Data Source=MANJULA-PC\\sqlexpress;Initial Catalog=PODManager;Integrated Security=True";

        private string _userCode;
        private string _companyCode;
        private System.Data.SqlClient.SqlConnection podDbConn;

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
			ShipmentsBuckets.Nodes.Add("UnAssigned Shipments");
			ShipmentsBuckets.Nodes.Add("Assigned Shipments");
			ShipmentsBuckets.Nodes.Add("Delivered Shipments (>T-3 days)");

            phoneMgr = new PhoneManager();

            podDbConn = new SqlConnection(connString);

            //TODO:: get loggedIn user information from somewhere
            _companyCode = "realminc";
            _userCode = "sohalr";
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
            this.components = new System.ComponentModel.Container();
            this.PODMainMenu = new System.Windows.Forms.MainMenu(this.components);
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
            this.AssignUnassignButton = new System.Windows.Forms.Button();
            this.SendButton = new System.Windows.Forms.Button();
            this.ShipmentDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.ShipmentDataGridView)).BeginInit();
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
            this.ShipmentsBuckets.Location = new System.Drawing.Point(5, 48);
            this.ShipmentsBuckets.Name = "ShipmentsBuckets";
            this.ShipmentsBuckets.Size = new System.Drawing.Size(144, 256);
            this.ShipmentsBuckets.TabIndex = 0;
            this.ShipmentsBuckets.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ShipmentsBuckets_AfterSelect);
            // 
            // ShipmentDetails
            // 
            this.ShipmentDetails.Location = new System.Drawing.Point(171, 197);
            this.ShipmentDetails.MultiColumn = true;
            this.ShipmentDetails.Name = "ShipmentDetails";
            this.ShipmentDetails.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.ShipmentDetails.Size = new System.Drawing.Size(780, 95);
            this.ShipmentDetails.TabIndex = 1;
            this.ShipmentDetails.SelectedIndexChanged += new System.EventHandler(this.ShipmentDetails_SelectedIndexChanged);
            // 
            // DeliveryDetails
            // 
            this.DeliveryDetails.Location = new System.Drawing.Point(171, 318);
            this.DeliveryDetails.Name = "DeliveryDetails";
            this.DeliveryDetails.Size = new System.Drawing.Size(539, 69);
            this.DeliveryDetails.TabIndex = 2;
            // 
            // PODMainToolBar
            // 
            this.PODMainToolBar.DropDownArrows = true;
            this.PODMainToolBar.Location = new System.Drawing.Point(0, 0);
            this.PODMainToolBar.Name = "PODMainToolBar";
            this.PODMainToolBar.ShowToolTips = true;
            this.PODMainToolBar.Size = new System.Drawing.Size(963, 42);
            this.PODMainToolBar.TabIndex = 3;
            // 
            // StatusBarMain
            // 
            this.StatusBarMain.Location = new System.Drawing.Point(0, 422);
            this.StatusBarMain.Name = "StatusBarMain";
            this.StatusBarMain.Size = new System.Drawing.Size(963, 22);
            this.StatusBarMain.TabIndex = 4;
            this.StatusBarMain.Text = "Status";
            // 
            // AssignUnassignButton
            // 
            this.AssignUnassignButton.Location = new System.Drawing.Point(171, 393);
            this.AssignUnassignButton.Name = "AssignUnassignButton";
            this.AssignUnassignButton.Size = new System.Drawing.Size(110, 25);
            this.AssignUnassignButton.TabIndex = 5;
            this.AssignUnassignButton.Text = "Assign";
            this.AssignUnassignButton.Click += new System.EventHandler(this.AllocateButton_Click);
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(291, 393);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(110, 25);
            this.SendButton.TabIndex = 6;
            this.SendButton.Text = "Send Assignments";
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // ShipmentDataGridView
            // 
            this.ShipmentDataGridView.AllowUserToAddRows = false;
            this.ShipmentDataGridView.AllowUserToDeleteRows = false;
            this.ShipmentDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ShipmentDataGridView.Location = new System.Drawing.Point(171, 41);
            this.ShipmentDataGridView.Name = "ShipmentDataGridView";
            this.ShipmentDataGridView.ReadOnly = true;
            this.ShipmentDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ShipmentDataGridView.Size = new System.Drawing.Size(780, 150);
            this.ShipmentDataGridView.TabIndex = 7;
            this.ShipmentDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ShipmentDataGridView_CellContentClick);
            // 
            // PODMainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(963, 444);
            this.Controls.Add(this.ShipmentDataGridView);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.AssignUnassignButton);
            this.Controls.Add(this.StatusBarMain);
            this.Controls.Add(this.PODMainToolBar);
            this.Controls.Add(this.DeliveryDetails);
            this.Controls.Add(this.ShipmentDetails);
            this.Controls.Add(this.ShipmentsBuckets);
            this.Menu = this.PODMainMenu;
            this.Name = "PODMainForm";
            this.Text = "Shipment Delivery Manager";
            ((System.ComponentModel.ISupportInitialize)(this.ShipmentDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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

		private void InstallPhoneAppMenuItem_Click(object sender, System.EventArgs e)
		{
		
		}

		private void ManagePhonesMenuItem_Click(object sender, System.EventArgs e)
		{
            phoneMgr.ShowDialog();
        }

		private void ShipmentsBuckets_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			//clear all items in ShipmentDetails
			ShipmentDetails.Items.Clear();
            if (ShipmentDataGridView.Rows.Count > 0 )
            {
                ShipmentDataGridView.DataSource = null;
            }

			TreeNode selectedNode = ShipmentsBuckets.SelectedNode;
			if(selectedNode.Text == "UnAssigned Shipments")
			{
//                PopulateShipmentGrid(ShipmentManager.ASSIGNMENT_STATUS.UN_ASSIGNED);
                PopulateShipmentGrid(ShipmentManager.ASSIGNMENT_STATUS.ALL);
                ShipmentDetails.Items.Add("UnAllocated Shipment 1");
				ShipmentDetails.Items.Add("UnAllocated Shipment 2");
				ShipmentDetails.Items.Add("UnAllocated Shipment 3");

                //Change the text of the buttons and enable them
                AssignUnassignButton.Text = "Assign";
                SendButton.Text = "Send Assignments";
                AssignUnassignButton.Enabled = true;
                SendButton.Enabled = true;
            }
			else if(selectedNode.Text == "Assigned Shipments")
			{
                PopulateShipmentGrid(ShipmentManager.ASSIGNMENT_STATUS.ASSIGNED);
                ShipmentDetails.Items.Add("Allocated Shipment 1");
				ShipmentDetails.Items.Add("Allocated Shipment 2");
				ShipmentDetails.Items.Add("Allocated Shipment 3");
				ShipmentDetails.Items.Add("Allocated Shipment 4");

                //Change the text of the buttons and enable them
                AssignUnassignButton.Text = "Un-Assign";
                AssignUnassignButton.Enabled = true;
                SendButton.Enabled = true;
            }
			else if(selectedNode.Text == "Deliverred Shipments")
			{
                PopulateShipmentGrid(ShipmentManager.ASSIGNMENT_STATUS.DELIVERED);
                ShipmentDetails.Items.Add("Deliverred Shipment 1");
				ShipmentDetails.Items.Add("Deliverred Shipment 2");
				ShipmentDetails.Items.Add("Deliverred Shipment 3");
				ShipmentDetails.Items.Add("Deliverred Shipment 4");
				ShipmentDetails.Items.Add("Deliverred Shipment 5");
				ShipmentDetails.Items.Add("Deliverred Shipment 6");

                //Disable the buttons
                AssignUnassignButton.Enabled = false;
                SendButton.Enabled = false;


			}
		}

		private void SendButton_Click(object sender, System.EventArgs e)
		{
            PODManager.ShipmentManager shpMgr = new ShipmentManager(_companyCode, _userCode);
            //bool bRetVal = shpMgr.sendShipmentAssignments(podDbConn);
            bool bRetVal = shpMgr.sendShipmentAssignments();

		}

		private void ShipmentDetails_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			//If We are in the Deliverred Shipments bucket, get delivery details
			//first clear DeliveryDetails 
 			DeliveryDetails.Items.Clear();
            if (ShipmentDetails.SelectedItems.Count < 1)
                return;

			TreeNode selectedNode = ShipmentsBuckets.SelectedNode;
			if(selectedNode.Text == "Deliverred Shipments")
			{
                for (int i = 0; i < ShipmentDetails.SelectedItems.Count; ++i)
                {
                    if (ShipmentDetails.SelectedItems[i].ToString() == "Deliverred Shipment 1")
                    {
                        DeliveryDetails.Items.Add("Deliverred Shipment 1 Deliverred on '20 Oct 2011");
                    }
                    
                    if (ShipmentDetails.SelectedItems[i].ToString() == "Deliverred Shipment 2")
                    {
                        DeliveryDetails.Items.Add("Deliverred Shipment 2 Deliverred on '19 Oct 2011");
                    }
                    
                    if (ShipmentDetails.SelectedItems[i].ToString() == "Deliverred Shipment 3")
                    {
                        DeliveryDetails.Items.Add("Deliverred Shipment 3 Deliverred on '21 Oct 2011");
                    }
                }
			}


		}

        private void AllocateButton_Click(object sender, EventArgs e)
        {
            if (getSelectedShipmentIds().Count > 0)
            {
                if (AssignUnassignButton.Text == "Assign")
                {

                    ShipmentAllocator shpAllocator = new ShipmentAllocator(podDbConn);
                    shpAllocator.PopulateEmployees();

                    DialogResult shpAllocatorReturn = shpAllocator.ShowDialog();
                    long employeeId = shpAllocator.SelectedContact;

                    if (shpAllocatorReturn == DialogResult.OK && (employeeId != 0))
                    {
                        ShipmentManager shpMgr = new ShipmentManager(_companyCode, _userCode);
                        string strErrors = "";
                        shpMgr.assignShipments(getSelectedShipmentIds(), employeeId, false, strErrors);
                    }

                    //remove all that were selected 
                    removeSelectedShipmentIds();
                }
                else if (AssignUnassignButton.Text == "Un-Assign")
                {
                    ShipmentManager shpMgr = new ShipmentManager(_companyCode, _userCode);
                    shpMgr.unassignShipments(podDbConn, getSelectedShipmentIds());

                    //remove all that were selected 
                    removeSelectedShipmentIds();
                }
            }
            return;
        }

        private void PopulateShipmentGrid(ShipmentManager.ASSIGNMENT_STATUS shipmentAsignmentStatus)
        {
            try
            {
                podDbConn.Open();
                PODManager.ShipmentManager  shpMgr = new ShipmentManager(_companyCode, _userCode);
                shpMgr.DBConnection = podDbConn;

                Dictionary<long, Shipment> shpments = new Dictionary<long, Shipment>();

                shpments = shpMgr.getShipmentsByStatus(shipmentAsignmentStatus);
                if (shpments.Count > 0)
                {
                    System.Console.WriteLine("shipment from DB by status = " + shpments[21].toXML(true).InnerXml);
                }

                Shipment shp21 = shpMgr.getShipmentByID(21);
                System.Console.WriteLine("shipment from DB by ID = " + shp21.toXML(true).InnerXml);


                Dictionary<long, Shipment> shpments2 = shpMgr.getShipmentsByStatusTest();
                System.Console.WriteLine("Test shipment = " + shpments2[1001].toXML(true).InnerXml);

                Dictionary<long, Contact> employees = shpMgr.getEmployeesForAssignment();

                if (employees.Count > 0)
                {
                    System.Console.WriteLine("Employee from DB, Full Name = " + employees[82].FullName
                            + ", Phone Number = " +  employees[82].PhoneNumber); 
                }

                if (podDbConn.State == ConnectionState.Open)
                    podDbConn.Close();
                

                // Test Assignments 
                List<ShipmentVersion> shipmentVersions = new List<ShipmentVersion>(5);
                ShipmentVersion shpVrsn1 = new ShipmentVersion(5, 21);
                ShipmentVersion shpVrsn2 = new ShipmentVersion(6, 23);
                ShipmentVersion shpVrsn3 = new ShipmentVersion(7, 24);

                shipmentVersions.Add(shpVrsn1);
                shipmentVersions.Add(shpVrsn2);
                shipmentVersions.Add(shpVrsn3);

                Contact empForAssignment = new Contact(5, 82, Contact.CONTACT_TYPE.EMPLOYEE);

                String strRetErrors = "";
                bool bAssignmentSucceeded = shpMgr.assignShipments(shipmentVersions, empForAssignment, false, strRetErrors);


                DataTable UndeliverredShipmentsTable = new DataTable("UnDeliveredShipments");
                DataColumn colShipmentId = new DataColumn("Shipment ID", typeof(long));
                DataColumn colShipmentSrc = new DataColumn("Shipment Source", typeof(String));
                DataColumn colShipmentSrcRef = new DataColumn("Shipment Ref", typeof(String));
                DataColumn colShipmentIsDeliverred = new DataColumn("Deliver To", typeof(String));
                

                UndeliverredShipmentsTable.Columns.Add(colShipmentId);
                UndeliverredShipmentsTable.Columns.Add(colShipmentSrc);
                UndeliverredShipmentsTable.Columns.Add(colShipmentSrcRef);
                UndeliverredShipmentsTable.Columns.Add(colShipmentIsDeliverred);

                if (shipmentAsignmentStatus == ShipmentManager.ASSIGNMENT_STATUS.ASSIGNED)
                {
                    DataColumn colShipmentAllocatedTo = new DataColumn("Allocated To", typeof(String));
                    UndeliverredShipmentsTable.Columns.Add(colShipmentAllocatedTo);
                }

                Dictionary<long, Shipment>.Enumerator shipEnum = shpments.GetEnumerator();
                while (shipEnum.MoveNext())
                {
                    PODBusinessObjects.Shipment currShipment = shipEnum.Current.Value;
                    DataRow currRow = UndeliverredShipmentsTable.NewRow();
                    currRow[0] = currShipment.ShipmentId;
                    currRow[1] = currShipment.ShipmentSrc;
                    currRow[2] = currShipment.ShipmentRef;
                    currRow[3] = currShipment.DeliverToAddress;
                    if (shipmentAsignmentStatus == ShipmentManager.ASSIGNMENT_STATUS.ASSIGNED)
                    {
                        currRow[4] = currShipment.getAssignedTo();
                    }

                    UndeliverredShipmentsTable.Rows.Add(currRow);
                }

                ShipmentDataGridView.DataSource = UndeliverredShipmentsTable;
            }
            catch (SqlException sqlExp)
            {
                System.Console.WriteLine("Exception : " + sqlExp.ToString());
            }
            catch(Exception ex)
            {
                System.Console.WriteLine("Exception : " + ex.ToString());
            }

            finally
            {
                podDbConn.Close();
            }

        }

        private List<long> getSelectedShipmentIds()
        {
            List<long> shipmentIds = new List<long>();
            int selShipments = ShipmentDataGridView.SelectedRows.Count;
            for (int i = 0; i < selShipments; ++i)
            {
                Type valType = ShipmentDataGridView[0, i].ValueType;
                long currShipmentID = (long)ShipmentDataGridView[0, i].Value;
                shipmentIds.Add(currShipmentID);
            }

            return shipmentIds;
        }

        private void removeSelectedShipmentIds()
        {
            int selShipments = ShipmentDataGridView.SelectedRows.Count;
            for (int i = 0; i < selShipments; ++i)
            {
                ShipmentDataGridView.Rows.Remove(ShipmentDataGridView.SelectedRows[0]);
            }
        }

        private void ShipmentDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

 	}
}
