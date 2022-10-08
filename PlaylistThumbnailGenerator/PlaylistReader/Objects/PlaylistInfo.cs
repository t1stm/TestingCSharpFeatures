namespace CustomPlaylistFormat.Objects
{
    public class PlaylistInfo
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Maker { get; set; }
        public long LastModified { get; set; }
        public bool IsAnonymous => string.IsNullOrEmpty(Maker);
        public bool IsPublic { get; set; } = true;
        public int Count { get; set; }

        public override string ToString()
        {
            return $"Name: '{Name}', Description: '{Description}', Maker: '{Maker}'";
        }
    }
}