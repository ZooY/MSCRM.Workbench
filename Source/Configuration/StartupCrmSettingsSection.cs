using System.Configuration;


namespace PZone.Configuration
{
    public class StartupCrmSettingsSection : ConfigurationSection
    {
        [ConfigurationProperty("connections")]
        public ConnectionsCollection Connections => (ConnectionsCollection)base["connections"];
    }
}