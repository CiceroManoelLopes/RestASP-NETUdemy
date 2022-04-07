using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestASP_NETUdemy.Model.Context;
using RestASP_NETUdemy.Business;
using RestASP_NETUdemy.Business.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestASP_NETUdemy.Repository;
using RestASP_NETUdemy.Repository.Implementations;

namespace RestASP_NETUdemy
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
            services.AddControllers();

            var connection = Configuration["MySQLConnection:MySQLConnectionString"];

            //Primeiro Parametro String conexao pegando do JSON de configuração
            //Segundo Paranetro solicitado pelo meu .NET serverVersion, coloquei null e validar
            services.AddDbContext<MySQLContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));

            //07-04-2022 Cicero Lopes
            //Versionamento de API 
            services.AddApiVersioning();


            //06-04-2022 Cícero Lopes
            //Injeção de Dependencia
            services.AddScoped<IPessoaBusiness, PessoaBusinessImplementation>(); 
            services.AddScoped<IPessoaRepository, PessoaRepositoryImplementation>(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
