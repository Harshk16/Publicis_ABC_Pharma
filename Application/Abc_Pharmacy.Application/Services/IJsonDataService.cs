using Abc_Pharmacy.Application.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Abc_Pharmacy.Application.Services
{
    public interface IJsonDataService
    {
        Task<List<T>> ReadAsync<T>(string filename, CancellationToken cancellationToken = default);
        Task WriteAsync<T>(string filename, List<T> data, CancellationToken cancellationToken = default);
    }
}
