using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Models;

namespace DataAccess
{
    public interface IStaffRepository
    {
        List<TblStaff> GetAllStaff();

        TblStaff GetStaffByID(Guid staffId);

        void Add(TblStaff staff);

        void Delete(TblStaff staff);

        void Update(TblStaff staff);

        bool Login(string Email, string Password);

        TblStaff GetCurrentAccount();
    }
}
