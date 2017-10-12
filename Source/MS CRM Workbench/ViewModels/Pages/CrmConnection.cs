using System;
using System.Net;
using System.ServiceModel.Description;
using System.Windows.Input;
using Microsoft.Xrm.Sdk.Client;
using PZone.Helpers;
using PZone.Wpf;


namespace PZone.ViewModels.Pages
{
    public class CrmConnection : ViewModel
    {
        private string _organizationUrl =  GetLastOrganizationUrl();
        private bool _useDefaultCredentials =  !DesignMode;
        private ICommand _connectCommand;


        public string OrganizationUrl
        {
            get { return _organizationUrl; }
            set { SetProperty(ref _organizationUrl, value); }
        }
        

        public bool UseDefaultCredentials
        {
            get { return _useDefaultCredentials; }
            set { SetProperty(ref _useDefaultCredentials, value); }
        }


        public ICommand Connect
        {
            get
            {
                return _connectCommand ?? (_connectCommand = new Command(arg =>
                       {
                           Busy = true;
                           var serviceUrl = UriHelper.Combine(OrganizationUrl, "XRMServices/2011/Organization.svc");
                           var credentials = new ClientCredentials();
                           credentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
                           App.Service = new OrganizationServiceProxy(new Uri(serviceUrl), null, credentials, null);
                           Busy = false;
                           //_connectionString = $"Data Source={connection.SqlHost};Initial Catalog={connection.OrgName}_MSCRM;Integrated Security=True;Connect Timeout=60";
                           //_db = new Lazy<SqlConnection>(() =>
                           //{
                           //    var con = new SqlConnection(_connectionString);
                           //    con.Open();
                           //    return con;
                           //});
                       }));
            }
        }


        public static string GetLastOrganizationUrl()
        {
            return "http://crmdevapp01/DEV01";
        }
    }
}