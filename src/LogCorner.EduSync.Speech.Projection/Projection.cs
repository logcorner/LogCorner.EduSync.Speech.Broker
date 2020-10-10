using System.Collections.Generic;
using LogCorner.EduSync.Speech.SharedKernel.Events;

namespace LogCorner.EduSync.Speech.Projection
{
    public abstract class Projection<T> : Entity<T>
    {
        protected Projection()
        {
        }

        public void ApplyEvent(IDomainEvent @event)
        {
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