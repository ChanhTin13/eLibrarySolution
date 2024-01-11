using Request.DomainRequests;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Request.RequestCreate
{
    public class BookCreate : DomainCreate
    {
        public string ISBN { get; set; }
        public bool status { get; set; } = true;
        //public Guid counterId { get; set; }
        [Required]
        public Guid titleId { get; set; } 
    }
}
