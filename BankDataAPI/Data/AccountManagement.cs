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

        // Presentation Layer fertig!!!! Aber warum bei ID??? Das gut also Ändern
        public static (int AccountID, string AccountNumber, int Balance, bool Active, int UserID)? GetAccountById(int id)
        {
            using var conn = new SQLiteConnection(connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT AccountID, AccountNumber, Balance, Active, UserID
                            FROM AccountTable WHERE AccountID = @id LIMIT 1;";
            cmd.Parameters.AddWithValue("@id", id);

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return (
                Convert.ToInt32(r["AccountID"]),
                Convert.ToString(r["AccountNumber"]) ?? "",
                Convert.ToInt32(r["Balance"]),
                Convert.ToBoolean(r["Active"]),
                Convert.ToInt32(r["UserID"])
            );
        }

    }
}
