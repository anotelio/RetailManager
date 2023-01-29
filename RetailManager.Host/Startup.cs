using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RandomNameGeneratorLibrary;
using RetailManager.Contracts.Enums;
using RetailManager.Contracts.Interfaces;
using RetailManager.Contracts.UnitOfWork;
using RetailManager.Data;

namespace RetailManager.WebHost;

public class Startup
{
    public Startup(IWebHostEnvironment env)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("kestrelConfig.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        this.configuration = builder.Build();
    }

    public readonly IConfiguration configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<KestrelServerOptions>(
            this.configuration.GetSection("Kestrel"));

        services.AddControllers();

        services.AddScoped<IRetailManagerUnitOfWork>(_ =>
            new RetailManagerUnitOfWork(GetConnection(DbConnectionType.RetailManagerDb)));

        services.AddScoped<IUserUnitOfWork>(_ =>
            new UserUnitOfWork(GetConnection(DbConnectionType.UsersDb)));

        services.AddScoped<CustomerRepository>();

        services.AddSingleton<IPersonNameGenerator, PersonNameGenerator>();
        services.AddSingleton<RandomNameCreator>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }

    private DbConnection GetConnection(DbConnectionType dbConnectionType) =>
        new SqlConnection(this.configuration
            .GetConnectionString(Enum.GetName(typeof(DbConnectionType), dbConnectionType)));
}
