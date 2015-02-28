using System;
using System.Web;
using FirstSimpleSite.EFDataContext;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Security.Principal;

namespace FirstSimpleSite.AutificationModules
{
    public class CustomAuntificationModule : IHttpModule
    {
        /// <summary>
        /// You will need to configure this module in the Web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }

        public void Init(HttpApplication context)
        {
            // Below is an example of how you can handle LogRequest event and provide 
            // custom logging implementation for it
            context.BeginRequest += new EventHandler(OnLogRequest);
            //context.AuthorizeRequest += new EventHandler(OnLogRequest);
        }

        #endregion

        public void OnLogRequest(Object source, EventArgs e)
        {
            string userNameCookie = ConfigurationManager.AppSettings["UserNameCookie"];
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            if (context.Request.Cookies[userNameCookie] != null)
            {
                if (context.Request.Cookies[userNameCookie].Value != null && context.Request.Cookies[userNameCookie].Value != "")
                {
                    context.Items.Add("result", ConfigurationManager.AppSettings["Ok"]);
                    GenericIdentity identity = new GenericIdentity(context.Request.Cookies[userNameCookie].Value);
                    GenericPrincipal principal = new GenericPrincipal(identity, null);
                    context.User = principal;
                    return;
                }
            }
            context.Response.Cookies.Add(new HttpCookie(userNameCookie, ""));
            context.Response.Cookies.Add(new HttpCookie(ConfigurationManager.AppSettings["StatusCookie"], ""));
            string login = string.Empty;
            string pass = string.Empty;
            if (context.Request.Form[ConfigurationManager.AppSettings["LoginKey"]] != null && context.Request.Form[ConfigurationManager.AppSettings["PassKey"]] != null)
            {
                login = context.Request.Form[ConfigurationManager.AppSettings["LoginKey"]];
                pass = context.Request.Form[ConfigurationManager.AppSettings["PassKey"]];
            }
            else
            {
                context.Items.Add("result", ConfigurationManager.AppSettings["BadDataMessage"]);
                return;
            }
            using (UserContext uc = new UserContext())
            {
                User user = uc.Set<User>().FirstOrDefault(u => u.Login == login);
                if (user != null)
                {
                    if (user.Pass == pass)
                    {
                        context.Items.Add("result", ConfigurationManager.AppSettings["Ok"]);
                        GenericIdentity identity = new GenericIdentity(user.Login);
                        GenericPrincipal principal = new GenericPrincipal(identity, null);
                        context.User = principal;
                    }
                    else
                    {
                        context.Items.Add("result", ConfigurationManager.AppSettings["BadPasswordMessage"]);
                    }

                }
                else
                {
                    context.Items.Add("result", ConfigurationManager.AppSettings["BadUserNameMessage"]);
                }
            } 
        }
    }
}
