using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feleves_EC211O
{
    public class RaceResult
    {
        public string Driver { get; set; } = string.Empty;
        public TimeSpan BestLap { get; set; }
        public TimeSpan Average { get; set; }
        public int LapCount { get; set; }
    }
}
