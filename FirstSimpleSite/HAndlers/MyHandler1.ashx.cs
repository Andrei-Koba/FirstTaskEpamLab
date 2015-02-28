using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstSimpleSite
{
    /// <summary>
    /// Summary description for MyHandler1
    /// </summary>
    public class MyHandler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write("This is MyHandler1.");
            //context.Response.WriteFile("~/contact.json");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}