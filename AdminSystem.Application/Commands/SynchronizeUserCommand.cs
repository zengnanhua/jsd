using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.Application.Commands
{
    public class SynchronizeUserCommand : IRequest<bool>
    {
    }
}
