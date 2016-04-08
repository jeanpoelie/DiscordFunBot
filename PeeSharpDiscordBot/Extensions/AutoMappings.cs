namespace PeeSharpDiscordBot.Extensions
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
								cfg.CreateMap<JokeModel, BusinessJokeModel>();
								cfg.CreateMap<BusinessJokeModel, JokeModel>();
								cfg.CreateMap<ValueModel, BusinessValueModel>();
								cfg.CreateMap<BusinessValueModel, ValueModel>();
							});
		}
	}
}