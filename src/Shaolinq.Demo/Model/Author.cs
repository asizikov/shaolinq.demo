namespace Shaolinq.Demo.Model
{
    [DataAccessObject]
    public abstract class Author : DataAccessObject<int>
    {
        [PersistedMember]
        public abstract string UserName { get; set; }

        [PersistedMember]
        public abstract string Email { get; set; }

        [RelatedDataAccessObjects]
        public abstract RelatedDataAccessObjects<Post> Posts { get; }
    }
}