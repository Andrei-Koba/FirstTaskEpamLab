using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Configuration;
using FirstSimpleSite.Security;

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
            string login = context.Request.Form[ConfigurationManager.AppSettings["LoginKey"]];
            if (result == ConfigurationManager.AppSettings["Ok"])
            {
                //json = "{\"status\":\"" + result + "\",\"userName\":\"" + context.Request.Form[ConfigurationManager.AppSettings["LoginKey"]] + "\" }";
                context.Response.Cookies[ConfigurationManager.AppSettings["Token"]].Value = EncryptDecrypt.Encrypt("userName=" + login);
                context.Response.Cookies[ConfigurationManager.AppSettings["UserNameCookie"]].Value = login;
                context.Response.Write(ConfigurationManager.AppSettings["Ok"]);
            }
            else
            {
                //json = "{\"status\":\"" + result + "\",\"userName\":\"\"}";
                context.Response.Cookies[ConfigurationManager.AppSettings["Token"]].Value = "";
                context.Response.Cookies[ConfigurationManager.AppSettings["UserNameCookie"]].Value = "";
                context.Response.Write(result);
            }
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