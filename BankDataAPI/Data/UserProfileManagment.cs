using System.Data.SQLite;
using BusinessLayerAPI.Models.Response;

namespace BankDataAPI.Data
{
    public class UserProfileManagment
    {
        private static string connectionString = "Data Source=mydatabase.db;Version=3;";

        public static UserDetailsResponse? DataGetUserByHandle(string handle)
        {
            if (string.IsNullOrWhiteSpace(handle))
                return null;

            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT *  
                FROM UserTable
                WHERE Handle = @handle
                LIMIT 1;";
            command.Parameters.AddWithValue("@handle", handle);

            using var reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            var userInformation = new UserDetailsResponse
            {
                UserID = Convert.ToInt32(reader["UserID"]),
                Handle = Convert.ToString(reader["Handle"]) ?? "",
                FirstName = Convert.ToString(reader["FirstName"]) ?? "",
                LastName = Convert.ToString(reader["LastName"]) ?? "",
                Email = Convert.ToString(reader["Email"]) ?? "",
                Password = Convert.ToString(reader["Password"]) ?? "",
                Address = Convert.ToString(reader["Address"]) ?? "",
                Phone = Convert.ToString(reader["Phone"]) ?? "",
                PicturePath = Convert.ToString(reader["PicturePath"]) ?? "",
                Admin = Convert.ToBoolean(reader["Admin"])
            };

            Console.WriteLine(userInformation.FirstName + userInformation.UserID.ToString());
            return userInformation;
        }
    }
}
