using MassTransit;
using Shared;

namespace Procuder
{

    class Message : IMessage
    {
        public string Text { get; set; }
    }

    class Program
    {

        static async Task Main(string[] args)
        {
            string rabbitMqUri = "amqps://bfqvojol:pQWSjSSPlDBCjGzCdWRc-fhk9qD_aeO8@cow.rmq2.cloudamqp.com/bfqvojol";
            string queue = "test-queue";
            string userName = "bfqvojol";
            string password = "pQWSjSSPlDBCjGzCdWRc-fhk9qD_aeO8";


            var bus = Bus.Factory.CreateUsingRabbitMq(factory =>
            {
                factory.Host(rabbitMqUri, config =>
                {
                    config.Username(userName);
                    config.Password(password);
                });
            });

            var sendToUri = new Uri($"{rabbitMqUri}/{queue}");
            var endPoint = await bus.GetSendEndpoint(sendToUri);

            await Task.Run(async () =>
            {
                while (true)
                {
                    Console.Write("Mesaj yaz : ");
                    Message message = new Message
                    {
                        Text = Console.ReadLine()
                    };
                    if (message.Text.ToUpper() == "C")
                    {
                        break;
                    }
                    await endPoint.Send<IMessage>(message);
                    Console.WriteLine("");
                }


            });
        }
    }
}