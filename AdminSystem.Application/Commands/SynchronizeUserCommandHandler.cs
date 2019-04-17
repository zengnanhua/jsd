using AdminSystem.Application.Queries;
using AdminSystem.Domain.AggregatesModel.UserAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdminSystem.Application.Commands
{
    /// <summary>
    /// 同步用户数据
    /// </summary>
    public class SynchronizeUserCommandHandler : IRequestHandler<SynchronizeUserCommand, bool>
    {
        private IRmsDataBaseQuery _iRmsDataBaseQuery;
        private IApplicationUserRepository _iApplicationUserRepository;
        public SynchronizeUserCommandHandler(IRmsDataBaseQuery iRmsDataBaseQuery, IApplicationUserRepository iApplicationUserRepository)
        {
            this._iRmsDataBaseQuery = iRmsDataBaseQuery;
            this._iApplicationUserRepository = iApplicationUserRepository;
        }
        public async Task<bool> Handle(SynchronizeUserCommand request, CancellationToken cancellationToken)
        {
           
            var list = await _iRmsDataBaseQuery.GetZmd_Ac_UsersAsync();
            if (list != null && list.Count > 0)
            {
                await _iApplicationUserRepository.UserDeleteAllAsync();

                foreach (var temp in list)
                {
                    ApplicationUser user = new ApplicationUser(temp.UserId, temp.TrueName, temp.Phone, temp.DeptCode);
                    _iApplicationUserRepository.AddUser(user);
                }
            }
            return await _iApplicationUserRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
