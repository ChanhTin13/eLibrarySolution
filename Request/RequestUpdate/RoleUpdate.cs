using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.CoreContants;

namespace Request.RequestUpdate
{
    public class RoleUpdate
    {
        public role code { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
    }
}
