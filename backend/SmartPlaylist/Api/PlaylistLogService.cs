using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Net;
using MediaBrowser.Model.Services;

namespace SmartPlaylist.Api
{
    public class PlaylistLogService : IService, IRequiresRequest
    {
        private readonly ISessionContext _sessionContext;
        public IRequest Request { get; set; }

        public PlaylistLogService(ISessionContext sessionContext)
        {
            _sessionContext = sessionContext;
        }

        public async Task Get(LogFileRequest request)
        {
            var user = GetUser();
            await Request.Response.TransmitFile(Plugin.Instance.SmartPlaylistStore.GetLogFilePath(user.Id, request.Id)
                , 0, 0, MediaBrowser.Model.IO.FileShareMode.Read, CancellationToken.None);
        }

        private User GetUser()
        {
            return _sessionContext.GetUser(Request);
        }
    }
}