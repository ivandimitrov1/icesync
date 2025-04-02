namespace IceSync.Application.Interfaces.UniversalLoaderApi
{
    public class UniversalLoaderApiOptions
    {

        public static string Section { get; set; } = "UniversalLoaderApi";

        public string? ApiUrl { get; set; }

        public string? ApiCompanyId { get; set; }

        public string? ApiUserId { get; set; }

        public string? ApiUserSecret { get; set; }
    }
}
