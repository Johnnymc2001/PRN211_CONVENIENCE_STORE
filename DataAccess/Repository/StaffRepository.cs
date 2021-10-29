using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class StaffRepository : IStaffRepository
    {
        public void Add(TblStaff staff) => StaffDAO.Instance.Add(staff);

        public void Delete(TblStaff staff) => StaffDAO.Instance.Delete(staff);

        public List<TblStaff> GetAllStaff() => StaffDAO.Instance.GetAll();

        public TblStaff GetStaffByID(string staffId) => StaffDAO.Instance.GetByID(staffId);

        public void Update(TblStaff staff) => StaffDAO.Instance.Update(staff);

        public TblStaff Login(string Email, string Password) => StaffDAO.Instance.Login(Email, Password);

        public TblStaff GetCurrentAccount() => StaffDAO.Instance.GetCurrentAccount();

        public TblStaff GetByName(string staffName) => StaffDAO.Instance.GetByName(staffName);
    }
}
