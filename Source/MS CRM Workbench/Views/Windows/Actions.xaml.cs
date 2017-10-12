using System;
using System.Linq;
using System.Text;
using PZone.Xrm;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xrm.Sdk;


namespace PZone.Views.Windows
{
    /// <summary>
    /// Interaction logic for Actions.xaml
    /// </summary>
    public partial class Actions : Window
    {
        public Actions()
        {
            InitializeComponent();

            var fetchXml = @"
<fetch>
  <entity name=""workflow"" >
    <attribute name=""name"" />
    <filter>
      <condition attribute=""category"" operator=""eq"" value=""3"" />
      <condition attribute=""type"" operator=""eq"" value=""2"" />
    </filter>
    <order attribute=""name"" />
    <link-entity name=""sdkmessage"" from=""sdkmessageid"" to=""sdkmessageid"" alias=""SdkMessage"" >
      <attribute name=""name"" />
      <link-entity name=""sdkmessagepair"" from=""sdkmessageid"" to=""sdkmessageid"" alias=""SdkMessagePair"" >
        <attribute name=""sdkmessagepairid"" />
        <filter>
          <condition attribute=""endpoint"" operator=""eq"" value=""api/data"" />
        </filter>
        <link-entity name=""sdkmessagerequest"" from=""sdkmessagepairid"" to=""sdkmessagepairid"" alias=""SdkMessageRequest"" >
          <attribute name=""sdkmessagerequestid"" />
          <attribute name=""primaryobjecttypecode"" />
        </link-entity>
      </link-entity>
    </link-entity>
  </entity>
</fetch>";
            ActionsList.ItemsSource = App.Service.RetrieveMultiple(fetchXml).Entities;
        }

        private void OnActionSelected(object sender, SelectionChangedEventArgs e)
        {
            var action = (Entity)ActionsList.SelectedItem;
            var primaryObject = action.GetAttributeValue<AliasedValue>("SdkMessageRequest.primaryobjecttypecode").Value.ToString();

            var query = new StringBuilder();
            query.AppendLine($"POST http://{App.ProjectSettings.Host}/{App.ProjectSettings.OrgName}/api/data/v8.2/{action.GetAttributeValue<AliasedValue>("SdkMessage.name").Value} HTTP/1.1");
            query.AppendLine("Accept: application/json");
            query.AppendLine("Content-Type: application/json; charset=utf-8");
            query.AppendLine("OData-MaxVersion: 4.0");
            query.AppendLine("OData-Version: 4.0");

            var fetchXml = $@"
<fetch>
  <entity name=""sdkmessagerequestfield"" >
    <attribute name=""parser"" />
    <attribute name=""name"" />
    <order attribute=""position"" />
    <filter>
        <condition attribute=""sdkmessagerequestid"" operator=""eq"" value=""{action.GetAttributeValue<AliasedValue>("SdkMessageRequest.sdkmessagerequestid").Value}"" />
    </filter>
  </entity>
</fetch>";
           var arguments = App.Service.RetrieveMultiple(fetchXml).Entities;
            if (arguments.Count > 0)
            {
                query.AppendLine(string.Empty);
                query.AppendLine("{");  
                query.AppendLine(string.Join($",{Environment.NewLine}", arguments.Where(a=>a.GetAttributeValue<string>("name")!="Target").Select(a => $@"  ""{a.GetAttributeValue<string>("name")}"": ""<{a.GetAttributeValue<string>("parser").Split(',').First()}>""")));
                if (primaryObject != "none")
                {
                    query.AppendLine(@"  ""Target"": {");
                    query.AppendLine($@"    ""@odata.type"": ""Microsoft.Dynamics.CRM.{primaryObject}"",");
                    query.AppendLine($@"    ""{primaryObject}id"": ""<{Guid.Empty}>""");
                    query.AppendLine(@"  }");
                }
                query.AppendLine("}");
            }

            Query.Text = query.ToString();
        }
    }
}
