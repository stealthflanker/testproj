namespace ReferenceService.WebApi.ServiceClients
{
    using Autofac;

    /// <summary>
    /// Модуль зависимостей
    /// </summary>
    public class ServiceClientsModule : Module
    {
        /// <summary>
        /// Регистрация зависимостей
        /// </summary>
        /// <param name="builder">Контейнер зависимостей <see cref="ContainerBuilder"/></param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            
            builder.RegisterType<PrintServerBridge>().AsImplementedInterfaces();
            builder.RegisterType<EventHub>().AsImplementedInterfaces();
        }
    }
}
