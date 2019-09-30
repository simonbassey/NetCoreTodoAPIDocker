using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using TodoDockerAPI.Core.Abstractions.Repositories;
using TodoDockerAPI.Data.Repositories;
using TodoDockerAPI.Core.Abstractions.Services;
using TodoDockerAPI.Services;

namespace TodoDockerAPI.API.Helpers.Extensions
{
    public static class StartupConfigExtension
    {

        public static CorsOptions ConfigureCorsPolicy(this CorsOptions corsOptions)
        {
            corsOptions.AddPolicy("AllowAll",
                                  corsPolicyBuilder => corsPolicyBuilder
                                  .AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .AllowAnyOrigin()
                                 );
            return corsOptions;
        }

        public static SwaggerGenOptions ConfigureSwagger(this SwaggerGenOptions options)
        {
            options.SwaggerDoc("DockerTodo_API_v1", new Info
            {
                Title = "Stanbic NetCore Todo API",
                Version = "v1",
                Description = "Documentation for Docker AspNetCore Todo API"
            });
            /*
            options.AddSecurityDefinition("Bearer", new ApiKeyScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "USER-KEY",
                In = "header",
                Type = "apiKey"
            });
            options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
            {
                { "Bearer", new string[] { } }
            });
            */
            options.ResolveConflictingActions((description) => description.First());
            return options;
        }


        public static SwaggerUIOptions ConfigureSwaggerUI(this SwaggerUIOptions options)
        {
            options.SwaggerEndpoint("/swagger/DockerTodo_API_v1/swagger.json", "Docker Todo API Docs");
            options.RoutePrefix = string.Empty;
            return options;
        }

        /// <summary>
        /// Adds the entity framework provider based on dbOptions in appSettings.Json
        /// </summary>
        /// <returns>The entity framework provider.</returns>
        /// <param name="services">Services.</param>
        /// <param name="configuration">Configuration.</param>
        public static IServiceCollection AddEntityFrameworkProvider(this IServiceCollection services, IConfiguration configuration)
        {
            var dbProviderIsSQLServer = Convert.ToBoolean(configuration["dbOptions:useSQLServer"]);
            if (dbProviderIsSQLServer)
                return services.AddEntityFrameworkSqlServer();
            return services.AddEntityFrameworkSqlite();
        }

        public static IServiceCollection ConfigureIOCContainer(this IServiceCollection services)
        {
            RegisterRepositories(ref services);
            RegisterServices(ref services);
            return services;
        }

        private static IServiceCollection RegisterRepositories(ref IServiceCollection services)
        {
            services.AddTransient<ITodoRepository, TodoRepository>();

            return services;
        }
        private static IServiceCollection RegisterServices(ref IServiceCollection services)
        {
            services.AddTransient<ITodoService, TodoService>();
            return services;
        }
    }
}
