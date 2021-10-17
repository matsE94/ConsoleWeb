using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace ConsoleWeb
{
    internal static class Program
    {
        public static async Task Main()
        {
            var application = new SuperWebApplication(); //IHttpApplication<HttpContext>
            application.ConfigureServices(services =>
            {
                services.AddLogging();
                services.AddTransient<PersonController>();
            });

            var kestrelOptions = Options.Create(new KestrelServerOptions
            {
                ApplicationServices = application.ServiceProvider,
            });
            var socketOptions = Options.Create(new SocketTransportOptions());
            IConnectionListenerFactory socketTransportFactory =
                new SocketTransportFactory(socketOptions, NullLoggerFactory.Instance);
            IServer server = new KestrelServer(kestrelOptions, socketTransportFactory, NullLoggerFactory.Instance);

            // starts on a separate thread
            // https://localhost:5001/
            await server.StartAsync(application, CancellationToken.None);
            Console.ReadKey();
        }
    }
}