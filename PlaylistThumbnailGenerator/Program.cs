using System.IO;
using CustomPlaylistFormat.Objects;
using PlaylistThumbnailGenerator;

/*var file = File.Open("./testing.play", FileMode.Open);

var decoder = new Decoder(file);

var playlist = decoder.Read();

var length = playlist.PlaylistItems?.Length ?? 0;
*/

const int length = 151;

var fileStream = File.Open("./testing.png", FileMode.Create);

var info = new PlaylistInfo
{
    Name = "Testing Playlist.",
    Maker = "t1stm#0329",
    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
    IsPublic = true
};

var generator = new Generator(fileStream);

generator.Generate(info);

