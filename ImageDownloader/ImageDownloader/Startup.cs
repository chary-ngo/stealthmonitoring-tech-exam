using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ImageDownloader.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ImageDownloader
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<MarsRoverQueryProvider>();
            services.AddHttpClient();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            IHttpClientFactory _clientFactory = app.ApplicationServices.GetService(typeof(IHttpClientFactory)) as IHttpClientFactory;

            lifetime.ApplicationStarted.Register(OnApplicationStartedAsync(_clientFactory).Wait);
        }

        private async Task<Action> OnApplicationStartedAsync(IHttpClientFactory httpClientFactory)
        {
            var client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44381/marsrover/DownloadImage");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                //deal with the response
                var result = await response.Content.ReadAsStringAsync();
            }

            return null;
        }
    }
}
