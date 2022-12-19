using LogCorner.EduSync.Speech.Command.SharedKernel.Events;
using System.Collections.Generic;

namespace LogCorner.EduSync.Speech.Projection
{
    public abstract class Projection<T> : Entity<T>
    {
        public long Version { get; private set; }

        private void ApplyEvent(IDomainEvent @event)
        {
            Version = @event.AggregateVersion;
            ((dynamic)this).Apply((dynamic)@event);
        }

        public void LoadFromHistory(IEnumerable<IDomainEvent> events)
        {
            foreach (var @event in events)
            {
                ApplyEvent(@event);
            }
        }

        public void Project(IDomainEvent @event)
        {
            ApplyEvent(@event);
        }
    }
}