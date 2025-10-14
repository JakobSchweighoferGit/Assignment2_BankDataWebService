using System.Data.SQLite;
using System.Reflection.Metadata;
using BusinessLayerAPI.Models.Request;
using BusinessLayerAPI.Models.Response;

namespace BankDataAPI.Data
{
    public class UserProfileManagement
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

        public static bool EditUserInformation(EditUserRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Handle))
                return false;

            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE UserTable
                SET 
                    FirstName   = COALESCE(NULLIF(@FirstName,   ''), FirstName),
                    LastName    = COALESCE(NULLIF(@LastName,    ''), LastName),
                    Email       = COALESCE(NULLIF(@Email,       ''), Email),
                    Phone       = COALESCE(NULLIF(@Phone,       ''), Phone),
                    Address     = COALESCE(NULLIF(@Address,     ''), Address),
                    PicturePath = COALESCE(NULLIF(@PicturePath, ''), PicturePath),
                    Password    = COALESCE(NULLIF(@Password,     ''), Password)
                WHERE Handle = @Handle;";

            command.Parameters.AddWithValue("@FirstName", req.FirstName ?? "");
            command.Parameters.AddWithValue("@LastName", req.LastName ?? "");
            command.Parameters.AddWithValue("@Email", req.Email ?? "");
            command.Parameters.AddWithValue("@Phone", req.Phone ?? "");
            command.Parameters.AddWithValue("@Address", req.Address ?? "");
            command.Parameters.AddWithValue("@PicturePath", req.PicturePath ?? "");
            command.Parameters.AddWithValue("@Password", req.Password ?? "");
            command.Parameters.AddWithValue("@Handle", req.Handle);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}
