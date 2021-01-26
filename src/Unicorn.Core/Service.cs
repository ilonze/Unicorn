using System;
using System.Collections.Generic;
using System.Text;

namespace Unicorn
{
    public class Service
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsHealth { get; set; }
        public string Schema { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string HealthCheck { get; set; }
        public int Weight { get; set; }
        public int Priority { get; set; }
        public string[] Tags { get; set; }
        public bool IsFromHub { get; set; }
        public DateTimeOffset RegistorTime { get; set; }
        public DateTimeOffset CheckTime { get; set; }
    }
}
