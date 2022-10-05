using System;
using System.Collections.Generic;
using System.IO;
using CustomPlaylistFormat;
using CustomPlaylistFormat.Objects;

var playlist = new List<string>
{
    "yt://Y21EkJO8Vn8", // ♫ ♫ ♫ Traktorist huligan. ♫ ♫ ♫
    "yt://U3v-gVNFJSU", // ♫ ♫ ♫ Majare, Mala Curkva, Govedarci i celiq region. Nazdrave na vsichki praznuvashti. ♫ ♫ ♫
    "vb7://a5c864bf71", // ♫ ♫ ♫ Mamino Anche, ana tika ♫ ♫ ♫
    "yt://wcZDsBy5Zzg", // ♫ ♫ ♫ Kuceka Konq Vihar 2012, "novata modifikaciq". Haide Chochko, haide Vihare, izlez, izlez Vihare. ♫ ♫ ♫
    "yt://1cEMLW39cbY", // ♫ ♫ ♫ Piyan, za koi li put obiistveno piyan... ♫ ♫ ♫
    "file:///hdd0/Diskove na Bashta mi/FOLK HIT'S(КЛАСИКА В ЖАНРА)/Super Ekspres - Stari Rani.mp3", // ♫ ♫ ♫ Stari rani ti otvori. ♫ ♫ ♫
}; // Enough shitposting.

// This will be replaced with more complex types later,
// but it also may not due to me storing all audio
// with a custom protocol URL.

var fileStream = File.Open("./testing.play", FileMode.Create);

var playlistInfo = new PlaylistInfo
{
    Name = "Testing Playlist",
    Maker = "t1stm",
    Description = "This is the most epic description ever.",
    LastModified = DateTime.UtcNow.Ticks,
    IsPublic = false
};

var encoder = new Encoder(fileStream, playlistInfo);

encoder.Encode(playlist);

fileStream.Close();

fileStream = File.Open("./testing.play", FileMode.Open);

var decoder = new Decoder(fileStream);

var recievedPlaylist = decoder.Read();

Console.WriteLine($"{recievedPlaylist} - Failed: {recievedPlaylist.FailedToParse} - Count: {recievedPlaylist.PlaylistItems?.Length}");

if (recievedPlaylist.PlaylistItems == null)
{
    Console.WriteLine("Recieved playlist items are null.");
    return;
}

foreach (var item in recievedPlaylist.PlaylistItems)
{
    Console.WriteLine(item);
}

fileStream.Close();