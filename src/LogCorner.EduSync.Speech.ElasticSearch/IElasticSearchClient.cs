using LogCorner.EduSync.Speech.ReadModel;
using LogCorner.EduSync.Speech.ReadModel.SpeechAggregate;
using System;

namespace LogCorner.EduSync.Speech.ElasticSearch
{
    public interface IElasticSearchClient<T> : IRepository<T, Guid> where T : Entity<Guid>
    {
    }
}