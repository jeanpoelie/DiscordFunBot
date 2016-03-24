namespace Business
{
	using System;
	using System.Configuration;
	using System.Data;

	using Models;

	using MySql.Data.MySqlClient;

	public class DatabaseCommunication
	{
		private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

		private static readonly Database Database = new Database(ConnectionString);

		public static void AddUser(BusinessDiscordUserModel user)
		{
			const string Query = @"INSERT INTO `Users` (`Id`, `Name`, `AddDate`, `BirthDate`, `Token`)  VALUES (@Id, @Name, @AddDate, @BirthDate, @Token)";
			
			Database.ExecuteQuery(
				Query,
				new MySqlParameter("@Id", user.Id),
				new MySqlParameter("@Name", user.Name),
				new MySqlParameter("@AddDate", DateTime.UtcNow),
				new MySqlParameter("@BirthDate", user.Birthdate),
				new MySqlParameter("@Token", Guid.NewGuid()));
		}

		public static void AddUserSuggestion(string suggestion, BusinessDiscordUserModel user)
		{
			const string Query = @"INSERT INTO `Suggestions` (`UserId`, `Description`)  VALUES (@UserId, @Suggestion)";

			Database.ExecuteQuery(
				Query,
				new MySqlParameter("@UserId", user.Id),
				new MySqlParameter("@Suggestion", suggestion));
		}

		public static DataTable FindUser(string name)
		{
			const string Query = @"SELECT `Users`.Id, Name, AddDate, UpdateDate, BirthDate, Ignored, Token, `Users`.`Role` as RoleId,`Roles`.`Description` as RoleDescription FROM `Users` INNER JOIN `Roles` ON `Users`.`Role`= `Roles`.`Id` WHERE `Name` = @Name";

			var dt = Database.ExecuteQuery(Query, new MySqlParameter("@Name", name)).Tables[0];

			return dt;
		}

		public static DataTable AuthorizeUser(string token, string uid)
		{
			const string Query = @"SELECT `Users`.Id, Name, AddDate, UpdateDate, BirthDate, Ignored, Token, `Users`.`Role` as RoleId,`Roles`.`Description` as RoleDescription FROM `Users` INNER JOIN `Roles` ON `Users`.`Role`= `Roles`.`Id` WHERE `Token` = @Token AND `Users`.`Id` = @Id";

			var dt = Database.ExecuteQuery(Query, new MySqlParameter("@Token", token), new MySqlParameter("@Id", uid)).Tables[0];

			return dt;
		}

		public static DataTable GetUser(long id)
		{
			const string Query = @"SELECT `Users`.Id, Name, AddDate, UpdateDate, BirthDate, Ignored, Token, `Users`.`Role` as RoleId,`Roles`.`Description` as RoleDescription FROM `Users` INNER JOIN `Roles` ON `Users`.`Role`= `Roles`.`Id` WHERE `Users`.`Id` = @Id";

			var dt = Database.ExecuteQuery(Query, new MySqlParameter("@Id", id)).Tables[0];

			return dt;
		}

        public static DataTable GetAllUsers()
		{
			const string Query = @"SELECT `Users`.`Id`, Name, AddDate, UpdateDate, BirthDate, Ignored, Token, `Users`.`Role` as RoleId,`Roles`.`Description` as RoleDescription FROM `Users` INNER JOIN `Roles` ON `Users`.`Role`= `Roles`.`Id`";

			var dt = Database.ExecuteQuery(Query).Tables[0];

			return dt;
		}

		public static DataTable GetAllRoles()
		{
			const string Query = @"SELECT `Id`,`Description` FROM `Roles`";

			var dt = Database.ExecuteQuery(Query).Tables[0];

			return dt;
		}

		public static DataTable GetRoleName(long id)
		{
			const string Query = @"SELECT `Description` FROM `Roles` WHERE `Id` = @Id";

			var dt = Database.ExecuteQuery(Query, new MySqlParameter("@Id", id)).Tables[0];

			return dt;
		}

		public static DataTable GetRoleId(string roleName)
		{
			const string Query = @"SELECT `Id` FROM `Roles` WHERE `Description` LIKE '%@Name%' LIMIT 0, 1";

			var dt = Database.ExecuteQuery(Query, new MySqlParameter("@Name", roleName)).Tables[0];

			return dt;
		}

		public static void UpdateUser(BusinessDiscordUserModel user)
		{
			const string Query = @"UPDATE `Users` SET `Name`=@Name, `UpdateDate`=@UpdateDate, `BirthDate`=@BirthDate, `Ignored`=@Ignored WHERE `Id`=@Id";

			Database.ExecuteQuery(
					Query,
					new MySqlParameter("@Id", user.Id),
					new MySqlParameter("@Name", user.Name),
					new MySqlParameter("@UpdateDate", DateTime.UtcNow),
					new MySqlParameter("@BirthDate", user.Birthdate),
					new MySqlParameter("@Ignored", user.Ignored));
		}

		public static void UpdateRole(BusinessDiscordUserModel user, int role)
		{
			const string Query = @"UPDATE `Users` SET`UpdateDate`=@UpdateDate, `Role`=@Role WHERE `Id`=@Id";

			Database.ExecuteQuery(
					Query,
					new MySqlParameter("@Id", user.Id),
					new MySqlParameter("@UpdateDate", DateTime.UtcNow),
					new MySqlParameter("@Role", role));
		}
	}
}