using AdminSystem.Domain.AggregatesModel.UserAggregate;
using AdminSystem.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context;
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public ApplicationUserRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public bool AddUser(ApplicationUser user)
        {
            if (user.IsTransient())
            {
                _context.ApplicationUsers
                   .Add(user);  
            }
            return true;
        }

        public async Task<bool> UserDeleteAllAsync()
        {
            var list =await _context.ApplicationUsers.ToListAsync();
            if (list != null && list.Count > 0)
            {
                foreach (var temp in list)
                {
                    _context.ApplicationUsers.Remove(temp);
                }
            }
            return true;
        }
    }
}
