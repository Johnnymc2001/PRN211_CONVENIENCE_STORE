using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ProductDAO
    {
        // Singleton Pattern
        private static ProductDAO instance = null;
        private static readonly object instanceLock = new object();
        private ProductDAO() { }
        public static ProductDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductDAO();
                    }
                    return instance;
                }
            }
        }


        public List<TblProduct> GetAllProduct()
        {
            List<TblProduct> listProduct = new List<TblProduct>();
            try
            {
                using (var ctx = new prn211group4Context())
                {
                    listProduct = ctx.TblProducts.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listProduct;
        }

        public List<TblProduct> GetAllAvailableProduct()
        {
            List<TblProduct> listProduct = new List<TblProduct>();
            try
            {
                using (var ctx = new prn211group4Context())
                {
                    listProduct = ctx.TblProducts.ToList();
                    listProduct.Where(product => product.Status.Equals("Available")).ToList<TblProduct>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listProduct;
        }

        public List<TblProduct> GetAllProductByCategory(string productID)
        {
            List<TblProduct> listProduct = new List<TblProduct>();
            try
            {
                using (var ctx = new prn211group4Context())
                {
                    listProduct = ctx.TblProducts.ToList();
                    listProduct.Where(product => product.CategoryId.Equals(productID)).ToList<TblProduct>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listProduct;
        }

        public void AddProduct(TblProduct newProduct)
        {
            try
            {
                using (var ctx = new prn211group4Context())
                {
                    ctx.TblProducts.Add(newProduct);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateProduct(TblProduct product)
        {
            using (var ctx = new prn211group4Context())
            {
                ctx.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                ctx.SaveChanges();
            }
        }

    }
}
