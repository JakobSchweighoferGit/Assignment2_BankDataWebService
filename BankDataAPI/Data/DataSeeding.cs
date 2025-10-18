using BusinessLayerAPI.Data;
using RequestsResponses;
using System.Data.SQLite;

namespace BankDataAPI.Data
{
    public class DataSeeding
    {
        private static string GetDbPath()
        {
            const string dbFileName = "mydatabase.db";
        
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbFileName);
        }

        private static string connectionString = $"Data Source={GetDbPath()};Version=3;";



        //define the directory to save txt lists of users and accounts
        private static readonly string ReportDirectory = "SeedReports";

        private static readonly string UserFilePath = Path.Combine(ReportDirectory, "UserList.txt");
        private static readonly string AccountFilePath = Path.Combine(ReportDirectory, "AccountList.txt");
        private static readonly string AdminFilePath = Path.Combine(ReportDirectory, "AdminList.txt");


        public static Random rand = Random.Shared;

        //struct which makes it convenient to get a users userid and accountid
        private struct AccountLink
        {
            public int AccountId { get; set; }
            public int UserId { get; set; }
        }

        //used to make sure the directory is always created
        private static void EnsureReportDirectoryExists()
        {
            if (!Directory.Exists(ReportDirectory))
            {
                Directory.CreateDirectory(ReportDirectory);
            }
        }

