using GrovePi;
using GrovePi.Sensors;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace HandsOnIoT
{
    internal class TemperatureSensor : SensorBase
    {
        public TemperatureSensor()
        {
            ImagePath = "ms-appx:///Assets/Sensors/TemperatureSensor.jpg";
            TestDescription = "The sensor will measure the temperature during 1 minute.";
        }

        IDHTTemperatureAndHumiditySensor GroveTempHumi = DeviceFactory.Build.DHTTemperatureAndHumiditySensor(Pin.DigitalPin2, DHTModel.Dht11);
        private String SGroveTempSensor;
        private String SGroveHumiditySensor;

        public override async Task Test()
        {
            try
            {
                GroveTempHumi.Measure();
                var tmp = GroveTempHumi.TemperatureInCelsius;
                TempToSend = tmp;
                DateToSend = DateTime.Now;
                SGroveTempSensor = "Temperature: " + tmp.ToString() + "℃";
                var tmp1 = GroveTempHumi.Humidity;
                SGroveHumiditySensor = "Humidity: " + tmp1.ToString() + "%";
            }
            catch (Exception ex)
            {

            }

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    State = SGroveTempSensor;
                });

        }

        public double TempToSend { get; set; }
        public DateTime DateToSend { get; set; }

        public override async Task Send()
        {
            string iotHubUri = "handsonlabiothub.azure-devices.net";
            string deviceId = "raspidevice";
            string deviceKey = "XNglOZ0cinCKCiKXNM+WbXvw9HWs/n/nTnuNqbVBRkQ=";
            try
            {
                var deviceClient = DeviceClient.Create(iotHubUri,
                        AuthenticationMethodFactory.CreateAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey),
                        TransportType.Http1);

                await Test();

                //var data = TempToSend;
                //var message = new Message(Encoding.ASCII.GetBytes(data.ToString()));

                var temperature = new
                {
                    temperature = TempToSend,
                    date = DateToSend
                };
                var messageString = JsonConvert.SerializeObject(temperature);
                var message = new Message(Encoding.UTF8.GetBytes(messageString));

                await deviceClient.SendEventAsync(message);
            }
            catch (Exception ex)
            {

            }

        }
    }
}
