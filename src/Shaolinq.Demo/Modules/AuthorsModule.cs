using System;
using System.Net;
using JetBrains.Annotations;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using Shaolinq.Demo.Domain;
using Shaolinq.Demo.Domain.Repository;
using Shaolinq.Demo.Requests;
using HttpStatusCode = Nancy.HttpStatusCode;

namespace Shaolinq.Demo.Modules
{
    [UsedImplicitly]
    public class AuthorsModule : NancyModule
    {
        private IAuthorRepository AuthorRepository { get; }
        private IPostRepository PostRepository { get; }

        public AuthorsModule([NotNull] IAuthorRepository authorRepository, [NotNull] IPostRepository postRepository) : base("authors")
        {
            if (authorRepository == null) throw new ArgumentNullException(nameof(authorRepository));
            if (postRepository == null) throw new ArgumentNullException(nameof(postRepository));
            AuthorRepository = authorRepository;
            PostRepository = postRepository;

            Get["/", true] = async (request, cancellationToken) => await authorRepository.GetAllAsync().ConfigureAwait(false);

            Get["/{id:int}", true] = async (request, cancellationToken) =>
            {
                Author author = await authorRepository.GetAsync(request.id).ConfigureAwait(false);
                if (author == null)
                {
                    return HttpStatusCode.NotFound;
                }
                return author;
            };

            Post["/", true] = async (request, cancellationToken) =>
            {
                var createAuthorRequest = this.Bind<CreateAuthorRequest>();
                var existingAuthor = await AuthorRepository.GetByNameAsync(createAuthorRequest.Name).ConfigureAwait(false);
                if (existingAuthor != null)
                {
                    return HttpStatusCode.Conflict;
                }

                var author = await AuthorRepository.CreateAsync(createAuthorRequest.Name, createAuthorRequest.Email).ConfigureAwait(false);

                var uriBuilder = new UriBuilder(Request.Url.SiteBase) {Path = Request.Path + $"/{author.Id}"};
                return Created(author, uriBuilder);
            };

            Post["/{id:int}/posts", true] = async (request, cancellationToken) =>
            {
                var createPostRequest = this.Bind<CreatePostRequest>();
                Author author = await AuthorRepository.GetAsync(request.id).ConfigureAwait(false);
                if (author == null)
                {
                    return HttpStatusCode.NotFound;
                }

                var post = await
                    PostRepository.CreateAsync(author.Id, new Post {Body = createPostRequest.Body, Title = createPostRequest.Title, Date = DateTime.UtcNow})
                        .ConfigureAwait(false);

                var uriBuilder = new UriBuilder(Request.Url.SiteBase) {Path = Request.Path + $"/{post.Id}"};
                return Created(post, uriBuilder);
            };

            Delete["/{id:int}", true] = async (request, cancellationToken) =>
            {
                var author = await authorRepository.GetAsync(request.id).ConfigureAwait(false);
                if (author == null) return HttpStatusCode.NotFound;

                await AuthorRepository.DeleteAsync(request.id).ConfigureAwait(false);
                return HttpStatusCode.OK;
            };
        }

        private Negotiator Created<TBody>(TBody post, UriBuilder uriBuilder)
        {
            return Negotiate.WithModel(post)
                .WithHeader(nameof(HttpResponseHeader.Location), uriBuilder.ToString())
                .WithStatusCode(HttpStatusCode.Created);
        }
    }
}