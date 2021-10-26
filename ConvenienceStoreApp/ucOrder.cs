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

        Guid orderId;
        List<TblOrderDetail> orderDetails = new List<TblOrderDetail>();
        List<TblProduct> products = new List<TblProduct>();


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
            double? total = 0;
            orderDetails.ForEach(od => total += od.TotalPrice);
            lblTotalPrice.Text = total.ToString();

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

        //-----------------------------------------------------------------------------------------------------
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

        private void Order_Load(object sender, EventArgs e)
        {
            RefreshProductDatabase();
            // Generate New Order ID On Load
            orderId = Guid.NewGuid();

            // Generate Guid until it unique
            while (null != orderRepository.GetByID(orderId))
            {
                orderId = Guid.NewGuid();
            }

            lblOrderId.Text = $"Order Id: {orderId.ToString()}";

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
                        if (newQuantity > product.Quantity)
                        {
                            MessageBox.Show($"There isn't enough product to be added!{Environment.NewLine}[{product.ProductName}'s left : {product.Quantity}]", "Order - Add Product", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            newOd.Quantity = newQuantity;
                            newOd.TotalPrice = product.Price * newQuantity;
                            orderDetails.Add(newOd);
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
                RefreshOrderLocal();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            TblOrderDetail orderDetail = GetCurrentOrderDetail();
            try
            {
                if (null != orderDetail)
                {
                    orderDetails.Remove(orderDetail);
                    RefreshOrderLocal();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Order - Remove Product", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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
                        RefreshOrderLocal();
                    }
                    else
                    {
                        orderDetail.Quantity = newQuantity;
                        orderDetail.TotalPrice = product.Price * newQuantity;
                        RefreshOrderLocal();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Order - Add 1 Product", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
