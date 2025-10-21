using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Config.Test
{
    public class CacheFile<T>
    {
        private readonly string _path;

        public CacheFile(string path)
        {
            if(string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));
            _path = path;
        }

        public bool Exists => File.Exists(_path);

        public async IAsyncEnumerable<T> ReadAsync([EnumeratorCancellation]CancellationToken token = default)
        {
            if (!Exists)
                throw new FileNotFoundException($"File '{Path.GetFullPath(_path)}' does not exist", _path);

            await using var stream = File.OpenRead(_path);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            await foreach(var item in JsonSerializer.DeserializeAsyncEnumerable<T>(stream, options, token))
                yield return item;
        }

        public async Task WriteAsync(IAsyncEnumerable<T> source, CancellationToken token = default)
        {
            await using var fs = File.Create(_path);
            await using var writer = new Utf8JsonWriter(fs, new JsonWriterOptions { Indented = true });

            writer.WriteStartArray();

            await foreach (var item in source.WithCancellation(token))
            {
                JsonSerializer.Serialize(writer, item);
            }

            writer.WriteEndArray();
            await writer.FlushAsync(token);
        }
    }
}
