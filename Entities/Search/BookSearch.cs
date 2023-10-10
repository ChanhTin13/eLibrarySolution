using Entities.DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Search
{
    public class BookSearch:BaseSearch
    {
        public string IBSN {  get; set; } 
        public Guid? titleId {  get; set; }
        public Guid? counterId { get; set; }
    }
}
