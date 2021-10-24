﻿using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    class StaffRepository : IStaffRepository
    {
        public void Add(TblStaff staff) => StaffDAO.Instance.Add(staff);

        public void Delete(TblStaff staff) => StaffDAO.Instance.Delete(staff);

        public List<TblStaff> GetAllStaff() => StaffDAO.Instance.GetAll();

        public TblStaff GetStaffByID(Guid staffId) => StaffDAO.Instance.GetByID(staffId);

        public void Update(TblStaff staff) => StaffDAO.Instance.Update(staff);

        public bool Login(string Email, string Password) => StaffDAO.Instance.Login(Email, Password);

        public TblStaff GetCurrentAccount() => StaffDAO.Instance.GetCurrentAccount();
    }
}
