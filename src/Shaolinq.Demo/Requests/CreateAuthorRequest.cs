using JetBrains.Annotations;

namespace Shaolinq.Demo.Requests
{
    [UsedImplicitly]
    public class CreateAuthorRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}