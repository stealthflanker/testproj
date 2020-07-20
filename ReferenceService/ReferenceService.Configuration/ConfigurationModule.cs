namespace ReferenceService.Configuration
{
    using Autofac;

    /// <summary>
    /// Autofac-модуль конфигураций
    /// </summary>
    public class ConfigurationModule : Module
    {
        /// <summary>
        /// Регистрирует типы в Autofac-контейнере
        /// </summary>
        /// <param name="builder">Фабрика контейнера</param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<AppConfigReader>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
