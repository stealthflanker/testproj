namespace ReferenceService
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Web.Http.SelfHost;
    using Common;
    using NLog;
    using NLog.Layouts;
    using Topshelf;
    using Topshelf.Runtime;
    using WebApi;

    /// <summary>
    /// Управляет Windows-сервисом WebApi
    /// </summary>
    internal class WebApiServiceControl : ServiceControl
    {
        private HostSettings _settings;

        private HttpSelfHostServer _server; 

        public WebApiServiceControl(HostSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Запускает сервис
        /// </summary>
        /// <param name="hostControl">HostControl</param>
        /// <returns>Результат запуска сервиса</returns>
        public bool Start(HostControl hostControl)
        {
            if (_server != null)
            {
                return false;
            }

            ReferenceServiceConfiguration _configuration;
            try
            {
                _configuration = ReferenceServiceConfiguration.Instance;
            }
            catch(ConfigurationErrorsException ex)
            {
                Console.WriteLine("[Failed] " + ex.Message);
                return false;
            }

            PrepareLog(_configuration.LogDir, _settings.InstanceName);

            InstanceConfigurationElement instConf = null;
            foreach (InstanceConfigurationElement instanceConf in _configuration.Instances)
            {
                if (instanceConf.Name == "@" + _settings.InstanceName)
                {
                    instConf = instanceConf;
                }
            }

            if (instConf == null)
            {
                Console.WriteLine(
                    string.Format("[Failed] Configure instance {0} in hc.reference/instances section of the .config file", _settings.InstanceName)
                );
                return false;
            }

            var baseUrl = new Uri(instConf.BaseUrl);
            var httpConf = new HttpSelfHostConfiguration(baseUrl);
            var server = new HttpSelfHostServer(httpConf);

            WebApplication.Initialize(httpConf, _configuration.UseMock);

            server.OpenAsync().Wait();

            Console.WriteLine("Listening {0}", baseUrl);

            _server = server;
            return true;
        }

        /// <summary>
        /// Останавливает сервис
        /// </summary>
        /// <param name="hostControl">HostControl</param>
        /// <returns>Результат остановки сервиса</returns>
        public bool Stop(HostControl hostControl)
        {
            if (_server == null)
            {
                return false;
            }

            _server.CloseAsync().Wait();
            _server = null;

            return true;
        }

        /// <summary>
        /// Предоставляет FQDN
        /// </summary>
        /// <returns>FQDN</returns>
        private static string GetFQDN()
        {
            string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            string hostName = Dns.GetHostName();

            if (!string.IsNullOrWhiteSpace(domainName))
            {
                domainName = "." + domainName;
                if (!hostName.EndsWith(domainName))
                {
                    hostName += domainName;
                }
            }

            return hostName.ToLowerInvariant();
        }

        /// <summary>
        /// Конфигурирование логирования
        /// </summary>
        /// <param name="logDir">Базовая директория с логами</param>
        /// <param name="instanceName">Название экземпляра windows-сервиса</param>
        private static void PrepareLog(string logDir, string instanceName)
        {
            var variables = LogManager.Configuration.Variables;
            variables.Add("fqdn", new SimpleLayout(GetFQDN()));
            variables.Add("logDir", new SimpleLayout(logDir));
            variables.Add("instance", new SimpleLayout(instanceName));
        }
    }
}
