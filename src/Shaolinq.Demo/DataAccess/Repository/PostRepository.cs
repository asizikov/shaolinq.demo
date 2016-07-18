using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using Shaolinq.Demo.Domain.Repository;
using Shaolinq.Demo.Model;
using Post = Shaolinq.Demo.Domain.Post;

namespace Shaolinq.Demo.DataAccess.Repository
{
    [UsedImplicitly]
    public class PostRepository : IPostRepository
    {
        private BlogModel Model { get; }
        private IMapper Mapper { get; }

        public PostRepository([NotNull] BlogModel model, [NotNull] IMapper mapper)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            Model = model;
            Mapper = mapper;
        }

        public async Task<IEnumerable<Post>> GetLatestAsync()
        {
            var posts = await Model.Posts.OrderBy(p => p.Date).Take(10).ToListAsync().ConfigureAwait(false);
            return posts.Select(p => Mapper.Map<Post>(p));
        }

        public async Task<Post> CreateAsync(int authorId, Post post)
        {
            var author = await Model.Authors.FirstOrDefaultAsync(a => a.Id == authorId).ConfigureAwait(false);
            if (author == null)
            {
                throw new InvalidOperationException($"Can not create post for non existing author with id {authorId}");
            }

            using (var scope = DataAccessScope.CreateReadCommitted())
            {
                var newPost = author.Posts.Create();
                Mapper.Map(post, newPost);

                await scope.CompleteAsync().ConfigureAwait(false);
                return Mapper.Map<Post>(newPost);
            }
        }
    }
}
