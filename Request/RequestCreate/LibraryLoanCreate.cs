﻿using Request.DomainRequests;
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
        public int totalBook { get; set; }
        public List<LibraryLoanDetail> listBookLoan {  get; set; }
    }
    public class LibraryLoanDetail 
    {
        //public Guid libraryLoanId { get; set; }
        public Guid bookId { get; set; }
        public string bookName {  get; set; }
        public string IBSN {  get; set; }
        /// <summary>
        /// ngày mượn
        /// </summary> 
        public double dateLoan { get; set; }
        /// <summary>
        /// ngày hết hạn
        /// </summary> 
        public double dateExpire { get; set; }
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
