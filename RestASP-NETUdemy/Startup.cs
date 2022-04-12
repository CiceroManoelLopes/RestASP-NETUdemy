using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestASP_NETUdemy.Model.Context;
using RestASP_NETUdemy.Business;
using RestASP_NETUdemy.Business.Implementations;
using System;
using System.Collections.Generic;
using RestASP_NETUdemy.Repository;
using Serilog;
using RestASP_NETUdemy.Repository.Generic;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Rewrite;
using RestASP_NETUdemy.Token.Service;
using RestASP_NETUdemy.Repository.UserApi;
using RestASP_NETUdemy.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

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
            var _tokenConfiguration = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("Settings.TokenConfigurations")).Configure(_tokenConfiguration);
            services.AddSingleton(_tokenConfiguration);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _tokenConfiguration.Issuer,
                    ValidAudience = _tokenConfiguration.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfiguration.Secret))
                };
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(
                    "Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
            
            services.AddControllers();

            var connection = Configuration["MySQLConnection:MySQLConnectionString"];

            //Primeiro Parametro String conexao pegando do JSON de configuração
            //Segundo Paranetro solicitado pelo meu .NET serverVersion, coloquei null e validar
            services.AddDbContext<MySQLContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));

            //Verificação para Migration
            if (Environment.IsDevelopment())
            {
                LocalMigrateDatabase(connection);
            }

            //07-04-2022 Cicero Lopes
            //Versionamento de API 
            services.AddApiVersioning();


            //Injeção do Swegger
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Rest .Net Core 5 - Cícero Lopes" }); });

            //Content Negociation
            //Devolver XML ou JSON conforme requisição
            services.AddMvc(Options =>
            {
                var mediaFormatterParaXML = Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/xml");
                var mediaFormatterParaJson = Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");

                Options.RespectBrowserAcceptHeader = true;
                Options.FormatterMappings.SetMediaTypeMappingForFormat("json", mediaFormatterParaJson);
                Options.FormatterMappings.SetMediaTypeMappingForFormat("xml", mediaFormatterParaXML);

            })
            .AddXmlSerializerFormatters();


            //06-04-2022 Cícero Lopes
            //Injeção de Dependencia
            services.AddScoped<IPessoaBusiness, PessoaBusinessImplementation>();           
            services.AddScoped<IBooksBusiness, BooksBusinessImplementation>();
            services.AddScoped<ILoginBusiness, LoginBusinessImpl>();

            services.AddTransient<ITokenService, TokenServiceImpl>();

            services.AddScoped<IUserRepository, UserRepositoryImpl>();
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepositoryImpl<>));       
        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();

            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                                  "REST API Core 5 - Cicero Lopes v1");
            });

            var option = new RewriteOptions();
            option.AddRedirect("^$","swagger");
            app.UseRewriter(option);

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
