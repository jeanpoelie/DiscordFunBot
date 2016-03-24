using System.Web.Mvc;

namespace WebApp.Controllers
{
	using System;

	using AutoMapper;

	using Business.Models;

	public class HomeController : BaseController
	{
		public ActionResult Index()
		{
			return View(DiscordUser);
		}

		[HttpPost]
		public void AddSuggestion(string suggestion)
		{
			try
			{
				var businessDiscordUser = Mapper.Map<BusinessDiscordUserModel>(DiscordUser);
				Business.User.AddSuggestion(suggestion, businessDiscordUser);
			}
			catch (Exception ex)
			{
				throw new ArgumentException(ex.Message);
			}
			
		}
    }
}