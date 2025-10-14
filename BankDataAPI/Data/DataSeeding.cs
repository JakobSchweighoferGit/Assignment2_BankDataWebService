using System.Data.SQLite;

namespace BankDataAPI.Data
{
    public class DataSeeding
    {
        private static string connectionString = "Data Source=mydatabase.db;Version=3;";
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
