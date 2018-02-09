using GrovePi;
using System;
using System.Collections.Generic;

namespace HandsOnIoT
{
    public class MainViewModel : ViewModelBase
    {
        private string message = "Welcome";

        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        public MainViewModel()
        {
            Sensors = new List<SensorBase>();

            // Check the board.
            var board = DeviceFactory.Build.GrovePi();
            if (board == null)
            {
                Message = "Sorry, your GrovePi board could not be detected.";
            }
            else
            {
                try
                {
                    Message = string.Format("Your GrovePi board is ready. (Firmware version {0})", board.GetFirmwareVersion());
                }
                catch (Exception)
                {
                    Message = "Your Grove board is ready.";
                }

                AddSensors();
            }
        }

        private List<SensorBase> _sensors;
        internal List<SensorBase> Sensors
        {
            get { return _sensors; }
            set { SetProperty(ref _sensors, value); }
        }

        private void AddSensors()
        {
            Sensors.Add(new TemperatureSensor() { Name = "Temperature Sensor", Port = "A2" });

        }

    }


}
