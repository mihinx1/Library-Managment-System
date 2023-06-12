using Core.DAL.Interface;
using Core.DAL.Models;
using Core.Library;
using Csla.Configuration;
using IPSBlazor.Configuration;
using IPSTemplate.AppServer.Helpers;

namespace IPSTemplate.AppServer.Filters
{
    public class StartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                // Identity database migration
                using var scope = app.ApplicationServices.CreateScope();

                //CSLA ConfigurationManager
                Csla.Configuration.ConfigurationManager.ConnectionStrings["ContextConnectionString"] = new ConnectionStringSettings() { ConnectionString = EnvironmentHelper.GetConnectionString("ContextConnectionString") };

                // Initialize or. update database
                Initializer.InitializeDB(scope.ServiceProvider.GetService<IRepository<SYSetting, SYSetting>>());

                // Configure IPSBlazor
                IPSBlazorBuilder.CreateBuilder(app.ApplicationServices).ConfigureLogging();

                TConverter.RegisterTypeConverter<string, CustomStringConverter>();

                next(app);
            };
        }
    }
}
