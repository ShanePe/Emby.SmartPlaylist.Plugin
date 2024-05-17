using System.Threading.Tasks;

namespace SmartPlaylist.Infrastructure.MessageBus
{
    public interface IMessageHandlerAsync<in T> where T : IMessage
    {
        Task HandleAsync(T message);
    }
}