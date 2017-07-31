using System.Collections.ObjectModel;
using PZone.ViewModels;


namespace PZone.Models
{
    public enum WebResourceFolderType
    {
        [Image("/UI/publisher.png")]
        Root,
        [Image("/UI/folder.png")]
        Common
    }

    public class WebResourceFolder : ObservableObject
    {
        private string _name;
        private WebResourceFolderType _type = WebResourceFolderType.Common;


        /// <summary>
        /// Название папки.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }


        public WebResourceFolderType Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }


        public ObservableCollection<ObservableObject> Items { get; set; } = new ObservableCollection<ObservableObject>();


        public WebResourceFolder(string name)
        {
            Name = name;
        }
    }
}