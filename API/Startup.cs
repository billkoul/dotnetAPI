using API.Config;
using System;
using System.IO;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Domain.Repositories;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers().AddNewtonsoftJson();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API",
                    Description = "API Demo",
                    Contact = new OpenApiContact
                    {
                        Name = "IT Dept",
                        Email = "test@domain.com"
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);  
            });


            /*services.AddScoped<ClientIpCheckActionFilter>(container =>
			{
				var loggerFactory = container.GetRequiredService<ILoggerFactory>();
				var logger = loggerFactory.CreateLogger<ClientIpCheckActionFilter>();

				return new ClientIpCheckActionFilter(
					Configuration["WhiteList"], logger);
			});*/

            services.AddCors();

            services.Configure<RabbitMqConfiguration>(Configuration.GetSection("RabbitMq"));
            services.AddTransient<IFileUpdateSender, FileUpdateSender>();

            
            services.AddScoped<IUploadRepository, UploadRepository>(serviceProvider =>
                new UploadRepository(Configuration.GetValue<string>("ConnectionString"), "C_UPLOADS"));
            
            services.AddMediatR(typeof(Startup));
        }

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.UseRequestInterceptor("(req) => { req.headers['authorization'] = 'apikeytest'; return req; }");
                c.DefaultModelsExpandDepth(-1);
            });

            //app.UseHttpsRedirection();

            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;
                var result = JsonConvert.SerializeObject(new { error = exception.Message });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }));

            app.UseRouting();

			app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

			app.UseAuthorization();

            //app.UseMiddleware<WhiteListMiddleware>(Configuration["WhiteList"]);
            app.UseMiddleware<AuthenticationToken>();

            app.UseEndpoints(endpoints =>
			{
                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}");
                
			});

		}
	}
}
