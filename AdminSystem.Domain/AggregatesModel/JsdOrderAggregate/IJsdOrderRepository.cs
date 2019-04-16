using AdminSystem.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Domain.AggregatesModel.JsdOrderAggregate
{
    public interface IJsdOrderRepository : IRepository<JsdOrder>
    {
        Task<JsdOrder> AddJsdOrderAsync(JsdOrder order);
        Task<JsdOrder> GetAsync(int jsdOrderId);
        void Update(JsdOrder order);
    }
}
