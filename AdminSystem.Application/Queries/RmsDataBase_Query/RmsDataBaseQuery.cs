using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace AdminSystem.Application.Queries
{
    public class RmsDataBaseQuery : IRmsDataBaseQuery
    {
        private IConfiguration _configuration;
        private IMemoryCache _memoryCache;
        string oracleConnection = string.Empty;
        public RmsDataBaseQuery(IConfiguration configuration, IMemoryCache memoryCache)
        {
            this._configuration = configuration;
            this._memoryCache = memoryCache;
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
           var resultList= _memoryCache.GetOrCreate<List<Zmd_Base_ConfigTab>>("zmd_base_config_cache", c =>
            {
                var list =  GetZmd_Base_Config();
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

            throw new Exception("Zmd_Base_ConfigTab中没有配置key");
        }

    }
}
