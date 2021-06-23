using System;
using System.Windows.Threading;
using OpenHardwareMonitor.Hardware;

namespace MyHardwareMonitor
{
    
    public class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
    }
    class ViewModel : ViewModelBase
    {
        private readonly DispatcherTimer _timer;
        public ViewModel()
        {
            computer.Open();
            computer.Accept(updateVisitor);
            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            _timer.Start();
            _timer.Tick += (o, e) =>
            {
                computer.Accept(updateVisitor);
                OnPropertyChanged(nameof(CpuStroke));
                OnPropertyChanged(nameof(GpuStroke));
                OnPropertyChanged(nameof(RamStroke));
                OnPropertyChanged(nameof(CpuTotal));
                OnPropertyChanged(nameof(GpuTotal));
                OnPropertyChanged(nameof(RamTotal));
                OnPropertyChanged(nameof(CpuName));
                OnPropertyChanged(nameof(GpuName));
                OnPropertyChanged(nameof(CpuTemperature));
                OnPropertyChanged(nameof(GpuTemperature));
            };
        }

        #region computer
        readonly UpdateVisitor updateVisitor = new UpdateVisitor();
        readonly Computer computer = new Computer()
        {
            CPUEnabled = true,
            GPUEnabled = true,
            MainboardEnabled = true,
            RAMEnabled = true,
            FanControllerEnabled = true,
            HDDEnabled = true
        };
        #endregion

        public string _cpuStroke;
        public string _gpuStroke;
        public string _ramStroke;
        public string _gpuName;
        public string _cpuName;
        public float? _cpuTemperature;
        public float? _gpuTemperature;
        public float? _cpuTotal;
        public float? _gpuTotal;
        public float? _ramTotal;

        public string CpuStroke
        {
            get
            {
                _cpuStroke = "";
                _cpuName = computer.Hardware[1].Name;
                _cpuStroke += computer.Hardware[1].Name+"\r";
                for (int j = 0; j < computer.Hardware[1].Sensors.Length; j++)
                {
                    if (computer.Hardware[1].Sensors[j].Name == "CPU Total")
                    {
                        _cpuStroke += computer.Hardware[1].Sensors[j].SensorType + " " + computer.Hardware[1].Sensors[j].Name + ": " + computer.Hardware[1].Sensors[j].Value.ToString() + "\r";
                        _cpuTotal = (float?)Math.Round((double)computer.Hardware[1].Sensors[j].Value, 2);
                    }
                    else if (computer.Hardware[1].Sensors[j].SensorType== SensorType.Temperature)
                    {
                        _cpuTemperature = computer.Hardware[1].Sensors[j].Value;
                        _cpuStroke += computer.Hardware[1].Sensors[j].SensorType + " " + computer.Hardware[1].Sensors[j].Name + ": " + computer.Hardware[1].Sensors[j].Value.ToString() + " °C" + "\r" ;
                    }
                    else
                    {
                        _cpuStroke += computer.Hardware[1].Sensors[j].SensorType + " " + computer.Hardware[1].Sensors[j].Name + ": " + computer.Hardware[1].Sensors[j].Value.ToString() + "\r";
                    }

                }
               return _cpuStroke;
            }
            set
            {}
        }
        public float? CpuTotal
        {
            get { return _cpuTotal; }
            set { }
        }
        public string CpuName
        {
            get { return _cpuName; }
            set { }
        }
        public float? CpuTemperature
        {
            get { return _cpuTemperature; }
            set { }
        }
        public string GpuStroke
        {
            get
            {
                _gpuStroke = "";
                _gpuName = computer.Hardware[3].Name;
                _gpuStroke += computer.Hardware[3].Name + "\r";
                for (int j = 0; j < computer.Hardware[3].Sensors.Length; j++)
                {
                    if (computer.Hardware[3].Sensors[j].Name == "GPU Core" & computer.Hardware[3].Sensors[j].SensorType== SensorType.Load)
                    {
                        _gpuStroke += computer.Hardware[3].Sensors[j].SensorType + " " + computer.Hardware[3].Sensors[j].Name + ": " + computer.Hardware[3].Sensors[j].Value.ToString() + "\r";
                        _gpuTotal = (float?)Math.Round((double)computer.Hardware[3].Sensors[j].Value, 2);
                    }
                    else if(computer.Hardware[3].Sensors[j].SensorType == SensorType.Temperature)
                    {
                        _gpuTemperature= computer.Hardware[3].Sensors[j].Value;
                    }
                    else
                    {
                        _gpuStroke += computer.Hardware[3].Sensors[j].SensorType + " " + computer.Hardware[3].Sensors[j].Name + ": " + computer.Hardware[3].Sensors[j].Value.ToString() + "\r";
                    }
                }
                return _gpuStroke;
            }
            set
            {}
        }
        public float? GpuTotal
        {
            get { return _gpuTotal; }
            set { }
        }
        public string GpuName
        {
            get { return _gpuName; }
            set { }
        }
        public float? GpuTemperature
        {
            get { return _gpuTemperature; }
            set { }
        }
        public string RamStroke
        {
            get
            {
                _ramStroke = "";
                _ramStroke += computer.Hardware[2].Name + "\r";
                for (int j = 0; j < computer.Hardware[2].Sensors.Length; j++)
                {
                    if (computer.Hardware[2].Sensors[j].Name == "Memory" & computer.Hardware[2].Sensors[j].SensorType == SensorType.Load)
                    {
                        _ramStroke += computer.Hardware[2].Sensors[j].SensorType + " " + computer.Hardware[2].Sensors[j].Name + ": " + computer.Hardware[2].Sensors[j].Value.ToString() + "\r";
                        _ramTotal = (float?)Math.Round((double)computer.Hardware[2].Sensors[j].Value, 2);
                    }
                    else
                    {
                        _ramStroke += computer.Hardware[2].Sensors[j].SensorType + " " + computer.Hardware[2].Sensors[j].Name + ": " + computer.Hardware[2].Sensors[j].Value.ToString() + "\r";
                    }
                }
                return _ramStroke;
            }
            set
            {}
        }
        public float? RamTotal
        {
            get { return _ramTotal; }
            set { }
        }

    }
}
