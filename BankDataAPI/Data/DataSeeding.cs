using System.Data.SQLite;
using System;
using System.Collections.Generic;
using BusinessLayerAPI.Data;
using BusinessLayerAPI.Models.Request;

namespace BankDataAPI.Data
{
    public class DataSeeding
    {
        private static string connectionString = "Data Source=mydatabase.db;Version=3;";

        private struct AccountLink
        {
            public int AccountId { get; set; }
            public int UserId { get; set; }
        }

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

        private static int ExecuteScalarCommand(SQLiteConnection connection, string sql)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                var result = command.ExecuteScalar();
                return result != DBNull.Value && result != null ? Convert.ToInt32(result) : 0;
            }
        }

        public static void SeedTestUsers()
        {
            try
            {
                using var connection = new SQLiteConnection(connectionString);
                connection.Open();

                if (ExecuteScalarCommand(connection, "SELECT COUNT(*) FROM UserTable;") > 0)
                {
                    Console.WriteLine("UserTable already contains data. Skipping user seed.");
                    return;
                }

                const string insertSql = @"
            INSERT INTO UserTable
                (Handle, FirstName, LastName, Email, Password, Address, Phone, PicturePath, Admin)
            VALUES
                (@Handle, @FirstName, @LastName, @Email, @Password, @Address, @Phone, @PicturePath, @Admin);
        ";
                int totalInserted = 0;

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

                        insertCommand.Parameters["@Handle"].Value = user.Handle;
                        insertCommand.Parameters["@FirstName"].Value = user.FirstName;
                        insertCommand.Parameters["@LastName"].Value = user.LastName;
                        insertCommand.Parameters["@Email"].Value = user.Email;
                        insertCommand.Parameters["@Password"].Value = user.Password;
                        insertCommand.Parameters["@Address"].Value = user.Address ?? (object)DBNull.Value;
                        insertCommand.Parameters["@Phone"].Value = user.Phone ?? (object)DBNull.Value;
                        insertCommand.Parameters["@PicturePath"].Value = user.PicturePath ?? (object)DBNull.Value;
                        insertCommand.Parameters["@Admin"].Value = user.Admin;

                        totalInserted += insertCommand.ExecuteNonQuery();
                    }
                }
                Console.WriteLine($"{totalInserted} Test Users Inserted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error seeding test data: " + ex.Message);
            }
        }

        public static void seedAccounts()
        {
            try
            {
                using var connection = new SQLiteConnection(connectionString);
                connection.Open();

                if (ExecuteScalarCommand(connection, "SELECT COUNT(*) FROM AccountTable;") > 0)
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
                        insertCommand.Parameters["@AccountNumber"].Value = RandomGen.GenNextAcctNumber();
                        insertCommand.Parameters["@Balance"].Value = RandomGen.GenRandomBalance();
                        insertCommand.Parameters["@UserID"].Value = userId;
                        totalInserted += insertCommand.ExecuteNonQuery();
                    }
                }
                Console.WriteLine($"✅ {totalInserted} Accounts Inserted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error seeding accounts: " + ex.Message);
            }
        }

        public static void seedTransactions()
        {
            try
            {
                using var connection = new SQLiteConnection(connectionString);
                connection.Open();

                if (ExecuteScalarCommand(connection, "SELECT COUNT(*) FROM TransactionTable;") > 0)
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

        public static void seedAdmin()
        {
            try
            {
                using var connection = new SQLiteConnection(connectionString);
                connection.Open();

                const string insertSql = @"
                INSERT INTO UserTable
                    (Handle, FirstName, LastName, Email, Password, Address, Phone, PicturePath, Admin)
                VALUES
                    (@Handle, @FirstName, @LastName, @Email, @Password, @Address, @Phone, @PicturePath, @Admin);
                ";
                int totalInserted = 0;

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

                 
                    CreateUserRequest user = RandomGen.GenRandomUser();

                    insertCommand.Parameters["@Handle"].Value = user + "Admin";
                    insertCommand.Parameters["@FirstName"].Value = user.FirstName;
                    insertCommand.Parameters["@LastName"].Value = user.LastName;
                    insertCommand.Parameters["@Email"].Value = user.Email;
                    insertCommand.Parameters["@Password"].Value = user.Password;
                    insertCommand.Parameters["@Address"].Value = user.Address ?? (object)DBNull.Value;
                    insertCommand.Parameters["@Phone"].Value = user.Phone ?? (object)DBNull.Value;
                    insertCommand.Parameters["@PicturePath"].Value = user.PicturePath ?? (object)DBNull.Value;
                    insertCommand.Parameters["@Admin"].Value = true;

                    totalInserted += insertCommand.ExecuteNonQuery();
                    
                }

                Console.WriteLine($"{totalInserted} Test Users Inserted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error seeding test data: " + ex.Message);
            }
        }
    }
}