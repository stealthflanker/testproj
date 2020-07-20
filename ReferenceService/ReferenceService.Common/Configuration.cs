namespace ReferenceService.Common
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Конфигурация экземпляра windows-сервиса
    /// </summary>
    public class InstanceConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// Название экземпляра
        /// </summary>
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
        }

        /// <summary>
        /// Базовый URL сервиса
        /// </summary>
        [ConfigurationProperty("baseUrl", IsRequired = true)]
        public string BaseUrl
        {
            get { return (string)this["baseUrl"]; }
        }
    }

    /// <summary>
    /// Колллекция конфигураций экземпляра windows-сервиса
    /// </summary>
    public class InstanceConfigurationCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Создаёт новый элемент конфигурация экземпляра windows-сервиса
        /// </summary>
        /// <returns>Конфигурация экземпляра windows-сервиса</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new InstanceConfigurationElement();
        }

        /// <summary>
        /// Предоставляет ключ коллекции конфигураций экземпляра windows-сервиса
        /// </summary>
        /// <param name="element">Конфигурация экземпляра windows-сервиса</param>
        /// <returns>Ключ коллекции конфигураций экземпляра windows-сервиса</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((InstanceConfigurationElement)element).Name;
        }
    }

    /// <summary>
    /// Конфигурация авторизационных данных
    /// </summary>
    public class CredentialsConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [ConfigurationProperty("username", IsRequired = true)]
        public string UserName
        {
            get { return (string)this["username"]; }
        }

        /// <summary>
        /// Пароль
        /// </summary>
        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get { return (string)this["password"]; }
        }
    }

    /// <summary>
    /// Конфигурация ReferenceService
    /// </summary>
    public class ReferenceServiceConfiguration : ConfigurationSection
    {
        /// <summary>
        /// Колллекция конфигураций экземпляра windows-сервиса
        /// </summary>
        [ConfigurationProperty("instances", IsRequired = true)]
        [ConfigurationCollection(typeof(InstanceConfigurationCollection), AddItemName = "instance")]
        public InstanceConfigurationCollection Instances
        {
            get { return (InstanceConfigurationCollection)this["instances"]; }
        }

        /// <summary>
        /// Базовая директория для лог-файлов
        /// </summary>
        [ConfigurationProperty("logDir", IsRequired = true)]
        public string LogDir
        {
            get { return (string)this["logDir"]; }
        }

        /// <summary>
        /// Использовать Mock
        /// </summary>
        [ConfigurationProperty("useMock", DefaultValue = false)]
        public bool UseMock
        {
            get { return (bool)this["useMock"]; }
        }

        /// <summary>
        /// Конфигурация авторизационных данных для PrintServer
        /// </summary>
        [ConfigurationProperty("printServer", IsRequired = true)]
        public CredentialsConfigurationElement PrintServer
        {
            get { return (CredentialsConfigurationElement)this["printServer"]; }
        }

        /// <summary>
        /// Конфигурация авторизационных данных для EventHub
        /// </summary>
        [ConfigurationProperty("eventHub", IsRequired = true)]
        public CredentialsConfigurationElement EventHub
        {
            get { return (CredentialsConfigurationElement)this["eventHub"]; }
        }

        private static void ProcessMissingElements(ConfigurationElement element)
        {
            foreach (PropertyInformation propertyInformation in element.ElementInformation.Properties)
            {
                var complexProperty = propertyInformation.Value as ConfigurationElement;
                if (complexProperty == null)
                {
                    complexProperty = propertyInformation.Value as ConfigurationElementCollection;
                    if (complexProperty == null)
                    {
                        continue;
                    }
                }

                if (propertyInformation.IsRequired && !complexProperty.ElementInformation.IsPresent)
                {
                    throw new ConfigurationErrorsException(
                        string.Format("Configure section hc.references/{0} of the .config file", propertyInformation.Name)
                    );
                }
                if (!complexProperty.ElementInformation.IsPresent)
                {
                    propertyInformation.Value = null;
                }
                else
                {
                    ProcessMissingElements(complexProperty);
                }
            }

        }

        private static Lazy<ReferenceServiceConfiguration> _configureation = new Lazy<ReferenceServiceConfiguration>(() => {
            var configuration = ConfigurationManager.GetSection("hc.references") as ReferenceServiceConfiguration;
            if(configuration == null)
            {
                throw new ConfigurationErrorsException(
                    string.Format("Configure section hc.references of the .config file")
                );
            }
            ProcessMissingElements(configuration);
            return configuration;
        });

        /// <summary>
        /// Экземепляр конфигурции ReferenceService
        /// </summary>
        public static ReferenceServiceConfiguration Instance
        {
            get { return _configureation.Value; }
        }
    }
}
