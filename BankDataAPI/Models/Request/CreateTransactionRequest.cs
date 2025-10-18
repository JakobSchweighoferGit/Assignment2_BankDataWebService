namespace BusinessLayerAPI.Models.Request
{
    public class CreateTransactionRequest
    {
        public int TransactionID { get; set; } 
        public string? Date { get; set; }
        public int Amount { get; set; }
        public string Type { get; set; }
        public Boolean Success { get; set; }
        public int AccountID { get; set; }
        public int? UserID { get; set; }

    }
}



/*
 * CREATE TABLE IF NOT EXISTS TransactionTable (
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
*/