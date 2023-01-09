using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EventBus.Messages.Common;
using EventBus.Messages.Event;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Ordering.API.Data;
using Ordering.API.EventBusConsumer;
using Ordering.API.Repositories;

namespace Ordering.API
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
            services.AddScoped<IOrderingContext, OrderingContext>();
            services.AddScoped<IOrderingRepository, OrderingRepository>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.API", Version = "v1" });
            });


            services.AddMassTransit(config =>
            {
                //設定Event處理器BasketCheckoutConsumer
                config.AddConsumer<BasketCheckoutConsumer>();
                config.UsingRabbitMq((busRegistrationContext, rabbitMqBusFactoryConfig) =>
                {
                    //使用高級列隊協議amqp://帳號:密碼@網址:port
                    rabbitMqBusFactoryConfig.Host(Configuration["EventBusSettings:HostAddress"]);
                    //設定接收端點(Queue的名子, (端點設定)=>{設定Event處理器}
                    rabbitMqBusFactoryConfig.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, endpointConfig =>
                    {
                        endpointConfig.ConfigureConsumer<BasketCheckoutConsumer>(busRegistrationContext);
                    });
                });
            });
            services.AddMassTransitHostedService();

            //也別忘記註冊我們的Event處理器
            services.AddHttpClient();
            services.AddScoped<BasketCheckoutConsumer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
