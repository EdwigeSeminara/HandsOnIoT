using GrovePi;
using GrovePi.Sensors;
using HandsOnIoT.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace HandsOnIoT
{
    public class MainViewModel : ViewModelBase
    {
        const string DefaultLocation = "Issy-Les-Moulineaux";

        private Timer _refreshTimer;

        public TemperatureSensor TemperatureSensor { get; set; }

        private string message = "Welcome";
        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        private string _state;
        public string State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        private string _log;
        public string Log
        {
            get { return _log; }
            set { SetProperty(ref _log, value); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        private double _temperature;
        public double Temperature
        {
            get { return _temperature; }
            set { SetProperty(ref _temperature, value); }
        }

        public ICommand TestCommand
        {
            get { return new DelegateCommand(OnTest); }
        }

        public ICommand SendCommand
        {
            get { return new DelegateCommand(OnSend); }
        }

        public MainViewModel()
        {
            CheckGroveBoard();

            _refreshTimer = new Timer(RefreshTimerCallBack, null, 0, 5000);
        }

        private async void RefreshTimerCallBack(object state)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    Temperature = TemperatureSensor.Measure();
                    State = string.Concat("Temperature: ", Temperature.ToString(), "℃");
                }
            );
        }

        /// <summary>
        /// Check the GrovePi board
        /// </summary>
        private void CheckGroveBoard()
        {
            // Check if the board is detected
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

                // Everything is ok, load the sensor !
                BuildSensor();
            }
        }

        /// <summary>
        /// Build sensor : here we are adding only a temperature & humidity sensor
        /// </summary>
        private void BuildSensor()
        {
            TemperatureSensor = new TemperatureSensor { Name = "Temperature Sensor", Port = "D2" };
            Sensors = new List<TemperatureSensor>();
            Sensors.Add(TemperatureSensor);
        }


        private List<TemperatureSensor> _sensors;
        public List<TemperatureSensor> Sensors
        {
            get { return _sensors; }
            set { SetProperty(ref _sensors, value); }
        }

        private void OnTest()
        {
            IsBusy = true;

            try
            {
                var temperature = TemperatureSensor.Measure();
                Temperature = temperature;
                State = string.Concat("Temperature: ", temperature.ToString(), "℃");

            }
            catch (Exception ex)
            {
                State = ex.Message;
                Debug.WriteLine("Error : " + TemperatureSensor.Name + " - " + ex.Message);
            }

            IsBusy = false;
        }

        private async void OnSend()
        {
            IsBusy = true;
            try
            {
                var temperatureToSend = TemperatureSensor.Measure();
                var result = await MainManager.Send(temperatureToSend, DateTime.Now, DefaultLocation);
                Log += result;
            }
            catch (Exception ex)
            {
                State = ex.Message;
                Debug.WriteLine("Error : " + TemperatureSensor.Name + " - " + ex.Message);
            }

            IsBusy = false;
        }


    }
}
