using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarbucksData.IRepository
{
    public interface IUnitOfWork
    {
        void Save();

        Task SaveAsync();

        IAdminRepository AdminRepo { get; set; }

        ICustomerRepository CustomerRepo { get; set; }
    }
}
