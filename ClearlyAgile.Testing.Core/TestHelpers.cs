using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClearlyAgile.Testing.Core
{
    public static class TestHelpers
    {
        public static TestHttpClient CreateTestHttpClient()
        {
            return new TestHttpClient(CreateTestHttpMessageHandler())
            {
                BaseAddress = new Uri("http://www.testsite.com/")
            };
        }

        public static TestHttpClient CreateTestHttpClient(string baseUrl)
        {
            return new TestHttpClient(CreateTestHttpMessageHandler())
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        public static TestHttpClient CreateTestHttpClient(HttpResponseMessage response)
        {
            return new TestHttpClient(CreateTestHttpMessageHandler(response))
            {
                BaseAddress = new Uri("http://www.testsite.com/")
            };
        }

        public static TestHttpClient CreateTestHttpClient(TestHttpMessageHandler messageHandler)
        {
            return new TestHttpClient(messageHandler)
            {
                BaseAddress = new Uri("http://www.testsite.com/")
            };
        }

        public static TestHttpMessageHandler CreateTestHttpMessageHandler(HttpResponseMessage response = null)
        {
            if (response == null)
            {
                return new TestHttpMessageHandler((request, cancellationToken) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
            }
            
            return new TestHttpMessageHandler((request, cancellationToken) => Task.FromResult(response));
        }

        public static IEnumerable<object[]> GetNonSuccessHttpStatusCodes()
        {
            foreach(var statusCode in Enum.GetValues(typeof(HttpStatusCode)))
            {
                var httpStatusCode = (HttpStatusCode) statusCode;

                //EDG: ignore success status codes
                if ((int) httpStatusCode >= 200 && (int) httpStatusCode <= 299)
                {
                    continue;
                }

                yield return new [] {statusCode};
            }
        }
    }
}
