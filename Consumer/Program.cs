using MassTransit;
using Shared;

namespace Procuder
{

    class Message : IMessage
    {
        public string Text { get; set; }
    }
    class MessageConsumer : IConsumer<IMessage>
    {
        public async Task Consume(ConsumeContext<IMessage> context) => Console.WriteLine($"Gelen mesaj : {context.Message.Text}");
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

                factory.ReceiveEndpoint(queue, endpoint => endpoint.Consumer<MessageConsumer>());
            });

            await bus.StartAsync();
            Console.ReadLine();
            await bus.StopAsync();


        }
    }
}