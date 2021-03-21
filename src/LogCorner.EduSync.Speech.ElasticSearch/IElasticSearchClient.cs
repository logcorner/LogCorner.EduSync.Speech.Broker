using LogCorner.EduSync.Speech.Projection;
using System;

namespace LogCorner.EduSync.Speech.ElasticSearch
{
    public interface IElasticSearchClient<in T> : IRepository<T, Guid> where T : Entity<Guid>
    {
    }
}