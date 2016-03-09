using Discord;

namespace PeeSharpDiscordBot
{
	using System;

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
	public class DiscordCommunityBot : Program
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
			if (e.Message.IsAuthor) return;

			if (!e.Message.Text.Contains(this.bot.CurrentUser.Name) || !e.Message.Text.Contains("@" + this.bot.CurrentUser.Name)) return;

			var message = e.Message.Text.ToLower();
			message = message.Replace("@" + this.bot.CurrentUser.Name.ToLower() + " ", "");
			message = message.Replace(this.bot.CurrentUser.Name.ToLower() + " ", "");
			Console.WriteLine(e.User.Name + " said: " + message);

			//var dt = DatabaseCommunication.GetAllUsers();


			#region [Everyone]
			if (message.Contains("hi!") || message.Contains("hi") || message.Contains("hello") || message.Contains("bonjour"))
			{
				e.Channel.SendMessage(e.User.Mention + " Greetings!");
				return;
			}

			if (message.Contains("!commands"))
			{
				e.Channel.SendMessage(
					e.User.Mention
					+ @" 
  **Community Commands**
- *!birthdaylist* **Answer:** *Get a list of all known birthdays* ***___Not Implemented Yet__***
- *!birthday* **Answer:** *Use Add/Remove to add or remove a birthday* **(Example: !birthday add @user dd/mm/yyy)** ***___Not Implemented Yet__***
- *!ignorelist* **Answer:** *Get a list of all the people that I ignore* ***___Not Implemented Yet__***
- *!commandwish* **Answer:** *Add a wish for commands* **(Example: !commandwish random quiz question)** ***___Not Implemented Yet__***

  **Admin Commands**
- *!ignore* **Answer:** *Add/Remove people from the ignore list* **(Example: !ignore add @user)** ***___Not Implemented Yet__***");
				return;
			}

			if (message.Contains("!ignorelist"))
			{
				// TODO : Get the list of ignored peoples.
				e.Channel.SendMessage(e.User.Mention + " Not implemented yet!");
				return;
			}

			if (message.Contains("!birthdaylist"))
			{
				// TODO : Get the list of ignored peoples.
				e.Channel.SendMessage(
					e.User.Mention 
					+ @" 
**List: **
- @Dakpan | JP 14/02/1994");
				return;
			}
			#endregion

			#region [KnownUsers]
			if (message.Contains("!birthday"))
			{
				// TODO : Get the list of ignored peoples.
				e.Channel.SendMessage(e.User.Mention + " Not implemented yet!");
				return;
			}

			if (message.Contains("!commandwish"))
			{
				// TODO : Get the list of ignored peoples.
				e.Channel.SendMessage(e.User.Mention + " Not implemented yet!");
				return;
			}
			#endregion

			#region [Organizer]
			// TODO:  IMPELEMENT
			#endregion

			#region [Admins]
			// TODO : Create list of admins that can do this. (Multiple roles might be nice)
			if (message.Contains("!ignore"))
			{
				if (e.User.Id == 132556381046833152)
				{
					if (message.Contains("add"))
					{
						// TODO : Add the user to the ignore list.
						return;
					}

					if (message.Contains("remove") || message.Contains("remove"))
					{
						// TODO : remove the user from the ignore list
						return;
					}
				}
				return;
			}
			#endregion

		}
		
	}
}
