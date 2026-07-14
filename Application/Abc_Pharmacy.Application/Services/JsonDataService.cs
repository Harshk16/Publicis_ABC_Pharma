using Abc_Pharmacy.Application.Dto;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Abc_Pharmacy.Application.Services
{
    public class JsonDataService : IJsonDataService
    {
        private readonly string _targetDataFolder;
        private readonly JsonSerializerOptions _jsonOptions;

   
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _fileLocks = new();

        public JsonDataService(IHostEnvironment env)
        {
            _targetDataFolder = Path.Combine(env.ContentRootPath, "..", "Abc_Pharmacy.Application", "Data");

            if (!Directory.Exists(_targetDataFolder))
            {
                _targetDataFolder = Path.Combine(AppContext.BaseDirectory, "Data");
            }

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
        }

        private SemaphoreSlim GetLock(string filename)
        {
            return _fileLocks.GetOrAdd(filename, _ => new SemaphoreSlim(1, 1));
        }

        public async Task<List<T>> ReadAsync<T>(string filename, CancellationToken cancellationToken = default)
        {
            var filePath = Path.Combine(_targetDataFolder, filename);

            if (!File.Exists(filePath))
            {
                return new List<T>();
            }

            var fileLock = GetLock(filename);
            await fileLock.WaitAsync(cancellationToken);

            try
            {
                var jsonText = await File.ReadAllTextAsync(filePath, cancellationToken);
                if (string.IsNullOrWhiteSpace(jsonText)) return new List<T>();

                return JsonSerializer.Deserialize<List<T>>(jsonText, _jsonOptions) ?? new List<T>();
            }
            finally
            {
                fileLock.Release();
            }
        }

        public async Task WriteAsync<T>(string filename, List<T> data, CancellationToken cancellationToken = default)
        {
            if (!Directory.Exists(_targetDataFolder))
            {
                Directory.CreateDirectory(_targetDataFolder);
            }

            var filePath = Path.Combine(_targetDataFolder, filename);
            var fileLock = GetLock(filename);

            await fileLock.WaitAsync(cancellationToken);

            try
            {
                var serializedJson = JsonSerializer.Serialize(data, _jsonOptions);
                await File.WriteAllTextAsync(filePath, serializedJson, cancellationToken);
            }
            finally
            {
                fileLock.Release();
            }
        }
    }
}
