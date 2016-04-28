using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace NetworkSellFood.Controllers
{
	[ValidateInput (false)]
	public class UserManageController : Controller
	{
		public ActionResult UserManageMain ()
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null)
				return this.RedirectToAction ("Index", "Home");
			if (!AdminOption.PermissionCheck (session, WebUserGroup.UserManage))
				return this.RedirectToAction ("Index", "Home");
			string search = this.Request.QueryString ["search"];
			if (search != null) {
				WebUser user = AdminOption.SearchUserByUserName (session, search);
				this.ViewData ["SearchUser"] = user;
			}
			return View (UserOption.GetUserBase (session));
		}

		[HttpPost]
		public string ResetPasswd(uint id)
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null) {
				return JsonConvert.SerializeObject (new {
					IsSuccess = false,
					Reason = "没有登录",
					Redict = "/"
				});
			}
			if (!AdminOption.PermissionCheck (session, WebUserGroup.UserManage)) {
				return JsonConvert.SerializeObject (new {
					IsSuccess = false,
					Reason = "没有权限",
					Redict = "/UserOpt/UserInfo"
				});
			}
			if (!AdminOption.ResetPassword (session, id)) {
				return JsonConvert.SerializeObject (new {
					IsSuccess = false,
					Reason = "初始化失败",
					Redict = "/UserOpt/UserInfo"
				});
			}
			return JsonConvert.SerializeObject (new {
				IsSuccess = true,
				Reason = "初始化成功",
				Redict = this.Request.UrlReferrer.AbsoluteUri
			});
		}
	}
}
