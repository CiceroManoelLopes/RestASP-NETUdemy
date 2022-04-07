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
using Serilog;

namespace RestASP_NETUdemy
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        //Migration e DataSet
        public IWebHostEnvironment Environment { get; }        
        public Startup(IConfiguration configuration, IWebHostEnvironment environment) //Sgundo paramertro para o migration (controle de versoes banco de dados)
        {
            Configuration = configuration;
            
            //Migration e DataSet
            Environment = environment;

            //Migration e DataSet e Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }      

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddControllers();

            var connection = Configuration["MySQLConnection:MySQLConnectionString"];

            //Primeiro Parametro String conexao pegando do JSON de configuração
            //Segundo Paranetro solicitado pelo meu .NET serverVersion, coloquei null e validar
            services.AddDbContext<MySQL>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));

            //Verificação para Migration
            if (Environment.IsDevelopment())
            {
                LocalMigrateDatabase(connection);
            }

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


        private void LocalMigrateDatabase(string connection)
        {
            //Vou Retornar daqui porque o Migration é como o Hibernate, cria e popula base de dados de forma automatica se a base não existir
            return;

            try
            {
                var evolveConnection = new MySql.Data.MySqlClient.MySqlConnection(connection);
                var evolve = new Evolve.Evolve(evolveConnection, msg => Log.Information(msg))
                {
                    Locations = new List<string> { "DbMigration/migration", "DbMigration/dataset" },
                    IsEraseDisabled = true,
                };
                evolve.Migrate();
            
            }
            catch (Exception ex)
            {
                Log.Error("Database Migration Falhou", ex);
                throw;
            }
        }
    }
}
