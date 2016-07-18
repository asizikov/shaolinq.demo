using System.Collections.Generic;
using JetBrains.Annotations;

namespace Shaolinq.Demo.Domain
{
    [UsedImplicitly]
    public class Author
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<Post> Posts { get; set; }
    }
}