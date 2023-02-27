using Hangfire.Dashboard;

namespace Innowise.Clinic.Scheduling.Services.HangfireService;

public class AllowGuestAccessToDashboardFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}