using Entities.DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Search
{
    public class LibraryLoanSearch:BaseSearch
    {
        public string code {  get; set; }
        public Guid? readerId {  get; set; }
        public Guid? librarianId { get; set; }
        public Guid? bookId { get; set; }
    }
}
