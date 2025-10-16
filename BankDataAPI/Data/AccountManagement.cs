using System.Data.SQLite;

namespace BusinessLayerAPI.Data
{
    public class AccountManagement
    {
        private static readonly string connectionString = "Data Source=mydatabase.db;Version=3;";

        public static bool InsertAccount(string accountNumber, int balance, int userId, bool active)
        {
            using var conn = new SQLiteConnection(connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            INSERT INTO AccountTable (AccountNumber, Balance, UserID, Active)
            VALUES (@acc, @bal, @uid, @act);";
            cmd.Parameters.AddWithValue("@acc", accountNumber);
            cmd.Parameters.AddWithValue("@bal", balance);
            cmd.Parameters.AddWithValue("@uid", userId);
            cmd.Parameters.AddWithValue("@act", active ? 1 : 0);

            try
            {
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("InsertBankaccount error: " + ex.Message);
                return false;
            }
        }
    }
}
