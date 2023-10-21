using Request.DomainRequests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Utilities;

namespace Request.RequestCreate
{
    public class LibraryLoanCreate: DomainCreate
    { 
        /// <summary>
        /// người mượn
        /// </summary>
        public Guid readerId { get; set; }
        /// <summary>
        /// thủ thư
        /// </summary>
        public Guid librarianId { get; set; }
        public List<LibraryLoanDetail> listBookLoan {  get; set; }
        /// <summary>
        /// ngày mượn=ngày lập phiếu
        /// </summary> 
        [JsonIgnore]
        public double dateLoan { get; set; } = Timestamp.Now();
        [JsonIgnore]
        public int totalBook { get; set; }  
    }
    public class LibraryLoanDetail 
    {
        //public Guid libraryLoanId { get; set; }
        public Guid bookId { get; set; }
        public string bookName {  get; set; }
        public string IBSN {  get; set; }
        /// <summary>
        /// ngày hết hạn
        /// mặc định 3d kể từ ngày
        /// </summary> 
        [JsonIgnore]
        public double dateExpire { get; set; }=Timestamp.Date(DateTime.Now.AddDays(3));
        /// <summary>
        /// ngày trả
        /// </summary>
        [JsonIgnore]
        public double? dateReturn { get; set; } = null;
        /// <summary>
        /// số lần gia hạn
        /// </summary>
        [JsonIgnore]
        public int renewals { get; set; } = 0;
        [JsonIgnore]
        public bool isReturned { get; set; } = false;
    }
}
