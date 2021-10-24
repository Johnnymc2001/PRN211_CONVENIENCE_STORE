using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IProductRepository
    {
        List<TblProduct> GetAllProduct();

        List<TblProduct> GetAllAvailableProduct();

        List<TblProduct> GetAllProductByCategory(string categoryID);

        void Add(TblProduct newProduct);

        void Update(TblProduct product);

    }
}
