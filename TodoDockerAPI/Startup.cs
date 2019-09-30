using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoDockerAPI.API.Helpers.Extensions;
using TodoDockerAPI.Core.Helpers;
using TodoDockerAPI.Data.Core;
using Microsoft.EntityFrameworkCore;

namespace TodoDockerAPI
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
            services.AddDbContext<TodoDbContext>(/*options => options.UseSqlite(Configuration.GetConnectionString("TodoAppDb")*/);
            services.AddEntityFrameworkProvider(Configuration);
            services.AddCors(options => options.ConfigureCorsPolicy());
            services.Configure<IISOptions>(config => config.ForwardClientCertificate = false);
            //register framework services
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(options => options.ConfigureSwagger());
            services.ConfigureIOCContainer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options => options.ConfigureSwaggerUI());
            app.UseMvc();

            app.UseCors("AllowAll");
            ServiceResolver.Register(app.ApplicationServices);
        }
    }
}
