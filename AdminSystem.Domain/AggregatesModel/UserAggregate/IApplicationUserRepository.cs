using AdminSystem.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Domain.AggregatesModel.UserAggregate
{
    public interface IApplicationUserRepository:IRepository<ApplicationUser>
    {
        bool AddUser(ApplicationUser user);
        Task<bool> UserDeleteAllAsync();
    }
}
