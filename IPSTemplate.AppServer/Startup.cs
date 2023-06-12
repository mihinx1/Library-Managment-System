using Core.DALEF.Configuration;
using Core.DALEF.ContextFactory;
using Csla.Configuration;
using Hellang.Middleware.ProblemDetails;
using IPSBlazor.Configuration;
using IPSTemplate.AppServer.Filters;
using IPSTemplate.AppServer.Helpers;
using IPSTemplate.AppServer.Middleware;
using IPSTemplate.AppServer.Services;
using IPSTemplate.BusinessLibrary.Interfaces;
using IPSTemplate.Dal.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IPSTemplate.AppServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;

            // https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#using-environment-variables-in-configuration-options
            var envPath = Path.Combine(env.ContentRootPath, ".env");
            if (File.Exists(envPath))
            {
                DotNetEnv.Env.Load();
            }

            // See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#content-formatting
            JsonConvert.DefaultSettings = () =>
                new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Include,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
#if DEBUG
                    Formatting = Formatting.Indented
#else
                    Formatting = Formatting.None
#endif
                };
        }

        public static IConfiguration Configuration { get; private set; } = default!;

        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddProblemDetailsExtended();

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            // end of strongly typed settings objects

            // user management & security services using aspnetcore identity
            services.AddDbContext<Context>(options => options.UseSqlServer(EnvironmentHelper.GetConnectionString("ContextConnectionString")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<TEIdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddRoles<TEIdentityRole>()
        .AddEntityFrameworkStores<Context>()
        .AddDefaultTokenProviders();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<TEIdentityUser>>();
            // end of user management & security services using aspnetcore identity

            // configure Cors
            services.AddCors()
                // Add useful interface for accessing the ActionContext outside a controller.
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                // Add useful interface for accessing the HttpContext outside a controller.
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                // Add useful interface for accessing the IUrlHelper outside a controller.
                .AddScoped(x => x
                    .GetRequiredService<IUrlHelperFactory>()
                    .GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext!))
                .AddMvcCore()
                .AddApiExplorer()
                .AddAuthorization();
            //end of cors 

            // configure default authorization policy builder
            services
                .AddControllers(config =>
                {
                    var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                    var defaultAuthPolicy = defaultAuthBuilder
                        .RequireAuthenticatedUser()
                        .Build();
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());

#if DEBUG
                    options.SerializerSettings.Formatting = Formatting.Indented;
#else
                        options.SerializerSettings.Formatting = Formatting.None;
#endif
                });
            // end of configure default authorization policy builder

            //configure Swagger
            services.AddApiVersioning(Configuration);
            // Check out Configuration/DependenciesConfig.cs/AddSwagger to do actual configuration. See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#documenting-api
            services.AddSwagger();

            //ignore methods that don't have a specified HttpMethod in Swagger See: https://github.com/domaindrivendev/Swashbuckle/issues/581
            services.AddSwaggerGen(c =>
            {
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out System.Reflection.MethodInfo methodInfo))
                        return false;

                    if (apiDesc.HttpMethod == null)
                        return false;

                    //check api descriptor group name (descriptors are grouped by ApiVersion)
                    if (apiDesc.GroupName != docName)
                        return false;

                    //filter methods by version https://medium.com/@mustaphaakinsola/custom-api-versioning-with-swashbuckle-asp-net-core-2-2-bd7b9beae2a
                    var controllerVersions = methodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    var actionVersions = methodInfo
                        .GetCustomAttributes(true)
                        .OfType<MapToApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    var controllerAndActionVersionsOverlap = controllerVersions.Intersect(actionVersions).Any();
                    var versions = controllerAndActionVersionsOverlap ? actionVersions : controllerVersions;

                    return versions.Any(v => $"{v}" == docName);
                });
                c.OperationFilter<MultipleOperationsWithSameVerbFilter>();
            });
            // end of Swagger

            services.AddMvc().AddXmlSerializerFormatters();

            // CSLA settings
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddRazorPages();
            services.AddServerSideBlazor().AddHubOptions(config => config.MaximumReceiveMessageSize = 1048576 * 100);

            services.AddLocalization();
            services.AddScoped(httpClient => new HttpClient
            {
                BaseAddress = new Uri(Configuration["AppSettings:BaseAddress"]),
                Timeout = TimeSpan.FromSeconds(Convert.ToInt32(Configuration["AppSettings:HttpClientTimeout"])),
            });
            services.AddHttpContextAccessor();
            services.AddDalEfCore();
            services.AddCsla(o => o.AddAspNetCore().AddServerSideBlazor());
            // end of CSLA settings

            services.AddIPSBlazor();
            services.AddScoped<IUserService, UserService>();

            //Configure StartUp routines
            services.AddTransient<IStartupFilter, StartupFilter>();
            // end of StartUp routines
        }

        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var serviceProvider = app.ApplicationServices;

            // Use an exception handler middleware before any other handlers
            // See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#unhandled-exceptions-handling
            app.UseProblemDetails();

            // See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#cross-origin-resource-sharing-cors-and-preflight-requests
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                //.AllowCredentials() //Specifying AllowAnyOrigin and AllowCredentials is an insecure configuration and can result in cross-site request forgery. The CORS service returns an invalid CORS response when an app is configured with both methods.
                );

            app.UseOptionsVerbHandler();    // Options verb handler must be added after CORS. See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#cross-origin-resource-sharing-cors-and-preflight-requests
            app.UseSwaggerWithOptions(serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>());   // Check out Configuration/MiddlewareConfig.cs/UseSwaggerWithOptions to do actual configuration. See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#documenting-api

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            var supportedCultures = UI.Blazor.Constants.SupportedCultures.Select(sc => sc.Culture).ToList();
            var supportedUICultures = UI.Blazor.Constants.SupportedUICultures.Select(sc => sc.Culture).ToList();
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(
                    UI.Blazor.Constants.SupportedCultures.First().Culture,
                    UI.Blazor.Constants.SupportedUICultures.First().Culture),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedUICultures
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                if (env.IsDevelopment())
                {
                    endpoints.MapControllers().WithMetadata(new AllowAnonymousAttribute());
                }
                else
                {
                    endpoints.MapControllers();
                }

                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            app.UseApiVersioning();
            app.UseCsla();

            //startup routine is done in StartupFilter
        }
    }
}
