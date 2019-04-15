using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.Application.Commands
{
    /// <summary>
    /// 同步配置信息
    /// </summary>
    public class SynchronizeAttributeConfigCommand: IRequest<bool>
    {
    }
}
