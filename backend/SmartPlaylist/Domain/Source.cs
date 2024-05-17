using SmartPlaylist.Contracts;

namespace SmartPlaylist.Domain
{
    public class Source
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }

        public Source() { }
        public Source(SourceDto dto)
        {
            Type = dto.Type;
            Id = dto.Id;
            Name = dto.Name;
        }
    }
}