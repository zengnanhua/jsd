using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.Domain.Events
{
    public class SendSmsDomainEvent : INotification
    {
        public string Mobile { get; set; }
        public string Content { get; set; }

        public SendSmsDomainEvent(string mobile,string content)
        {
            this.Mobile = mobile;
            this.Content = content;
        }
    }
}
