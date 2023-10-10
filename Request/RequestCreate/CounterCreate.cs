using Request.DomainRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.RequestCreate
{
    public class CounterCreate:DomainCreate
    {
        public string counterName {  get; set; }
    }
}
