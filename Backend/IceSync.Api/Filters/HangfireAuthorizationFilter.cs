using Hangfire.Dashboard;

namespace IceSync.Api.Filters;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // Allow dashboard for all enviroments
        return true;
    }
}

