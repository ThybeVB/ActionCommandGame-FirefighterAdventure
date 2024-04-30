namespace ActionCommandGame.RestApi.Models
{
    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }
    }
}
