using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esquire.common.Models
{
    public class UsersListDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }


        public DateTime CreateOn { get; set; }
    }
}
