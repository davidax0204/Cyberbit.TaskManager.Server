using Cyberbit.TaskManager.Server.AuthorizeHelpers;
using Cyberbit.TaskManager.Server.Bl;
using Cyberbit.TaskManager.Server.Dal;
using Cyberbit.TaskManager.Server.Interfaces;
using Cyberbit.TaskManager.Server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cyberbit.TaskManager.Server
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
            services.Configure<MvcNewtonsoftJsonOptions>(config =>
                config.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddDbContext<BackendDbContext>(opt => opt.UseSqlite("Data Source=TaskManager.db"));
            services.AddScoped<IUsersBl, UsersBl>();
            services.AddScoped<IUsersDal, UsersDal>();
            services.AddScoped<ITasksBl, TasksBl>();
            services.AddScoped<ITasksDal, TasksDal>();
            services.AddScoped<ICategoriesBl, CategoriesBl>();
            services.AddScoped<ICategoriesDal, CategoriesDal>();
            services.AddSingleton(typeof(IAutoMapperService), typeof(AutoMapperService));

            services.AddControllers().AddNewtonsoftJson();
            services.AddCors(o => o.AddPolicy("MyCorsPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:4200", "http://localhost:4203", "http://localhost:4201", "http://localhost:4202")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cyberbit.TaskManager.Server", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });

            SeedDefaultData(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cyberbit.TaskManager.Server v1");
                });
            }

            app.UseCors("MyCorsPolicy");
            app.UseRouting();
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void SeedDefaultData(IServiceCollection services)
        {
            using (var servicesContainer = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = servicesContainer.ServiceProvider.GetRequiredService<BackendDbContext>();

                // Note:
                // Any data inserted to the DB is deleted on load
                // To persist data remark the following line
                dbContext.Database.EnsureDeleted();

                var notExisted = dbContext.Database.EnsureCreated();
                if (notExisted)
                    dbContext.SeedMockData();
            }
        }
    }
}
