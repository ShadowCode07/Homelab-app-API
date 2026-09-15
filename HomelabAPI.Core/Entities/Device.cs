using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomelabAPI.Core.Entities
{
    public class Device : BaseClass
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        //public DateTime LastSeenAt { get; set; }
        //public DateTime CreatedAt { get; set; }
    }
}
