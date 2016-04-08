namespace WebApp.Models
{
	using System;

	using Business.Models;

	public class DiscordUserModel
	{
		public long Id { get; set; }

		public string Name { get; set; }

		public DateTime? AddDate { get; set; }

		public DateTime? UpdateDate { get; set; }

		public DateTime? Birthdate { get; set; }

		public bool Ignored { get; set; }

		public bool ForceAllBirthdayNotifications { get; set; }

		public string Token { get; set; }

		public int RoleId { get; set; }

		public string RoleDescription { get; set; }
	}
}