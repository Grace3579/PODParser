using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace POD
{
	/// <summary>
	/// Summary description for PhoneManager.
	/// </summary>
	public class PhoneManager : System.Windows.Forms.Form
	{
		private System.Windows.Forms.DataGrid PhonesDataGrid;
		private System.Data.DataSet PhonesDataSet;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PhoneManager()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			//Initialize the List view with the currently registerred Phones
			PopulatePhones();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		private void PopulatePhones()
		{
			//get the configured list of phones from the DB and add to the list
			// dummy data for prototype

			// Create two DataTables.
			DataTable tEmployees = new DataTable("Employees");
			DataTable tPhones = new DataTable("Phones");

			// Create two columns, and add them to the first table.
			DataColumn cEmployeID = new DataColumn("EmployeeID", typeof(int));
			DataColumn cFirstName = new DataColumn("FirstName");
			DataColumn cLastName = new DataColumn("LastName");
			DataColumn cActive = new DataColumn("Active", typeof(bool));
			tEmployees.Columns.Add(cEmployeID);
			tEmployees.Columns.Add(cFirstName);
			tEmployees.Columns.Add(cLastName);
			tEmployees.Columns.Add(cActive);

			// Create three columns, and add them to the second table.
			DataColumn cPhoneNum = new DataColumn("PhoneNum");
			DataColumn cFKEmployeID = new DataColumn("EmployeeID", typeof(int));

			tPhones.Columns.Add(cPhoneNum);
			tPhones.Columns.Add(cFKEmployeID);

			// Add the tables to the DataSet.
			PhonesDataSet.Tables.Add(tEmployees);
			PhonesDataSet.Tables.Add(tPhones);

			// Create a DataRelation, and add it to the DataSet.
			DataRelation dr = new DataRelation
				("EmplToPhone", cEmployeID , cFKEmployeID);
			PhonesDataSet.Relations.Add(dr);

			//add dummy data to both tables
			DataRow empRow1 = tEmployees.NewRow();
			empRow1["EmployeeID"] = 111;
			empRow1["FirstName"] = "FirstName1";
			empRow1["LastName"] = "LastName1";
			empRow1["Active"] = true;
			tEmployees.Rows.Add(empRow1);

			DataRow empRow2 = tEmployees.NewRow();
			empRow2["EmployeeID"] = 112;
			empRow2["FirstName"] = "FirstName2";
			empRow2["LastName"] = "LastName2";
			empRow2["Active"] = true;
			tEmployees.Rows.Add(empRow2);

			DataRow phoneRow1 = tPhones.NewRow();
			phoneRow1["PhoneNum"] = "919-556-4567";
			phoneRow1["EmployeeID"] = 111;
			tPhones.Rows.Add(phoneRow1);

			DataRow phoneRow2 = tPhones.NewRow();
			phoneRow2["PhoneNum"] = "919-987-4567";
			phoneRow2["EmployeeID"] = 112;
			tPhones.Rows.Add(phoneRow2);

			//Bind the DataSet to the Data Grid
			PhonesDataGrid.SetDataBinding(PhonesDataSet, "Employees");
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.PhonesDataGrid = new System.Windows.Forms.DataGrid();
			this.PhonesDataSet = new System.Data.DataSet();
			((System.ComponentModel.ISupportInitialize)(this.PhonesDataGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PhonesDataSet)).BeginInit();
			this.SuspendLayout();
			// 
			// PhonesDataGrid
			// 
			this.PhonesDataGrid.DataMember = "";
			this.PhonesDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.PhonesDataGrid.Location = new System.Drawing.Point(15, 16);
			this.PhonesDataGrid.Name = "PhonesDataGrid";
			this.PhonesDataGrid.Size = new System.Drawing.Size(500, 248);
			this.PhonesDataGrid.TabIndex = 1;
			this.PhonesDataGrid.Navigate += new System.Windows.Forms.NavigateEventHandler(this.PhonesDataGrid_Navigate);
			// 
			// PhonesDataSet
			// 
			this.PhonesDataSet.DataSetName = "PhonesDataSet";
			this.PhonesDataSet.Locale = new System.Globalization.CultureInfo("en-US");
			// 
			// PhoneManager
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(528, 278);
			this.Controls.Add(this.PhonesDataGrid);
			this.Name = "PhoneManager";
			this.Text = "Phone Manager";
			((System.ComponentModel.ISupportInitialize)(this.PhonesDataGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PhonesDataSet)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion


		private void Phonelist_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}

		private void PhonesDataGrid_Navigate(object sender, System.Windows.Forms.NavigateEventArgs ne)
		{
		
		}
	}
}
