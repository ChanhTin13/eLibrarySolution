using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Entities
{
    public class tbl_LibraryLoan:DomainEntities.DomainEntities
    {
        /// <summary>
        /// mã phiếu
        /// </summary>
        public string code {  get; set; }
        /// <summary>
        /// người mượn
        /// </summary>
        public Guid readerId {  get; set; }
        [NotMapped]
        public Guid readerName { get; set; }
        /// <summary>
        /// thủ thư
        /// </summary>
        public Guid librarianId {  get; set; }
        [NotMapped]
        public Guid librarianName { get;set; }
        public int totalBook { get; set; }
        public string listBookLoan { get; set; }
        public bool isReturnAll { get; set; } = false;
    }
}
