using System.Net.Http;

namespace ClearlyAgile.Testing.Core
{
    public class TestHttpClient : HttpClient
    {
        public TestHttpMessageHandler MessageHandler { get; set; }

        public TestHttpClient(TestHttpMessageHandler messageHandler) : base(messageHandler)
        {
            this.MessageHandler = messageHandler;
        }
    }
}
