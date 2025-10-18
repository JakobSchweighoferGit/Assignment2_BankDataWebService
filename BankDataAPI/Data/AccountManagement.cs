using System.Data.SQLite;
using BankDataAPI.Data;
using RequestsResponses;

namespace BusinessLayerAPI.Data
{
    public class AccountManagement
    {
        private static readonly string connectionString = "Data Source=mydatabase.db;Version=3;";

        private static readonly string ReportDirectory = "SeedReports";

        private static readonly string UserFilePath = Path.Combine(ReportDirectory, "UserList.txt");
        private static readonly string AccountFilePath = Path.Combine(ReportDirectory, "AccountList.txt");
        private static readonly string AdminFilePath = Path.Combine(ReportDirectory, "AdminList.txt");
        public static bool InsertAccount(string accountNumber, int balance, int userId)
        {
            using var conn = new SQLiteConnection(connectionString);
            conn.Open();


            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            INSERT INTO AccountTable (AccountNumber, Balance, UserID)
            VALUES (@acc, @bal, @uid, @act);";
            cmd.Parameters.AddWithValue("@acc", accountNumber);
            cmd.Parameters.AddWithValue("@bal", balance);
            cmd.Parameters.AddWithValue("@uid", userId);
         

            String contents = $"Account Number: {accountNumber}, Balance: {balance}$, UserID: {userId}";
            try
            {
                if( cmd.ExecuteNonQuery() > 0)
                {

                    DataSeeding.WriteToFile(AccountFilePath, contents);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("InsertBankaccount error: " + ex.Message);
                return false;
            }

            return false;
        }

        public static (int AccountID, string AccountNumber, int Balance, bool Active, int UserID, string Handle)? GetAccountById(int id)
        {
            using var conn = new SQLiteConnection(connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT 
                    a.AccountID,
                    a.AccountNumber,
                    a.Balance,
                    a.Active,
                    a.UserID,
                    u.Handle
                FROM AccountTable a
                INNER JOIN UserTable u ON u.UserID = a.UserID
                WHERE a.AccountID = @id
                LIMIT 1;";
            cmd.Parameters.AddWithValue("@id", id);

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return (
                Convert.ToInt32(r["AccountID"]),
                Convert.ToString(r["AccountNumber"]) ?? "",
                Convert.ToInt32(r["Balance"]),
                Convert.ToBoolean(r["Active"]),
                Convert.ToInt32(r["UserID"]),
                Convert.ToString(r["Handle"]) ?? ""
            );
        }

        public static bool UpdateAccount(EditBankAccountRequest req)
        {
            using var conn = new SQLiteConnection(connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE AccountTable
                SET 
                    AccountNumber = COALESCE(NULLIF(@acc, ''), AccountNumber),
                    Balance       = @bal,
                    Active        = @act,
                    UserID        = (SELECT UserID FROM UserTable WHERE Handle = @handle)
                WHERE AccountID = @id;
            ";

            cmd.Parameters.AddWithValue("@acc", req.AccountNumber);
            cmd.Parameters.AddWithValue("@bal", req.Balance);
            cmd.Parameters.AddWithValue("@act", req.Active ? 1 : 0);
            cmd.Parameters.AddWithValue("@handle", req.Handle);
            cmd.Parameters.AddWithValue("@id", req.AccountID);

            try
            {
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
            catch (SQLiteException ex)
            { 
                return false;
            }
        }

        public static bool DeleteAccount(string accountNumber)
        {
            using var conn = new SQLiteConnection(connectionString);

            const string sql = @"
                DELETE FROM AccountTable
                WHERE AccountNumber = @AccountNumberToDelete;
            ";

            try
            {
                conn.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@AccountNumberToDelete", accountNumber);

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected == 1;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"SQLite Error deleting account by accountNumber '{accountNumber}': {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error deleting account by accountNumber '{accountNumber}': {ex.Message}");
                return false;
            }
        }


    }
}
