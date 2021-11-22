using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ClearlyAgile.Testing.Core
{
    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private Utf8JsonWriter _jsonWriter = new Utf8JsonWriter(new MemoryStream());
        private readonly IEnumerator<T> _inner;

        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken) => Task.FromResult(_inner.MoveNext());

        public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());

        public T Current => _inner.Current;

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_jsonWriter != null)
            {
                await _jsonWriter.DisposeAsync().ConfigureAwait(false);
            }

            _jsonWriter = null;
        }
    }
}
