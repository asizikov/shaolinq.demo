namespace Shaolinq.Demo.Model
{
    [DataAccessModel]
    public abstract class BlogModel : DataAccessModel
    {
        [DataAccessObjects]
        public abstract DataAccessObjects<Post> Posts { get; }

        [DataAccessObjects]
        public abstract DataAccessObjects<Author> Authors { get; }
    }
}