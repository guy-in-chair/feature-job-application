namespace TeknorixAPI.Helpers
{
    public static class GlobalHelpers
    {
        public static string[] GetDevModeCorsAllowedClientUrls(IConfiguration configuration)
        {
            return Convert.ToString(configuration["CorsAllowedDevClientUrls"]).Split(',');
        }
        public static string[] GetReleaseModeCorsAllowedClientUrls(IConfiguration configuration)
        {
            return Convert.ToString(configuration["CorsAllowedReleaseClientUrls"]).Split(',');
        }
    }
}
