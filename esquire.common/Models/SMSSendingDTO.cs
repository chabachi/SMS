using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esquire.common.Models
{
    public class SMSSendingDTO
    {
        public string MobileNumber { get; set; }
        public string Customer { get; set; }
        public string MessageType { get; set; }
        public string Message { get; set; }
        public string ExecutedBy { get; set; }
    }
}
