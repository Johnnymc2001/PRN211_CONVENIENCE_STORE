using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccess;
using DataAccess.Repository;
using BusinessObject.Models;

namespace ConvenienceStoreApp
{
    public partial class ucStaffManagement : UserControl
    {
        public ucStaffManagement()
        {
            InitializeComponent();
        }

        IStaffRepository repoStaff = new StaffRepository();
        BindingSource source;

        private void ucStaffManagement_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                LoadStaffList();
                txtStaffID.ReadOnly = true;
                txtFullname.ReadOnly = true;
                txtAddress.ReadOnly = true;
                txtPhoneNumber.ReadOnly = true;
                txtPassword.ReadOnly = true;
                txtRoleID.ReadOnly = true;
                txtStatusID.ReadOnly = true;
                txtEmail.ReadOnly = true;
            }
        }

        public void LoadStaffList()
        {
            List<TblStaff> staffs = null;
            try
            {
                staffs = repoStaff.GetAllStaff();

                source = new BindingSource();
                source.DataSource = staffs;

                txtStaffID.DataBindings.Clear();
                txtFullname.DataBindings.Clear();
                txtAddress.DataBindings.Clear();
                txtPhoneNumber.DataBindings.Clear();
                txtPassword.DataBindings.Clear();
                txtRoleID.DataBindings.Clear();
                txtStatusID.DataBindings.Clear();
                txtEmail.DataBindings.Clear();

                txtStaffID.DataBindings.Add("Text", source, "StaffId");
                txtFullname.DataBindings.Add("Text", source, "Fullname");
                txtAddress.DataBindings.Add("Text", source, "Address");
                txtPhoneNumber.DataBindings.Add("Text", source, "PhoneNumber");
                txtPassword.DataBindings.Add("Text", source, "Password");
                txtRoleID.DataBindings.Add("Text", source, "RoleId");
                txtStatusID.DataBindings.Add("Text", source, "StatusId");
                txtEmail.DataBindings.Add("Text", source, "Email");

                dgvStaffList.DataSource = null;
                dgvStaffList.DataSource = source;

            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Load Staff List", MessageBoxButtons.OK);
            } 

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                TblStaff staff = repoStaff.SearchByIdAndName(txtSearchStaffID.Text, txtSearchStaffName.Text);
                if(staff != null)
                {
                    source = new BindingSource();
                    source.DataSource = staff;

                    dgvStaffList.DataSource = null;
                    dgvStaffList.DataSource = source;
                }
                else
                {
                    MessageBox.Show("Staff not found", "Search Staff", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Search Staff", MessageBoxButtons.OK);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearchStaffID.Clear();
            txtSearchStaffName.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "Add")
            {

                dgvStaffList.ClearSelection();
                dgvStaffList.CurrentCell = null;

                txtStaffID.Clear();
                txtFullname.Clear();
                txtAddress.Clear();
                txtPhoneNumber.Clear();
                txtPassword.Clear();
                txtRoleID.Clear();
                txtStatusID.Clear();
                txtEmail.Clear();

                txtStaffID.DataBindings.Clear();
                txtFullname.DataBindings.Clear();
                txtAddress.DataBindings.Clear();
                txtPhoneNumber.DataBindings.Clear();
                txtPassword.DataBindings.Clear();
                txtRoleID.DataBindings.Clear();
                txtStatusID.DataBindings.Clear();
                txtEmail.DataBindings.Clear();

                txtStaffID.Enabled = true;
                txtStaffID.ReadOnly = false;
                txtFullname.ReadOnly = false;
                txtAddress.ReadOnly = false;
                txtPhoneNumber.ReadOnly = false;
                txtPassword.ReadOnly = false;
                txtRoleID.ReadOnly = false;
                txtStatusID.ReadOnly = false;
                txtEmail.ReadOnly = false;

                dgvStaffList.Enabled = false;

                btnAdd.Text = "Save";
                btnUpdate.Text = "Cancel";
            }
            else if(btnAdd.Text == "Save")
            {
                try
                {
                    TblStaff staff = new TblStaff
                    {
                        StaffId = txtStaffID.Text,
                        FullName = txtFullname.Text,
                        Address = txtAddress.Text,
                        PhoneNumber = txtPhoneNumber.Text,
                        Password = txtPassword.Text,
                        RoleId = txtRoleID.Text,
                        StatusId = txtStatusID.Text,
                        Email = txtEmail.Text,
                    };

                    if (txtStaffID.Enabled)
                    {
                        repoStaff.Add(staff);
                    }
                    else
                    {
                        repoStaff.Update(staff);
                    }

                    btnAdd.Text = "Add";
                    btnUpdate.Text = "Update";

                    txtStaffID.ReadOnly = true;
                    txtFullname.ReadOnly = true;
                    txtAddress.ReadOnly = true;
                    txtPhoneNumber.ReadOnly = true;
                    txtPassword.ReadOnly = true;
                    txtRoleID.ReadOnly = true;
                    txtStatusID.ReadOnly = true;
                    txtEmail.ReadOnly = true;

                    dgvStaffList.Enabled = true;

                    LoadStaffList();
                } catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Add new staff", MessageBoxButtons.OK);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(btnUpdate.Text == "Update")
            {
                txtStaffID.Enabled = false;
                txtFullname.ReadOnly = false;
                txtAddress.ReadOnly = false;
                txtPhoneNumber.ReadOnly = false;
                txtPassword.ReadOnly = false;
                txtRoleID.ReadOnly = false;
                txtStatusID.ReadOnly = false;
                txtEmail.ReadOnly = false;

                btnAdd.Text = "Save";
                btnUpdate.Text = "Cancel";
            }
            else if(btnUpdate.Text == "Cancel")
            {
                txtStaffID.Enabled = true;
                txtStaffID.ReadOnly = true;
                txtFullname.ReadOnly = true;
                txtAddress.ReadOnly = true;
                txtPhoneNumber.ReadOnly = true;
                txtPassword.ReadOnly = true;
                txtRoleID.ReadOnly = true;
                txtStatusID.ReadOnly = true;
                txtEmail.ReadOnly = true;

                dgvStaffList.Enabled = true;

                btnAdd.Text = "Add";
                btnUpdate.Text = "Update";

                LoadStaffList();
            }
        }
    }
}
