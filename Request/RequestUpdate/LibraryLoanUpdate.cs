using Request.DomainRequests;
using Request.RequestCreate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.RequestUpdate
{
    public class LibraryLoanUpdate:DomainUpdate
    {
        /// <summary>
        /// người mượn
        /// </summary>
        public Guid readerId { get; set; }
        /// <summary>
        /// thủ thư
        /// </summary>
        public Guid librarianId { get; set; }
        public int totalBook { get; set; }
        public List<LibraryLoanDetail> listBookLoan { get; set; }
        public bool isReturnAll { get; set; } 
    }
}
