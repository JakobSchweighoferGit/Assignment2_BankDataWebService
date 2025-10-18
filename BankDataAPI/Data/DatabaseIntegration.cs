using System.Data.SQLite;
using BankDataAPI.Models;

namespace BankDataAPI.Data
{
    public class DatabaseIntegration
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
                            Amount        INTEGER     NOT NULL,
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
    }
}
