using System.Collections.Generic;
using LogCorner.EduSync.Speech.Command.SharedKernel.Events;

namespace LogCorner.EduSync.Speech.Projection
{
    public abstract class Projection<T> : Entity<T>
    {
        public long Version { get; private set; }

        protected Projection()
        {
        }

        public void ApplyEvent(IDomainEvent @event)
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