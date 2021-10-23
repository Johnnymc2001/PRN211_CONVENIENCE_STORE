using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class TblOrderDetail
    {
        public Guid OrderId { get; set; }
        public string ProductId { get; set; }
        public int? Quantity { get; set; }
        public double? TotalPrice { get; set; }

        public virtual TblOrder Order { get; set; }
        public virtual TblProduct Product { get; set; }
    }
}
