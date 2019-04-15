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
using AdminSystem.Domain.AggregatesModel.AttributeConfigAggregate;
using MediatR;
using AdminSystem.Application.Commands;

namespace AdminSystem.Api.Infrastructure
{
    public class ApplicationDbContextSeed
    {
        public async Task SeedAsync(ApplicationDbContext context, IHostingEnvironment env, IServiceProvider service)
        {
            var logger= service.GetService<ILogger<ApplicationDbContextSeed>>();
            var mediator = service.GetService<IMediator>();
            if (!context.ApplicationUsers.Any())
            {
                #region 初始数据时同步Rms用户数据
                logger.LogInformation("#########初始数据时同步Rms用户数据开始################");
                await mediator.Send(new SynchronizeUserCommand());
                logger.LogInformation("#########初始数据时同步Rms用户数据结束################");
                #endregion
            }
            if (!context.AttributeConfigs.Any())
            {
                logger.LogInformation("#########初始数据时同步Config配置数据开始################");
              
                await mediator.Send(new SynchronizeAttributeConfigCommand());
                logger.LogInformation("#########初始数据时同步Config配置数据结束################");
            }
        }
    }
}
