using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Projection
{
    public interface IRepository<in T, TIdentifier> where T : Entity<TIdentifier>
    {
        Task CreateAsync(T entity);
    }
}

