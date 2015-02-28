using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace FirstSimpleSite.HAndlers
{
    /// <summary>
    /// Summary description for LogOutHandler
    /// </summary>
    public class LogOutHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Cookies[ConfigurationManager.AppSettings["StatusCookie"]].Value = "";
            context.Response.Cookies[ConfigurationManager.AppSettings["UserNameCookie"]].Value = "";
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