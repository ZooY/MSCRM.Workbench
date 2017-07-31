using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;


namespace PZone
{
    /// <summary>
    /// Interaction logic for WebResourcesWindow.xaml
    /// </summary>
    public partial class WebResourcesWindow : Window
    {
        public WebResourcesWindow()
        {
            InitializeComponent();
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Name.IsEnabled = false;
            GetButton.IsEnabled = false;

            var webResourceName = Name.Text;

            using (var con = App.Db)
            {
                // Получаем ID веб-ресурса.
                var webResource = App.Service.RetrieveMultiple(new QueryByAttribute("webresource")
                {
                    ColumnSet = new ColumnSet("webresourceid", "name"),
                    Attributes = { "name" },
                    Values = { webResourceName }
                }).Entities.First();
                var webResourceId = webResource.Id;
                WebResourceId.Text = webResourceId.ToString();

                var dependencies = (RetrieveDependentComponentsResponse)App.Service.Execute(new RetrieveDependentComponentsRequest
                {
                    ComponentType = 61 /* Web Resource https://msdn.microsoft.com/en-us/library/gg328546.aspx#bkmk_componenttype*/,
                    ObjectId = webResourceId
                });
                foreach (var entity in dependencies.EntityCollection.Entities)
                {
                    WriteLine();
                    var requiredObjectId = entity.GetAttributeValue<Guid>("requiredcomponentobjectid");
                    string prefix;
                    if (requiredObjectId == webResourceId)
                    {
                        prefix = "dependent";
                        WriteLine("~~~ Зависимый компонент ~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    }
                    else
                    {
                        prefix = "required";
                        WriteLine("~~~ Родительский компонент ~~~~~~~~~~~~~~~~~~~~~~~");
                    }
                    var objectId = entity.GetAttributeValue<Guid>($"{prefix}componentobjectid");
                    var objectType = entity.GetAttributeValue<OptionSetValue>($"{prefix}componenttype").Value;
                    WriteLine($"Solution ID = {entity.GetAttributeValue<Guid>($"{prefix}componentbasesolutionid")}");
                    WriteLine($"Parent ID = {entity.GetAttributeValue<Guid>($"{prefix}componentparentid")}");
                    WriteLine($"Type = ({objectType}) {entity.FormattedValues[$"{prefix}componenttype"]}");
                    WriteLine($"Object ID = {objectId}");
                    GetObjectInfo(App.Service, con, objectId, objectType, webResourceName);
                }
            }

            Name.IsEnabled = true;
            GetButton.IsEnabled = true;
        }


        private void WriteLine(string message = null)
        {
            Result.Text += message + Environment.NewLine;
        }


        private void GetObjectInfo(IOrganizationService service, SqlConnection con, Guid objectId, int objectType, string webResourceName)
        {
            if (objectType == 60)
                GetFormInfo(service, objectId, webResourceName);
            if (objectType == 50)
                GetRibbonInfo(con, objectId, webResourceName);
        }


        private void GetRibbonInfo(SqlConnection con, Guid objectId, string webResourceName)
        {
            WriteLine("\tИнформация о Ribbon:");
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = $"SELECT [CommandDefinition], [Entity], [Command] FROM [RibbonCommand] WHERE [RibbonCustomizationId]='{objectId}'";
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var xml = rdr["CommandDefinition"].ToString();
                        var doc = XElement.Parse(xml);
                        foreach (var action in doc.Element("Actions").Elements())
                        {
                            if (action.Name != "JavaScriptFunction" || action.Attribute("Library").Value != "$webresource:" + webResourceName)
                                continue;
                            WriteLine($"\tEntity Name = {rdr["Entity"]}, Command = {rdr["Command"]}, Function = {action.Attribute("FunctionName").Value}");
                        }
                    }
                }
            }
        }


        private void GetFormInfo(IOrganizationService service, Guid objectId, string webResourceName)
        {
            WriteLine("\tИнформация о форме:");
            var form = service.Retrieve("systemform", objectId, new ColumnSet("objecttypecode", "formid", "type", "name", "description", "formxml"));
            WriteLine($"\tEntity Name = {form.GetAttributeValue<string>("objecttypecode")}");
            WriteLine($"\tId = {form.GetAttributeValue<Guid>("formid")}");
            WriteLine($"\tType = ({form.GetAttributeValue<OptionSetValue>("type").Value}) {form.FormattedValues["type"]}");
            WriteLine($"\tName = {form.GetAttributeValue<string>("name")}");
            WriteLine($"\tDescription = {form.GetAttributeValue<string>("description")}");
            var xml = form.GetAttributeValue<string>("formxml");

            WriteLine("\t\tИспользование на форме:");
            var element = XElement.Parse(xml);

            foreach (var el in element.Element("formLibraries").Elements("Library"))
                if (el.Attribute("name")?.Value == webResourceName)
                    WriteLine("\t\t- загружен как библиотека формы");

            foreach (var eventEl in element.Element("events").Elements("event"))
            {
                var eventName = eventEl.Attribute("name").Value;
                foreach (var handler in eventEl.Element("Handlers").Elements("Handler"))
                    if (handler.Attribute("libraryName")?.Value == webResourceName)
                        WriteLine($"\t\t- используется в обработчике события {eventName}, функция {handler.Attribute("functionName").Value}.");
            }
        }
    }
}