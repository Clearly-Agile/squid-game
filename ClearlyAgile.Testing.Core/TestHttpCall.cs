using System.Collections.Generic;

namespace ClearlyAgile.Testing.Core
{
    public class TestHttpCall
    {
        public string FullUrl { get; set; }

        public string Url { get; set; }

        public string UrlWithQueryString { get; set; }

        public int TimesCalled { get; set; }

        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public IDictionary<string, string> ContentHeaders { get; set; } = new Dictionary<string, string>();

        public string ContentString { get; set; }
    }
}
