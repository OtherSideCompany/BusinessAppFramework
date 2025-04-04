using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application.DomainObjectEvents
{
    public interface IDomainObjectEventPublisher
    {
        Task PublishAsync<TEvent>(TEvent domainEvent);
    }
}
