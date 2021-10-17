using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleWeb
{
    public class SuperWebApplication : IHttpApplication<HttpContext>
    {
        public IServiceProvider ConfigureServices(Action<IServiceCollection> configure)
        {
            configure(ServiceCollection);
            // ServiceCollection.AddTransient<IActionResultExecutor<JsonResult>>();
            ServiceCollection.AddControllers();
            ServiceProvider = ServiceCollection.BuildServiceProvider(); // build once
            return ServiceProvider;
        }

        private IServiceCollection ServiceCollection { get; } = new ServiceCollection();
        public IServiceProvider? ServiceProvider { get; set; }

        
        public HttpContext CreateContext(IFeatureCollection contextFeatures)
        {
            return new DefaultHttpContext(contextFeatures)
            {
                RequestServices = ServiceProvider ?? ServiceCollection.BuildServiceProvider()
            };
        }

        public async Task ProcessRequestAsync(HttpContext context)
        {
            
            var httpRequest = context.Request;
            var serviceProvider = context.RequestServices;
            switch (httpRequest.Path)
            {
                case "/person":
                {
                    var controller = serviceProvider.GetRequiredService<PersonController>();
                    var actionResult = controller.Get();
                    await actionResult
                        .ExecuteResultAsync(new ActionContext
                        (
                            context,
                            new RouteData(new RouteValueDictionary()),
                            new ActionDescriptor()
                        ));
                    break;   
                }
                case "/":
                default:
                    await context.Response.WriteAsync("Hello World!");
                    break;
            }
        }

        public void DisposeContext(HttpContext context, Exception exception)
        {
            
        }
    }
}