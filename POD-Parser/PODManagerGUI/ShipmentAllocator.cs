using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PODBusinessObjects;
using System.Data.SqlClient;

namespace POD
{
    public partial class ShipmentAllocator : Form
    {
        private SqlConnection podDbConn;
        private DataTable EmployeesDataTable;
        private long _selectedContactId;

        public ShipmentAllocator(SqlConnection conn)
        {
            InitializeComponent();
            podDbConn = conn;
            EmployeesDataTable = new DataTable("Employees");
        }

        public void PopulateEmployees()
        {
            EmployeesDataTable.Clear();
            EmployeesDataTable.Columns.Clear();

            SortedList<long, Contact> employees;
            try
            {
                podDbConn.Open();
                PODManager.ContactManager contactMgr = new PODManager.ContactManager();

                employees = contactMgr.getEmployees(podDbConn);

                DataColumn colEmployeeId = new DataColumn("Employee ID", typeof(long));
                DataColumn colEmployeeName = new DataColumn("Employee Name", typeof(String));
                DataColumn colEmployeePhoneNumber = new DataColumn("Employee Phone Number", typeof(String));

                EmployeesDataTable.Columns.Add(colEmployeeId);
                EmployeesDataTable.Columns.Add(colEmployeeName);
                EmployeesDataTable.Columns.Add(colEmployeePhoneNumber);

                for (int i = 0; i < employees.Count; ++i)
                {
                    Contact currEmployee = employees.Values[i];
                    DataRow currRow = EmployeesDataTable.NewRow();
                    currRow[0] = currEmployee.ContactId;
                    currRow[1] = currEmployee.FirstName + " " + currEmployee.LastName;
                    Phone mobilePhone = currEmployee.getMobilePhone();
                    if(mobilePhone != null)
                        currRow[2] = mobilePhone.ToString();

                    EmployeesDataTable.Rows.Add(currRow);
                }

                EmployeesDataGridView.DataSource = EmployeesDataTable;

            }
            catch (SqlException sqlExp)
            {
                System.Console.WriteLine("Exception : " + sqlExp.ToString());
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception : " + ex.ToString());
            }
            finally
            {
                podDbConn.Close();
            }
        }

        public long SelectedContact
        {
            get
            {
                return _selectedContactId;
            }
        }

        private void EmployeesDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = 0;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if(EmployeesDataGridView.SelectedRows.Count > 1)
            {
                MessageBox.Show("Please Select Only one Row", "Select One Employee", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (EmployeesDataGridView.SelectedRows.Count < 1)
            {
                MessageBox.Show("Please Select one Row", "Select One Employee", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _selectedContactId = Convert.ToInt64( EmployeesDataGridView.SelectedRows[0].Cells[0].Value);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            _selectedContactId = 0;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
