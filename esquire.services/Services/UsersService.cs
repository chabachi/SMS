using esquire.common;
using esquire.common.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace esquire.services.Services
{
    public interface IUsersService
    {
        public Users FindByEmail(string email);
        public DefaultResponse Registration(Users member);

        public DefaultResponse SendAndSave(SMSSendingDTO data);

        public List<UsersListDTO> GetAllMembers();
        public void ActivateDeactivateMember(string email, bool Activate);
    }
    public class UsersService : IUsersService
    {
        private readonly IMemoryCache _cache;
        private readonly ConfigurationSettings _configuration;
        private readonly IDBService<Users> _dateMembers;
        private readonly IDBService<SMSData> _dataSMS;

        public UsersService(IMemoryCache cache, ConfigurationSettings configuration, IDBService<Users> dateMembers, IDBService<SMSData> dataSMS)
        {
            _dateMembers = dateMembers;
            _cache = cache;
            _configuration = configuration;
            _dataSMS = dataSMS;
        }

        public Users FindByEmail(string email)
        {
            var member = _dateMembers.FilterBy(x => x.Email == email).FirstOrDefault();
            return member;
        }

        public DefaultResponse Registration(Users member)
        {
            member.Active = false;
            _dateMembers.InsertOne(member);

            return new DefaultResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Description = "SUCCESS",
                IsSuccess = true
            };
        }

        public DefaultResponse SendAndSave(SMSSendingDTO data)
        {
            SMSData sMSData = new SMSData();
            sMSData.SMSDetails = new List<SMSDetail>();

            var check = _dataSMS.FindOne(x=>x.MobileNumber == data.MobileNumber);
            if (check != null)
            {
                var sendSMS = SendSMS(data);

                if (sendSMS.Count == 0)
                {
                    return new DefaultResponse
                    {
                        HttpStatusCode = System.Net.HttpStatusCode.OK,
                        Description = "Failed to send SMS.",
                        IsSuccess = false
                    };
                }

                var smsData = sendSMS.FirstOrDefault();

                check.SMSDetails.Add(new SMSDetail
                {
                    DateTimeSent = DateTime.UtcNow.AddHours(8),
                    Message = data.Message,
                    MessageId = smsData.message_id,
                    MessageType = data.MessageType,
                    ExecuteByUser = data.ExecutedBy
                });

                _dataSMS.ReplaceOne(check);
            }
            else
            {
                var sendSMS = SendSMS(data);

                if (sendSMS.Count == 0)
                {
                    return new DefaultResponse
                    {
                        HttpStatusCode = System.Net.HttpStatusCode.OK,
                        Description = "Failed to send SMS.",
                        IsSuccess = false
                    };
                }

                var smsData = sendSMS.FirstOrDefault();
                sMSData.CreateDate = DateTime.Now;
                sMSData.Customer = data.Customer;
                sMSData.MobileNumber = data.MobileNumber;
                sMSData.NetWork = smsData.network;

                sMSData.SMSDetails.Add(new SMSDetail 
                {
                    DateTimeSent = DateTime.UtcNow.AddHours(8),
                    Message = data.Message,
                    MessageId = smsData.message_id,
                    MessageType = data.MessageType,
                    ExecuteByUser = data.ExecutedBy
                });

                _dataSMS.InsertOneV2(sMSData);
            }

            return new DefaultResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Description = "SUCCESS",
                IsSuccess = true
            };
        }

        public List<SemaphoreResponse> SendSMS(SMSSendingDTO data)
        {
            string result = "";
            using (WebClient client = new WebClient())
            {
                byte[] response =
                client.UploadValues("https://semaphore.co/api/v4/messages", new NameValueCollection()
                {
                    { "apikey", "" },
                    { "number", data.MobileNumber },
                    { "message", data.Message },
                });
                 result = System.Text.Encoding.UTF8.GetString(response);
            }

            if (string.IsNullOrEmpty(result))
                return new List<SemaphoreResponse>();

            return JsonConvert.DeserializeObject<List<SemaphoreResponse>>(result);
        }

        public List<UsersListDTO> GetAllMembers()
        {
            List<UsersListDTO> returnData = new List<UsersListDTO>();
            var deliquentData = _dateMembers.FilterBy(x=>true);
            var member = _dateMembers.FilterBy(x => true).ToList();

            foreach (var x in member)
            {
                string memberId = x.Id.ToString();
                returnData.Add(new UsersListDTO
                {
                    Id = x.Id.ToString(),
                    Email = x.Email,
                    Active = x.Active,
                    CreateOn = x.CreateDate
                });
            }

            return returnData;
        }

        public void ActivateDeactivateMember(string email, bool Activate)
        {
            var member = _dateMembers.FilterBy(x => x.Email == email).FirstOrDefault();
            member.Active = Activate;
            _dateMembers.ReplaceOne(member);
        }
    }
}
