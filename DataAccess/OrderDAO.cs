using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    class OrderDAO
    {
        // ----------------------------------------------- Singleton Pattern -----------------------------------------------

        private static OrderDAO instance = null;
        private static readonly object instanceLock = new object();
        private OrderDAO() { }
        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                    return instance;
                }
            }
        }


        // -----------------------------------------------------------

        public List<TblOrder> GetAll()
        {
            List<TblOrder> orders = new List<TblOrder>();
            using (var ctx = new prn211group4Context())
            {
                orders = ctx.TblOrders.ToList();
            }
            return orders;
        }

        public TblOrder GetUsingID(Guid orderId)
        {
            TblOrder order = null;
            using (var ctx = new prn211group4Context())
            {
                order = ctx.TblOrders.SingleOrDefault(order => order.OrderId.Equals(orderId));
            }
            return order;
        }

        public void Add(TblOrder order)
        {
            using (var ctx = new prn211group4Context())
            {
                ctx.TblOrders.Add(order);
                ctx.SaveChanges();
            }
        }

        public void Delete(TblOrder order)
        {
            using (var ctx = new prn211group4Context())
            {
                ctx.TblOrders.Remove(order);
                ctx.SaveChanges();
            }
        }


        public void Update(TblOrder order)
        {
            using (var ctx = new prn211group4Context())
            {
                ctx.Entry(order).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                ctx.SaveChanges();
            }
        }
    }
}
