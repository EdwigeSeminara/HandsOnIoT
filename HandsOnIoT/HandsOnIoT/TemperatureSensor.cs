using GrovePi;
using GrovePi.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace HandsOnIoT
{
    public class TemperatureSensor
    {
        public string Name { get; set; }

        public string Port { get; set; }

        public bool State { get; set; }


        IDHTTemperatureAndHumiditySensor GroveTempHumi = DeviceFactory.Build.DHTTemperatureAndHumiditySensor(Pin.DigitalPin2, DHTModel.Dht11);

        public double Measure()
        {
            var tmp = 0.0;
            try
            {
                GroveTempHumi.Measure();
                tmp = GroveTempHumi.TemperatureInCelsius;

                return tmp;
            }
            catch (Exception ex)
            {
                return tmp;
            }


        }
    }
}
