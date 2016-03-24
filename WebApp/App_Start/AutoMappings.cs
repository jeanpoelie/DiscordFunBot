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
			Mapper.CreateMap<DiscordUserModel, BusinessDiscordUserModel>();
			Mapper.CreateMap<BusinessDiscordUserModel, DiscordUserModel>();

			Mapper.CreateMap<RoleModel, BusinessRoleModel>();
			Mapper.CreateMap<BusinessRoleModel, RoleModel>();
		}
	}
}