using System.Web.Mvc;

namespace WebApp.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

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
			ViewBag.Users = Mapper.Map<IList<DiscordUserModel>>(Business.User.GetAll());
			ViewBag.UserSubscriptions = Mapper.Map<List<UserSubscriptionModel>>(subscribedUsers);
			var user = Mapper.Map<DiscordUserModel>(Business.User.Get(DiscordUser.Id));
			return this.View(user);
		}

		[HttpPost]
		public ActionResult UserSubscriptions(List<UserSubscriptionModel> userSubscriptionModels)
		{
			try
			{
				var businessUserSubscriptionModels = Mapper.Map<List<BusinessUserSubscriptionModel>>(userSubscriptionModels).Distinct().ToList();
				businessUserSubscriptionModels.ForEach(busm => busm.UserId = DiscordUser.Id);
				Business.UserSubscription.Add(businessUserSubscriptionModels);
				ViewBag.Users = Mapper.Map<IList<DiscordUserModel>>(Business.User.GetAll());

				ViewBag.SuccessMessage = "You have successfully saved your subscriptions!";
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
			}

			ViewBag.UserSubscriptions = Mapper.Map<List<UserSubscriptionModel>>(userSubscriptionModels);
			var user = Mapper.Map<DiscordUserModel>(Business.User.Get(DiscordUser.Id));
			return this.View(user);
		}

		public ActionResult AddUserSubscriptions()
		{
			var model = new UserSubscriptionModel();
			ViewBag.Users = Mapper.Map<IList<DiscordUserModel>>(Business.User.GetAll());

			return PartialView("~/Views/Account/_UserSubscription.cshtml", model);
		}
	}
}