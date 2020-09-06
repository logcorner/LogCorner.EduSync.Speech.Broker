using LogCorner.EduSync.Speech.ReadModel.SpeechAggregate;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ReadModel
{
    public interface IRepository<in T, TIdentifier> where T : Entity<TIdentifier>
    {
        Task CreateAsync(T entity);
    }
}