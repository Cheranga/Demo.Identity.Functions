using System.Threading.Tasks;

namespace Demo.Identity.PurchaseOrders.Infrastructure
{
    public interface IMessagePublisher
    {
        Task<bool> PublishAsync<TMessage>(TMessage message) where TMessage : class;
    }
}