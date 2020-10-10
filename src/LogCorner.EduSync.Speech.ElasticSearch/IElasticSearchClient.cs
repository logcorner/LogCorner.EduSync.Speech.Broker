using System;
using LogCorner.EduSync.Speech.Projection;

namespace LogCorner.EduSync.Speech.ElasticSearch
{
    public interface IElasticSearchClient<in T> : IRepository<T, Guid> where T : Entity<Guid>
    {
    }
}