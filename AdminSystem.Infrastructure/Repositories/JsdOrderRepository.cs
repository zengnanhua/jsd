using AdminSystem.Domain.AggregatesModel.JsdOrderAggregate;
using AdminSystem.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Repositories
{
    public class JsdOrderRepository: IJsdOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public JsdOrderRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<JsdOrder> AddJsdOrderAsync(JsdOrder order)
        {
            return (await _context.JsdOrders.AddAsync(order)).Entity;
        }
        public async Task<JsdOrder> GetAsync(int jsdOrderId)
        {
            var order = await _context.JsdOrders.FindAsync(jsdOrderId);
            if (order != null)
            {
                await _context.Entry(order)
                  .Collection(i => i.OrderItem).LoadAsync();
            }
            return order;
        }
        public async Task<JsdOrder> GetJsdByJsdOrderCodeAsync(string orderCode)
        {
            var order = await _context.JsdOrders.FirstOrDefaultAsync(c=>c.OrderCode== orderCode);
            if (order != null)
            {
                await _context.Entry(order)
                  .Collection(i => i.OrderItem).LoadAsync();
            }
            return order;
        }
        public void Update(JsdOrder order)
        {
            _context.JsdOrders.Update(order);
        }
    }
}
