
using MassTransit;
using Shared;

namespace Consumer3
{
    class MessageA : IMessageA
    {
        public string Text { get; set; }
    }
    class MessageAConsumer : IConsumer<IMessageA>
    {
        public async Task Consume(ConsumeContext<IMessageA> context) => Console.WriteLine($"test-queue-1 Gelen mesaj : {context.Message.Text}");
    }
    class MessageB : IMessageB
    {
        public string Text { get; set; }
    }
    class MessageBConsumer : IConsumer<IMessageB>
    {
        public async Task Consume(ConsumeContext<IMessageB> context) => Console.WriteLine($"test-queue-1 Gelen mesaj : {context.Message.Text}");
    }
    class MessageC : IMessageC
    {
        public string Text { get; set; }
    }
    class MessageCConsumer : IConsumer<IMessageC>
    {
        public async Task Consume(ConsumeContext<IMessageC> context) => Console.WriteLine($"test-queue-1 Gelen mesaj : {context.Message.Text}");
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
                factory.Host(rabbitMqUri, conf =>
                {
                    conf.Username(userName);
                    conf.Password(password);
                });

                factory.ReceiveEndpoint(queue, configureEndPoint =>
                {
                    configureEndPoint.Consumer<MessageAConsumer>();
                    configureEndPoint.Consumer<MessageBConsumer>();
                    configureEndPoint.Consumer<MessageCConsumer>();
                });

                factory.UseCircuitBreaker(configurator =>
                {
                    configurator.TrackingPeriod = TimeSpan.FromMinutes(1);//How much time for tracking after getting error.
                    configurator.TripThreshold = 15;//Percent of error from requests.
                    configurator.ActiveThreshold = 10;//It refers to the number of errors that can be received in a row.
                    configurator.ResetInterval = TimeSpan.FromMinutes(5);//Waiting time for after getting error (15).
                });

                factory.UseMessageRetry(r => r.Immediate(5));//Trying 5 time for getting message from queue if it not proccessed and error still continues , it pass to .error queue and continue trying.
                factory.UseRateLimit(1000, TimeSpan.FromMinutes(1));//Takes 1000 message in 1 min.
            });

            await bus.StartAsync();
            Console.ReadLine();
            await bus.StopAsync();

        }
    }
}