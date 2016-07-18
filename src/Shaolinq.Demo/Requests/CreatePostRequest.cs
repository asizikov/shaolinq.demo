using JetBrains.Annotations;

namespace Shaolinq.Demo.Requests
{
    [UsedImplicitly]
    public class CreatePostRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }
}