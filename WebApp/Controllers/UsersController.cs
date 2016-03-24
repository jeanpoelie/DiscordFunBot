using System.Web.Mvc;

namespace WebApp.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Web;

	using AutoMapper;

	using Business.Models;

	using Models;

	public class UsersController : BaseController
	{
		public ActionResult Index()
		{
			if (!UserIsInRole(Role.Admin))
			{
				throw new HttpException(403, "You are not allowed.");
			}

			ViewBag.Users = Mapper.Map<List<DiscordUserModel>>(Business.User.GetAll());
			return View(DiscordUser);
		}

		public ActionResult Edit()
		{
			if (!UserIsInRole(Role.Admin))
			{
				  throw new HttpException(403, "You are not allowed.");
			}

			long? id = RouteData.Values["id"] != null ? long.Parse(RouteData.Values["id"].ToString()) : -1;
            if (id < 0)
			{
				throw new NullReferenceException("This user does not exist.");
			}

			var user = Mapper.Map<DiscordUserModel>(Business.User.Get((long)id));

			ViewBag.Roles = Mapper.Map<List<RoleModel>>(Business.Role.GetAll());
			return this.View(user);
		}

		[HttpPost]
		public ActionResult Edit(DiscordUserModel user)
		{
			if (!UserIsInRole(Role.Admin))
			{
				throw new HttpException(403, "You are not allowed.");
			}

			try
			{
				if (ModelState.IsValid)
				{
					var businessUser = Mapper.Map<BusinessDiscordUserModel>(user);
					Business.User.Update(businessUser);
					ViewBag.SuccessMessage = "You have successfully been updated!";
				}

				return this.RedirectToAction("Index");
			}
			catch  (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
			}
			
			return this.View(user);
		}

		
	}
}