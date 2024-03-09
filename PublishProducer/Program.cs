using MassTransit;
using Shared;

namespace Consumer1
{
    class Message : IMessage
    {
        public string Text { get; set; }
    }

    class Program
    {


        static async Task Main()
        {
            string rabbitMqUri = "amqps://bfqvojol:pQWSjSSPlDBCjGzCdWRc-fhk9qD_aeO8@cow.rmq2.cloudamqp.com/bfqvojol";
            string queue = "test-queue1";
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
                        break;
                    await bus.Publish<IMessage>(message);
                    Console.WriteLine("");
                }
            });

        }
    }
}