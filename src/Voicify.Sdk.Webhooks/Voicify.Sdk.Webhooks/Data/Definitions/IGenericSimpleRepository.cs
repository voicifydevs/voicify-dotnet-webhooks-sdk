using System.Collections.Generic;
using System.Threading.Tasks;

namespace Voicify.Sdk.Webhooks.Data.Definitions
{
    public interface IGenericSimpleRepository<T>
    {
        Task<T> AddOrUpdateAsync(T entity);
        Task<T> DeleteAsync(string id);
        Task<T> FindAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}
