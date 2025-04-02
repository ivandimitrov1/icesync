using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using IceSync.Application.Utils;
using IceSync.Infrastructure.UniversalLoaderApi.Requests;
using System.Net.Http.Headers;


namespace IceSync.Application.Interfaces.UniversalLoaderApi;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly IUniversalLoaderAuthApi _authApi;
    private readonly IOptionsMonitor<UniversalLoaderApiOptions> _universalLoaderOptions;
    private readonly IMemoryCache _memoryCache;

    public AuthHeaderHandler(
        IUniversalLoaderAuthApi authApi,
        IOptionsMonitor<UniversalLoaderApiOptions> universalLoaderOptions,
        IMemoryCache memoryCache)
    {
        _authApi = authApi;
        _universalLoaderOptions = universalLoaderOptions;
        _memoryCache = memoryCache;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var authRequest = new AuthRequest()
        {
            ApiUserId = _universalLoaderOptions.CurrentValue.ApiUserId,
            ApiCompanyId = _universalLoaderOptions.CurrentValue.ApiCompanyId,
            ApiUserSecret = _universalLoaderOptions.CurrentValue.ApiUserSecret,
        };

        string token;
        if (_memoryCache.TryGetValue(authRequest.ApiUserId, out token))
        {
            if (!JwtValidator.IsTokenExpired(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await base.SendAsync(request, cancellationToken);
            }
        }

        token = await _authApi.GetAccessTokenAsync(authRequest);
        _memoryCache.Set(authRequest.ApiUserId, token);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}
