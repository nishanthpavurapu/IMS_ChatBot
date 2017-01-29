using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace amsBOT
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           // string primary_amsid = Session["amscurrentuser"].ToString();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
