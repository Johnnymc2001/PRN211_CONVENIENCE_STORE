using BusinessObject.Models;
using DataAccess.Repository;
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
    public partial class ucSelfOrdersView : UserControl
    {

        IOrderRepository orderRepository = new OrderRepository();
        IOrderDetailRepository orderDetailRepository = new OrderDetailRepository();
        IProductRepository productRepository = new ProductRepository();
        IStaffRepository staffRepository = new StaffRepository();

        public TblStaff loggedStaff { get; set; }
        public List<TblOrder> orders;
        // -----------------------------------------------------------------------------------------------------------

        private void RefreshOrderDatabase()
        {
            orders = orderRepository.GetAll();
            RefreshOrderLocal();
        }

        private void RefreshOrderLocal()
        {
            orders = orders.FindAll(o => o.StaffId == loggedStaff.StaffId);
            UpdateGridViewOrder(orders);
        }

        private void UpdateGridViewOrder(List<TblOrder> orders)
        {
            dgvOrder.DataSource = null;
            dgvOrder.DataSource = orders;
        }

        // -----------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------
        public ucSelfOrdersView()
        {
            InitializeComponent();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshOrderDatabase();
        }

        private void ucSelfOrdersView_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                loggedStaff = staffRepository.GetStaffByID("cfec1bf2-1f3a-45dc-9473-478d5fb13006");
                Trace.WriteLine(loggedStaff.StaffId);
            }
        }
    }
}
