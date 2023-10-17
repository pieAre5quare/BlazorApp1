using System.Threading.Channels;

namespace BlazorServer
{
    public class ChannelManager
    {
        public ChannelReader<byte>? Channel { get; set; }
    }
}
