namespace WebApp.Models
{
	public class RoleModel
	{
		public int Id { get; set; }

		public string Description { get; set; }
	}

	public enum Role {  Admin = 1, Orgainser = 2, User = 3 }
}