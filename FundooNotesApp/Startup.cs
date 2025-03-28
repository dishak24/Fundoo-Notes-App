using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services;
//using RepositoryLayer.Context;

namespace FundooNotesApp
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

            //for configure database connection
            services.AddDbContext<FundooDBContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DbConnection"]));

            services.AddTransient<IUserRepo, UserRepo>();
            services.AddTransient<IUserManager, UserManager>();

            //For swagger
            // services.AddSwaggerGen();

            //for adding Authorization in swagger --- to paste token
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Fun-Doo-Notes API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter you valid Token",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                                        {
                                            {
                                                new OpenApiSecurityScheme
                                                {
                                                    Reference = new OpenApiReference
                                                    {
                                                        Type = ReferenceType.SecurityScheme,
                                                            Id = "Bearer"
                                                    }
                                                },
                                                new string[] {}
                                            }
                                         });
            });

            services.AddAuthentication(x =>
                                        {
                                            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                                        }).AddJwtBearer( o =>
                                                        {
                                                            var key = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]);
                                                            o.SaveToken = true;
                                                            o.TokenValidationParameters = new TokenValidationParameters
                                                            {
                                                                ValidateIssuer = false,
                                                                ValidateAudience = false,
                                                                ValidateLifetime = true,
                                                                ValidateIssuerSigningKey = true,
                                                                ValidIssuer = Configuration["Jwt:Issue"],
                                                                ValidAudience = Configuration["Jwt:Audience"],
                                                                IssuerSigningKey = new SymmetricSecurityKey(key)
                                                            };
                                                        });

            //Configuring MassTrasit --- RabbitMQ
            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.UseHealthCheck(provider);
                    config.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                }));
            });
            services.AddMassTransitHostedService();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //authentication must be on top
            app.UseAuthentication();

            app.UseHttpsRedirection();

            // This middleware serves generated Swagger document as a JSON endpoint
            app.UseSwagger();

            // This middleware serves the Swagger documentation UI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee API V1");
            });
            //Remaining as it is.

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
