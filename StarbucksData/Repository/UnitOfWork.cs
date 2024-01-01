using Microsoft.Extensions.Logging;
using StarbucksData.IRepository;
using StarbucksWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarbucksData.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            AdminRepo = new AdminRepository(db);
            CustomerRepo = new CustomerRepository(db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public async Task SaveAsync()
        {
           await _db.SaveChangesAsync();
        }

        public IAdminRepository AdminRepo { get; set; }


        public ICustomerRepository CustomerRepo { get; set; }
    }
}
