namespace ReferenceService.WebApi
{
    using System.Reflection;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;
    using System.Web.Http.Validation;
    using Autofac;
    using Autofac.Integration.WebApi;
    using Common;
    using Filters;
    using Repositories;
    using ServiceClients;
    using ServiceClients.Mock;

    /// <summary>
    /// Приложение WebApi
    /// </summary>
    public static class WebApplication
    {
        /// <summary>
        /// Инициализация приложения
        /// </summary>
        /// <param name="configuration">HTTP-конфигурация</param>
        /// <param name="useMock">Использовать Mock</param>
        public static void Initialize(HttpConfiguration configuration, bool useMock = false)
        {
            ConfigureDependencies(configuration, useMock);
            ConfigureApi(configuration);
            RegisterRoutes(configuration);

            configuration.EnsureInitialized();
        }

        /// <summary>
        /// Конфигурирование зависимостей в Autofac
        /// </summary>
        /// <param name="configuration">HTTP-конфигурация</param>
        /// /// <param name="useMock">Использовать Mock</param>
        private static void ConfigureDependencies(HttpConfiguration configuration, bool useMock = false)
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            if (useMock)
            {
                builder.RegisterModule<ServiceClientsModuleMock>();
            }
            else
            {
                builder.RegisterModule<ServiceClientsModule>();
            }

            builder.RegisterModule<RepositoriesModule>();

            var container = builder.Build();
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        /// <summary>
        /// Конфигурирование WebApi
        /// </summary>
        /// <param name="configuration">HTTP-конфигурация</param>
        private static void ConfigureApi(HttpConfiguration configuration)
        {
            // Message handlers
            configuration.MessageHandlers.Add(new HttpErrorMessageHandler());

            // Filters
            configuration.Filters.Add(new ExceptionFilterAttribute());

            // CORS
            configuration.EnableCors();

            // Clear formatters
            var jsonFormatter = configuration.Formatters.JsonFormatter;
            configuration.Formatters.Clear();
            configuration.Formatters.Add(jsonFormatter);


            configuration.MapHttpAttributeRoutes();

            configuration.Services.Clear(typeof(IBodyModelValidator));

            // Configure selectors
            configuration.Services.Replace(typeof(IHttpControllerSelector), new ControllerSelector(configuration));
            configuration.Services.Replace(typeof(IHttpActionSelector), new ControllerActionSelector());
        }

        /// <summary>
        /// Конфигурирование роутинга
        /// </summary>
        /// <param name="configuration">HTTP-конфигурация</param>
        private static void RegisterRoutes(HttpConfiguration configuration)
        {
            configuration.Routes.MapHttpRoute(
                name: "Error404",
                routeTemplate: "{*url}",
                defaults: new { controller = Constants.ErrorControllerName, action = Constants.ErrorNotFoundHandle }
            );

            configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
