using System;
using System.Threading.Tasks;

namespace PeeSharpDiscordBot
{
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Threading;

	using Extensions;

	public class BotBase
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Started program.");

			AutoMappings.RegisterMappings();

			var communityBot = new DiscordCommunityBot();

			var funBot = new DiscordBotFun();

			while (true)
			{
				Thread.Sleep(1);
			}
		}

		public async Task<string> GetJSONDataViaAJAX(string url, string navigation)
		{
			var guid = new Guid();
			Console.WriteLine("GET request to URL: " + url + navigation + " (" + guid + ")");
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(url);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				// New code:
				HttpResponseMessage response = await client.GetAsync(navigation);
				if (response.IsSuccessStatusCode)
				{
					var answer = await response.Content.ReadAsStringAsync();
					Console.WriteLine("GET request (" + guid + ") Response: " + answer);
					return answer;
				}
			}

			return string.Empty;
		}

		public int DaysUntillDate(DateTime startDate, DateTime endDate)
		{
			if (startDate < endDate)
			{
				startDate = startDate.AddYears(1);
			}

			return (startDate - endDate).Days;
		}

		public async void ScheduleAction(Action action, DateTime ExecutionTime)
		{
			if (DateTime.Now > ExecutionTime)
			{
				ExecutionTime = ExecutionTime.AddDays(1);
			}

			await Task.Delay((int)Math.Abs(ExecutionTime.Subtract(DateTime.Now).TotalMilliseconds));
			action();
		}
	}
}
