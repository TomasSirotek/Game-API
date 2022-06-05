using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using API.Configuration;
using API.Data;
using API.ExternalServices;
using API.Identity.Entities;
using API.Identity.Managers;
using API.Repositories;
using API.Services;
using API.Services.Interfaces;
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
           //  services.AddDbContext<DataContext>(options => options.UseNpgsql(Configuration.GetConnectionString("PostgresAppCon")));
          
          // services.AddEntityFrameworkNpgsql()
          //       .AddDbContext<DataContext>(o => Configuration.GetConnectionString("PostgresAppCon"));
            // Enable CORS
            services.AddCors(c =>
            {
               c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });


            // services.AddDefaultIdentity<AppUser>(options =>
            //     {
            //         options.Password.RequireDigit = false;
            //         options.Password.RequireLowercase = false;
            //         options.Password.RequireNonAlphanumeric = false;
            //         options.Password.RequireUppercase = false;
            //         options.Password.RequiredLength = 4;
            //         options.User.AllowedUserNameCharacters =
            //             "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            //
            //     })
            //     .AddRoles<AppRole>()
            //     .AddEntityFrameworkStores<DataContext>();

            // needs more look into it 
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Authenticate";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            
            // wtf I need this ? 
            // JSON Serialization // Nuget package
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            
             // Email Service ( could use reformat and taking from appsettings ) 
             // Smtp Sender settings 
            services
                .AddFluentEmail(Configuration.GetSection("Email")["Sender"])
                .AddRazorRenderer()
                .AddSmtpSender(new SmtpClient(Configuration.GetSection("Email")["Server"])
                {
                    UseDefaultCredentials = false,
                    Port = Convert.ToInt32(Configuration.GetSection("Email")["Port"]) ,
                    Credentials = new NetworkCredential("willard.orn27@ethereal.email", "HAG3njycGFEprwfqVj"),
                    EnableSsl = true,
                });
            
            // dependency injection
            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IAppRoleService, AppRoleService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            
            services.AddScoped<IJWToken, JWToken>();
            services.AddScoped<IEmailService, EmailService>();
            
          
            
            services.AddMvc ();
            services.AddRazorPages ();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); 

            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
            
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

            var key = Encoding.UTF8.GetBytes(Configuration["JwtConfig:Secret"]);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = false;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });

        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
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

            app.UseStaticFiles();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
          //  CreateRoles(serviceProvider).Wait();
        }
        
        // private async Task CreateRoles(IServiceProvider serviceProvider)
        // {
        //     var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //     var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        //     string[] roleNames = { "Administrator", "User" };
        //     IdentityResult roleResult;
        //     foreach (var roleName in roleNames)
        //     {
        //         var roleExist = await roleManager.RoleExistsAsync(roleName);
        //         if (!roleExist)
        //         {
        //             roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
        //         }
        //     }
        //     AppUser userAdmin = await userManager.Users.FirstOrDefaultAsync(u => u.Email == "new@yahoo.com");
        //     if (userAdmin != null)
        //     {
        //         await userManager.AddToRoleAsync(userAdmin, "Administrator");
        //         await userManager.AddToRoleAsync(userAdmin, "User");
        //     }
        //     AppUser userUser = await userManager.Users.FirstOrDefaultAsync(u => u.Email != "admin@yahoo.com");
        //     if (userUser != null)
        //     {
        //         await userManager.AddToRoleAsync(userUser, "User");
        //     }
        // }
    }


}