        //retrieve the list of userids using sql statement
        private static List<int> GetUserIds(SQLiteConnection connection)
        {
            var userIds = new List<int>();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT UserID FROM UserTable ORDER BY UserID;";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userIds.Add(reader.GetInt32(0));
                    }
                }
            }
            return userIds;
        }

        //get all users userids by accountid
        private static List<AccountLink> GetAccountLinks(SQLiteConnection connection)
        {
            var accountLinks = new List<AccountLink>();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT AccountID, UserID FROM AccountTable ORDER BY AccountID;";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        accountLinks.Add(new AccountLink
                        {
                            AccountId = reader.GetInt32(0),
                            UserId = reader.GetInt32(1)
                        });
                    }
                }
            }
            return accountLinks;
        }

        //helper func to execture commands
        private static int ExecuteScalarCommand(SQLiteConnection connection, string sql)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                var result = command.ExecuteScalar();
                return result != DBNull.Value && result != null ? Convert.ToInt32(result) : 0;
            }
        }

        //write to the account / user profile files to log user details
        public static void WriteToFile(string filePath, string content)
        {
            try
            {
                string directoryPath = Path.GetDirectoryName(filePath);

                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine(content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file '{filePath}': {ex.Message}");
            }
        }

        //helper func to ensure table has been emptied properly
        private static bool CheckIfTableIsEmpty(string tableName)
        {


            try
            {
                using var connection = new SQLiteConnection(connectionString);
                connection.Open();
                using var command = connection.CreateCommand();

                command.CommandText = $"SELECT COUNT(*) FROM {tableName};";


                object result = command.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    return true;
                }

                long count = Convert.ToInt64(result);

                return count == 0;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"[WARNING] SQLite Error checking table {tableName}: {ex.Message}. Assuming empty.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] General Error checking table {tableName}: {ex.Message}. Assuming empty.");
                return true;
            }
        }

        //seed 100 mock usrs randomly, uses the randomgen class to get values
        public static void SeedTestUsers()
        {
            try
            {
                using var connection = new SQLiteConnection(connectionString);
                connection.Open();

                if (!CheckIfTableIsEmpty("UserTable"))
                {
                    Console.WriteLine("UserTable already contains data. Skipping user seed.");
                    return;
                }

                EnsureReportDirectoryExists();
                var userReportLines = new List<string>();


                const string insertSql = @"
            INSERT INTO UserTable
                (Handle, FirstName, LastName, Email, Password, Address, Phone, PicturePath, Admin)
            VALUES
                (@Handle, @FirstName, @LastName, @Email, @Password, @Address, @Phone, @PicturePath, @Admin);
        ";
                int totalInserted = 0;


                //use paramaterization to protect against sql injectiom
                using (var insertCommand = new SQLiteCommand(insertSql, connection))
                {
                    insertCommand.Parameters.Add("@Handle", System.Data.DbType.String);
                    insertCommand.Parameters.Add("@FirstName", System.Data.DbType.String);
                    insertCommand.Parameters.Add("@LastName", System.Data.DbType.String);
                    insertCommand.Parameters.Add("@Email", System.Data.DbType.String);
                    insertCommand.Parameters.Add("@Password", System.Data.DbType.String);
                    insertCommand.Parameters.Add("@Address", System.Data.DbType.String);
                    insertCommand.Parameters.Add("@Phone", System.Data.DbType.String);
                    insertCommand.Parameters.Add("@PicturePath", System.Data.DbType.String);
                    insertCommand.Parameters.Add("@Admin", System.Data.DbType.Boolean);

                    for (int i = 0; i < 100; i++)
                    {
                       
                        CreateUserRequest user = RandomGen.GenRandomUser();

                        insertCommand.Parameters["@Handle"].Value = user.Handle + rand.Next(0, 10000);
                        insertCommand.Parameters["@FirstName"].Value = user.FirstName;
                        insertCommand.Parameters["@LastName"].Value = user.LastName;
                        insertCommand.Parameters["@Email"].Value = user.Email;
                        insertCommand.Parameters["@Password"].Value = user.Password;
                        insertCommand.Parameters["@Address"].Value = user.Address ?? (object)DBNull.Value;
                        insertCommand.Parameters["@Phone"].Value = user.Phone ?? (object)DBNull.Value;
                        insertCommand.Parameters["@PicturePath"].Value = user.PicturePath ?? (object)DBNull.Value;
                        insertCommand.Parameters["@Admin"].Value = user.Admin;

                        String contents = $"Handle: {user.Handle}, First Name: {user.FirstName}, Last Name: {user.LastName}, " +
                                          $"Email: {user.Email}, Password: {user.Password}, Address: {user.Address}, Phone: {user.Phone}, PicturePath: {user.PicturePath}";

                        userReportLines.Add(contents);

                        totalInserted += insertCommand.ExecuteNonQuery();
                    }
                }
                File.WriteAllLines(UserFilePath, userReportLines);

                Console.WriteLine($"{totalInserted} Test Users Inserted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error seeding test data: " + ex.Message);
            }
        }

        //seed 100 bank accounts, one for every user
        public static void seedAccounts()
        {
            try
            {
                using var connection = new SQLiteConnection(connectionString);
                connection.Open();

                if (!CheckIfTableIsEmpty("AccountTable"))
                {
                    Console.WriteLine("AccountTable already contains data. Skipping account seed.");
                    return;
                }

                List<int> userIds = GetUserIds(connection);

                if (userIds.Count == 0)
                {
                    Console.WriteLine("Cannot seed accounts: UserTable is empty. Run SeedTestUsers first.");
                    return;
                }

                EnsureReportDirectoryExists();
                var acctReportLines= new List<string>();

                const string insertSql = @"
                INSERT INTO AccountTable 
                        (AccountNumber, Balance, UserID)
                        VALUES
                    (@AccountNumber, @Balance, @UserID);
                ";
                int totalInserted = 0;


                using (var insertCommand = new SQLiteCommand(insertSql, connection))
                {
                    insertCommand.Parameters.Add("@AccountNumber", System.Data.DbType.String);
                    insertCommand.Parameters.Add("@Balance", System.Data.DbType.Int32);
                    insertCommand.Parameters.Add("@UserID", System.Data.DbType.Int32);

                    foreach (int userId in userIds)
                    {
                        int acctNumber = RandomGen.GenNextAcctNumber();
                        int balance = RandomGen.GenRandomBalance();

                        insertCommand.Parameters["@AccountNumber"].Value = acctNumber;
                        insertCommand.Parameters["@Balance"].Value = balance;
                        insertCommand.Parameters["@UserID"].Value = userId;
                        totalInserted += insertCommand.ExecuteNonQuery();

                        String contents = $"Account Number: {acctNumber}, Balance: {balance}$, UserID: {userId}";

                        acctReportLines.Add(contents);
                    }
                }

                File.WriteAllLines(AccountFilePath, acctReportLines);

                Console.WriteLine($" {totalInserted} Accounts Inserted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error seeding accounts: " + ex.Message);
            }
        }

        //seed some mock transactions too
        public static void seedTransactions()
        {
            try
            {
                using var connection = new SQLiteConnection(connectionString);
                connection.Open();

                if (!CheckIfTableIsEmpty("TransactionTable"))
                {
                    Console.WriteLine("TransactionTable already contains data. Skipping transaction seed.");
                    return;
                }

                if (ExecuteScalarCommand(connection, "SELECT COUNT(*) FROM AccountTable;") == 0)
                {
                    Console.WriteLine("Cannot seed transactions: AccountTable is empty. Run seedAccounts first.");
                    return;
                }

                List<AccountLink> accountLinks = GetAccountLinks(connection);

                const string insertSql = @"
                INSERT INTO TransactionTable 
                        (Amount, Type, Success, AccountID, UserID)
                        VALUES
                    (@Amount, @Type, 1, @AccountID, @UserID);
                ";
                int totalInserted = 0;



                using (var insertCommand = new SQLiteCommand(insertSql, connection))
                {
                    insertCommand.Parameters.Add("@Amount", System.Data.DbType.Int32);
                    insertCommand.Parameters.Add("@Type", System.Data.DbType.String);
                    insertCommand.Parameters.Add("@AccountID", System.Data.DbType.Int32);
                    insertCommand.Parameters.Add("@UserID", System.Data.DbType.Int32);

                    foreach (var link in accountLinks)
                    {

                        int amount = RandomGen.GenRandomBalance();
                        string type = amount >= 0 ? "Deposit" : "Withdrawal";

                        insertCommand.Parameters["@Amount"].Value = amount;
                        insertCommand.Parameters["@Type"].Value = type;
                        insertCommand.Parameters["@AccountID"].Value = link.AccountId;
                        insertCommand.Parameters["@UserID"].Value = link.UserId;

                        totalInserted += insertCommand.ExecuteNonQuery();


                    }
                }
                Console.WriteLine($"{totalInserted} Transactions Inserted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error seeding transactions: " + ex.Message);
            }
        }

        //seed a single admin user for testing purposes
        public static void seedAdmin()
        {
            try
            {
                using var connection = new SQLiteConnection(connectionString);
                connection.Open();

                if (ExecuteScalarCommand(connection, "SELECT COUNT(*) FROM UserTable WHERE Admin = 1;") > 0)
                {
                    Console.WriteLine("Admin user already exists. Skipping admin seed.");
                    return;
                }

                EnsureReportDirectoryExists();
                var adminReportLines = new List<string>();


                var adminUser = new
                {
                    Handle = "SuperAdmin",
                    FirstName = "System",
                    LastName = "Administrator",
                    Email = "admin@bankapi.com",
                    Password = "AdminPassword123",
                    Address = "123 Main St",
                    Phone = "555-1212",
                    PicturePath = (string)null,
                    Admin = true
                };

                const string insertSql = @"
                INSERT INTO UserTable
                    (Handle, FirstName, LastName, Email, Password, Address, Phone, PicturePath, Admin)
                VALUES
                    (@Handle, @FirstName, @LastName, @Email, @Password, @Address, @Phone, @PicturePath, @Admin);
                ";
                int totalInserted = 0;

                using (var insertCommand = new SQLiteCommand(insertSql, connection))
                {
                    insertCommand.Parameters.Add("@Handle", System.Data.DbType.String).Value = adminUser.Handle;
                    insertCommand.Parameters.Add("@FirstName", System.Data.DbType.String).Value = adminUser.FirstName;
                    insertCommand.Parameters.Add("@LastName", System.Data.DbType.String).Value = adminUser.LastName;
                    insertCommand.Parameters.Add("@Email", System.Data.DbType.String).Value = adminUser.Email;
                    insertCommand.Parameters.Add("@Password", System.Data.DbType.String).Value = adminUser.Password;

                    insertCommand.Parameters.Add("@Address", System.Data.DbType.String).Value = adminUser.Address ?? (object)DBNull.Value;
                    insertCommand.Parameters.Add("@Phone", System.Data.DbType.String).Value = adminUser.Phone ?? (object)DBNull.Value;
                    insertCommand.Parameters.Add("@PicturePath", System.Data.DbType.String).Value = adminUser.PicturePath ?? (object)DBNull.Value;

                    insertCommand.Parameters.Add("@Admin", System.Data.DbType.Boolean).Value = true;

                    String contents = $"Handle: {adminUser.Handle}, First Name: {adminUser.FirstName}, Last Name: {adminUser.LastName}, " +
                                           $"Email: {adminUser.Email}, Password: {adminUser.Password}, Address: {adminUser.Address}, Phone: {adminUser.Phone}, PicturePath: {adminUser.PicturePath}";

                    adminReportLines.Add(contents);
                    totalInserted = insertCommand.ExecuteNonQuery();



                }
                File.WriteAllLines(AdminFilePath, adminReportLines);
                Console.WriteLine($"{totalInserted} Admin User (Handle: {adminUser.Handle}) Inserted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error seeding admin data: " + ex.Message);
            }
        }

        //empty the entire database, should be called everytime a new demo is occuring
        public static void EmptyDatabase()
        {
            Console.WriteLine("--- Starting Database Emptying Process ---");

            try
            {
                string fullDbPath = GetDbPath();
                string dbFileName = Path.GetFileName(fullDbPath);

                Console.WriteLine($"Target database file: {fullDbPath}");

                if (File.Exists(fullDbPath))
                {
                    SQLiteConnection.ClearAllPools();

                    File.Delete(fullDbPath);
                    Console.WriteLine($" Database file '{dbFileName}' successfully deleted.");
                    Console.WriteLine("The database schema will be recreated on the next application startup.");
                }
                else
                {
                    Console.WriteLine($"[INFO] Database file '{dbFileName}' not found. Nothing to delete.");
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"[ERROR] Failed to delete database file: {ex.Message}");
                Console.WriteLine("HINT: This usually means another process (often the running application itself) has an open lock on the file. Ensure the application is fully stopped before calling this function.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FATAL ERROR] An unexpected error occurred during database deletion: {ex.Message}");
            }
        }
    }
}