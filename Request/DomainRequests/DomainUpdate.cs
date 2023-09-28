using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;
using Utilities;
namespace Request.DomainRequests
{
    public class DomainUpdate
    {
        [Required]
        public Guid id { get; set; }
    }
}
