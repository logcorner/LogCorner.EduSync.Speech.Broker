using System.Threading.Tasks;
using LogCorner.EduSync.Speech.ReadModel.SpeechAggregate;

namespace LogCorner.EduSync.Speech.ReadModel
{
    public interface IRepository<in T, TIdentifier> where T : Entity<TIdentifier>
    {
        Task CreateAsync(T entity);
    }
}