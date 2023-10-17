using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;

namespace BlazorServer
{
    public class StreamingHub : Hub
    {
        public ChannelManager ChannelManager { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;
        //private readonly IHubContext hubConext;
        public StreamingHub(ChannelManager channelManager, IWebHostEnvironment webHostEnvironment)
        {
            ChannelManager = channelManager;
            _webHostEnvironment = webHostEnvironment;
            //this.hubConext = hubConext;
        }

        public async Task RequestStream(Guid streamId, string fileName)
        {
            //sends message to service client to stream audio

            //get the service client
            var test = this.Clients.AllExcept(Context.ConnectionId);

            //todo: set up placeholder channel reader with stream id

            //send filename and stream id for service 
            await test.SendAsync("StartStream", streamId, fileName);
        }

        



        public async Task UploadStream(IAsyncEnumerable<byte> stream)
        {
            await foreach(var item in stream)
            {
                await Clients.Others.SendAsync("RecieveStream", item);
            }
            
        }

        public ChannelReader<byte> GetStream()
        {

            return ChannelManager.Channel;
            
        }

        public ChannelReader<byte[]> Counter(CancellationToken cancellationToken)
        {
            var channel = Channel.CreateUnbounded<byte[]>();

            // We don't want to await WriteItemsAsync, otherwise we'd end up waiting 
            // for all the items to be written before returning the channel back to
            // the client.
            _ = WriteItemsAsync(channel.Writer, cancellationToken);
            
            return channel.Reader;
        }

        private async Task WriteItemsAsync(
            ChannelWriter<byte[]> writer,
            CancellationToken cancellationToken)
        {
            Exception localException = null;
            try
            {
                var test = Path.Combine(_webHostEnvironment.ContentRootPath, "Free_Test_Data_500KB_MP3.mp3");
                var test2 = File.Exists(test);
                var bytes = File.ReadAllBytes(test);

                await writer.WriteAsync(bytes, cancellationToken);
                //foreach (var fileByte in bytes)
                //{
                //    await writer.WriteAsync(fileByte, cancellationToken);
                //}
                //var mp3Reader = new FileStream(test, FileMode.Open);
                //var reader = new StreamReader(mp3Reader);
                //string? s = "";
                //while ((s = reader.ReadLine()) != null)
                //{
                //    await writer.WriteAsync(s, cancellationToken);
                //    Console.WriteLine(s);
                //}

                //for (var i = 0; i < count; i++)
                //{
                //    await writer.WriteAsync(i, cancellationToken);

                //    // Use the cancellationToken in other APIs that accept cancellation
                //    // tokens so the cancellation can flow down to them.
                //    await Task.Delay(delay, cancellationToken);
                //}
            }
            catch (Exception ex)
            {
                localException = ex;
            }
            finally
            {
                writer.Complete(localException);
            }
        }

    }
}
