using BaseAPI;
using Extensions;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Models.AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.IO;
using System.Text;
using Utilities;

namespace API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            Log.Logger = new Serilog.LoggerConfiguration()
              .ReadFrom.Configuration(Configuration)
              .CreateLogger();

            Configuration = configuration;
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        readonly string SignalROrigins = "SignalROrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();
            //SQL
            services.AddDbContext<AppDbContext.AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ELibraryDbContext"));
            });
            //Automapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(AppAutoMapper).Assembly);

            services.AddHttpClient();

            //Using ..BaseAPI/ServiceExtensions
            services.ConfigureRepositoryWrapper();
            services.ConfigureService();
            services.ConfigureSwagger();

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder
                    .WithOrigins("https://localhost:5001")
                    .AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   ;
                });
                options.AddPolicy(SignalROrigins,
                builder =>
                {
                    builder
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials()
                   .SetIsOriginAllowed(hostName => true)
                   ;
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();

            services.AddSession(options =>
            {
                //// Set a short timeout for easy testing.
                //options.IdleTimeout = TimeSpan.FromSeconds(10);
                //options.Cookie.HttpOnly = true;
                //// Make the session cookie essential
                //options.Cookie.IsEssential = true;
            });

            var key = Encoding.ASCII.GetBytes(appSettings.secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSignalR();

            //// 200 MB
            //const int maxRequestLimit = 209715200;
            //services.Configure<IISServerOptions>(options =>
            //{
            //    options.MaxRequestBodySize = maxRequestLimit;
            //});
            //////nếu dùng kes
            ////services.Configure<KestrelServerOptions>(options =>
            ////{
            ////    options.Limits.MaxRequestBodySize = maxRequestLimit;
            ////});
            //services.Configure<FormOptions>(x =>
            //{
            //    x.ValueLengthLimit = maxRequestLimit;
            //    x.MultipartBodyLengthLimit = maxRequestLimit;
            //    x.MultipartHeadersLengthLimit = maxRequestLimit;
            //});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            loggerFactory.AddSerilog();

            //serviceProvider.MigrationDatabase(Configuration);

            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error-local-development");
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "Upload")),
                RequestPath = string.Empty
            });

            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(
            //         Path.Combine(env.ContentRootPath, "Template")),
            //    RequestPath = "/Template",
            //});

            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(
            //         Path.Combine(env.ContentRootPath, "Upload/User")),
            //    RequestPath = "/User",
            //});

            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(
            //         Path.Combine(env.ContentRootPath, "Upload/QRCode")),
            //    RequestPath = "/QRCode",
            //});
            app.UseStaticHttpContext();

            app.UseSession();
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<JwtMiddleware>();
            app.UseAuthorization();

            //dùng cho build static
            app.UseDefaultFiles();
            app.UseStaticFiles();

            //Hangfire
            app.UseHangfireDashboard();
            //RecurringJob.AddOrUpdate<IDayOffConfigService>(
            //    "Cập nhật trạng thái ngày nghỉ - Chạy một giờ một lần",
            //    x => x.UpdateStatus(),
            //    "* */1 * * *");

            //# field #   meaning        allowed values
            //# -------   ------------   --------------
            //#    1      minute         0-59
            //#    2      hour           0-23
            //#    3      day of month   1-31
            //#    4      month          1-12 (or names, see below)
            //#    5      day of week    0-7 (0 or 7 is Sun, or use names)

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "source base");
                c.InjectStylesheet("../css/swagger.min.css");
                c.RoutePrefix = "docs";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/hubs/notifications").RequireCors(SignalROrigins);
                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}"
                  );
            });
        }
    }
}
