using System;
using System.Threading.Tasks;
using MediaBrowser.Model.Services;
using SmartPlaylist.Contracts;
using SmartPlaylist.Domain;
using SmartPlaylist.Domain.CriteriaDefinition;
using System.Linq;
using SmartPlaylist.Handlers.Commands;

namespace SmartPlaylist.Api
{
    [Route("/smartplaylist", "POST", Summary = "")]
    public class SaveSmartPlaylist : SmartPlaylistDto, IReturn<ResponseDto<SmartPlaylistDto>>
    { }

    [Route("/smartplaylist/sort", "POST", Summary = "")]
    public class SaveSortJobPlaylist : SmartPlaylistDto, IReturn<ResponseDto<SmartPlaylistDto>>
    { }

    [Route("/smartplaylist/log/{Id}", "GET", Summary = "")]
    public class LogFileRequest : IReturn
    {
        public string Id { get; set; }

    }

    [Route("/smartplaylist/{Id}/{Keep}", "DELETE", Summary = "")]
    public class DeleteSmartPlaylist : IReturnVoid
    {
        public string Id { get; set; }
        public bool Keep { get; set; }
    }

    [Route("/smartplaylist/appData", "GET", Summary = "")]
    public class GetAppData : IReturn<AppDataResponse>
    { }

    [Route("/smartplaylist/info/{Id}", "GET", Summary = "")]
    public class GetPlaylistItems : IReturn<SmartPlaylistInfoDto>
    {
        public string Id { get; set; }
    }

    [Route("/smartplaylist/info/{Id}", "POST", Summary = "")]
    public class ExecutePlaylist : IReturn<SmartPlaylistInfoDto>
    {
        public string Id { get; set; }
    }

    [Route("/smartplaylist/export/{payload}", "GET", Summary = "")]
    public class ExportPlaylists : IReturn
    {
        public string payload { get; set; }
    }

    [Route("/smartplaylist/import", "POST", Summary = "")]
    public class Import : IReturn<ResponseDto<string>>
    {
        public string type { get; set; }
        public string UploadFile { get; set; }
    }

    [Route("/smartplaylist/explain_rules", "POST", Summary = "")]
    public class ExplainPlaylistRules : SmartPlaylistDto, IReturn<ResponseDto<HierarchyStringDto>>
    { }
}