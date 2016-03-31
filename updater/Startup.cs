using Owin;

namespace RESTService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
        }
    }
}
