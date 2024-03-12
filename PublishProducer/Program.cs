using MassTransit;
using Shared;

namespace Consumer1
{
    class MessageA : IMessageA
    {
        public string Text { get; set; }
    }
    class MessageB : IMessageB
    {
        public string Text { get; set; }
    }
    class MessageC : IMessageC
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

            await bus.Publish<IMessageA>(new MessageA { Text = "Message A" });
            await bus.Publish<IMessageB>(new MessageB { Text = "Message B" });
            await bus.Publish<IMessageC>(new MessageC { Text = "Message C" });
            Console.WriteLine("Mesajlar gönderilmiştir.");

            Console.Read();
        }
    }
}