using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Timestamp
    {
        public static double Now()
        {
            var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            return timestamp;
        }
        public static double Date(DateTime date)
        {
            var timestamp = new DateTimeOffset(date).ToUnixTimeMilliseconds();
            return timestamp;
        }
        public static DateTime ToDatetime(long timestamp)
        {
            DateTime rslt = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
            return rslt;
        }
        public static double AddMinutes(double time, double minutes)
        {
            return time + (minutes * 60000);
        }
    }
}
