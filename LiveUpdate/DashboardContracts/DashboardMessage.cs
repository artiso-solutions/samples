using System;

namespace DashboardContracts
{
    public class DashboardMessage
    {
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public MessageTypes Type { get; set; }
    }
}