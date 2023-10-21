using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class tbl_Counter:DomainEntities.DomainEntities
    {
        public string counterName {  get; set; }
        [NotMapped]
        public int totalBook { get; set; }
    }
}
