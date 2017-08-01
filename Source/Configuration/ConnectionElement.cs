using System.Configuration;


namespace PZone.Configuration
{
    public class ConnectionElement : ConfigurationElement
    {

        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("orgname", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string OrgName
        {
            get { return (string)base["orgname"]; }
            set { base["orgname"] = value; }
        }

        [ConfigurationProperty("host", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Host
        {
            get { return (string)base["host"]; }
            set { base["host"] = value; }
        }

        [ConfigurationProperty("sqlhost", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string SqlHost
        {
            get { return (string)base["sqlhost"]; }
            set { base["sqlhost"] = value; }
        }

        [ConfigurationProperty("project", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string ProjectPath
        {
            get { return (string)base["project"]; }
            set { base["project"] = value; }
        }
    }
}
