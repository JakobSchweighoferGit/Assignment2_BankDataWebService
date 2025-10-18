using System.Data.SQLite;
using System.Reflection.Metadata;
using BusinessLayerAPI.Models;
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

        public static List<UserSearchInformation> GetUsersBySearch(string? searchTerm)
        {
            var result = new List<UserSearchInformation>();

            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                    SELECT 
                        u.Handle,
                        u.Email,
                        a.AccountNumber,
                        a.AccountID
                    FROM UserTable u
                    LEFT JOIN AccountTable a ON a.UserID = u.UserID
                    WHERE
                        (@term IS NULL) 
                        OR u.Handle       LIKE '%' || @term || '%'
                        OR u.Email        LIKE '%' || @term || '%'
                        OR a.AccountNumber LIKE '%' || @term || '%'
                    ORDER BY u.Handle;";

            var term = string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm;
            command.Parameters.AddWithValue("@term", (object?)term ?? DBNull.Value);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var accountIdObj = reader["AccountID"];
                int? accountId = accountIdObj == DBNull.Value ? null : Convert.ToInt32(accountIdObj);

                result.Add(new UserSearchInformation
                {
                    Handle = reader["Handle"]?.ToString() ?? "",
                    Email = reader["Email"]?.ToString() ?? "",
                    AccNumber = reader["AccountNumber"]?.ToString() ?? "",
                    AccountID = accountId
                });
            }

            return result;
        }


        public static bool InsertUser(CreateUserRequest req)
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
            INSERT INTO UserTable
                (Handle, FirstName, LastName, Email, Password, Address, Phone, PicturePath, Admin)
            VALUES
                (@Handle, @FirstName, @LastName, @Email, @Password, @Address, @Phone, @PicturePath, @Admin);";

            cmd.Parameters.AddWithValue("@Handle", req.Handle);
            cmd.Parameters.AddWithValue("@FirstName", req.FirstName);
            cmd.Parameters.AddWithValue("@LastName", req.LastName);
            cmd.Parameters.AddWithValue("@Email", req.Email);
            cmd.Parameters.AddWithValue("@Password", req.Password);   
            cmd.Parameters.AddWithValue("@Address", req.Address ?? "");
            cmd.Parameters.AddWithValue("@Phone", req.Phone ?? "");
            cmd.Parameters.AddWithValue("@PicturePath", req.PicturePath ?? "");
            cmd.Parameters.AddWithValue("@Admin", req.Admin ? 1 : 0);

            try
            {
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("InsertUser error: " + ex.Message);
                return false;
            }
        }

        public static bool DeleteUserByHandle(string handle)
        {
            if (string.IsNullOrWhiteSpace(handle))
            {
           
                return false;
            }

            const string sql = @"
                DELETE FROM UserTable
                WHERE Handle = @HandleToDelete;
            ";

            using (var connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@HandleToDelete", handle);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected == 1;
                    }
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine($"SQLite Error deleting user by handle '{handle}': {ex.Message}");
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"General Error deleting user by handle '{handle}': {ex.Message}");
                    return false;
                }
            }
        }

        public static bool DeleteUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            const string sql = @"
                DELETE FROM UserTable
                WHERE Email = @EmailToDelete;
            ";

            using (var connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@EmailToDelete", email);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected == 1;
                    }
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine($"SQLite Error deleting user by email '{email}': {ex.Message}");
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"General Error deleting user by email '{email}': {ex.Message}");
                    return false;
                }
            }
        }
    }
}
