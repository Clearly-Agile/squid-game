using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ClearlyAgile.Testing.Core
{
    public class TestHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _sendAsync;

        public ICollection<TestHttpCall> Calls { get; set; } = new Collection<TestHttpCall>();

        public TestHttpMessageHandler()
        {
        }

        public TestHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsync)
        {
            _sendAsync = sendAsync;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var call = this.Calls.SingleOrDefault(c => c.Url.Equals(request.RequestUri.AbsolutePath,StringComparison.CurrentCultureIgnoreCase));

            if (call == null)
            {
                call = new TestHttpCall()
                {
                    FullUrl = request.RequestUri.AbsoluteUri,
                    Url = request.RequestUri.AbsolutePath,
                    UrlWithQueryString = request.RequestUri.PathAndQuery,
                    Headers = request.Headers.ToDictionary(h => h.Key, h => h.Value.FirstOrDefault())
                };

                if (request.Content != null)
                {
                    call.ContentString = await request.Content.ReadAsStringAsync();
                    call.ContentHeaders = request.Content.Headers.ToDictionary(h => h.Key, h => h.Value.FirstOrDefault());
                }

                this.Calls.Add(call);
            }

            call.TimesCalled++;

            if (_sendAsync != null)
            {
                return await _sendAsync(request, cancellationToken);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
