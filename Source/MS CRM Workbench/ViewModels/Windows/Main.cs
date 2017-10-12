using System.Windows.Controls;
using Microsoft.Crm.Sdk.Messages;
using PZone.Xrm;


namespace PZone.ViewModels.Windows
{
    public class Main : ViewModel
    {
        private Page _page;
        private string _user;


        public string Host => App.ProjectSettings.Host;
        public string Organization => App.ProjectSettings.OrgName;


        public string User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }


        public Page Page
        {
            get { return _page; }
            private set { SetProperty(ref _page, value); }
        }


        public Main()
        {
            //if (App.Service == null)
                ConnectToCrm();
        }


        public void ConnectToCrm()
        {
            //var connectionPage = new CrmConnection();
            //connectionPage.
            var user = App.Service.Execute<WhoAmIResponse>(new WhoAmIRequest());
            //user.
            Page = new PZone.Views.Pages.Publisher();
        }
    }
}