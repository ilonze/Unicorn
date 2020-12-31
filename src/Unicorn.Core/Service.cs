﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Unicorn.Core
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
        public DateTime RegistorTime { get; set; }
        public DateTime CheckTime { get; set; }
    }
}