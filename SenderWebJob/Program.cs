using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;

namespace EventHubSender
{
    public class EventhubMessage
    {
        public int MessageNo { get; set; }
        public DateTime timestamp { get; set; }
    }
    class Program
    {
        private static EventHubClient eventHubClient;
        private const string EhConnectionString = "Endpoint=sb://pikapemahub.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=/dzsvuLsVRlymt1enUYYUa1pPHQJLtfVGl1bk3CPuB0=";
        private const string EhEntityPath = "Eventhub1";

        static void Main(string[] args)
        {
            //MainAsync(args).GetAwaiter().GetResult();

            Console.WriteLine("Start sending message to Event Hub");
            SendingMessages();
        }

        public static void SendingMessages()
        {
            // Creates an EventHubsConnectionStringBuilder object from a the connection string, and sets the EntityPath.
            // Typically the connection string should have the Entity Path in it, but for the sake of this simple scenario
            // we are using the connection string from the namespace.
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EhConnectionString)
            {
                EntityPath = EhEntityPath
            };

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            EventhubMessage message;
            var i = 0;
            while (true)
            {
                try
                {
                    message = new EventhubMessage { MessageNo = i++, timestamp = DateTime.Now };

                    var ehmessage = JsonConvert.SerializeObject(message);
                    Console.WriteLine($"{DateTime.Now} > Sending message: {ehmessage}");
                    eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(ehmessage)));
                }
                catch (Exception exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                    Console.ResetColor();
                }
            }
        }

    }
}
