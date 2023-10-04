using System.Threading.Channels;

namespace BlazorServer
{
    public class ChannelManager
    {
        public ChannelReader<int>? Channel { get; set; }
    }
}
