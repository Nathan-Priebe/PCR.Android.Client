using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Runtime;

namespace PCR.Client.Android
{
    [Preserve(AllMembers = true)]
    public class Applications
    {
        public string App { get; set; }
        public string AppImage { get; set; }
        public int AppId { get; set; }
        public string AppPath { get; set; }
    }
}
