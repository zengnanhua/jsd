using AdminSystem.Domain.AggregatesModel.UserAggregate;
using AdminSystem.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
namespace AdminSystem.Infrastructure.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public ApplicationUserRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration;
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

        public async Task<ApplicationUser> GetUserByMobileAsync(string mobile)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(c=>c.Phone==mobile);

            return user;
        }

        public async Task<bool> UserDeleteAllAsync()
        {
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("MysqlConnection")))
            {
                //truncate applicationusers
                await connection.ExecuteAsync("delete from applicationusers");
            }
            //var list =await _context.ApplicationUsers.ToListAsync();
            //if (list != null && list.Count > 0)
            //{
            //    foreach (var temp in list)
            //    {
            //        _context.ApplicationUsers.Remove(temp);
            //    }
            //}
            return true;
        }
    }
}
