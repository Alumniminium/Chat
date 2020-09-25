using System;
using System.Collections.ObjectModel;
using GUI.Database;

namespace GUI.UI.Models
{
    public class UserModel
    {
        private string _avatarUrl;
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Online { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public string AvatarUrl
        {
            get => Db.GetCacheImage(_avatarUrl);
            set => _avatarUrl = value;
        }

        public ObservableCollection<UserModel> Friends { get; set; }
        public ObservableCollection<VirtualServerModel> Servers { get; set; }
        public DateTime LastActivity { get; set; }
    }
}