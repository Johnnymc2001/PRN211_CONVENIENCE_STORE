using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class TblCategory
    {
        public TblCategory()
        {
            TblProducts = new HashSet<TblProduct>();
        }

        public string CategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<TblProduct> TblProducts { get; set; }
    }
}
