using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace PLANET2.ViewModels
{
    class ProcessesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private DispatcherTimer timer;
        private Process selected;
        private int priority = 8;
        

        public ObservableCollection<Process> Processes { get; } = new ObservableCollection<Process>();
        public Process Selected {
            get => selected;
            set {
                selected = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Selected)));
            }
        }
        public List<int> Priorities { get; } = new List<int> { 4, 8, 13, 24 }; 
        public int Priority
        {
            get => priority;
            set
            {
                priority = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Priority)));
            }
        }
        public ProcessesViewModel()
        {
            timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += UpdateProcesses;
            timer.Start();
        }
       

        public ICommand ChangePriority { get { return new RelayCommand(ChangePriorityExecute, () => true); } }
        public ICommand KillProcess { get { return new RelayCommand(KillProcessExecute, () => true); } }
        private void KillProcessExecute()
        {
            Selected.Kill();
            Selected = null;
        }
        private void ChangePriorityExecute()
        {
            timer.Stop();
            switch (priority)
            {
                case 4:
                    Selected.PriorityClass = ProcessPriorityClass.Idle;
                    break;
                case 8:
                    Selected.PriorityClass = ProcessPriorityClass.Normal;
                    break;
                case 13:
                    Selected.PriorityClass = ProcessPriorityClass.High;
                    break;
                case 24:
                    Selected.PriorityClass = ProcessPriorityClass.RealTime;
                    break;
                default:
                    Selected.PriorityClass = ProcessPriorityClass.Normal;
                    break;
            }
            timer.Start();
        }

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
