using Request.DomainRequests;
using System;
using System.ComponentModel.DataAnnotations;

namespace Request.RequestCreate
{
    public class BookCreate : DomainCreate
    {
        public string ISBN { get; set; }
        public bool status { get; set; }
        public string counterId { get; set; }
        public Guid? titleId { get; set; }
    }
}
