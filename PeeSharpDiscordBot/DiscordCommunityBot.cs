namespace PeeSharpDiscordBot
{
	using Discord;

	using System;
	using System.Collections.Generic;
	using System.IO.Pipes;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	using AutoMapper;

	using Business.Models;

	using Models;

	/*
		TODO: 
		- Keep track of birthday
		- Notify for birthdays
		- Add wishlist for birthday presents
		- Add ignore a user option (make sure it never ignores me)
		- Add special notification option for specific celebrations.
	*/

	/// <summary>
	/// Robot for community stuff
	/// </summary>
	public class DiscordCommunityBot : BotBase
	{
		/// <summary>
		/// The bot itself
		/// </summary>
		private readonly DiscordClient bot;

		/// <summary>
		/// Initializes a new instance of the <see cref="DiscordCommunityBot"/> class. 
		/// The constructor
		/// </summary>
		public DiscordCommunityBot()
		{
			Console.WriteLine("Started community bot.");

			bot = new DiscordClient(); 

			// Bot events
			bot.MessageReceived += Bot_MessageReceived;
			bot.MessageSent += Bot_MessageSent;

			// Connect the bot
			bot.Connect("discordcommunityrobot@gmail.com", "ditisgeheim");

			// Testing functions
			GlobalBirthdayNotification();

			// Scheduled Actions 
			ScheduleAction(new Action(SynchronizeUsers), new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0));
			ScheduleAction(new Action(GlobalBirthdayNotification), new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 11, 0, 0));
			ScheduleAction(new Action(BirthdayNotification), new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 9, 0, 0));
			
			//bot.Wait();
		}

		/// <summary>
		/// Some events/reactions
		/// </summary>
		/// <param name="sender">The sender</param>
		/// <param name="e">The event</param>
		private async void Bot_MessageReceived(object sender, MessageEventArgs e)
		{
			if (e.Message.IsAuthor)
			{
				return;
			}

			string baseUrl = "http://localhost:54016/";

			if (e.Channel.IsPrivate)
			{
				var user = new DiscordUserModel();
				var users = Mapper.Map<List<DiscordUserModel>>(Business.User.GetAll());
				var message = e.Message.Text.ToLower();

				foreach (var _user in users.Where(_user => _user.Id == (long)e.User.Id))
				{
					user = new DiscordUserModel
					{
						AddDate = _user.AddDate,
						Id = (long)e.User.Id,
						Name = e.User.Name,
						UpdateDate = _user.UpdateDate,
						Birthdate = _user.Birthdate,
						Ignored = _user.Ignored,
						RoleId = _user.RoleId,
						Token = _user.Token
					};

					var businessUser = Mapper.Map<BusinessDiscordUserModel>(user);
					Business.User.Update(businessUser);
				}

				if (user.Id == 0)
				{
					user = new DiscordUserModel { AddDate = null, Id = (long)e.User.Id, Name = e.User.Name, UpdateDate = null };
					var businessUser = Mapper.Map<BusinessDiscordUserModel>(user);
					Business.User.Add(businessUser);
				}

#region [Everyone]
				if (e.Message.Text.Contains("!webapp") || e.Message.Text.Contains("webapp"))
				{
					await e.User.SendMessage(@"Please go to " + baseUrl + "?uid=" + user.Id + "&token=" + user.Token +
" *Share this url on your own risk!*");
				}

				if (e.Message.Text.Contains("!commands") || e.Message.Text.Contains("!help"))
				{
					await e.User.SendMessage(@"  
  **Private Commands**
- *!birthdaylist* **Answer:** *Get a list of all known birthdays*	 
- *!ignorelist* **Answer:** *Get a list of all the people that I ignore*
- *!webapp* **Answer:** *To edit your account, add your birthday etc.*

  **Admin Commands**
- *All admin commands are moved to the webapp*");
					return;
				}

				if (message.Contains("!ignorelist"))
				{
					if (user.Ignored)
					{
						return;
					}

					var ignoreList = users.Where(_user => _user.Ignored)
										.Aggregate(string.Empty, (current, _user) => current + ("- " + _user.Name + @"
"));

					await e.Channel.SendMessage($"{e.User.Mention} \r\n**Ignored people: **\r\n{(string.IsNullOrEmpty(ignoreList) ? "I like talking to everyone" : ignoreList)}");
					return;
				}

				if (message.Contains("!birthdaylist"))
				{
					if (user.Ignored)
					{
						return;
					}

					var birthdayList = users.Where(_user => _user.Birthdate != null)
						.Aggregate(string.Empty, (current, _user) => current + ("- " + _user.Name + ": " + _user.Birthdate.Value.ToShortDateString() + " *Days Left: " + DaysUntillDate(_user.Birthdate.Value.AddYears(DateTime.Today.Year - _user.Birthdate.Value.Year), DateTime.Today) + @"*
"));
					Console.Write(birthdayList.Count());
					await e.Channel.SendMessage($"{e.User.Mention} \r\n**Birthdays: **\r\n{(string.IsNullOrEmpty(birthdayList) ? "There are no known birthdays." : birthdayList)}");
					return;
				}
				#endregion

				#region [User]

				if (user.RoleId <= 3)
				{
					
				}
				#endregion

				#region [Organizer]
				// TODO:  IMPELEMENT
				if (user.RoleId <= 2)
				{

				}
				#endregion

				#region [Admins]
				if (user.RoleId <= 1)
				{
					
				}
				#endregion
			}
			else
			{
				// If we do not get mentioned, do not react.
				if (!e.Message.IsMentioningMe())
				{
					return;
				}

				var message = e.Message.Text.ToLower();
				message = message.Replace("@" + this.bot.CurrentUser.Name.ToLower() + " ", string.Empty);
				message = message.Replace(this.bot.CurrentUser.Name.ToLower() + " ", string.Empty);
				Console.WriteLine(e.User.Name + " said: " + message);

				var user = new DiscordUserModel();
				var users = Mapper.Map<List<DiscordUserModel>>(Business.User.GetAll());

				foreach (var _user in users.Where(_user => _user.Id == (long)e.User.Id))
				{
					user = new DiscordUserModel
					{
						AddDate = _user.AddDate,
						Id = (long)e.User.Id,
						Name = e.User.Name,
						UpdateDate = _user.UpdateDate,
						Birthdate = _user.Birthdate,
						Ignored = _user.Ignored,
						RoleId = _user.RoleId
					};

					var businessUser = Mapper.Map<BusinessDiscordUserModel>(user);
					Business.User.Update(businessUser);
				}

				if (user.Id == 0)
				{
					user = new DiscordUserModel { AddDate = null, Id = (long)e.User.Id, Name = e.User.Name, UpdateDate = null };
					var businessUser = Mapper.Map<BusinessDiscordUserModel>(user);
					Business.User.Add(businessUser);
				}


				#region [Everyone]
				if (message.Contains("!commands") || message.Contains("!help"))
				{
					await e.User.SendMessage(@" 
  **Private Commands**
- *!birthdaylist* **Answer:** *Get a list of all known birthdays*	 
- *!ignorelist* **Answer:** *Get a list of all the people that I ignore*
- *!webapp* **Answer:** *To edit your account, add your birthday etc.*

  **Admin Commands**
- *All admin commands are moved to the webapp*");

					await e.Channel.SendMessage(e.User.Mention + " I have send you a private message!");
                    return;
				}

				// Connect to talking API
				if (message.Contains("hi!") || message.Contains("hi") || message.Contains("hello") || message.Contains("bonjour"))
				{
					await e.Channel.SendMessage(e.User.Mention + " Greetings!");
					return;
				}
				#endregion
			}
		}

		public async void BirthdayNotification()
		{
			Console.WriteLine("Sending Birthday Notifications.");
			var birthdayUsers = Mapper.Map<List<DiscordUserModel>>(Business.User.GetUsersWithBirthdate(DateTime.Today));

			if (birthdayUsers == null || !birthdayUsers.Any())
			{
				Console.WriteLine("There are no birthdays today.");
				return;
			}

			foreach (var birthdayUser in birthdayUsers)
			{
				var subscribedUsers = Mapper.Map<List<DiscordUserModel>>(Business.UserSubscription.GetSubscribers(birthdayUser.Id));

				foreach (var subscribedUser in subscribedUsers)
				{
					var discordUser = bot.Servers.SelectMany(s => s.Users.Where(u => u.Id == (ulong)subscribedUser.Id)).FirstOrDefault();

					if (discordUser != null && birthdayUser.Birthdate != null)
					{
						await discordUser.SendMessage("Hey, It's " + birthdayUser.Name + " birthday today! They are now " + (DateTime.Today.Year - birthdayUser.Birthdate.Value.Date.Year) + ".");
					}
				}
			}
		}

		public void GlobalBirthdayNotification()
		{
			Console.WriteLine("Sending Global Birthday Notifications.");
			Thread.Sleep(3000);
			var birthdayUsers = Mapper.Map<List<DiscordUserModel>>(Business.User.GetUsersWithBirthdate(DateTime.Today));

			if (birthdayUsers == null || !birthdayUsers.Any())
			{
				Console.WriteLine("There are no birthdays today.");
				return;
			}

			foreach (var birthdayUser in birthdayUsers)
			{
				foreach (var server in bot.Servers)
				{
					foreach (var channel in server.TextChannels)
					{
						if (channel.Name.ToLower() == "birthdays" || channel.Name.ToLower() == "notifications")
						{
							if (birthdayUser.Birthdate != null)
							{
								channel.SendMessage(
									"Hey, It's " + birthdayUser.Name + " birthday today! They are now "
									+ (DateTime.Today.Year - birthdayUser.Birthdate.Value.Date.Year) + ".");
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Synchronize all the users from all the Discord Servers with my current database (Only add/update)
		/// </summary>
		public void SynchronizeUsers()
		{
			Console.WriteLine("Synchronizing Users.");
			
			var servers = bot.Servers;
			foreach (var businessUser in from server in servers from user in server.Users select new BusinessDiscordUserModel { Id = (long)user.Id, Name = user.Name })
			{
				Business.User.UpdateOrInsert(businessUser);
			}
			Console.WriteLine("Done Synchronizing.");
		}

		public static void ImAlive()
		{
			Console.WriteLine("I'm reached");
		}


		private void Bot_MessageSent(object sender, MessageEventArgs e)
		{
			Console.WriteLine("[" + DateTime.UtcNow + "] [Event]: Message Send. [Action] "+ e.Message + " Send to channel: " + e.Channel.Name + " To Server: " + e.Server.Name);
		}
	}
}
