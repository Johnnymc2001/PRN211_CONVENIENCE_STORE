using BusinessObject.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvenienceStoreApp
{
    public partial class frmLogin : Form
    {
        IStaffRepository StaffRepository = new StaffRepository();
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            
            if (null!=StaffRepository.Login(txtUsername.Text, txtPassword.Text))
            {
                TblStaff CurrentAccount = StaffRepository.GetCurrentAccount();
                Trace.WriteLine(CurrentAccount.ToString());
                frmMain frmMain = new frmMain
                {
                    frmLogin = this,
                    CurrentAccount = CurrentAccount
                };
                this.Hide();
                frmMain.Show();
            }
            else
            {
                MessageBox.Show("Email or Password not exist!");
            }
        }

        private void btnClose_Click(object sender, EventArgs e) => Application.Exit();
    }
}
