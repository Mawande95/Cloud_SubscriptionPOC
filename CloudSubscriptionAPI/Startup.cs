﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CloudSubscriptionAPI.Data;
using CloudSubscriptionAPI.Mapper;
using CloudSubscriptionAPI.Repository.IRepository;
using CloudSubscriptionAPI.Repository;
using CloudSubscriptionAPI.Service.Iservice;
using CloudSubscriptionAPI.Service;

namespace CloudSubscriptionAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors(c => c.AddPolicy("AllowOrigin", options => options.WithOrigins("https://localhost:44342")));

            services.AddMemoryCache();


            if (_env.IsProduction())
            {

            }
            else if (_env.IsDevelopment())
            {

            }
            else if (_env.IsStaging())
            {

            }

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICloudServiceRepository, CloudServiceRepository>();
            services.AddScoped<ICloudService, CloudService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddAutoMapper(typeof(APIMappings));

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();
            var appSettingsSection = Configuration.GetSection("AppSettings");
            var appSettingsSection2 = Configuration.GetSection("ConnectionStrings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
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

            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        );
            services.AddAuthorization(
                options =>
                {
                    options.AddPolicy("AddUser", policy => policy.RequireClaim("AddUser"));
                    options.AddPolicy("UpdateUser", policy => policy.RequireClaim("UpdateUser"));

                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider, ILoggerFactory loggerFactory)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
#if DEBUG
                // For Debug in Kestrel
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API V1");
#else
               // To deploy on IIS
               c.SwaggerEndpoint("/mysite/swagger/v1/swagger.json", "Web API V1");
#endif
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            // app.UseCors(x => x.WithOrigins("https://localhost:44342"));
            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseElmahIo();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}

