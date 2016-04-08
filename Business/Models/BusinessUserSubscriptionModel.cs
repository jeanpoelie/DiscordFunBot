namespace Business.Models
{
	public class BusinessUserSubscriptionModel
	{
		public long UserId { get; set; }

		public string UserName { get; set; }

		public long SubscriptionUserId { get; set; }

		public string SubscriptionUserName { get; set; }
	}
}