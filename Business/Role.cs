namespace Business
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Extensions;
	using Models;

	public class Role
	{ 
		public static void Add(BusinessRoleModel role)
		{
			if (role == null)
			{
				throw new NullReferenceException("The user is empty");
			}

			throw new NotImplementedException();
		}

		public static string GetRoleById(long id)
		{
			if (id <= 0)
			{
				throw new NullReferenceException("This user does not exist.");
			}

			return DatabaseCommunication.GetRoleName(id).Rows[0][0].ToString();
		}

		public static long GetRoleByName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new NullReferenceException("This user does not exist.");
			}

			return DatabaseCommunication.GetRoleId(name).ToList<long>().FirstOrDefault();
		}

		public static IList<BusinessRoleModel> GetAll()
		{
			return DatabaseCommunication.GetAllRoles().ToList<BusinessRoleModel>();
		}
	}
}