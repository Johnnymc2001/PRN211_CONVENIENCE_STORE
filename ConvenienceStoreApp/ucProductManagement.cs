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
    public partial class ucProductManagement : UserControl
    {
        public ucProductManagement()
        {
            InitializeComponent();
        }

        IProductRepository repoProduct = new ProductRepository();
        BindingSource source;

        private void ucProductManagement_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                LoadAllProductList();
                cboSelect.SelectedIndex = 0;

                txtProductID.Enabled = false;
                txtProductID.ReadOnly = true;
                txtProductName.ReadOnly = true;
                txtPrice.ReadOnly = true;
                txtQuantity.ReadOnly = true;
                txtCategoryID.ReadOnly = true;
                txtStatusID.ReadOnly = true;
            }
        }
        public void LoadAllProductList()
        {
            List<TblProduct> products = null;
            try
            {
                products = repoProduct.GetAllProduct();

                source = new BindingSource();
                source.DataSource = products;

                txtProductID.DataBindings.Clear();
                txtProductName.DataBindings.Clear();
                txtPrice.DataBindings.Clear();
                txtQuantity.DataBindings.Clear();
                txtCategoryID.DataBindings.Clear();
                txtStatusID.DataBindings.Clear();

                txtProductID.DataBindings.Add("Text", source, "ProductId");
                txtProductName.DataBindings.Add("Text", source, "ProductName");
                txtPrice.DataBindings.Add("Text", source, "Price");
                txtQuantity.DataBindings.Add("Text", source, "Quantity");
                txtCategoryID.DataBindings.Add("Text", source, "CategoryId");
                txtStatusID.DataBindings.Add("Text", source, "StatusId");


                dgvProductList.DataSource = null;
                dgvProductList.DataSource = source;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load Staff List", MessageBoxButtons.OK);
            }

        }

        public void LoadAllAvailableProduct()
        {
            List<TblProduct> products = null;
            try
            {
                products = repoProduct.GetAllAvailableProduct();

                source = new BindingSource();
                source.DataSource = products;

                txtProductID.DataBindings.Clear();
                txtProductName.DataBindings.Clear();
                txtPrice.DataBindings.Clear();
                txtQuantity.DataBindings.Clear();
                txtCategoryID.DataBindings.Clear();
                txtStatusID.DataBindings.Clear();

                txtProductID.DataBindings.Add("Text", source, "ProductId");
                txtProductName.DataBindings.Add("Text", source, "ProductName");
                txtPrice.DataBindings.Add("Text", source, "Price");
                txtQuantity.DataBindings.Add("Text", source, "Quantity");
                txtCategoryID.DataBindings.Add("Text", source, "CategoryId");
                txtStatusID.DataBindings.Add("Text", source, "StatusId");

                dgvProductList.DataSource = null;
                dgvProductList.DataSource = source;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load Staff List", MessageBoxButtons.OK);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearchValue.Clear();
            LoadAllProductList();
        }

        private void btnAvailable_Click(object sender, EventArgs e)
        {
            txtSearchValue.Clear();
            LoadAllAvailableProduct();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try {
                List<TblProduct> products = new List<TblProduct>();
                TblProduct product = null;

                if (!cboSelect.SelectedItem.ToString().Equals("") && !txtSearchValue.Text.Trim().Equals(""))
                {
                    if (cboSelect.SelectedItem.ToString().Equals("Product ID"))
                    {
                        product = repoProduct.GetProductByID(txtSearchValue.Text);
                        source = new BindingSource();
                        source.DataSource = product;
                    }
                    else if (cboSelect.SelectedItem.ToString().Equals("Product Name"))
                    {
                        products = repoProduct.GetAllProductByName(txtSearchValue.Text);
                        source = new BindingSource();
                        source.DataSource = products;
                    }
                    else if (cboSelect.SelectedItem.ToString().Equals("Category ID"))
                    {
                        products = repoProduct.GetAllProductByCategory(txtSearchValue.Text);
                        source = new BindingSource();
                        source.DataSource = products;
                    }
                    if (product != null || products != null)
                    {
                        dgvProductList.DataSource = null;
                        dgvProductList.DataSource = source;
                    }
                    else
                    {
                        MessageBox.Show("Product not found", "Search", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("Blank", "Search", MessageBoxButtons.OK);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Search Product", MessageBoxButtons.OK);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text.Equals("Add"))
            {
                btnAdd.Text = "Save";
                btnUpdate.Text = "Cancel";

                txtProductID.Clear();
                txtProductName.Clear();
                txtPrice.Clear();
                txtQuantity.Clear();
                txtCategoryID.Clear();
                txtStatusID.Clear();

                txtProductID.DataBindings.Clear();
                txtProductName.DataBindings.Clear();
                txtPrice.DataBindings.Clear();
                txtQuantity.DataBindings.Clear();
                txtCategoryID.DataBindings.Clear();
                txtStatusID.DataBindings.Clear();

                txtProductID.Enabled = true;
                txtProductID.ReadOnly = false;
                txtProductName.ReadOnly = false;
                txtPrice.ReadOnly = false;
                txtQuantity.ReadOnly = false;
                txtCategoryID.ReadOnly = false;
                txtStatusID.ReadOnly = false;

                dgvProductList.Enabled = false;
                dgvProductList.ClearSelection();
                dgvProductList.CurrentCell = null;

            }
            else if (btnAdd.Text.Equals("Save"))
            {
                try
                {
                    TblProduct product = new TblProduct
                    {
                        ProductId = txtProductID.Text,
                        ProductName = txtProductName.Text,
                        Price = double.Parse(txtPrice.Text),
                        Quantity = int.Parse(txtQuantity.Text),
                        CategoryId = txtCategoryID.Text,
                        StatusId = txtStatusID.Text
                    };
                    if (txtProductID.Enabled)
                    {
                        repoProduct.Add(product);
                    }
                    else
                    {
                        repoProduct.Update(product);
                    }

                    txtProductID.Enabled = false;
                    txtProductID.ReadOnly = true;
                    txtProductName.ReadOnly = true;
                    txtPrice.ReadOnly = true;
                    txtQuantity.ReadOnly = true;
                    txtCategoryID.ReadOnly = true;
                    txtStatusID.ReadOnly = true;

                    dgvProductList.Enabled = true;
                    LoadAllProductList();

                    btnAdd.Text = "Add";
                    btnUpdate.Text = "Update";
                } catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Add product", MessageBoxButtons.OK);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnUpdate.Text.Equals("Update"))
            {
                txtProductID.Enabled = false;
                txtProductID.ReadOnly = true;
                txtProductName.ReadOnly = false;
                txtPrice.ReadOnly = false;
                txtQuantity.ReadOnly = false;
                txtCategoryID.ReadOnly = false;
                txtStatusID.ReadOnly = false;

                btnAdd.Text = "Save";
                btnUpdate.Text = "Cancel";
            }
            else if(btnUpdate.Text.Equals("Cancel"))
            {
                txtProductID.Enabled = false;
                txtProductID.ReadOnly = true;
                txtProductName.ReadOnly = true;
                txtPrice.ReadOnly = true;
                txtQuantity.ReadOnly = true;
                txtCategoryID.ReadOnly = true;
                txtStatusID.ReadOnly = true;

                btnAdd.Text = "Add";
                btnUpdate.Text = "Update";

                dgvProductList.Enabled = true;
                LoadAllProductList();
            }
        }
    }
}
