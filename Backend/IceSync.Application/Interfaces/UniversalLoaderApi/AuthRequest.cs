using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IceSync.Infrastructure.UniversalLoaderApi.Requests;

public class AuthRequest
{
    public string ApiCompanyId { get; set; }

    public string ApiUserId { get; set; }

    public string ApiUserSecret { get; set; }
}
