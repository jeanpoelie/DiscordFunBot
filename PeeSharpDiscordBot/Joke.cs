namespace PeeSharpDiscordBot
{
	using System.Linq;

	using Newtonsoft.Json.Linq;

	public class Joke
	{
		public string type { get; set; }

		public Value value { get; set; }
	}
}