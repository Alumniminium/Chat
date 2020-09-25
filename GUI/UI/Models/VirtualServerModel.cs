using System.Collections.ObjectModel;
using Avalonia.Media.Imaging;
using GUI.Database;

namespace GUI.UI.Models
{
    public class VirtualServerModel
    {
        private string _iconUrl;
        public int Id;
        public string Name { get; set; } = "DefaultName";
        public Bitmap Icon => new Bitmap(IconUrl);
        public string IconUrl
        {
            get => Db.GetCacheImage(_iconUrl);
            set => _iconUrl = value;
        }
        public ObservableCollection<UserModel> Users;
        public ObservableCollection<ChannelModel> Channels;
    }
}