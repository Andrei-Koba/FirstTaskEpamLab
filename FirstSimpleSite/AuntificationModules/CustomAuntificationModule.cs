using System;
using System.Web;
using FirstSimpleSite.EFDataContext;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Security.Principal;
using FirstSimpleSite.Security;
using System.Web.Security;

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
            string token = ConfigurationManager.AppSettings["Token"];
            string userNameCookie = ConfigurationManager.AppSettings["UserNameCookie"];
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            context.Response.Cookies.Clear();
            context.Response.Cookies.Clear();
            if (context.Request.Cookies[token] != null)
            {
                if (context.Request.Cookies[token].Value != null && context.Request.Cookies[token].Value.Length > 2)
                {
                    string cookie = EncryptDecrypt.Decrypt(context.Request.Cookies[token].Value);
                    string[] arr = cookie.Split('=');
                    if (arr[0] == "userName")
                    {
                        if (IsInDB(arr[1]))
                        {
                            context.Items.Add("result", ConfigurationManager.AppSettings["Ok"]);
                            GenericIdentity identity = new GenericIdentity(arr[1]);
                            GenericPrincipal principal = new GenericPrincipal(identity, null);
                            context.User = principal;
                            return;
                        }
                        else
                        {
                            context.Response.Cookies.Remove(token);
                        }
                    }
                }
            }

            context.Response.Cookies.Add(new HttpCookie(token, ""));
            context.Response.Cookies.Add(new HttpCookie(userNameCookie, ""));
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

            if (IsInDB(login, pass))
            {
                context.Items.Add("result", ConfigurationManager.AppSettings["Ok"]);
                GenericIdentity identity = new GenericIdentity(login);
                GenericPrincipal principal = new GenericPrincipal(identity, null);
                context.User = principal;

            }
            else
            {
                context.Items.Add("result", ConfigurationManager.AppSettings["BadPasswordMessage"]);
            }
        }

        private bool IsInDB(string userName)
        {
            using (UserContext uc = new UserContext())
            {
                User user = uc.Set<User>().FirstOrDefault(u => u.Login == userName);
                if (user != null) return true;
                else return false;
            } 
        }

        private bool IsInDB(string userName, string pass)
        {
            using (UserContext uc = new UserContext())
            {
                User user = uc.Set<User>().FirstOrDefault(u => u.Login == userName);
                if (user != null)
                {
                    if (user.Pass == pass) return true;
                    else return false;

                }
                else return false;
            } 
        }
    }
}
