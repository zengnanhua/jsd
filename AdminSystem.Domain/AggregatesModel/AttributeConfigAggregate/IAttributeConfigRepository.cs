using AdminSystem.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Domain.AggregatesModel.AttributeConfigAggregate
{
    public interface IAttributeConfigRepository: IRepository<AttributeConfig>
    {
        Task<bool> AttributeConfigAddAsync(AttributeConfig config);
        Task<bool> AttributeConfigDeleteAllAsync();
        Task<List<AttributeConfig>> GetAttributeConfigListAsync();
        List<AttributeConfig> GetAttributeConfigList();
    }
}
