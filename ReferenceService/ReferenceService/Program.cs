namespace ReferenceService
{
    using Common.Log;
    using NLog.Config;
    using Topshelf;

    /// <summary>
    /// Стартовый класс сервиса
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Точка входа
        /// </summary>
        /// <param name="args">Аргументы командной строки</param>
        public static void Main(string[] args)
        {
            ConfigurationItemFactory.Default.Layouts.RegisterDefinition("LogLayout", typeof(LogLayout));

            HostFactory.Run(host =>
            {

                host.Service(settings =>
                {
                    return new WebApiServiceControl(settings);
                });

                host.SetServiceName("ReferenceService");

                host.SetDisplayName("Reference Service");
                host.SetDescription("Full Reference Service");

                host.RunAsLocalSystem();
                host.StartAutomaticallyDelayed();
            });
        }  
    }
}
