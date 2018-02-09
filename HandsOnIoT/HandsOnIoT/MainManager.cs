using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Microsoft.Azure.Devices.Client;
using System.Text;
using Newtonsoft.Json;
namespace HandsOnIoT
{
    public static class MainManager
    {
        const string IoTHubUri = "handsonlabiothub.azure-devices.net"; // Enter your IoT Hub url here
        const string DeviceId = "raspidevice"; // Enter the name of your device
        const string DeviceKey = "XNglOZ0cinCKCiKXNM+WbXvw9HWs/n/nTnuNqbVBRkQ="; // Enter your device key


        public static async Task Send(double temperatureToSend, DateTime dateToSend, string locationToSend)
        {
            try
            {
                var deviceClient = DeviceClient.Create(IoTHubUri,
                        AuthenticationMethodFactory.CreateAuthenticationWithRegistrySymmetricKey(DeviceId, DeviceKey),
                        TransportType.Http1);

                var temperature = new
                {
                    temperature = temperatureToSend,
                    date = dateToSend,
                    location = locationToSend
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
