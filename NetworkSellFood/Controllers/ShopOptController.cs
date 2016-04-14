using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace NetworkSellFood.Controllers
{
	[ValidateInput (false)]
	public class ShopOptController : Controller
	{
		public ActionResult AddressInfo ()
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null)
				return this.RedirectToAction ("Index", "Home");
			if (this.Request.HttpMethod.ToUpper ().Equals ("POST")) {
				
			}
			WebUserAddressCollection wac = UserOption.GetAllAddressInfo (session);
			ViewData ["AddressCollection"] = wac;
			return View (UserOption.GetUserBase (session));
		}

		public ActionResult MyCart()
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null)
				return this.RedirectToAction ("Index", "Home");
			return View (UserOption.GetUserBase (session));
		}

		public ActionResult MyIndent()
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null)
				return this.RedirectToAction ("Index", "Home");
			return View (UserOption.GetUserBase (session));
		}

		[HttpPost]
		public ActionResult AddressAdd ()
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null)
				return this.RedirectToAction ("Index", "Home");
			string commit = this.Request.Form ["commit"];
			string info = this.Request.Form ["information"];
			string person = this.Request.Form ["callperson"];
			string phone = this.Request.Form ["callphone"];
			if (!UserOption.AddAddressInfo (session, commit, info, person, phone)) {
				return Redirect ("/ShopOpt/AddressInfo?status=fail");
			}
			return RedirectToAction ("AddressInfo");
		}

		[HttpPost]
		public string ChangeAddress()
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null)
				return JsonConvert.SerializeObject (new {
					IsSuccess = false
				});
			string aid = this.Request.Form ["aid"];
			string commit = this.Request.Form ["commit"];
			string info = this.Request.Form ["information"];
			string person = this.Request.Form ["callperson"];
			string phone = this.Request.Form ["callphone"];
			if (!UserOption.ModifyAddressInfo (session, Convert.ToUInt32 (aid), commit, info, person, phone)) {
				return JsonConvert.SerializeObject (new {
					IsSuccess = false
				});
			}
			return JsonConvert.SerializeObject (new {
				IsSuccess = true
			});
		}

		[HttpPost]
		public string DeleteAddress()
		{
			WebSessionUser session = this.Session ["user"] as WebSessionUser;
			if (session == null)
				return JsonConvert.SerializeObject (new {
					IsSuccess = false
				});
			string aid = this.Request.Form ["aid"];
			if (!UserOption.RemoveAddressInfo (session, Convert.ToUInt32 (aid))) {
				return JsonConvert.SerializeObject (new {
					IsSuccess = false
				});
			}
			return JsonConvert.SerializeObject (new {
				IsSuccess = true
			});
		}
	}
}