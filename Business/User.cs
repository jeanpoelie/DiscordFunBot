namespace Business
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Extensions;
	using Models;

	public class User
	{ 
		public static void Add(BusinessDiscordUserModel user)
		{
			if (user == null)
			{
				throw new NullReferenceException("The user is empty");
			}

			DatabaseCommunication.AddUser(user);
		}

		public static IList<BusinessDiscordUserModel> Find(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new NullReferenceException(name);
			}

			return DatabaseCommunication.FindUser(name).ToList<BusinessDiscordUserModel>();
		}

		public static BusinessDiscordUserModel Get(long id)
		{
			if (id <= 0)
			{
				throw new NullReferenceException("This user does not exist.");
			}
			
			return DatabaseCommunication.GetUser(id).ToList<BusinessDiscordUserModel>().FirstOrDefault();
		}

		public static IList<BusinessDiscordUserModel> GetAll()
		{
			return DatabaseCommunication.GetAllUsers().ToList<BusinessDiscordUserModel>();
		}

		public static IList<BusinessDiscordUserModel> GetSubscribedUsers(BusinessDiscordUserModel user)
		{
			return DatabaseCommunication.GetSubscribedUsers(user.Id).ToList<BusinessDiscordUserModel>();
		}

		public static IList<BusinessDiscordUserModel> GetUsersWithBirthdate(DateTime birthDate)
		{
			return DatabaseCommunication.GetUsersWithBirthdate(birthDate).ToList<BusinessDiscordUserModel>();
		}

		public static void Update(BusinessDiscordUserModel user)
		{
			if (user == null)
			{
				throw new NullReferenceException("The user is empty.");
			}

			DatabaseCommunication.UpdateUser(user);
		}

		public static void UpdateOrInsert(BusinessDiscordUserModel user)
		{
			var discordUser = Get(user.Id);

			if (discordUser == null)
			{
				Console.WriteLine("Added a new user: " + user.Name);
				Add(user);
				return;
			}

			Console.WriteLine("Updated a user: " + user.Name);
			Update(discordUser);
		}

		public static void UpdateRole(BusinessDiscordUserModel user, int role)
		{
			if (user == null)
			{
				throw new NullReferenceException("The user is empty.");
			}

			if (role <= 0)
			{
				throw new NullReferenceException("The users new role is not provided.");
			}

			DatabaseCommunication.UpdateRole(user, role);
		}

		public static void UpdateRoleFromName(BusinessDiscordUserModel user, string roleName)
		{
			if (user == null)
			{
				throw new NullReferenceException("The user is empty.");
			}

			if (string.IsNullOrEmpty(roleName))
			{
				throw new NullReferenceException("The users new role is not provided.");
			}

			var roleId = DatabaseCommunication.GetRoleId(roleName).ToList<int>().FirstOrDefault();
			UpdateRole(user, roleId);
		}

		public static BusinessDiscordUserModel AuthorizeUser(string token, string uid)
		{
			var user = DatabaseCommunication.AuthorizeUser(token, uid).ToList<BusinessDiscordUserModel>();
			return user.FirstOrDefault();
		}
	}
}