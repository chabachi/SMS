using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace esquire.common.Models
{
    public class DefaultResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string Description { get; set; }
        public bool IsSuccess { get; set; }
    }
}
