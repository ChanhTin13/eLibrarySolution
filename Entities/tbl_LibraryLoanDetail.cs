using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Entities
{
    public class tbl_LibraryLoanDetail:DomainEntities.DomainEntities
    {
        public Guid libraryLoanId { get; set; }
        public Guid bookId { get; set; }
        [NotMapped]
        public Guid bookName { get; set; }
        /// <summary>
        /// ngày mượn
        /// </summary>
        public double dateLoan { get; set; } = Timestamp.Now();
        /// <summary>
        /// ngày hết hạn
        /// </summary>
        public double dateExpire {  get; set; }
        /// <summary>
        /// ngày trả
        /// </summary>
        public double? dateReturn {  get; set; }
        /// <summary>
        /// số lần gia hạn
        /// </summary>
        public int renewals { get; set; } = 0;
        public bool isReturned {  get; set; }=false;
    }
}
