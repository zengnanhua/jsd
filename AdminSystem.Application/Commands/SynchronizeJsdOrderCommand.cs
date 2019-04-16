using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.Application.Commands
{
    public class SynchronizeJsdOrderCommand : IRequest<bool>
    {
        /// <summary>
        /// 格式 yyyy-MM-dd 如（2019-04-16）
        /// </summary>
        public string Date { get; set; }

        public SynchronizeJsdOrderCommand(string date)
        {
            this.Date = date;
        }
    }
}
