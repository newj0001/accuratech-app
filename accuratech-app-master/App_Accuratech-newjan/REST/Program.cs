using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace REST
{
    public class Program
    {
        static void Main(string[] args)
        {
            var config = new HttpSelfHostConfiguration("http://localhost:44333/");
            
            config.Routes.MapHttpRoute(
                "API Default",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional, });


            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }

            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
