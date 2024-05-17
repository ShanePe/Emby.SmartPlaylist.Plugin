using System.Collections.Generic;
using MediaBrowser.Controller.Dto;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;

namespace SmartPlaylist.Services
{
    public interface IUserItemsProvider
    {
        IEnumerable<BaseItem> GetItems(User user, string[] itemTypes);
    }

    public class UserItemsProvider : IUserItemsProvider
    {
        private readonly ILibraryManager _libraryManager;

        public UserItemsProvider(ILibraryManager libraryManager)
        {
            _libraryManager = libraryManager;
        }

        public IEnumerable<BaseItem> GetItems(User user, string[] itemTypes)
        {
            var query = GetItemsQuery(user, itemTypes);
            return BaseItem.LibraryManager.GetItemsResult(query).Items;
            //return _libraryManager.GetUserRootFolder().GetItems(query).Items;
        }

        private static InternalItemsQuery GetItemsQuery(User user, string[] itemTypes)
        {
            return new InternalItemsQuery(user)
            {
                IncludeItemTypes = new string[] { "Episode" },

                Recursive = true,
                IsVirtualItem = false,
                DtoOptions = new DtoOptions(true)
                {
                    EnableImages = false
                }
            };
        }
    }
}