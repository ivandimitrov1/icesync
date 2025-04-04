using Refit;
using IceSync.Infrastructure.UniversalLoaderApi.Requests;

namespace IceSync.Application.Interfaces.UniversalLoaderApi;

public interface IUniversalLoaderAuthApi
{
    [Post("/authenticate")]
    Task<string> GetAccessTokenAsync([Body] AuthRequest authRequest);
}
