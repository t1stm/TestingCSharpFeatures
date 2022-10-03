using System.Collections.Generic;

namespace PlayingWithStreams
{
    public class StreamData
    {
        public byte[] Data { get; init; } = null!;
        public int Offset;
        public int Count;
    }
}