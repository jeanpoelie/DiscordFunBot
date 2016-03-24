namespace PeeSharpDiscordBot
{
	using Discord;

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;

	using AutoMapper;

	using Business.Models;

	using Extensions;
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

			bot.MessageReceived += Bot_MessageReceived;

			bot.Connect("discordcommunityrobot@gmail.com", "ditisgeheim");

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

				if (e.Message.Text.Contains("!webapp") || e.Message.Text.Contains("webapp"))
				{
					await e.Channel.SendMessage(@"Please go to " + baseUrl + "?uid=" + user.Id + "&token=" + user.Token +
"*Share this url on your own risk!*");
				}

				if (e.Message.Text.Contains("!commands"))
				{
					await e.Channel.SendMessage(
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
					e.User.Mention
						+ @" 
  **Community Commands**
- *!webapp* **Answer:** *To edit your account*");
					return;
				}
			}
			else
			{
				if (!e.Message.Text.Contains(this.bot.CurrentUser.Name) || !e.Message.Text.Contains("@" + this.bot.CurrentUser.Name))
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
				if (message.Contains("hi!") || message.Contains("hi") || message.Contains("hello") || message.Contains("bonjour"))
				{
					await e.Channel.SendMessage(e.User.Mention + " Greetings!");
					return;
				}

				if (message.Contains("!commands"))
				{
					await e.Channel.SendMessage(
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
					e.User.Mention
						+ @" 
  **Community Commands**
- *!birthdaylist* **Answer:** *Get a list of all known birthdays*	 
- *!ignorelist* **Answer:** *Get a list of all the people that I ignore*
- *!birthday* **Answer:** *Use Add/Remove to add or remove a birthday* **(Example: !birthday add @user dd/mm/yyy)** ***__Moved to Webapp__***
- *!commandwish* **Answer:** *Add a wish for commands* **(Example: !commandwish random quiz question)** ***__Moved to Webapp__***

  **Admin Commands**
- *!ignore* **Answer:** *Add/Remove people from the ignore list* **(Example: !ignore add @user)**
- *!role* **Answer:** *Change someones role* **(Example: !role @{user} #{role})** ***__Moved to Webapp__***

*Private message me and ask for your own webapp unique url.*");
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

					await e.Channel.SendMessage($"{e.User.Mention} \r\n**Birthdays: **\r\n{(string.IsNullOrEmpty(birthdayList) ? "There don't know anyone who has a birthday" : birthdayList)}");
					return;
				}
				#endregion

				#region [User]

				if (user.RoleId <= 3)
				{
					//if (message.Contains("!birthday"))
					//{
					//	if (user.Ignored)
					//	{
					//		return;
					//	}

					//	if (message.Contains("add"))
					//	{
					//		var regex = new Regex(@"\b\d{2}-\d{2}-\d{4}\b");
					//		var result = regex.Match(message);
					//		if (result.Success)
					//		{
					//			DateTime date;
					//			if (DateTime.TryParse(result.Value, out date))
					//			{
					//				date = DateTime.Parse(result.Value);
					//				user.Birthdate = date;
					//				var businessUser = Mapper.Map<BusinessDiscordUserModel>(user);
					//				Business.User.Update(businessUser);

					//				await e.Channel.SendMessage(e.User.Mention + @" I have successfully uploaded your birthday into my memory.");
					//				return;
					//			}

					//			await
					//				e.Channel.SendMessage(
					//					e.User.Mention
					//					+ @" Please try again, this is not a valid date, the format should be DD-MM-YYYY *Example: 01-01-1970*");
					//			return;
					//		}
					//		return;
					//	}

					//	if (message.Contains("remove") || message.Contains("remove"))
					//	{
					//		user.Birthdate = null;
					//		var businessUser = Mapper.Map<BusinessDiscordUserModel>(user);
					//		Business.User.Update(businessUser);
					//		await e.Channel.SendMessage(e.User.Mention + @" I have successfully erased your birthday from my memory.");

					//		return;
					//	}

					//	return;
					//}

					//if (message.Contains("!commandwish"))
					//{
					//	// TODO : Get the list of ignored peoples.
					//	await e.Channel.SendMessage(e.User.Mention + " Not implemented yet!");
					//	return;
					//}
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
					if (message.Contains("!ignore"))
					{
						if (message.Contains("add"))
						{
							var messages = message.Split(' ');
							foreach (var msg in messages)
							{
								if (msg.Contains("@"))
								{
									var name = msg.Split('@')[1];
									var foundUsers = Mapper.Map<List<DiscordUserModel>>(Business.User.Find(name));
									var foundUser = foundUsers.FirstOrDefault();
									if (foundUser != null)
									{
										foundUser.Ignored = true;
										var businessUser = Mapper.Map<BusinessDiscordUserModel>(foundUser);
										Business.User.Update(businessUser);
										await e.Channel.SendMessage(e.User.Mention + @" I will be ignoring " + name + " from now on.");
										return;
									}

									await
										e.Channel.SendMessage(e.User.Mention + @" I cannot find a user with the name: " + name + " in my memory.");
									return;
								}
							}
							await e.Channel.SendMessage(e.User.Mention + @" Please tag the user you would like to ignore like this *!ignore add @{user}.");
							return;
						}

						if (message.Contains("remove") || message.Contains("remove"))
						{
							var messages = message.Split(' ');
							foreach (var msg in messages)
							{
								if (msg.Contains("@"))
								{
									var name = msg.Split('@')[1];
									var foundUsers = Mapper.Map<List<DiscordUserModel>>(Business.User.Find(name));
									var foundUser = foundUsers.FirstOrDefault();
									if (foundUser != null)
									{
										foundUser.Ignored = false;
										var businessUser = Mapper.Map<BusinessDiscordUserModel>(foundUser);
										Business.User.Update(businessUser);
										await e.Channel.SendMessage(e.User.Mention + @" I will also be listening to " + name + " from now on.");
										return;
									}

									await
										e.Channel.SendMessage(e.User.Mention + @" I cannot find a user with the name: " + name + " in my memory.");
									return;
								}
							}
							await e.Channel.SendMessage(e.User.Mention + @" Please tag the user you would like to ignore like this *!ignore add @{user}.");
							return;
						}
					}

					//if (message.Contains("!role"))
					//{
					//	var messages = message.Split(' ');
					//	var userName = string.Empty;
					//	var userRole = string.Empty;

					//	foreach (var msg in messages.Where(msg => msg.Contains("@")))
					//	{
					//		userName = msg.Split('@')[1];
					//	}

					//	foreach (var msg in messages.Where(msg => msg.Contains("#")))
					//	{
					//		userRole = msg.Split('#')[1];
					//	}

					//	if (userRole != "admin" && userRole != "organizer" && userRole != "knownuser")
					//	{
					//		await e.Channel.SendMessage(e.User.Mention + @" Please use a proper role for example *Admin, Organizer or KnownUser*");
					//		return;
					//	}

					//	var foundUsers = Mapper.Map<List<DiscordUserModel>>(Business.User.Find(userName));
					//	var foundUser = foundUsers.FirstOrDefault();
					//	if (foundUser != null)
					//	{
					//		var businessUser = Mapper.Map<BusinessDiscordUserModel>(foundUser);
					//		Business.User.UpdateRoleFromName(businessUser, userRole);
					//		await e.Channel.SendMessage(e.User.Mention + @" I have updated the role of name: " + userName + " to role: " + userRole + ".");
					//		return;
					//	}

					//	await e.Channel.SendMessage(e.User.Mention + @" Please tag the user you would like to edit like this *!role @{user} #{role}.");
					//	return;
					//}
				}
				#endregion
			}



		}
		
	}
}
