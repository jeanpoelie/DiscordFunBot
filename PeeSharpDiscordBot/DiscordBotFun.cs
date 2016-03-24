namespace PeeSharpDiscordBot
{
	using System;

	using Discord;

	using Newtonsoft.Json.Linq;

	/*
		TODO: 
		- Jokes with subjects
		- React with songs thru voice when someone says a certain phrase.
		- 
	*/

	/// <summary>
	/// My first bot just for shits and giggles.
	/// </summary>
	public class DiscordBotFun : BotBase
	{

		/// <summary>
		/// The bot itself
		/// </summary>
		private readonly DiscordClient bot;

		/// <summary>
		/// Initializes a new instance of the <see cref="DiscordBotFun"/> class. 
		/// The constructor
		/// </summary>
		public DiscordBotFun()
		{
			Console.WriteLine("Started fun bot.");

			bot = new DiscordClient();

			bot.MessageReceived += Bot_MessageReceived;

			bot.Connect("botjeepee@gmail.com", "ditisgeheim");

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

			if (e.Message.Text.ToLower().Contains("@retard"))
			{
				e.Channel.SendMessage(e.User.Mention + " Greetings!");
			}

			// Check if the bot is mentioned.
			if (!e.Message.Text.Contains(this.bot.CurrentUser.Name) || !e.Message.Text.Contains("@" + this.bot.CurrentUser.Name)) return;

				var message = e.Message.Text.ToLower();
			message = message.Replace("@" + this.bot.CurrentUser.Name.ToLower() + " ", "");
			message = message.Replace(this.bot.CurrentUser.Name.ToLower() + " ", "");
			Console.WriteLine(e.User.Name + " said: " + message);

			if (message.Contains("hi!") || message.Contains("hi") || message.Contains("hello") || message.Contains("bonjour"))
			{
				e.Channel.SendMessage(e.User.Mention + " Greetings!");
			}

			if (message.Contains("how are you"))
			{
				e.Channel.SendMessage(
					e.User.Mention+ @" I am great, thank you, how are you?");
			}

			if (message.Contains("!help"))
			{
				e.Channel.SendMessage(
					e.User.Mention
					+ @" **!commands** *For all the commands*");
			}

			if (message.Contains("!commands"))
			{
				e.Channel.SendMessage(
					e.User.Mention
					+ @" 
					- **command:** *!joke* **Answer:** *A funny joke* __***Will be adding options for subjects later***___
					");
			}

			if (message.Contains("tell me a joke") || message.Contains("random joke") || message.Contains("!joke"))
			{
				var json = await this.GetJSONDataViaAJAX("http://api.icndb.com", "jokes/random");
				dynamic data = JObject.Parse(json);
				e.Channel.SendMessage(e.User.Mention + ": " + data.value.joke);
			}
		}
		
	}
}
