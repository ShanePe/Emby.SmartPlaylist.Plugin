using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Net;
using MediaBrowser.Model.Services;
using SmartPlaylist.Contracts;
using SmartPlaylist.Infrastructure;
using SmartPlaylist.Services.SmartPlaylist;

namespace SmartPlaylist.Api
{
    public class IOService : IService, IRequiresRequest
    {
        private readonly ISmartPlaylistStore _smartPlaylistStore;
        private readonly ISessionContext _sessionContext;
        public IRequest Request { get; set; }

        public IOService(ISessionContext sessionContext)
        {
            _sessionContext = sessionContext;
            _smartPlaylistStore = Plugin.Instance.SmartPlaylistStore;
        }

        public void Get(ExportPlaylists request)
        {
            string tempFile = string.Empty;
            try
            {
                byte[] decodedBytes = Convert.FromBase64String(request.payload);
                string[] ids = Encoding.UTF8.GetString(decodedBytes).Split(',');
                tempFile = _smartPlaylistStore.ExportAsync(ids).Result;

                Request.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Path.GetFileName(tempFile) + "\"");
                Request.Response.TransmitFile(tempFile, 0, 0, MediaBrowser.Model.IO.FileShareMode.Read, System.Threading.CancellationToken.None);
            }
            finally
            {
                if (!string.IsNullOrEmpty(tempFile))
                    _smartPlaylistStore.Delete(tempFile);
            }
        }

        public ResponseDto<string> Post(Import import)
        {
            try
            {
                var user = GetUser();
                if (import.type.Equals("import", StringComparison.OrdinalIgnoreCase))
                {
                    DataUrl data = new DataUrl(import.UploadFile);
                    if (!data.ContentType.Equals("application/zip", StringComparison.OrdinalIgnoreCase))
                        throw new Exception("Invalid uploaded file, expected a zip file.");

                    var res = _smartPlaylistStore.ImportAsync(data.FileData, user.Id).Result;

                    return ResponseDto<string>.CreateSuccess(res);
                }

                return ResponseDto<string>.CreateSuccess("Uploaded successfully");
            }
            catch (Exception ex)
            {
                return ResponseDto<string>.CreateError(ex.Message);
            }
        }

        private User GetUser()
        {
            return _sessionContext.GetUser(Request);
        }
    }
}