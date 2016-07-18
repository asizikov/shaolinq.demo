using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Shaolinq.Demo.Domain.Repository
{
    public interface IPostRepository
    {
        [ItemNotNull]
        Task<IEnumerable<Post>> GetLatestAsync();

        [ItemNotNull]
        Task<Post> CreateAsync(int authorId, Post post);
    }
}
