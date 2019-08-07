using System.Collections.ObjectModel;

namespace AvaloniaMVVMClient.UI.Models
{
    public class ChannelModel
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public ObservableCollection<MessageModel> Messages { get; set; }
    }
}