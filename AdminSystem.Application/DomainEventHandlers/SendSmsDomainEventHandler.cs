using AdminSystem.Domain.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdminSystem.Application.DomainEventHandlers
{
    public class SendSmsDomainEventHandler : INotificationHandler<SendSmsDomainEvent>
    {
        public async Task Handle(SendSmsDomainEvent notification, CancellationToken cancellationToken)
        {
            
        }
    }
}
