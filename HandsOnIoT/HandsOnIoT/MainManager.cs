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
        const string IoTHubUri = "handsonlabiothub.azure-devices.net"; // Enter your IoT Hub url here : IoT Hub HostName
        const string DeviceId = "raspidevice"; // Enter the name of your device
        const string DeviceKey = "XNglOZ0cinCKCiKXNM+WbXvw9HWs/n/nTnuNqbVBRkQ="; // Enter your device key : connectionString primary key


        public static async Task<string> Send(double temperatureToSend, DateTime dateToSend, string locationToSend)
        {
            StringBuilder log = new StringBuilder();
            try
            {

                log.AppendLine("Connexion à Azure IoT Hub...");
                var deviceClient = DeviceClient.Create(IoTHubUri,
                        AuthenticationMethodFactory.CreateAuthenticationWithRegistrySymmetricKey(DeviceId, DeviceKey),
                        TransportType.Http1);

                log.AppendLine("Préparation du message...");
                var temperature = new
                {
                    temperature = temperatureToSend,
                    date = dateToSend,
                    location = locationToSend
                };

                log.AppendLine("Sérialisation de l'objet à envoyer...");
                var messageString = JsonConvert.SerializeObject(temperature);

                log.AppendLine("Encodage...");
                var message = new Message(Encoding.UTF8.GetBytes(messageString));

                log.AppendLine("Envoi du message...");
                await deviceClient.SendEventAsync(message);
                log.AppendLine("Message envoyé !");
            }
            catch (Exception ex)
            {
                log.AppendLine("Erreur lors de l'envoi du message");
            }

            return log.ToString();

        }
    }
}
