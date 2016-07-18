using System;

namespace Shaolinq.Demo.Model
{
    [DataAccessObject]
    public abstract class Post : DataAccessObject<Guid>
    {
        [PersistedMember]
        public abstract string Title { get; set; }

        [PersistedMember]
        public abstract string Body { get; set; }

        [BackReference]
        public abstract Author Author { get; set; }

        [PersistedMember]
        public abstract DateTime Date { get; set; }
    }
}