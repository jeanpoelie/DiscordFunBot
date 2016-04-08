namespace Business
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Extensions;
	using Models;

	public class Suggestion
	{
		public static void Add(BusinessSuggestionModel suggestion)
		{
			if (suggestion == null)
			{
				throw new NullReferenceException("The suggestion is empty");
			}

			DatabaseCommunication.AddUserSuggestion(suggestion);
		}

		public static IList<BusinessSuggestionModel> GetAll()
		{
			return DatabaseCommunication.GetAllSuggestions().ToList<BusinessSuggestionModel>();
		}
	}
}