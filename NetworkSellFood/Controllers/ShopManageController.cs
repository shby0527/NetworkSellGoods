using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetworkSellFood.Controllers
{
	[ValidateInput (false)]
	public class ShopManageController : Controller
	{
		public ActionResult GoodsManage()
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null)
				return this.RedirectToAction ("Index", "Home");
			if (!(AdminOption.PermissionCheck (session, WebUserGroup.AddMenu) ||
				AdminOption.PermissionCheck (session, WebUserGroup.DeleteMenu) ||
				AdminOption.PermissionCheck (session, WebUserGroup.AddType) ||
				AdminOption.PermissionCheck (session, WebUserGroup.DeleteType)))
				return this.RedirectToAction ("Index", "Home");
			return View (UserOption.GetUserBase (session));
		}

		public ActionResult TypeManage()
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null)
				return this.RedirectToAction ("Index", "Home");
			if (!(AdminOption.PermissionCheck (session, WebUserGroup.AddMenu) ||
				AdminOption.PermissionCheck (session, WebUserGroup.DeleteMenu) ||
				AdminOption.PermissionCheck (session, WebUserGroup.AddType) ||
				AdminOption.PermissionCheck (session, WebUserGroup.DeleteType)))
				return this.RedirectToAction ("Index", "Home");
			return View (UserOption.GetUserBase (session));
		}

		public ActionResult IndentManage()
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null)
				return this.RedirectToAction ("Index", "Home");
			if (!(AdminOption.PermissionCheck (session, WebUserGroup.AddMenu) ||
				AdminOption.PermissionCheck (session, WebUserGroup.DeleteMenu) ||
				AdminOption.PermissionCheck (session, WebUserGroup.AddType) ||
				AdminOption.PermissionCheck (session, WebUserGroup.DeleteType)))
				return this.RedirectToAction ("Index", "Home");
			return View (UserOption.GetUserBase (session));
		}
	}
}
