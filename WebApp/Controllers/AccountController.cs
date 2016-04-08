using System.Web.Mvc;

namespace WebApp.Controllers
{
	using System;
	using System.Collections.Generic;

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

		public ActionResult UserSubscriptions()
		{
			var subscribedUsers = Business.UserSubscription.GetAll(DiscordUser.Id, DiscordUser.Name);
			return this.View(Mapper.Map<List<UserSubscriptionModel>>(subscribedUsers));
		}

		[HttpPost]
		public ActionResult UserSubscriptions(List<UserSubscriptionModel> userSubscriptionModels)
		{
			var businessUserSubscriptionModels = Mapper.Map<List<BusinessUserSubscriptionModel>>(userSubscriptionModels);
			businessUserSubscriptionModels.ForEach(busm => busm.UserId = DiscordUser.Id);
			Business.UserSubscription.Add(businessUserSubscriptionModels);
			return this.View(userSubscriptionModels);
		}

		public ActionResult AddUserSubscriptions()
		{
			var model = new UserSubscriptionModel();
			ViewBag.Users = Mapper.Map<IList<DiscordUserModel>>(Business.User.GetAll());

			return PartialView("~/Views/Account/_UserSubscription.cshtml", model);
		}
	}
}