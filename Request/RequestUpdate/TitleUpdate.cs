﻿using Request.DomainRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.RequestUpdate
{
    public class TitleUpdate:DomainUpdate
    { 
        public string name { get; set; }
        //public int quantity { get; set; } 
        public Guid? authorId { get; set; } = null;
        /// <summary>
        /// ví dụ: 1,2,3
        /// </summary>
        public string categoryId { get; set; } = new Guid().ToString();
    }
}
