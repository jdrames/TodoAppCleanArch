using Domain.Common;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IDomainEventHandlerService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
