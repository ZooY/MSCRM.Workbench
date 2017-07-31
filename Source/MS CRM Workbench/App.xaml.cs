using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.ServiceModel.Description;
using System.Windows;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using PZone.Configuration;


namespace PZone
{
    // ReSharper disable once RedundantExtendsListEntry
    public partial class App : Application
    {
        private static readonly string _connectionString;
        private static readonly Lazy<SqlConnection> _db;

        public static IOrganizationService Service { get; set; }

        public static SqlConnection Db => _db.Value;


        static App()
        {
            var section = (StartupCrmSettingsSection)ConfigurationManager.GetSection("crmSettings");
            var connection = section.Connections[0];
            var serviceUrl = $"http://{connection.Host}/{connection.OrgName}/XRMServices/2011/Organization.svc";
            var credentials = new ClientCredentials();
            credentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
            Service = new OrganizationServiceProxy(new Uri(serviceUrl), null, credentials, null);

            _connectionString = $"Data Source={connection.SqlHost};Initial Catalog={connection.OrgName}_MSCRM;Integrated Security=True;Connect Timeout=60";
            _db = new Lazy<SqlConnection>(() =>
            {
                var con = new SqlConnection(_connectionString);
                con.Open();
                return con;
            });
        }
    }
}


// https://toster.ru/q/231775
// https://stackoverflow.com/questions/15936428/binding-contentcontrol-content-for-dynamic-content