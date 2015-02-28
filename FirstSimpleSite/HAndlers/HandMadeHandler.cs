using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;


namespace FirstSimpleSite
{
    public class HandMadeHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.WriteFile("~/contact.json");
        }
    }
}