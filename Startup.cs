using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Text;
using API.Configuration;
using API.Engines.Cryptography;
using API.ExternalServices.Email;
using API.Identity.Services.Role;
using API.Identity.Services.User;
using API.Repositories;
using API.Repositories.Role;
using API.Repositories.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Npgsql;
using Swashbuckle.AspNetCore.Filters;

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
            string dbConnectionString = Configuration.GetConnectionString("PostgresAppCon");
            // Inject IDbConnection, with implementation from SqlConnection class.
            services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection(dbConnectionString));
            // Enable CORS
            services.AddCors(c =>
            {
               c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            
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
            
            // JSON Serialization // Nuget package
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            
             // Smtp Sender settings 
            services
                .AddFluentEmail(Configuration.GetSection("Email")["Sender"])
                .AddRazorRenderer()
                .AddSmtpSender(new SmtpClient(Configuration.GetSection("Email")["Server"])
                {
                    UseDefaultCredentials = false,
                    Port = Convert.ToInt32(Configuration.GetSection("Email")["Port"]) ,
                    Credentials = new NetworkCredential(Configuration.GetSection("Email")["Auth:UserName"],Configuration.GetSection("Email")["Auth:Password"]),
                    EnableSsl = true,
                });
            
            // dependency injection container 
            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IAppRoleService, AppRoleService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IJWToken, JWToken>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICryptoEngine, CryptoEngine>();
            // DI end
            
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
        }
    }


}