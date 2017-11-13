using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PLANET2.ViewModels
{
    class ProcessesViewModel : INotifyPropertyChanged
    {
        private Process selected;
        public Process Selected {
            get => selected;
            set {
                selected = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Selected)));
            }
        }
        public ObservableCollection<Process> Processes { get; } = new ObservableCollection<Process>();

        public ProcessesViewModel()
        {
            var timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += UpdateProcesses;
            timer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void UpdateProcesses(object sender, EventArgs e)
        {
            Process tmp = Selected;
            Processes.Clear();
            foreach(var p in Process.GetProcesses())
            {
                Processes.Add(p);
            }
            if(tmp != null)
                Selected = Processes.Where(x => x.Id == tmp.Id).First();
        }

    }
}
