namespace PeeSharpDiscordBot
{
	using System.Data;
	using System.Data.SqlClient;
	using System.Configuration;

	public class DatabaseCommunication
	{
		private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

		private static readonly Database Database = new Database(ConnectionString);

		public static DataTable GetAllUsers()
		{
			const string Query = @"SELECT * FROM [Users]";

			var dt = Database.ExecuteQuery(Query).Tables[0];

			return dt;
		}
	}
}