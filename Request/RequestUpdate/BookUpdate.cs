using Request.DomainRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.RequestUpdate
{
    public class BookUpdate:DomainUpdate
    {
        public string ISBN { get; set; }
    }
}
 