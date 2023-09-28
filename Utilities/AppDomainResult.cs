using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utilities
{
    public class AppDomainResult
    {
        public AppDomainResult()
        {
            //Messages = new List<string>();
        }
        public bool success { get; set; }
        public object data { get; set; }
        public int resultCode { get; set; }
        //public IList<string> Messages { get; set; }
        public string resultMessage { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
