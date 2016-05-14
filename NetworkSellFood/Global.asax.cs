using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Configuration;
using PluginLoader.Loader;

namespace NetworkSellFood
{
	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes (RouteCollection routes)
		{
			routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");

			routes.MapRoute (
				"Avator",
				"ava/{uid}",
				new {controller = "UserOpt",action = "Avator",uid = "0"}
			);

			routes.MapRoute (
				"GoodsPic",
				"pic/{pid}",
				new {controller = "ShopOpt",action = "GoodsPic",pid = "0"}
			);

			routes.MapRoute ("AppLogin", 
				"appinterface/login", 
				new {controller = "UserOpt",action = "AppLogin"});
			routes.MapRoute ("AppRegister",
				"appinterface/register",
				new {controller = "UserOpt",action = "AppRegister"});
			
			routes.MapRoute (
				"Default",
				"{controller}/{action}/{id}",
				new { controller = "Home", action = "Index", id = "" }
			);

		}

		public static void RegisterGlobalFilters (GlobalFilterCollection filters)
		{
			filters.Add (new HandleErrorAttribute ());
		}

		protected void Application_Start ()
		{
			string passwdplugin = this.Server.MapPath (
				                      WebConfigurationManager.AppSettings.Get ("PasswdPlugin"));
			string textplugin = this.Server.MapPath (
				                    WebConfigurationManager.AppSettings.Get ("TextPlugin"));
			//插件预加载
			PluginLoader<AbPasswdPlugin.AbsPassword>.Load (passwdplugin);
			PluginLoader<AbTextProcess.AbTextProc>.Load (textplugin);

			AreaRegistration.RegisterAllAreas ();
			RegisterGlobalFilters (GlobalFilters.Filters);
			RegisterRoutes (RouteTable.Routes);
		}
	}
}
