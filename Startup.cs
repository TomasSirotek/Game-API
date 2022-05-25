using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Configuration;
using API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API
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
            // var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);
            //
            // var tokenValidationParams = new TokenValidationParameters {
            //     ValidateIssuerSigningKey = true,
            //     IssuerSigningKey = new SymmetricSecurityKey(key),
            //     ValidateIssuer = false,
            //     ValidateAudience = false,
            //     ValidateLifetime = true,
            //     RequireExpirationTime = false,
            //     ClockSkew = TimeSpan.Zero
            // };

          //   services.AddSingleton(tokenValidationParams);
            // Enable CORS
            services.AddCors(c =>
            {
               c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            
            // JSON Serialization // Nuget package
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            
            services.AddMvc ();
            services.AddRazorPages ();

            services.AddDbContext<DataContext>(
                    o => Configuration.GetConnectionString("PostgresAppCon")
            );

            services.AddControllers();
            
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "E-Commerce", Version = "v1" });
                c.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme
                {
                    In=ParameterLocation.Header,
                    Description = "Please insert token",
                    Name="Authorization",
                    Type=SecuritySchemeType.ApiKey,
                   
                });
             
               c.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,  
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(Configuration["JwtConfig:Secret"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            // services.AddAuthentication(options => {
            //         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //         options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //     })
            //     .AddJwtBearer(jwt => {
            //         jwt.SaveToken = true;
            //         jwt.TokenValidationParameters = tokenValidationParams;
            //     });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Commerce v1"));
            }

            app.UseCors((options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication ();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
    // internal class AuthResponsesOperationFilter : IOperationFilter
    // {
    //     public void Apply(OpenApiOperation operation, OperationFilterContext context)
    //     {
    //         var attributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
    //             .Union(context.MethodInfo.GetCustomAttributes(true));
    //
    //         if (attributes.OfType<IAllowAnonymous>().Any())
    //         {
    //             return;
    //         }
    //
    //         var authAttributes = attributes.OfType<IAuthorizeData>();
    //
    //         if (authAttributes.Any())
    //         {
    //             operation.Responses["401"] = new OpenApiResponse { Description = "Unauthorized" };
    //
    //             if (authAttributes.Any(att => !String.IsNullOrWhiteSpace(att.Roles) || !String.IsNullOrWhiteSpace(att.Policy)))
    //             {
    //                 operation.Responses["403"] = new OpenApiResponse { Description = "Forbidden" };
    //             }
    //
    //             operation.Security = new List<OpenApiSecurityRequirement>
    //             {
    //                 new OpenApiSecurityRequirement
    //                 {
    //                     {
    //                         new OpenApiSecurityScheme
    //                         {
    //                             Reference = new OpenApiReference
    //                             {
    //                                 Id = "BearerAuth",
    //                                 Type = ReferenceType.SecurityScheme
    //                             }
    //                         },
    //                         Array.Empty<string>()
    //                     }
    //                 }
    //             };
    //         }
    //     }
    // }

}