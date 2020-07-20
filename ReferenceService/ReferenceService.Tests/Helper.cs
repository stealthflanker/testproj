namespace ReferenceService.Tests
{
    using System.Web.Http;
    using ReferenceService.WebApi;

    internal static class Helper
    {
        public static HttpConfiguration CreateHttpConfiguration()
        {
            var httpConf = new HttpConfiguration();

            WebApplication.Initialize(httpConf);
            return httpConf;
        }
    }
}
