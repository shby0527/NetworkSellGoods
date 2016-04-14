using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
			return View (UserOption.GetUserBase (session));
		}
	}
}
