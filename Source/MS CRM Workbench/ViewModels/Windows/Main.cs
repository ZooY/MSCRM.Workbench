using System.Windows.Controls;
using PZone.ViewModels.Pages;


namespace PZone.ViewModels.Windows
{
    public class Main : ViewModel
    {
        private Page _page;


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
            Page = new PZone.Views.Pages.Publisher();
        }
    }
}