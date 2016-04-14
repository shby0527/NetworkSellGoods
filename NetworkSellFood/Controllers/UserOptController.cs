using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace NetworkSellFood.Controllers
{
	[ValidateInput (false)]
	public class UserOptController : Controller
	{
		public ActionResult UserInfo ()
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null)
				return this.RedirectToAction ("Index", "Home");
			if (this.Request.HttpMethod.ToUpper ().Equals ("POST")) {
				if (this.Request.Form ["nickname"] != null) {
					UserOption.ChangeNickName (session, this.Request.Form ["nickname"]);
				}
			}
			return View (UserOption.GetUserBase (session));
		}

		public ActionResult Register ()
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null)
				return View ();
			else
				return View (UserOption.GetUserBase (session));
		}

		public ActionResult LoginPage ()
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null)
				return View ("Login");
			else
				return this.RedirectToAction ("UserInfo");
			
		}

		public ActionResult LogOut ()
		{
			this.Session.Remove ("user");
			HttpCookie cookies = new HttpCookie ("userId");
			cookies.Expires = DateTime.Now.AddDays (-1);
			this.Response.Cookies.Add (cookies);
			return this.RedirectToAction ("Index", "Home");
		}

		public ActionResult AvatorUp ()
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null)
				return this.RedirectToAction ("Index", "Home");
			return View (UserOption.GetUserBase (session));
		}

		[HttpPost]
		public ActionResult AvatorUpload ()
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null)
				return this.RedirectToAction ("Index", "Home");
			else if (!session.LoginSign)
				return this.RedirectToAction ("Index", "Home");
			if (this.Request.Files ["AvaUp"] == null)
				return this.RedirectToAction ("AvatorUp");
			if (this.Request.Files ["AvaUp"].ContentLength > 204800)
				return this.Redirect ("/UserOpt/UserInfo?avaImage=big"); 
			string fileName = DateTime.Now.Ticks.ToString ();
			string SavePath = this.Server.MapPath (string.Format ("~/Content/Avator/{0}.jpg", fileName));
			try {
				using (Image img = Image.FromStream (this.Request.Files ["AvaUp"].InputStream)) {
					string oldimg = UserOption.AvaImage (session.UID);
					if (oldimg != null) {
						System.IO.File.Delete (this.Server.MapPath (string.Format ("~/Content/Avator/{0}", oldimg)));
					}
					img.Save (SavePath, ImageFormat.Jpeg);
					UserOption.ChangeAvator (session, string.Format ("{0}.jpg", fileName));
					return this.RedirectToAction ("AvatorUp");
				}
			} catch (Exception) {
				return this.Redirect ("/UserOpt/UserInfo?avaImage=fail");
			}
		}

		public ActionResult Avator (uint uid)
		{
			string Path = UserOption.AvaImage (uid);
			if (Path == null)
				Path = "default.jpg";
			byte[] data;
			using (FileStream fs = System.IO.File.OpenRead (this.Server.MapPath (string.Format ("~/Content/Avator/{0}", Path)))) {
				data = new byte[fs.Length];
				fs.Read (data, 0, (int)fs.Length);
				fs.Close ();
			}
			return this.File (data, "image/jpeg");
		}

		public ActionResult PasswdChange ()
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null)
				return this.RedirectToAction ("Index", "Home");
			return View (UserOption.GetUserBase (session));
		}

		public ActionResult SafetyEmail ()
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null)
				return this.RedirectToAction ("Index", "Home");
			if (this.Request.HttpMethod.ToUpper ().Equals ("POST")) {
				string email = this.Request.Form ["email"];
				if (email != null && email != "") {
					UserOption.ChangeUserEmail (session, email);
				}
			}
			ViewData ["email"] = UserOption.GetEmail (session);
			ViewData ["IsChecked"] = UserOption.GetVailStatus (session);
			return View (UserOption.GetUserBase (session));
		}

		public ActionResult RealInfo ()
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null)
				return this.RedirectToAction ("Index", "Home");
			if (this.Request.HttpMethod.ToUpper ().Equals ("POST")) {
				string name = this.Request.Form ["name"];
				byte sex = Convert.ToByte (this.Request.Form ["sex"]);
				byte licensetype = Convert.ToByte (this.Request.Form ["licensetype"]);
				string licenseid = this.Request.Form ["licenseid"];
				string phonenumber = this.Request.Form ["phonenumber"];
				UserOption.ChangeRealInfo (session, name, sex, licensetype, licenseid, phonenumber);
			}
			WebUserRealInfomation info = UserOption.GetRealInfo (session);
			this.ViewData ["realinfo"] = info;
			return View (UserOption.GetUserBase (session));	
		}

		public ActionResult VailEmail (string id)
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null)
				return this.RedirectToAction ("Index", "Home");
			if (!UserOption.EmailVailCheck (session, id)) {
				return this.Redirect ("/UserOpt/SafetyEmail?status=fail");
			}
			return this.Redirect ("/UserOpt/SafetyEmail?status=success");
		}

		public string ResendEmail ()
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null)
				return JsonConvert.SerializeObject (new {
					IsSuccess = false
				});

			if (!UserOption.ResendEmail (session)) {
				return JsonConvert.SerializeObject (new {
					IsSuccess = false
				});
			}
			return JsonConvert.SerializeObject (new {
				IsSuccess = true
			});
		}

		[HttpPost]
		public string UserLogin ()
		{
			string username = this.Request.Form ["username"];
			string passwd = this.Request.Form ["passwd"];
			string vcode = this.Request.Form ["vcode"];
			bool isPassed = false;
			if (vcode.ToUpper () != this.Session ["VCode"].ToString ().ToUpper ()) {
				goto RTLN;
			}
			UserOption uo = new UserOption ();
			uo.UserName = username;
			uo.Password = passwd;
			if (uo.UserLogin ()) {
				isPassed = true;
				this.Session ["user"] = uo.SessionStatus;
			} else {
				goto RTLN;
			}
			if (this.Request.Form ["rememberme"] != null) {
				HttpCookie cookie = new HttpCookie ("userId");
				//Cookie 自动登录待完善
				cookie.Values.Add ("username", username);
				cookie.Values.Add ("password", passwd);
				cookie.Expires = DateTime.Now.AddDays (3);
				this.Response.Cookies.Add (cookie);
			}
			RTLN:
			if (isPassed) {
				return JsonConvert.SerializeObject (new {IsPassed = true,Href = "/"});
			} else {
				return JsonConvert.SerializeObject (new {IsPassed = false});
			}
		}

		[HttpPost]
		public string AppLogin ()
		{
			string username = this.Request.Form ["username"];
			string password = this.Request.Form ["password"];
			if (username == null || password == null) {
				return JsonConvert.SerializeObject (
					new {
						LoginStatus = false
					});
			}
			UserOption uo = new UserOption ();
			uo.UserName = username;
			uo.Password = password;
			if (!uo.UserLogin ())
				return JsonConvert.SerializeObject (
					new {
						LoginStatus = false
					});
			return JsonConvert.SerializeObject (
				new {LoginStatus = true,
					UserInfo = new {
						UserName = uo.SessionStatus.UserName,
						UID = uo.SessionStatus.UID,
						pwd = password
					}
					});
		}

		[HttpPost]
		public string AppRegister ()
		{
			string username = this.Request ["username"];
			string password = this.Request ["password"];
			if (username == null || password == null)
				return JsonConvert.SerializeObject (new  {
					RegisterStatus = false
				});
			if (username.Length < 6 || password.Length < 6)
				return JsonConvert.SerializeObject (new  {
					RegisterStatus = false
				});
			if (UserOption.CheckUser (username))
				return JsonConvert.SerializeObject (new  {
					RegisterStatus = false
				});
			if (!UserOption.RegisterUser (username, password))
				return JsonConvert.SerializeObject (new  {
					RegisterStatus = false
				});
			return JsonConvert.SerializeObject (new  {
				RegisterStatus = true
			});
		}

		[HttpPost]
		public string CheckUser ()
		{
			string username = this.Request.Form ["user"];
			if (username == null)
				username = "";
			bool isExists = UserOption.CheckUser (username);
			return JsonConvert.SerializeObject (new {IsExists = isExists});
		}

		[HttpPost]
		public string CheckVCode ()
		{
			string vcode = this.Request.Form ["vcode"];
			if (vcode == null)
				vcode = "";
			bool passed = false;//不通过
			if (vcode.ToUpper () == this.Session ["VCode"].ToString ().ToUpper ())
				passed = true;
			return JsonConvert.SerializeObject (new {IsPassed = passed});
		}

		[HttpPost]
		public string PostRegister ()
		{
			string Username = this.Request.Form ["username"];
			string Passwd = this.Request.Form ["passwd"];
			string Passwda = this.Request.Form ["passwda"];
			string VCode = this.Request.Form ["vcode"];
			if (UserOption.CheckUser (Username)) {
				goto RTL;
			}
			if (Passwd != Passwda) {
				goto RTL;
			}
			if (VCode.ToUpper () != this.Session ["VCode"].ToString ().ToUpper ()) {
				goto RTL;
			}
			if (UserOption.RegisterUser (Username, Passwd)) {
				return JsonConvert.SerializeObject (new {
					IsRegister = true,
					Href = "/UserOpt/LoginPage"
				});
			}
			RTL:
			return JsonConvert.SerializeObject (new {
				IsRegister = false
			});
		}

		[HttpPost]
		public string PasswdChg ()
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null)
				return JsonConvert.SerializeObject (new {
					IsPassed = false
				});
			if (!session.LoginSign)
				return JsonConvert.SerializeObject (new {
					IsPassed = false
				});
			string oldpwd = this.Request.Form ["oldpasswd"];
			string pwd = this.Request.Form ["passwd"];
			string pwda = this.Request.Form ["passwda"];
			if (pwd.Length < 6)
				return JsonConvert.SerializeObject (new {
					IsPassed = false
				});
			if (pwd != pwda)
				return JsonConvert.SerializeObject (new {
					IsPassed = false
				});
			if (!UserOption.ChangePassword (session, oldpwd, pwd))
				return JsonConvert.SerializeObject (new {
					IsPassed = false
				});
			return JsonConvert.SerializeObject (new {
				IsPassed = true
			});
		}
	}
}
