﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Feleves_EC211O
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly StopwatchService _service = new();
        private readonly DispatcherTimer _uiTimer;

        public ObservableCollection<string> Drivers { get; } =
            new ObservableCollection<string> { "Hamilton", "Verstappen", "Leclerc", "Norris", "Alonos", "Piastri", "Russel" };

        private string _selectedDriver = "Hamilton";
        public string SelectedDriver
        {
            get => _selectedDriver;
            set { _selectedDriver = value; OnPropertyChanged(); }
        }

        private string _currentTime = "00:00:00";
        public string CurrentTime
        {
            get => _currentTime;
            set { _currentTime = value; OnPropertyChanged(); }
        }

        public ObservableCollection<LapRecord> LapTimes { get; } = new();

        public ICommand StartStopCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand LapCommand { get; }
        public ICommand ShowResultsCommand { get; }

        public MainViewModel()
        {
            _uiTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            _uiTimer.Tick += (_, __) => CurrentTime = _service.Elapsed.ToString(@"hh\:mm\:ss");
            _uiTimer.Start();

            StartStopCommand = new RelayCommand(StartStop);
            ResetCommand = new RelayCommand(Reset);
            LapCommand = new RelayCommand(Lap);
            ShowResultsCommand = new RelayCommand(ShowResults);
        }

        private void StartStop()
        {
            if (_service.IsRunning) _service.Stop();
            else _service.Start();
        }

        private void Reset()
        {
            _service.Reset();
            CurrentTime = "00:00:00";
        }

        private void Lap()
        {

            var allLaps = _service.GetLaps();
            var driverLapCount = allLaps.Count(l => l.Driver == SelectedDriver);

            var lap = new LapRecord
            {
                Number = driverLapCount + 1,
                Time = _service.Elapsed,
                Driver = SelectedDriver
            };

            _service.AddLap(lap);
            LapTimes.Insert(0, lap);
        }

        private void ShowResults()
        {
            var allLaps = _service.GetLaps();

            if (allLaps.Count == 0)
            {
                MessageBox.Show("Nincs elérhető köridő!", "Eredménytábla", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var results = allLaps
                .GroupBy(l => l.Driver)
                .Select(g => new RaceResult
                {
                    Driver = g.Key,
                    BestLap = g.Min(l => l.Time),
                    Average = new TimeSpan(Convert.ToInt64(g.Average(l => l.Time.Ticks))),
                    LapCount = g.Count()
                })
                .OrderBy(r => r.BestLap)
                .ToList();

            var window = new ResultsWindow(results);
            window.ShowDialog();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
