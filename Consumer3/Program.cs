
using MassTransit;
using Shared;

namespace Consumer1
{
    class Message : IMessage
    {
        public string Text { get; set; }
    }
    class MessageConsumer : IConsumer<IMessage>
    {
        public async Task Consume(ConsumeContext<IMessage> context) => Console.WriteLine($"test-queue-3 Gelen mesaj : {context.Message.Text}");
    }

    class Program
    {


        static async Task Main()
        {
            string rabbitMqUri = "amqps://bfqvojol:pQWSjSSPlDBCjGzCdWRc-fhk9qD_aeO8@cow.rmq2.cloudamqp.com/bfqvojol";
            string queue = "test-queue3";
            string userName = "bfqvojol";
            string password = "pQWSjSSPlDBCjGzCdWRc-fhk9qD_aeO8";


            var bus = Bus.Factory.CreateUsingRabbitMq(factory =>
            {
                factory.Host(rabbitMqUri, conf =>
                {
                    conf.Username(userName);
                    conf.Password(password);
                });

                factory.ReceiveEndpoint(queue, configureEndPoint => configureEndPoint.Consumer<MessageConsumer>());
            });

            await bus.StartAsync();
            Console.ReadLine();
            await bus.StopAsync();

        }
    }
}