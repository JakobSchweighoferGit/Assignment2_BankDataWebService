using System.Data.SQLite;
using BankDataAPI.Models;

namespace BankDataAPI.Data
{
    public class DBManager
    {
        private static string connectionString = "Data Source=mydatabase.db;Version=3;";
    
        public static bool CreateTable()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        //Create UserTable
                        command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS UserTable ( 
                            UserID        INTEGER PRIMARY KEY AUTOINCREMENT,
                            Handle        TEXT        NOT NULL UNIQUE,  
                            FirstName     TEXT        NOT NULL,
                            LastName      TEXT        NOT NULL,
                            Email         TEXT        NOT NULL UNIQUE,
                            Password      TEXT        NOT NULL,
                            Address       TEXT,
                            Phone         TEXT,
                            PicturePath   TEXT,
                            Admin         BOOLEAN     NOT NULL DEFAULT 0
                        )";
                        command.ExecuteNonQuery();

                        //Create AccountTable
                        command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS AccountTable (
                            AccountID     INTEGER PRIMARY KEY AUTOINCREMENT,
                            AccountNumber TEXT        NOT NULL UNIQUE,
                            Balance  INTEGER     NOT NULL DEFAULT 0,
                            UserID        INTEGER     NOT NULL,
                            FOREIGN KEY (UserID) REFERENCES UserTable(UserID)
                                ON UPDATE CASCADE
                                ON DELETE RESTRICT
                        )";
                        command.ExecuteNonQuery();

                        //Create TransactionTable
                        command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS TransactionTable (
                            TransactionID INTEGER PRIMARY KEY AUTOINCREMENT,
                            Date          TEXT        NOT NULL DEFAULT (datetime('now')),
                            Amount   INTEGER     NOT NULL,
                            Type          TEXT        NOT NULL,
                            Success       BOOLEAN     NOT NULL DEFAULT 1,
                            AccountID     INTEGER     NOT NULL,
                            UserID        INTEGER,
                            FOREIGN KEY (AccountID) REFERENCES AccountTable(AccountID)
                                ON UPDATE CASCADE
                                ON DELETE RESTRICT,
                            FOREIGN KEY (UserID) REFERENCES UserTable(UserID)
                                ON UPDATE CASCADE
                                ON DELETE SET NULL
                        )";
                        command.ExecuteNonQuery();

                        connection.Close();
                    }
                }
                Console.WriteLine("Table created successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // Create table failed
            }
        }


        public static UserDetails? DataGetUserByHandle(string handle)
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

            var userInformation = new UserDetails
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

        public static void SeedTestUsers()
        {
            try
            {
                using var connection = new SQLiteConnection(connectionString);
                connection.Open();

                // Prüfen, ob es schon Einträge gibt
                using (var check = connection.CreateCommand())
                {
                    check.CommandText = "SELECT COUNT(*) FROM UserTable;";
                    var count = Convert.ToInt32(check.ExecuteScalar());

                    if (count > 0)
                    {
                        Console.WriteLine("ℹ️  Testdaten bereits vorhanden – Seed wird übersprungen.");
                        return;
                    }
                }

                using (var insert = connection.CreateCommand())
                {
                    insert.CommandText = @"
                INSERT INTO UserTable 
                    (Handle, FirstName, LastName, Email, Password, Address, Phone, PicturePath, Admin)
                VALUES
                    ('jdoe', 'John', 'Doe', 'john.doe@example.com', 'test123', '123 Main St', '1234567890', NULL, 0),
                    ('asmith', 'Alice', 'Smith', 'alice.smith@example.com', 'test123', '456 Elm St', '9876543210', NULL, 0),
                    ('admin', 'Bob', 'Admin', 'admin@example.com', 'admin123', 'Admin HQ', '111222333', NULL, 1);
            ";
                    int rows = insert.ExecuteNonQuery();
                    Console.WriteLine($"✅ {rows} Test-User erfolgreich eingefügt.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Fehler beim Seed der Testdaten: " + ex.Message);
            }
        }

    }
}
