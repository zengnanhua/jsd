using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Application.Queries
{
    public interface IRmsDataBaseQuery
    {
        Task<List<zmd_ac_usersTab>> GetZmd_Ac_UsersAsync();
        Task<string> GetZmd_Base_ConfigyValueByKeyCacheAsync(string key);
        Task<List<Zmd_Base_ConfigTab>> GetZmd_Base_ConfigAsync();
    }
}
