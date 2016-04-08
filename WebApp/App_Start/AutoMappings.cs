namespace WebApp
{
	using AutoMapper;

	using Business.Models;

	using Models;

	/// <summary>
	/// The auto mappings.
	/// </summary>
	public class AutoMappings
	{
		public static void RegisterMappings()
		{
			Mapper.Initialize(
				cfg =>
					{
						cfg.CreateMap<BusinessRoleModel, RoleModel>();
						cfg.CreateMap<RoleModel, BusinessRoleModel>();
						cfg.CreateMap<DiscordUserModel, BusinessDiscordUserModel>();
						cfg.CreateMap<BusinessDiscordUserModel, DiscordUserModel>();
						cfg.CreateMap<UserSubscriptionModel, BusinessUserSubscriptionModel>();
						cfg.CreateMap<BusinessUserSubscriptionModel, UserSubscriptionModel>();
						cfg.CreateMap<SuggestionModel, BusinessSuggestionModel>();
						cfg.CreateMap<BusinessSuggestionModel, SuggestionModel>();
					});;
		}
	}
}