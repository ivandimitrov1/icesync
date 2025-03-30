using Refit;
using IceSync.Infrastructure.UniversalLoaderApi.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Application.Interfaces.UniversalLoaderApi;

public interface IUniversalLoaderAuthApi
{
    [Post("/authenticate")]
    Task<string> GetAccessTokenAsync([Body] AuthRequest authRequest);
}
