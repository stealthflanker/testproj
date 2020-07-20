namespace ReferenceService.WebApi.ServiceClients.Mock
{
    using Autofac;

    /// <summary>
    /// Модуль зависимостей
    /// </summary>
    public class ServiceClientsModuleMock : Module
    {
        /// <summary>
        /// Регистрация зависимостей
        /// </summary>
        /// <param name="builder">Контейнер зависимостей <see cref="ContainerBuilder"/></param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<PrintServerBridgeMock>().AsImplementedInterfaces();
            builder.RegisterType<EventHubMock>().AsImplementedInterfaces();
        }
    }
}
