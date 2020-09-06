using LogCorner.EduSync.Speech.SharedKernel.Events;
using System.Collections.Generic;

namespace LogCorner.EduSync.Speech.ReadModel.SpeechAggregate
{
    public abstract class ReaModelAggregate<T> : Entity<T>
    {
        protected ReaModelAggregate()
        {
        }

        public void ApplyEvent(IDomainEvent @event, long version)
        {
            ((dynamic)this).Apply((dynamic)@event);
        }

        public void LoadFromHistory(IEnumerable<IDomainEvent> events)
        {
            foreach (var @event in events)
            {
                ApplyEvent(@event, @event.AggregateVersion);
            }
        }
    }
}