using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace DiscordBot.Tools
{
    public class StreamSpreader : Stream
    {
        private FeedableStream[] Destinations { get; }
        private CancellationToken Token { get; set; }

        public StreamSpreader(CancellationToken token, params Stream[] destinations)
        {
            Destinations = destinations.Select(r => new FeedableStream(r)).ToArray();
            Token = token;
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            foreach (var feedableStream in Destinations)
            {
                Token.ThrowIfCancellationRequested();
                Console.WriteLine("Writing to FeedableStream.");
                feedableStream.Write(buffer, offset, count);
            }
        }

        public override bool CanRead => false;
        public override bool CanSeek => false;
        public override bool CanWrite => true;
        public override long Length => 0;
        public override long Position { get; set; }
    }
}