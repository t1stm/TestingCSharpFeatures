using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PlayingWithStreams;

var data = new byte[]
{
    84, 7, 70, 6, 0, 5, 34
};

var stream = new MemoryStream(data);
stream.Position = 0;
var destinations = new Stream[]
{
    new MemoryStream(),
    new MemoryStream(),
    new MemoryStream(),
    new MemoryStream()
};

var spreader = new StreamSpreader(CancellationToken.None, destinations);

for (var i = 0; i < 4; i++)
{
    stream.Position = 0;
    stream.CopyTo(spreader);
}

Task.Delay(333).Wait(); // Artificial delay to see if the spreader is working.

for (var i = 0; i < destinations.Length; i++)
{
    var destination = destinations[i];
    destination.Position = 0;
    Span<byte> readData = new(new byte[destination.Length]);
    var readBytes = destination.Read(readData);
    var stringified = string.Concat(readData.ToArray().Select(r => $"'{r}' ").ToArray()).Trim();
    
    Console.WriteLine($"({i}) => Bytes - {readBytes}: \"{stringified}\"");
}