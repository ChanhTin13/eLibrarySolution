using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class tbl_Counter:DomainEntities.DomainEntities
    {
        public string counterName {  get; set; }
    }
}
