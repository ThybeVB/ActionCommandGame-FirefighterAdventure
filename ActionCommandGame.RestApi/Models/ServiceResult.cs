using ActionCommandGame.RestApi.Models.Enums;

namespace ActionCommandGame.RestApi.Models
{
    public class ServiceResult
    {
        public bool IsSuccessful => Messages.All(m => m.Type != ServiceMessageType.Error);

        public IList<ServiceMessage> Messages { get; set; } = new List<ServiceMessage>();
    }
}
