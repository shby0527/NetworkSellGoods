using System;
using System.Web.Configuration;

namespace NetworkSellFood
{
	public static class DatabaseUser
	{

		public static string DataServer {
			get {
				return WebConfigurationManager.AppSettings.Get ("DataServer");
			}
		}

		public static string DataPort {
			get {
				return WebConfigurationManager.AppSettings.Get ("DataPort");
			}
		}

		public static string DataUser {
			get {
				return WebConfigurationManager.AppSettings.Get ("DataUser");
			}
		}

		public static string DataPasswd {
			get {
				return WebConfigurationManager.AppSettings.Get ("DataPasswd");
			}
		}

		public static string Database {
			get {
				return WebConfigurationManager.AppSettings.Get ("Database");
			}
		}
	}
}

