using ActionCommandGame.RestApi.Models.Enums;

namespace ActionCommandGame.RestApi.Models
{
    public class ServiceMessage
    {
        public required string Code { get; set; }
        public required string Message { get; set; }
        public ServiceMessageType Type { get; set; }
    }
}
