using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Shaolinq.Demo.Domain.Repository
{
    public interface IAuthorRepository
    {
        [ItemNotNull]
        Task<IEnumerable<Author>> GetAllAsync();

        [ItemCanBeNull]
        Task<Author> GetAsync(int id);

        [ItemCanBeNull]
        Task<Author> GetByNameAsync([NotNull]string name);

        Task<Author> CreateAsync([NotNull]string userName, [NotNull] string email);

        Task DeleteAsync(int id);
    }
}
