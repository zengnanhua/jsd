using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Application.Queries
{
    public interface IApplicationUserQuery
    {
        Task<ApplicationUserViewModel> GetUserAsync(int id);
        /// <summary>
        /// 获取极速达订单状态
        /// </summary>
        /// <returns></returns>
        Task<List<GetJsdOrderOutput>> GetJsdOrderListAsync(string oprDate);

        Task<ResultData<List<GetJsdOrderListPageAsyncDtoResult>>> GetJsdOrderListPageAsync(GetJsdOrderListPageAsyncDtoInput param, string deptCode);
    }
}
