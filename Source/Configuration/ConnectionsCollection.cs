using System.Configuration;


namespace PZone.Configuration
{
    [ConfigurationCollection(typeof(ConnectionElement), AddItemName = "connection")]
    public class ConnectionsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ConnectionElement();
        }


        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ConnectionElement)(element)).Name;
        }

        public ConnectionElement this[int idx] => (ConnectionElement)BaseGet(idx);
    }
}