using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using PZone.Controls;
using PZone.ViewModels;
using PZone.Wpf;


namespace PZone.Models
{
    /// <summary>
    /// Веб-ресурс.
    /// </summary>
    public class WebResource : ObservableObject
    {
        private Guid? _cachedId;
        private Guid _id;
        private string _name;
        private string _displayName;
        private string _description;
        private string _filePath;
        private bool _autoPublish;
        private WebResourceStatus _status;
        private string _statusText;
        private ICommand _publishCommand;
        private WebResourceType _type;

        private readonly Dictionary<string, WebResourceType> _extensionTypeMap = new Dictionary<string, WebResourceType>
        {
            { ".html", WebResourceType.Html }, { ".htm", WebResourceType.Html }, { ".css", WebResourceType.Css }, { ".js", WebResourceType.Js }, { ".xml", WebResourceType.Xml }, { ".png", WebResourceType.Png },
            { ".jpg", WebResourceType.Jpg }, { ".gif", WebResourceType.Gif }, { ".xap", WebResourceType.Xap }, { ".xsl", WebResourceType.Xsl }, { ".ico", WebResourceType.Ico }
        };


        /// <summary>
        /// Идентификатор веб-ресурса.
        /// </summary>
        public Guid Id
        {
            get { return _id; }
            set
            {
                _cachedId = _id;
                SetProperty(ref _id, value);
            }
        }


        /// <summary>
        /// Имя веб-ресурса.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }


        /// <summary>
        /// Отображаемое имя веб-ресурса.
        /// </summary>
        public string DisplayName
        {
            get { return _displayName; }
            set { SetProperty(ref _displayName, value); }
        }


        /// <summary>
        /// Описание веб-ресурса.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }


        /// <summary>
        /// Адрес файла на диске.
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
            set { SetProperty(ref _filePath, value?.ToLower()); }
        }


        /// <summary>
        /// Akfu автоматической публикации изменений.
        /// </summary>
        public bool AutoPublish
        {
            get { return _autoPublish; }
            set { SetProperty(ref _autoPublish, value); }
        }


        /// <summary>
        /// Статус веб-ресурса.
        /// </summary>
        public WebResourceStatus Status
        {
            get { return _status; }
            set
            {
                SetProperty(ref _status, value);
            }
        }

        
        /// <summary>
        /// Текст статуса веб-ресурса.
        /// </summary>
        public string StatusText
        {
            get { return _statusText; }
            set { SetProperty(ref _statusText, value); }
        }


        /// <summary>
        /// Тип веб-ресурса.
        /// </summary>
        public WebResourceType Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }


        public WebResource()
        {
        }


        public WebResource(Entity resource)
        {
            Type = (WebResourceType)resource.GetAttributeValue<OptionSetValue>("webresourcetype").Value;
            Id = resource.Id;
            Name = resource.GetAttributeValue<string>("name");
            DisplayName = resource.GetAttributeValue<string>("displayname");
            Description = resource.GetAttributeValue<string>("description");
        }


        public WebResource(string name, string filePath)
        {
            Name = name;
            FilePath = filePath;
            var fileExtension = Path.GetExtension(filePath);
            Type = _extensionTypeMap[fileExtension];
        }


        public ICommand Publish
        {
            get
            {
                return _publishCommand ?? (_publishCommand = new Command(o =>
                       {
                           Status = WebResourceStatus.Busy;
                           var thread = new Thread(() =>
                           {
                               try
                               {
                                   var id = GetCrmId(Name);
                                   App.Service.Update(new Entity("webresource", id) { ["content"] = Convert.ToBase64String(File.ReadAllBytes(FilePath)) });
                                   App.Service.Execute(new OrganizationRequest
                                   {
                                       RequestName = "PublishXml",
                                       Parameters = new ParameterCollection { new KeyValuePair<string, object>("ParameterXml", $"<importexportxml><webresources><webresource>{id}</webresource></webresources></importexportxml>") }
                                   });

                                   //Dispatcher.BeginInvoke(new Action(delegate
                                   //{
                                   //    publishControl.Content = "Publish";
                                   //    publishControl.Background = SystemColors.ControlBrush;
                                   //}));
                                   Status = WebResourceStatus.Ready;
                               }
                               catch (Exception ex)
                               {
                                   //TODO: Реализовать отображение ошибки.
                                   Status = WebResourceStatus.Error;
                                   StatusText = ex.Message;
                               }
                           });
                           thread.Start();
                       }));
            }
        }


        public void CheckChanged(object sender, FileSystemEventArgs e)
        {
            if (!e.FullPath.Equals(FilePath, StringComparison.OrdinalIgnoreCase))
                return;
            Status = WebResourceStatus.Changes;
            if (AutoPublish)
                Publish.Execute(null);
        }


        private Guid GetCrmId(string name)
        {
            if (_cachedId.HasValue)
                return _cachedId.Value;
            _cachedId = App.Service.RetrieveMultiple(new FetchExpression(
                "<fetch top='1' no-lock='true'>" +
                "<entity name='webresource'>" +
                "<attribute name='webresourceid' />" +
                "<filter>" +
                $"<condition attribute='name' operator='eq' value='{name}' />" +
                "</filter>" +
                "</entity>" +
                "</fetch>")).Entities.FirstOrDefault()?.Id;
            if (_cachedId.HasValue)
                return _cachedId.Value;
            throw new Exception($"Веб-ресурс с именем \"{name}\" не найден в CRM.");
        }
    }
}