using AdminSystem.Domain.AggregatesModel.UserAggregate;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using AdminSystem.Infrastructure.Common;

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

        public async Task<ResultData<List<GetJsdOrderListPageAsyncDtoResult>>> GetJsdOrderListPageAsync(GetJsdOrderListPageAsyncDtoInput param)
        {
            int page = !string.IsNullOrEmpty(param.Page) ? Convert.ToInt32(param.Page) : 1;
            int pageSize= !string.IsNullOrEmpty(param.PageSize) ? Convert.ToInt32(param.PageSize) : 10;
            string sql = $@"select b.*,a.* from jsdorders a inner join jsdorderitems b
                            on a.Id=b.JsdOrderId 
                            where EXISTS(
                             SELECT id from(
		                            select z.id from jsdorders z  order by z.oprdate desc  limit {(page-1)*pageSize},{pageSize}
	                            ) zz  where zz.id =a.Id
                            )";
            using (var connection = new MySqlConnection(_connectionString))
            {
                var list = (await connection.QueryAsync<GetJsdOrderListPageAsyncDtoOutput>(sql)).ToList();

                List<GetJsdOrderListPageAsyncDtoResult> resultList = new List<GetJsdOrderListPageAsyncDtoResult>();
                foreach (var temp in list)
                {
                    var tempHead = resultList.Where(c => c.Head.Id == temp.Id).FirstOrDefault();
                    if (tempHead == null)
                    {
                        tempHead = new GetJsdOrderListPageAsyncDtoResult();
                        temp.Mobile = (temp.Mobile ?? "").toMobileMask();
                        temp.TrueName = (temp.TrueName ?? "").toMemberNameMask();
                        tempHead.Head = temp;

                        resultList.Add(tempHead);
                    }
                    if (tempHead.DetailList == null)
                    {
                        tempHead.DetailList = new List<JsdOrderItemDot>();
                    }
                    JsdOrderItemDot tempDetail = new JsdOrderItemDot()
                    {
                        Qty = temp.Qty,
                        Price = temp.Price,
                        ProductCode = temp.ProductCode,
                        ProductName = temp.ProductName,
                        Category = temp.Category,
                        MaterialCode = temp.MaterialCode
                    };
                    tempHead.DetailList.Add(tempDetail);
                }


                return ResultData<List<GetJsdOrderListPageAsyncDtoResult>>.CreateResultDataSuccess("成功", resultList);

            }

        }
    }
}
