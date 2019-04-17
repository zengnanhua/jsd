using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using AdminSystem.Domain.AggregatesModel.AttributeConfigAggregate;

namespace AdminSystem.Application.Queries
{
    public class RmsDataBaseQuery : IRmsDataBaseQuery
    {
        private IConfiguration _configuration;
        private IMemoryCache _memoryCache;
        private IAttributeConfigRepository _attributeConfigRepository;
        string oracleConnection = string.Empty;
        public RmsDataBaseQuery(IConfiguration configuration, IMemoryCache memoryCache, IAttributeConfigRepository attributeConfigRepository)
        {
            this._configuration = configuration;
            this._memoryCache = memoryCache;
            this._attributeConfigRepository = attributeConfigRepository;
            oracleConnection = _configuration.GetConnectionString("ZmdOracle");
        }
        /// <summary>
        /// 获取rms所有用户
        /// </summary>
        /// <returns></returns>
        public async Task<List<zmd_ac_usersTab>> GetZmd_Ac_UsersAsync()
        {
            using (OracleConnection conn = new OracleConnection(oracleConnection))
            {
                var list = (await conn.QueryAsync<zmd_ac_usersTab>("select a.userid,a.truename,a.phone,a.deptcode from zmd_ac_users a where a.enabled='1' and a.phone is not null")).ToList();
                return list;
            }
        }
        /// <summary>
        /// 用户rms 所有配置
        /// </summary>
        /// <returns></returns>
        public async Task<List<Zmd_Base_ConfigTab>> GetZmd_Base_ConfigAsync()
        {
            using (OracleConnection conn = new OracleConnection(oracleConnection))
            {
                var list = (await conn.QueryAsync<Zmd_Base_ConfigTab>("select * from zmd_base_config")).ToList();
                return list;
            }
        }
        private List<Zmd_Base_ConfigTab> GetZmd_Base_Config()
        {
            using (OracleConnection conn = new OracleConnection(oracleConnection))
            {
                var list = (conn.Query<Zmd_Base_ConfigTab>("select a.configvalue,a.configtext,a.configdesc,a.enabled from zmd_base_config a where a.enabled='1'")).ToList();
                return list;
            }
        }
        public async Task<string> GetZmd_Base_ConfigyValueByKeyCacheAsync(string key)
        {
            
           //从缓存zhogn
           var resultList= _memoryCache.GetOrCreate<List<AttributeConfig>>("AttributeConfigCache", c =>
            {
                var list =  _attributeConfigRepository.GetAttributeConfigList();
                return list;
            });

            if (resultList != null)
            {
                var entity = resultList.FirstOrDefault(c => c.ConfigValue == key);
                if (entity != null)
                {
                    return entity.ConfigText;
                }
                
            }

            throw new Exception("AttributeConfigCache中没有同步key");
        }


        public async Task<List<zmd_oms_head>> GetZmd_oms_headAsync(string oprdate)
        {
            try
            {
                string sql = "  select a.OrderCode,a.DeptCode,a.OprDate,a.Status from  zmd_oms_header a where a.oprdate=:vOprDate";
                DynamicParameters param = new DynamicParameters();
                param.Add(":vOprDate", oprdate);
                using (OracleConnection conn = new OracleConnection(oracleConnection))
                {
                    var list = (await conn.QueryAsync<zmd_oms_head>(sql, param)).ToList();
                    if (list == null)
                    {
                        list = new List<zmd_oms_head>();
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                return new List<zmd_oms_head>();
            }
        }
    }
}
