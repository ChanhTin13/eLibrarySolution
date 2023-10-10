using Request.DomainRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.RequestUpdate
{
    public class CounterUpdate:DomainUpdate
    {
        public string counterName {  get; set; }
    }
}
