using System;
using JetBrains.Annotations;

namespace Shaolinq.Demo.Domain
{
    [UsedImplicitly]
    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
    }
}