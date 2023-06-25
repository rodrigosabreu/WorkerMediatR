using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Worker.Settings
{
    public class SplunkLoggerOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Token { get; set; }
        public string SourceName { get; set; }
        public string SourceType { get; set; }
       

    }
}
