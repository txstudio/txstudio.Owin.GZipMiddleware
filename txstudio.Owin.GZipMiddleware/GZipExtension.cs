using Owin;

namespace txstudio.Owin.GZipMiddleware
{
    public static class GZipExtension
    {
        public static void UseGZipRequest(this IAppBuilder app)
        {
            app.Use<GZipMiddleware>();
        }
    }
}
