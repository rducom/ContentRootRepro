using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Middleware.Tests
{
    public class TestFactory<T> : WebApplicationFactory<T> where T : class
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            IWebHostBuilder builder = WebHost.CreateDefaultBuilder();

            // With or without UseSolutionRelativeContentRoot, result is same, the delegate is never called
            // the content root is overiden before ConfigureServices
            // CreateClient() fails in WebHostBuilder.Build()
            builder.ConfigureServices(services =>
            {
                builder.UseSolutionRelativeContentRoot("../../", "ContentRootIssue.sln");
            });
            return builder.UseStartup<T>();
        }
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ForceCultureMiddleware>();
            app.UseMvc();
        }
    }

    public class ContentRootTests : IClassFixture<TestFactory<Startup>>
    {
        private readonly TestFactory<Startup> _factory;

        public ContentRootTests(TestFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public void TryCreateClient()
        {
            HttpClient client = _factory.CreateClient();
        }
    }
}
