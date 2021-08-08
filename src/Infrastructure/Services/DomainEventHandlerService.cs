using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class DomainEventHandlerService : IDomainEventHandlerService
    {
        private readonly ILogger _logger;
        private readonly IPublisher _mediator;

        public DomainEventHandlerService(ILogger<DomainEventHandlerService> logger, IPublisher publisher)
        {
            _logger = logger;
            _mediator = publisher;
        }

        public async Task Publish(DomainEvent domainEvent)
        {
            _logger.LogInformation("Domain Event Triggered: {event}", domainEvent.GetType().Name);
            await _mediator.Publish(GetNotificationCorrespondingToDomainEvent(domainEvent));
        }

        private INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
        {
            return (INotification)Activator.CreateInstance(typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent);
        }
    }
}
