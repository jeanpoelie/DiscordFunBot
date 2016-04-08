using System.Web.Mvc;

namespace WebApp.Controllers
{
	using System;
	using System.Collections.Generic;

	using AutoMapper;

	using Business.Models;

	using WebApp.Models;

	public class HomeController : BaseController
	{
		public ActionResult Index()
		{
			return View(DiscordUser);
		}

		[HttpPost]
		public void AddSuggestion(string suggestionText)
		{
			try
			{
				var businessDiscordUser = Mapper.Map<BusinessDiscordUserModel>(DiscordUser);
				var suggestion = new SuggestionModel { Suggestion = suggestionText, UserId = businessDiscordUser.Id };
				Business.Suggestion.Add(Mapper.Map<BusinessSuggestionModel>(suggestion));
			}
			catch (Exception ex)
			{
				throw new ArgumentException(ex.Message);
			}
			
		}

		public ActionResult Suggestions()
		{
			var model = Mapper.Map<List<SuggestionModel>>(Business.Suggestion.GetAll());
			return View(model);
		}
    }
}