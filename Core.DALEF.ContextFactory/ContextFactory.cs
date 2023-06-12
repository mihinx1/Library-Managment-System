using Core.DALEF.ContextFactory.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Core.DALEF.ContextFactory;

internal class ContextFactory : IContextfactory, IDesignTimeDbContextFactory<Context>
{
    public Context CreateDbContext(string[] args)
    {
        return CreateContext(Csla.Configuration.ConfigurationManager.ConnectionStrings["ContextConnectionString"].ConnectionString);
    }

    Context IDesignTimeDbContextFactory<Context>.CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.development.json")
            .Build();

        var builder = new DbContextOptionsBuilder<Context>();
        var connectionString = configuration.GetConnectionString("ContextConnectionString");

        builder.UseSqlServer(connectionString);

        return new Context(builder.Options);
    }

    private static Context CreateContext(string connectionString)
    {
        var contextOptions = new DbContextOptions<Context>();

        // Switch connection string if another database is selected
        if (!string.IsNullOrEmpty(connectionString))
        {
            var contextOptionsBuilder = new DbContextOptionsBuilder<Context>();
            contextOptions = contextOptionsBuilder.UseSqlServer(connectionString).Options;
        }

        return new Context(contextOptions);
    }
}
