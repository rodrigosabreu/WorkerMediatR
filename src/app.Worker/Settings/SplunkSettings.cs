using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Worker.Settings
{
    public class SerilogSettings
    {
        public string[] Using { get; set; }
        public string MinimumLevel { get; set; }
        public SerilogSink[] WriteTo { get; set; }
    }

    public class SerilogSink
    {
        public string Name { get; set; }
        public SplunkArgs Args { get; set; }
    }

    public class SplunkArgs
    {
        public string SplunkHost { get; set; }
        public string Token { get; set; }
        public int SplunkPort { get; set; }
        public string SourceType { get; set; }
        public string EventFormatter { get; set; }
    }

}
