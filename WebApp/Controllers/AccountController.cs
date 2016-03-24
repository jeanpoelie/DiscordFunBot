using System.Web.Mvc;

namespace WebApp.Controllers
{
	using System;

	using AutoMapper;

	using Business.Models;

	using Models;

	public class AccountController : BaseController
	{
		public ActionResult Index()
		{
			return View(DiscordUser);
		}

		public ActionResult Edit()
		{
			return this.View(DiscordUser);
		}

		[HttpPost]
		public ActionResult Edit(DiscordUserModel user)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var businessUser = Mapper.Map<BusinessDiscordUserModel>(user);
					Business.User.Update(businessUser);
					ViewBag.SuccessMessage = "You have successfully been updated!";
				}

				return this.View("Index", user);
			}
			catch  (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
			}
			
			return this.View(user);
		}
	}
}