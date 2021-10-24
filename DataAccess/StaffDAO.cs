using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace DataAccess
{
    class StaffDAO
    {
        //// THIS IS SINGLETON PATTERN ////

        private static StaffDAO instance = null;
        private static readonly object instanceLock = new object();
        private StaffDAO() { }
        public static StaffDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new StaffDAO();
                    }
                    return instance;
                }
            }
        }

        // THIS IS GET ALL STAFF

        public List<TblStaff> GetAll()
        {
            List<TblStaff> staffs = new List<TblStaff>();
            using (var ctx = new prn211group4Context())
            {
                staffs = ctx.TblStaffs.ToList();
            }
            return staffs;
        }

        // THIS IS GET STAFF BY ID

        public TblStaff GetByID(Guid staffId)
        {
            TblStaff staff = null;
            using (var ctx = new prn211group4Context())
            {
                staff = ctx.TblStaffs.SingleOrDefault(staff => staff.StaffId.Equals(staffId));
            }
            return staff;
        }

        // THIS IS ADD NEW STAFF

        public void Add(TblStaff staff)
        {
            using (var ctx = new prn211group4Context())
            {
                ctx.TblStaffs.Add(staff);
                ctx.SaveChanges();
            }
        }

        // THIS IS DELETE STAFF

        public void Delete(TblStaff staff)
        {
            using (var ctx = new prn211group4Context())
            {
                ctx.TblStaffs.Remove(staff);
                ctx.SaveChanges();
            }
        }

        // THIS IS UPDATE STAFF

        public void Update(TblStaff staff)
        {
            using (var ctx = new prn211group4Context())
            {
                ctx.Entry(staff).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        // THIS IS GET CURRENT LOGIN ACCOUNT

        private TblStaff CurrentAccount { get; set; }
        public TblStaff GetCurrentAccount() => CurrentAccount;

        public bool Login(string Email, string Password)
        {
            TblStaff staff = null;
            using (var ctx = new prn211group4Context())
            {
                staff = ctx.TblStaffs.SingleOrDefault(staff => staff.Email.Equals(Email) && staff.Password.Equals(Password));
            }
            if (staff != null)
            {
                CurrentAccount = staff;
                return true;
            }
            return false;
        }
    }
}
