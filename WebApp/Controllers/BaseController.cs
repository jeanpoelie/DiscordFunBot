using System.Web.Mvc;

namespace WebApp.Controllers
{
	using System;
	using System.Web;

	using AutoMapper;

	using Models;

	public class BaseController : Controller
	{
		public static DiscordUserModel DiscordUser;

		private ActionResult Lockout()
		{
			return View("Lockout");
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			HttpCookie uidCookie = new HttpCookie("uid");
			HttpCookie tokenCookie = new HttpCookie("token");

			string token = Request["token"];
			string UID = Request["uid"];

			if (string.IsNullOrEmpty(token) && Request.Cookies["token"] != null)
			{
				token = Request.Cookies["token"].Value;
			}

			if (string.IsNullOrEmpty(UID) && Request.Cookies["uid"] != null)
			{
				UID = Request.Cookies["uid"].Value;
			}

			if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(UID))
			{
				filterContext.Result = this.View("Lockout");
				return;
			}

			DiscordUser = Mapper.Map<DiscordUserModel>(Business.User.AuthorizeUser(token, UID));

			if (DiscordUser == null)
			{
				throw new NullReferenceException("This user does not exist.");
			}

			uidCookie.Value = UID;
			uidCookie.Expires = DateTime.UtcNow.AddMonths(1);

			tokenCookie.Value = token;
			tokenCookie.Expires = DateTime.UtcNow.AddMonths(1);
			Response.Cookies.Add(uidCookie);
			Response.Cookies.Add(tokenCookie);

			ViewBag.RoleId = DiscordUser.RoleId;
		}

		public static bool UserIsInRole(Role role)
		{
			return DiscordUser.RoleId == (int)role;
		}
	}
}