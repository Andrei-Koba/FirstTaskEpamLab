using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Configuration;

namespace FirstSimpleSite.HAndlers
{
    /// <summary>
    /// Summary description for AutificationHandler
    /// </summary>
    public class AutificationHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string result = (string)context.Items["result"];
            string json = string.Empty;
            if (result == ConfigurationManager.AppSettings["Ok"])
            {
                //json = "{\"status\":\"" + result + "\",\"userName\":\"" + context.Request.Form[ConfigurationManager.AppSettings["LoginKey"]] + "\" }";
                context.Response.Cookies[ConfigurationManager.AppSettings["StatusCookie"]].Value = ConfigurationManager.AppSettings["Ok"];
                context.Response.Cookies[ConfigurationManager.AppSettings["UserNameCookie"]].Value = context.Request.Form[ConfigurationManager.AppSettings["LoginKey"]];
            }
            else
            {
                //json = "{\"status\":\"" + result + "\",\"userName\":\"\"}";
                context.Response.Cookies[ConfigurationManager.AppSettings["StatusCookie"]].Value = result;
                context.Response.Cookies[ConfigurationManager.AppSettings["UserNameCookie"]].Value = "";
            }
            //context.Response.Clear();
            //context.Response.ContentType = "application/json; charset=utf-8";
            //context.Response.Write(json);
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