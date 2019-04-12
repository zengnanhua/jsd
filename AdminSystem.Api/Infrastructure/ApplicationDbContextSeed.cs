using AdminSystem.Application.Queries;
using AdminSystem.Domain.AggregatesModel.UserAggregate;
using AdminSystem.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AdminSystem.Api.Infrastructure
{
    public class ApplicationDbContextSeed
    {
        public async Task SeedAsync(ApplicationDbContext context, IHostingEnvironment env, IServiceProvider service)
        {
            var logger= service.GetService<ILogger<ApplicationDbContextSeed>>();
            
            if (!context.ApplicationUsers.Any())
            {


                #region 初始数据时同步Rms用户数据
                logger.LogInformation("#########初始数据时同步Rms用户数据开始################");
                var zmd_ac_usersQuery= service.GetService<IRmsDataBaseQuery>();
                var list = await zmd_ac_usersQuery.GetZmd_Ac_UsersAsync();
                if (list != null && list.Count > 0)
                {
                    foreach (var temp in list)
                    {
                        ApplicationUser user = new ApplicationUser(temp.UserId,temp.TrueName,temp.Phone,temp.DeptCode);
                        context.ApplicationUsers.Add(user);
                    }
                }
                logger.LogInformation("#########初始数据时同步Rms用户数据结束################");
                #endregion
                await context.SaveChangesAsync();
            }
        }
    }
}
