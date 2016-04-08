namespace Business
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Extensions;
	using Models;

	public class UserSubscription
	{ 
		public static void Add(List<BusinessUserSubscriptionModel> subscriptions)
		{
			if (subscriptions.Any())
			{
				throw new NullReferenceException("The subscription is empty");
			}

			DatabaseCommunication.DeleteUserSubscription(subscriptions[0].UserId);

			foreach (var subscription in subscriptions)
			{
				DatabaseCommunication.AddUserSubscription(subscription);
			}
		}

		/// <summary>
		/// Gets all the users this person is subscribed to
		/// </summary>
		/// <param name="id">The ID of this user.</param>
		/// <param name="userName">The name of this user</param>
		/// <returns>A list of subscribed Users.</returns>
		public static IList<BusinessUserSubscriptionModel> GetAll(long id, string userName)
		{
			return DatabaseCommunication.GetSubscribedUsersList(id, userName).ToList<BusinessUserSubscriptionModel>();
		}

		public static IList<BusinessDiscordUserModel> GetSubscribers(long id)
		{
			return DatabaseCommunication.GetSubscribers(id).ToList<BusinessDiscordUserModel>();
		}
    }
}