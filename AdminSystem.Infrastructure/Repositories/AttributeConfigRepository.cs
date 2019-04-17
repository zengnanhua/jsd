using AdminSystem.Domain.AggregatesModel.AttributeConfigAggregate;
using AdminSystem.Domain.SeedWork;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Repositories
{
    public class AttributeConfigRepository : IAttributeConfigRepository
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

        public AttributeConfigRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            this._configuration = configuration;
        }
        public async Task<bool> AttributeConfigAddAsync(AttributeConfig config)
        {
            if (config.IsTransient())
            {
                await  _context.AttributeConfigs
                   .AddAsync(config);
            }
            return true;
        }
        public async Task<bool> AttributeConfigDeleteAllAsync()
        {
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("MysqlConnection")))
            {
                await connection.ExecuteAsync("delete from attributeconfigs");
            }
            //var list= await _context.AttributeConfigs.ToListAsync();
            //if (list != null && list.Count > 0)
            //{
            //    foreach (var temp in list)
            //    {
            //        _context.AttributeConfigs.Remove(temp);
            //    }
            //}
            return true;
        }

        public async Task<List<AttributeConfig>> GetAttributeConfigListAsync()
        {
            return await _context.AttributeConfigs.ToListAsync();
        }
        public  List<AttributeConfig> GetAttributeConfigList()
        {
            return  _context.AttributeConfigs.ToList();
        }
    }
}
