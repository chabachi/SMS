using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esquire.common.Models
{
    [BsonCollection("Users")]
    [BsonIgnoreExtraElements]
    public class Users : Document
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }

        /// <summary>
        /// MEMBER
        /// ADMIN
        /// </summary>
        public string UserType { get; set; }

        public bool Active { get; set; }
    }
}
