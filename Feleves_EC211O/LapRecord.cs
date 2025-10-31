using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feleves_EC211O
{
    public class LapRecord
    {
        public int Number { get; set; }
        public string Driver { get; set; } = string.Empty;
        public TimeSpan Time { get; set; }
    }
}
