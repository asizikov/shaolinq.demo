using System;
using JetBrains.Annotations;
using Nancy;
using Shaolinq.Demo.Domain.Repository;

namespace Shaolinq.Demo.Modules
{
    [UsedImplicitly]
    public class IndexModule : NancyModule
    {
        private IPostRepository PostRepository { get; }

        public IndexModule([NotNull] IPostRepository postRepository)
        {
            PostRepository = postRepository;
            if (postRepository == null) throw new ArgumentNullException(nameof(postRepository));

            Get["/", true] = async (request, cancellationToken) =>
            {
                var latestPosts = await PostRepository.GetLatestAsync().ConfigureAwait(false);
                return new {Title = "Latest posts", Posts = latestPosts};
            };
        }
    }
}