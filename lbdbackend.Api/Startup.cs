using lbdbackend.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lbdbackend.Core.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using lbdbackend.Service.Services;
using lbdbackend.Service.Interfaces;
using Microsoft.OpenApi.Models;
using lbdbackend.Service.Mappings;
using FluentValidation.AspNetCore;
using lbdbackend.Service.DTOs.AccountDTOs;
using Microsoft.AspNetCore.Diagnostics;
using lbdbackend.Service;
using lbdbackend.Data.Repositories;
using lbdbackend.Core.Repositories;
using P225NLayerArchitectura.Service.Exceptions;
using Microsoft.AspNetCore.Http;
using lbdbackend.Service.Exceptions;

namespace lbdbackend.Api
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
            services.AddControllers().AddNewtonsoftJson(options => {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }).AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<LoginDTOValidator>());

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = Configuration.GetSection("JWT:Issuer").Value,
                    ValidAudience = Configuration.GetSection("JWT:Audience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JWT:SecurityKey").Value)),
                };
            });


            services.AddAutoMapper(options => {
                options.AddProfile(new MappingProfile());
            });

            services.AddScoped<IJWTManager, JWTManager>();

            services.AddScoped<IGenreRepository, GenreRepository>();    
            services.AddScoped<IGenresService, GenresService>();

            services.AddScoped<IProfessionsService, ProfessionsService>();
            services.AddScoped<IProfessionRepository, ProfessionRepository>();    

            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IPersonRepository, PersonRepository>();    
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseExceptionHandler(error => {
                error.Run(async context => {
                    var feature = context.Features.Get<IExceptionHandlerPathFeature>();

                    int statustCode = 500;
                    string errorMessage = "Internal Server Error";

                    if (feature.Error is AlreadyExistException) {
                        statustCode = 409;
                        errorMessage = feature.Error.Message;
                    }
                    else if (feature.Error is ItemNotFoundException) {
                        statustCode = 404;
                        errorMessage = feature.Error.Message;
                    }
                    else if (feature.Error is BadRequestException) {
                        statustCode = 415;
                        errorMessage = feature.Error.Message;
                    }


                    context.Response.StatusCode = statustCode;
                    await context.Response.WriteAsync(errorMessage);
                });
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
