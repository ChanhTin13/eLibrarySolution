using Request.DomainRequests;
using Request.RequestCreate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Request.RequestUpdate
{
    public class LibraryLoanUpdate : DomainUpdate
    {
        public Guid? bookId { get; set; }
        public string IBSN { get; set; }
    } 
}
