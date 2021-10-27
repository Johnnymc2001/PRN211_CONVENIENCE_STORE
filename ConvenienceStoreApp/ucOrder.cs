using BusinessObject.Models;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace ConvenienceStoreApp
{
    public partial class ucOrder : UserControl
    {
        class DataGridViewOrderDetailObject
        {
            public string ProductId { get; set; }
            public string ProductName { get; set; }
            public int? Quantity { get; set; }
        }

        class DataGridViewProductObject
        {
            public string ProductId { get; set; }
            public string ProductName { get; set; }
            public int? Quantity { get; set; }
            public double? Price { get; set; }
        }

        IOrderRepository orderRepository = new OrderRepository();
        IOrderDetailRepository orderDetailRepository = new OrderDetailRepository();
        IProductRepository productRepository = new ProductRepository();
        IStaffRepository staffRepository = new StaffRepository();

        Guid orderId;
        List<TblOrderDetail> orderDetails = new List<TblOrderDetail>();
        List<TblProduct> products = new List<TblProduct>();

        public TblStaff loggedStaff { get; set; }
        //Dictionary<TblProduct, Int32> orderList = new Dictionary<TblProduct, Int32>();

        public ucOrder()
        {
            InitializeComponent();
        }

        //----------------------------------Product---------------------------------------------------
        private void RefreshProductDatabase()
        {
            products = productRepository.GetAllProduct();
            RefreshProductLocal();
        }

        private void RefreshProductLocal()
        {
            UpdateGridViewProducts(products);
        }

        private void SearchProducts(string productName)
        {
            List<TblProduct> searchResult = new List<TblProduct>();
            searchResult = products.FindAll(prod => prod.ProductName.Contains(productName));
            UpdateGridViewProducts(searchResult);
        }

        private void UpdateGridViewProducts(List<TblProduct> products)
        {
            List<DataGridViewProductObject> listObj = products
                .Select(o => new DataGridViewProductObject()
                {
                    ProductId = o.ProductId,
                    ProductName = o.ProductName,
                    Quantity = o.Quantity,
                    Price = o.Price,
                }).ToList();
            dgvProducts.DataSource = null;
            dgvProducts.DataSource = listObj;
        }

        //----------------------------------OrderDetail---------------------------------------------------
        private void RefreshOrderLocal()
        {
            double total = CalculateTotal();

            UpdateGridViewOrder(orderDetails);

        }

        private void UpdateGridViewOrder(List<TblOrderDetail> orderDetails)
        {
            List<DataGridViewOrderDetailObject> listObj = orderDetails
                .Select(o => new DataGridViewOrderDetailObject()
                {
                    ProductId = o.ProductId,
                    ProductName = products.Single(pr => pr.ProductId == o.ProductId).ProductName,
                    Quantity = o.Quantity,
                }).ToList();
            dgvOrderDetails.DataSource = null;
            dgvOrderDetails.DataSource = listObj;
        }

        //-----------------------------------Load and Miscs-----------------------------------------------------------
        private double CalculateTotal()
        {
            double total = 0;
            orderDetails.ForEach(od => total += (double)od.TotalPrice);
            lblTotalPrice.Text = total.ToString();
            return total;
        }

        private void GenerateNewOrder()
        {
            // Clean out orderDetails
            orderDetails.Clear();
            RefreshOrderLocal();
            txtCustomerName.Text = "";

            // Refresh Product Lists
            RefreshProductDatabase();

            // Get a new Guid
            orderId = Guid.NewGuid();

            // Generate Guid until it unique
            while (null != orderRepository.GetByID(orderId))
            {
                orderId = Guid.NewGuid();
            }

            lblOrderId.Text = $"Order Id: {orderId.ToString()}";
            rtbAction.Text = $"New Order";
        }

        private void ChangeButtonState()
        {
            // Order Detail Button
            if (orderDetails.Count == 0)
            {
                btnPlus.Enabled = false;
                btnMinus.Enabled = false;
                btnRemove.Enabled = false;
                btnCheckout.Enabled = false;
            } else
            {
                btnPlus.Enabled = true;
                btnMinus.Enabled = true;
                btnRemove.Enabled = true;
                btnCheckout.Enabled = true;
            }
        }

        private void Order_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                GenerateNewOrder();
                ChangeButtonState();
                // Get ME :D
                loggedStaff = staffRepository.GetStaffByID(Guid.Parse("cfec1bf2-1f3a-45dc-9473-478d5fb13006"));
            }
        }

        //----------------------------------------Normal Event----------------------------------------------------

        private TblProduct GetCurrentProduct()
        {
            TblProduct product = null;
            try
            {
                Object obj = dgvProducts.CurrentRow.DataBoundItem;
                if (null != obj)
                {
                    DataGridViewProductObject dataObj = (DataGridViewProductObject)obj;
                    product = products.SingleOrDefault(p => p.ProductId.Equals(dataObj.ProductId));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Missing Data", "Order - Get Product", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return product;
        }

        private TblOrderDetail GetCurrentOrderDetail()
        {
            TblOrderDetail orderDetail = null;
            try
            {
                Object obj = dgvOrderDetails.CurrentRow.DataBoundItem;
                if (null != obj)
                {
                    DataGridViewOrderDetailObject dataObj = (DataGridViewOrderDetailObject)obj;
                    orderDetail = orderDetails.SingleOrDefault(o => o.ProductId.Equals(dataObj.ProductId));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Missing Data", "Order - Get Order", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return orderDetail;
        }

        //-----------------------------------------------------------------------------------------------------
        private void TryAddProduct()
        {
            TblProduct product = GetCurrentProduct();
            int quantity = (int)txtQuantity.Value;

            try
            {
                // Get Product Successfully
                if (product != null)
                {
                    TblOrderDetail od = orderDetails.SingleOrDefault(p => p.ProductId == product.ProductId);
                    //TblOrderDetail od = orderList.GetValueOrDefault(product);
                    // Order doesn't contains product
                    if (od == null)
                    {
                        TblOrderDetail newOd = new()
                        {
                            OrderId = orderId,
                            ProductId = product.ProductId,
                            Quantity = 0,
                            TotalPrice = product.Price,
                        };

                        int newQuantity = (int)(newOd.Quantity + quantity);
                        // Check Quantity
                        if (newQuantity > product.Quantity)
                        {
                            MessageBox.Show($"There isn't enough product to be added!{Environment.NewLine}[{product.ProductName}'s left : {product.Quantity}]", "Order - Add Product", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            newOd.Quantity = newQuantity;
                            newOd.TotalPrice = product.Price * newQuantity;
                            orderDetails.Add(newOd);
                            rtbAction.Text = $"Added {quantity} {product.ProductName}";
                        }
                    }
                    // Order contains product
                    else
                    {
                        int newQuantity = (int)(od.Quantity + quantity);

                        // New quanity exceed product's quantity
                        if (newQuantity > product.Quantity)
                        {
                            MessageBox.Show($"There isn't enough product to be added!{Environment.NewLine}[{product.ProductName}'s left : {product.Quantity}]", "Order - Add Product", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        // Quantity enough
                        else
                        {
                            od.Quantity = newQuantity;
                            od.TotalPrice = product.Price * newQuantity;
                            rtbAction.Text = $"Added {quantity} {product.ProductName}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Order - Add Product", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                txtQuantity.Value = 1;
                RefreshOrderLocal();
                ChangeButtonState();
            }
            
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshProductDatabase();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            String searchValue = txtSearch.Text;
            if (null != searchValue)
            {
                SearchProducts(searchValue);
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TryAddProduct();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            TblOrderDetail orderDetail = GetCurrentOrderDetail();
            try
            {
                if (null != orderDetail)
                {
                    orderDetails.Remove(orderDetail);
                    TblProduct product = products.SingleOrDefault(p => p.ProductId.Equals(orderDetail.ProductId));
                    rtbAction.Text = $"Removed {product.ProductName}";
                    RefreshOrderLocal();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Order - Remove Product", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ChangeButtonState();
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            TblOrderDetail orderDetail = GetCurrentOrderDetail();
            try
            {
                if (null != orderDetail)
                {
                    TblProduct product = products.SingleOrDefault(p => p.ProductId.Equals(orderDetail.ProductId));
                    if ((orderDetail.Quantity + 1) <= product.Quantity)
                    {
                        int? newQuantity = orderDetail.Quantity + 1;
                        orderDetail.Quantity = newQuantity;
                        orderDetail.TotalPrice = product.Price * newQuantity;
                        rtbAction.Text = $"Added 1 {product.ProductName}";
                        RefreshOrderLocal();
                    }
                    else
                    {
                        {
                            MessageBox.Show($"There isn't enough product to be added!{Environment.NewLine}[{product.ProductName}'s left : {product.Quantity}]", "Order - Add 1 Product", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Order - Add 1 Product", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ChangeButtonState();
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            TblOrderDetail orderDetail = GetCurrentOrderDetail();
            try
            {
                if (null != orderDetail)
                {
                    TblProduct product = products.SingleOrDefault(p => p.ProductId.Equals(orderDetail.ProductId));
                    int? newQuantity = orderDetail.Quantity - 1;
                    if (newQuantity == 0)
                    {
                        orderDetails.Remove(orderDetail);
                        rtbAction.Text = $"Removed {product.ProductName}";
                        RefreshOrderLocal();
                    }
                    else
                    {
                        orderDetail.Quantity = newQuantity;
                        orderDetail.TotalPrice = product.Price * newQuantity;
                        rtbAction.Text = $"Removed 1 {product.ProductName}";
                        RefreshOrderLocal();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Order - Add 1 Product", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ChangeButtonState();
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (orderDetails.Count > 0)
            {
                
                TblOrder checkoutOrder = new()
                {
                    OrderId = orderId,
                    //StaffId = loggedStaff.StaffId,
                    CustomerName = txtCustomerName.Text,
                    Date = DateTime.Now,
                    OrderPrice = CalculateTotal(),
                    StatusId = "CHECKED_OUT",
                };

                orderRepository.Add(checkoutOrder);

                // Just make sure products is up to date
                RefreshProductDatabase();

                foreach (TblOrderDetail od in orderDetails)
                {
                    orderDetailRepository.Add(od);
              
                    // Get Product out and ready to update
                    TblProduct product = products.Single(p => p.ProductId == od.ProductId);
                    product.Quantity -= od.Quantity;
                    productRepository.Update(product);
                }

                ucBill uch = new ucBill()
                {
                    order = checkoutOrder,
                    orderDetails = orderDetails,
                    loggedStaff = loggedStaff,
                    customerName = txtCustomerName.Text,
                    products = products,
                };

                uch.Show();
                SystemSounds.Beep.Play();
                GenerateNewOrder();
            }
            else
            {
                MessageBox.Show($"You must first add item before checkout!", "Order - Checkout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ChangeButtonState();
        }

        private void dgvProducts_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            TryAddProduct();
        }
    }
}
