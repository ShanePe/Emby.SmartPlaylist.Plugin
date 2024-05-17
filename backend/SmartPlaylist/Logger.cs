using System;
using MediaBrowser.Model.Logging;

namespace SmartPlaylist
{
    public class Logger
    {
        public static Logger Instance = null;
        private readonly ILogger _logger;

        public Logger(ILogger logger)
        {
            _logger = logger;
        }

        public void LogDebug(string message, object o)
        {
            _logger.Debug("[SmartPlaylist]:" + message, o);
        }

        public void LogDebug(string message)
        {
            _logger.Debug("[SmartPlaylist]:" + message);
        }

        public void LogError(Exception e)
        {
            _logger.ErrorException($"[SmartPlaylist][Error]: {e.Message}", e);
        }
    }
}