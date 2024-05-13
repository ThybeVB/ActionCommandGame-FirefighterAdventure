namespace ActionCommandGame.Sdk
{
    public class GameSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GameSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //todo
    }
}
