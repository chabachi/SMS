using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esquire.common.Models
{
    [BsonCollection("SMSData")]
    [BsonIgnoreExtraElements]
    public class SMSData : Document
    {
        public string MobileNumber { get; set; }
        public string Customer { get; set; }
        public string NetWork { get; set; }
        public List<SMSDetail> SMSDetails { get; set; }
    }

    public class SMSDataList
    {
        public string MobileNumber { get; set; }
        public string Customer { get; set; }
        public string NetWork { get; set; }
        public int SmsCount { get; set; }
        public DateTime DateTimeSent { get; set; }
        public List<SMSDetail> SMSDetails { get; set; }
    }

    public class SMSDetail
    { 
        public string MessageId { get; set; }

        public string MessageType { get; set; }
        public string Message { get; set; }

        public DateTime DateTimeSent { get; set; }

        public string ExecuteByUser { get; set; }

        public string Status { get; set; }
    }
}
