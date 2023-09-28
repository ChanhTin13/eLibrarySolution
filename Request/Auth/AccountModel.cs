using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Auth
{
    public class AccountModel
    {
        public Guid id { get; set; }
        public string fullName { get; set; }
        public string roles { get; set; }
        public bool? isAdmin { get; set; }
    }
}
