using AdminSystem.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.Domain.AggregatesModel.AttributeConfigAggregate
{
    public class AttributeConfig : Entity, IAggregateRoot
    {
        public string ConfigValue { get; private set; }
        public string ConfigText { get; private set; }
        public string ConfigDesc { get; private set; }
        public bool Enabled { get; private set; }

        public AttributeConfig(string configValue,string configText,string configDesc )
        {
            this.ConfigValue = configValue;
            this.ConfigText = configText;
            this.ConfigDesc = configDesc;
            this.Enabled = true;
        }
    }
}
