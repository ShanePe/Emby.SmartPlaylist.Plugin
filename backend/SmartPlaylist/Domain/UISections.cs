using SmartPlaylist.Contracts;

namespace SmartPlaylist.Domain
{
    public class UISections
    {
        public bool Setup { get; set; }
        public bool Sort { get; set; }
        public bool Rules { get; set; }
        public bool Notes { get; set; }

        public UISections() { }

        public UISections(UISectionsDto dto)
        {
            Setup = dto.Setup;
            Sort = dto.Sort;
            Rules = dto.Rules;
            Notes = dto.Notes;
        }

        public UISectionsDto ToDto()
        {
            return new UISectionsDto()
            {
                Setup = this.Setup,
                Sort = this.Sort,
                Rules = this.Rules,
                Notes = this.Notes
            };
        }
    }
}