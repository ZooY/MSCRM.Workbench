using System;
using PZone.Xrm;
using System.Windows;
using Microsoft.Xrm.Sdk;


namespace PZone.Views.Windows
{
    /// <summary>
    /// Interaction logic for UserPrivileges.xaml
    /// </summary>
    public partial class UserPrivileges : Window
    {
        public UserPrivileges()
        {
            InitializeComponent();

            var fetchXml = @"
<fetch>
  <entity name=""systemuser"">
    <attribute name=""fullname"" />
    <order attribute=""fullname"" />
  </entity>
</fetch>";
            comboBox.ItemsSource =  App.Service.RetrieveMultiple(fetchXml).Entities;


//            fetchXml = @"
//<fetch>
//  <entity name=""systemuser"">
//    <attribute name=""fullname"" />
//    <order attribute=""fullname"" />
//  </entity>
//</fetch>";
//            var users = App.Service.RetrieveMultiple(fetchXml);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var user = (Entity)comboBox.SelectedItem;

            var fetchXml = @"
<fetch>
  <entity name=""role"" >
    <attribute name=""roletemplateid"" />
    <attribute name=""parentroleid"" />
    <attribute name=""name"" />
    <attribute name=""iscustomizable"" />
    <attribute name=""organizationid"" />
    <attribute name=""importsequencenumber"" />
    <attribute name=""businessunitid"" />
    <attribute name=""roleid"" />
    <attribute name=""componentstate"" />
    <attribute name=""roleidunique"" />
    <attribute name=""versionnumber"" />
    <attribute name=""ismanaged"" />
    <attribute name=""parentrootroleid"" />
    <order attribute=""name"" />
    <link-entity name=""systemuserroles"" from=""roleid"" to=""roleid"" >
      <filter>
        <condition attribute=""systemuserid"" operator=""eq"" value=""" + user.Id+@""" />
      </filter>
    </link-entity>
  </entity>
</fetch>";
            var roles = App.Service.RetrieveMultiple(fetchXml).Entities;
            textBox.Text = "";
            foreach (var role in roles)
            {
                textBox.Text += role.GetAttributeValue<string>("name") + Environment.NewLine;
            }
        }
    }
}