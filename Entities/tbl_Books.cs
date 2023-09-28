using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class tbl_Books : DomainEntities.DomainEntities
    {
        public string ISBN {  get; set; }
        public bool status {  get; set; }
        public string counterId {  get; set; }
        public Guid? titleId { get; set; }
    }
}
