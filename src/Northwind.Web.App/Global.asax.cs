namespace Northwind.Web.App
{
    using System.Web.Http;
    using Northwind.Web.App.App_Start;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            UnityConfig.RegisterComponents();                       
        }


    }
}
