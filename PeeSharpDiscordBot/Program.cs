using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace PeeSharpDiscordBot
{
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Threading;

	using Newtonsoft.Json;

	public class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Started program.");

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
			Console.WriteLine("GET request to URL: " + url + navigation + " ("+ guid+")");
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


	}
}
