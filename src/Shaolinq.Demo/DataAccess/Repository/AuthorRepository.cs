using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using Shaolinq.Demo.Domain.Repository;
using Shaolinq.Demo.Model;
using Author = Shaolinq.Demo.Domain.Author;

namespace Shaolinq.Demo.DataAccess.Repository
{
    [UsedImplicitly]
    public class AuthorRepository : IAuthorRepository
    {
        private BlogModel Model { get; }
        private IMapper Mapper { get; }

        public AuthorRepository([NotNull] BlogModel model, [NotNull] IMapper mapper)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            Model = model;
            Mapper = mapper;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            var authors = await Model.Authors.ToListAsync().ConfigureAwait(false);
            return authors.Select(author => Mapper.Map<Model.Author, Author>(author));
        }

        public async Task<Author> GetAsync(int id)
        {
            var author = await Model.Authors.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);
            return Mapper.Map<Author>(author);
        }

        public async Task<Author> GetByNameAsync(string name)
        {
            var author = await Model.Authors.FirstOrDefaultAsync(a => a.UserName.ToLower() == name.ToLower()).ConfigureAwait(false);
            return Mapper.Map<Author>(author);
        }

        public async Task<Author> CreateAsync(string userName, string email)
        {
            using (var scope = DataAccessScope.CreateReadCommitted())
            {
                var author = Model.Authors.Create();
                author.Email = email;
                author.UserName = userName;
                await scope.CompleteAsync().ConfigureAwait(false);

                return Mapper.Map<Author>(author);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var scope = DataAccessScope.CreateReadCommitted())
            {
                await Model.Authors.DeleteAsync(author => author.Id == id).ConfigureAwait(false);
                await scope.CompleteAsync().ConfigureAwait(false);
            }
        }
    }
}