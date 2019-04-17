using AdminSystem.Application.Queries;
using AdminSystem.Domain.AggregatesModel.AttributeConfigAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdminSystem.Application.Commands
{
    /// <summary>
    /// 同步配置信息
    /// </summary>
    public class SynchronizeAttributeConfigCommandHandler : IRequestHandler<SynchronizeAttributeConfigCommand, bool>
    {
        private IRmsDataBaseQuery _iRmsDataBaseQuery;
        private IAttributeConfigRepository _attributeConfigRepository;
        public SynchronizeAttributeConfigCommandHandler(IRmsDataBaseQuery iRmsDataBaseQuery, IAttributeConfigRepository attributeConfigRepository)
        {
            this._iRmsDataBaseQuery = iRmsDataBaseQuery;
            this._attributeConfigRepository = attributeConfigRepository;
        }
        public async Task<bool> Handle(SynchronizeAttributeConfigCommand request, CancellationToken cancellationToken)
        {
            var list= await _iRmsDataBaseQuery.GetZmd_Base_ConfigAsync();
            if (list != null && list.Count > 0)
            {
                await _attributeConfigRepository.AttributeConfigDeleteAllAsync();

                foreach (var temp in list)
                {
                    AttributeConfig entity = new AttributeConfig(temp.ConfigValue, temp.ConfigText, temp.ConfigDesc);
                    await _attributeConfigRepository.AttributeConfigAddAsync(entity);
                }

            }
            //n
            _iRmsDataBaseQuery.RefreshAttributeConfig();
            return await _attributeConfigRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
