using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feleves_EC211O
{
    public class StopwatchService
    {
        private DateTime? _startTime;
        private TimeSpan _elapsed = TimeSpan.Zero;
        private readonly List<LapRecord> _laps = new();

        public bool IsRunning { get; private set; }

        public TimeSpan Elapsed =>
            IsRunning ? _elapsed + (DateTime.Now - _startTime!.Value) : _elapsed;

        public void Start()
        {
            if (IsRunning) return;
            IsRunning = true;
            _startTime = DateTime.Now;
        }

        public void Stop()
        {
            if (!IsRunning) return;
            _elapsed += DateTime.Now - _startTime!.Value;
            IsRunning = false;
        }

        public void Reset()
        {
            _startTime = null;
            _elapsed = TimeSpan.Zero;
            IsRunning = false;
        }

        public void AddLap(LapRecord lap)
        {
            _laps.Add(lap);
        }

        public IReadOnlyList<LapRecord> GetLaps() => _laps.AsReadOnly();
    }
}
