using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models; // Import necessário para OpenApiInfo
using Swashbuckle.AspNetCore.SwaggerGen; // Opcional, mas útil para configurações avançadas
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using backend_registro_livros.Data; // Namespace do DbContext
using Pomelo.EntityFrameworkCore.MySql.Infrastructure; // Para usar MySQL com Pomelo
using backend_registro_livros.Utils;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;

namespace backend_registro_livros
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
            // Adiciona as configurações de CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Backend - Registro Livros",
                        Version = "v1",
                        Description = "Biblioteca - Cadastro de Livros"
                    });
            });

            services.AddDbContext<BibliotecaContext>(options =>
            {
                options
                .UseMySql(
                    Configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 21)) // Configura a versão do MySQL
                )
                
                .EnableSensitiveDataLogging() // Se precisar de dados sensíveis no log
                .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.MultipleCollectionIncludeWarning)); // Configuração de warnings
            });


            services.AddControllers(options =>
            {
                // Adiciona uma convenção para transformar tokens de rota em letras minúsculas
                options.Conventions.Add(new RouteTokenTransformerConvention(new LowerCaseTransformer()));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblioteca API V1");
            });

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
