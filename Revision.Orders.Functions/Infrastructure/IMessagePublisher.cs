using System.Threading.Tasks;

namespace Revision.Orders.Functions.Infrastructure
{
    public interface IMessagePublisher
    {
        Task<bool> PublishAsync<TMessage>(TMessage message) where TMessage : class;
    }
}