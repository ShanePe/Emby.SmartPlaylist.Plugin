using SmartPlaylist.Contracts;
using System;
using System.Linq;

namespace SmartPlaylist.Domain
{
    public class NewItemOrder
    {
        public IOrder OrderBy { get; set; }

        public bool HasSort => !(OrderBy is OrderNone);

        public NewItemOrder() { }
        public NewItemOrder(SmartPlaylistNewItemOrderDto dto)
        {
            OrderBy = dto.HasSort ? IOrder.GetOrderFromString(dto.OrderBy) : new OrderNone();
        }

        public SmartPlaylistNewItemOrderDto ToDto()
        {
            return new SmartPlaylistNewItemOrderDto()
            {
                HasSort = HasSort,
                OrderBy = OrderBy.Name
            };
        }
    }
}