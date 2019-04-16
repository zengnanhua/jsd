﻿using AdminSystem.Domain.AggregatesModel.UserAggregate;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace AdminSystem.Application.Queries
{
    public class ApplicationUserQuery: IApplicationUserQuery
    {
        private string _connectionString = string.Empty;
        public ApplicationUserQuery(string constr)
        {
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
        }
        public async Task<ApplicationUserViewModel> GetUserAsync(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var result= (await connection.QueryAsync<ApplicationUserViewModel>("select * from ApplicationUsers  WHERE Id=@id", new { id })).FirstOrDefault();
                return result;
            }
            
        }
        /// <summary>
        /// 获取极速达订单状态
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetJsdOrderOutput>> GetJsdOrderListAsync(string oprDate)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var result = (await connection.QueryAsync<GetJsdOrderOutput>("select  * from jsdorders a where a.OprDate=@oprDate",new { oprDate })).ToList();
                return result;
            }
        }
    }
}
