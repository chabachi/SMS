using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esquire.common
{
    public class ConfigurationSettings : IConfigurationSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

        public string BaseUrl { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string GoogleClientId { get; set; }
        public string GoogleClientSecret { get; set; }
        public string GoogleRedirectUri { get; set; }
        public string GoogleRegistertUri { get; set; }

        public int AuthenticationMinutes { get; set; }
    }

    public interface IConfigurationSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }

        string Email { get; set; }
        string Password { get; set; }
        string Host { get; set; }
        string Port { get; set; }
        string BaseUrl { get; set; }

        string GoogleClientId { get; set; }
        string GoogleClientSecret { get; set; }
        string GoogleRedirectUri { get; set; }
        string GoogleRegistertUri { get; set; }
        int AuthenticationMinutes { get; set; }

    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class BsonCollectionAttribute : Attribute
    {
        public string CollectionName { get; }

        public BsonCollectionAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}
