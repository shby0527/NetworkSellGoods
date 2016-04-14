using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using PluginLoader.Loader;
using AbPasswdPlugin;

namespace NetworkSellFood.Controllers
{
	[ValidateInput(false)]
	public class HomeController : Controller
	{
		public ActionResult Index ()
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null) {
				HttpCookie cookie = this.Request.Cookies ["userId"];
				if (cookie != null) {
					UserOption uo = new UserOption ();
					uo.UserName = cookie.Values ["username"];
					uo.Password = cookie.Values ["password"];
					if (!uo.UserLogin ()) {
						HttpCookie respon = new HttpCookie ("userId");
						respon.Expires = DateTime.Now.AddDays (-1);
						this.Response.Cookies.Add (respon);
						return View ();
					}
					this.Session ["user"] = uo.SessionStatus;
					return View (UserOption.GetUserBase (uo.SessionStatus));
				}
				return View ();
			}
			else
				return View (UserOption.GetUserBase (session));
		}

		/// <summary>
		/// 产生验证码
		/// </summary>
		/// <returns>The code.</returns>
		public ActionResult VailCode ()
		{
			byte[] data;
			#region 产生验证码
			char[] allselect = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray ();
			const int vcodelen = 5;
			Random rmd = new Random ();
			StringBuilder sb = new StringBuilder (vcodelen);
			for (int i = 0; i < vcodelen; i++) {
				sb.Append (allselect [rmd.Next (0, allselect.Length)]);
			}
			string vcode = sb.ToString ();
			this.Session ["VCode"] = vcode;
			#endregion
			using (MemoryStream ms = new MemoryStream ()) {
				using (Bitmap bmp = new Bitmap (80, 26)) {
					using (Graphics gp = Graphics.FromImage (bmp)) {
						Color color = Color.FromArgb (rmd.Next (100, 255),
							rmd.Next (100, 255),
							rmd.Next (100, 255));
						gp.Clear (color);
						using (Brush brush = new SolidBrush (Color.FromArgb (rmd.Next (0, 100),
							rmd.Next (0, 100),
							rmd.Next (0, 100)))) {
							using (Font font = new Font ("Times New Roman", 16f)) {
								gp.DrawString (vcode, font, brush, 8, 2);

								#region 画200个干扰点
								for (int i = 0; i < 150; i++) {
									bmp.SetPixel (rmd.Next (0, 80), rmd.Next (0, 26), Color.FromArgb (rmd.Next (0, 0xffffff)));
								}
								#endregion

								#region 画5条干扰线
								using (Pen pen = new Pen (Color.FromArgb (rmd.Next (0, 100), rmd.Next (0, 100), rmd.Next (0, 100)), 1)) {
									for (int i = 0; i < 5; i++) {
										Point xp = new Point (rmd.Next (0, 80), rmd.Next (0, 26));
										Point yp = new Point (rmd.Next (0, 80), rmd.Next (0, 26));
										gp.DrawLine (pen, xp, yp);
										pen.Color = Color.FromArgb (rmd.Next (0, 150), rmd.Next (0, 150), rmd.Next (0, 150));
									}
								}
								gp.Flush();
								#endregion
								bmp.Save (ms, ImageFormat.Jpeg);
								data = ms.GetBuffer ();
							}
						}
					}
				}
			}
			return this.File (data, "image/jpeg");
		}
	}
}

