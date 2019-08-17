using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using WebAPI_DistributedSQLServer_Cache.BusinessLayer;
using WebAPI_DistributedSQLServer_Cache.BusinessLayer.Interfaces;
using WebAPI_DistributedSQLServer_Cache.DataaccessLayer;
using WebAPI_DistributedSQLServer_Cache.DataaccessLayer.Interfaces;
using WebAPI_DistributedSQLServer_Cache.DataAdapter;

namespace WebAPI_DistributedSQLServer_Cache
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Student API", Version = "v1" });
            });
            services.AddDistributedSqlServerCache(option =>
            {
                option.ConnectionString = Configuration.GetValue<string>("ConnectionString");
                option.SchemaName = "dbo";
                option.TableName = "cacheTable";
            });
            services.AddAutoMapper(typeof(Startup));
            services.AddDbContext<StudentDBContext>(option=> option.UseInMemoryDatabase("InmemoryDatabase"));
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IStudentDataAdapeter, StudentDataAdapeter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student API");
            });
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
