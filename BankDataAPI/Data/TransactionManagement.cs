using BusinessLayerAPI.Models.Request;
using System.Collections.Generic;
using System.Data.SQLite;

namespace BusinessLayerAPI.Data
{
    //class responsible for updating client balances and logging transaction records to db
    public class TransactionManagement
    {
        private static readonly string connectionString = "Data Source=mydatabase.db;Version=3;";

        //method to create transaction from transaction req
        public static Boolean createTransaction(CreateTransactionRequest req)
        {
            if (req == null)
            {
                throw new ArgumentNullException(nameof(req), "Transaction request cannot be null.");
            }

            //use paramaterization to protect against sql injection
            const string sql = @"
                INSERT INTO TransactionTable 
                    (Date, Amount, Type, Success, AccountID, UserID)
                VALUES
                    (@Date, @Amount, @Type, @Success, @AccountID, @UserID);
            ";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {

                    object dateValue = string.IsNullOrEmpty(req.Date) ? DBNull.Value : (object)req.Date;
                    command.Parameters.AddWithValue("@Date", dateValue);

                    command.Parameters.AddWithValue("@Amount", req.Amount);

                    command.Parameters.AddWithValue("@Type", req.Type);

                    command.Parameters.AddWithValue("@Success", req.Success ? 1 : 0);

                    command.Parameters.AddWithValue("@AccountID", req.AccountID);

                    object userIdValue = req.UserID.HasValue ? (object)req.UserID.Value : DBNull.Value;
                    command.Parameters.AddWithValue("@UserID", userIdValue);

                    try
                    {
                        return command.ExecuteNonQuery() > 0;
                    }
                    catch (SQLiteException ex)
                    {
                        Console.WriteLine("Create Transaction error: " + ex.Message);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("General error during transaction creation: " + ex.Message);
                        return false;
                    }
                }
            }
        }


        //method to update balance from update req
        public static Boolean updateBalance(CreateBalanceUpdateRequest req)
        {
            //ensure its not null
            if (req == null)
            {
                throw new ArgumentNullException(nameof(req), "Balance update request cannot be null.");
            }

            //parameterize command to avoid sql injection
            const string sql = @"
                UPDATE AccountTable
                SET Balance = Balance + @ChangeAmount
                WHERE AccountID = @AccountID;
            ";

            using (var connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        int rowsAffected;

                        command.Parameters.AddWithValue("@AccountID", req.AccountID);

                        command.Parameters.AddWithValue("@ChangeAmount", req.IncrAmount);

                        rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                             
                            Console.WriteLine($"Update Balance failed: Account with ID {req.AccountID} not found.");
                            return false;
                        }

                        return true;
                    }
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine("Update Balance error: " + ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("General error during balance update: " + ex.Message);
                    return false;
                }
            }
        }
    }
}

