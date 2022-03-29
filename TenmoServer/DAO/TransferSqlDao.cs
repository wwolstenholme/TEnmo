using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;
using System.Data.SqlClient;

namespace TenmoServer.DAO
{
    public class TransferSqlDao : ITransferDao
    {
        private readonly string connectionString;
        public TransferSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public TenmoAccount CheckBalance(int accountId)
        {
            TenmoAccount tenmoAccount = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT balance FROM account WHERE account_Id = @accountId", conn);
                    cmd.Parameters.AddWithValue("@accountId", accountId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        tenmoAccount = GetAccountFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return tenmoAccount;
        
        }
        public Transfer Create(Transfer transfer)
        {
            //int newTransferId;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                //SqlCommand cmd = new SqlCommand("INSERT INTO transfer (transfer_type_id, transfer_status_id, account_from, account_to, amount) " +
                //                                "OUTPUT INSERTED.transfer_id " +
                //                                "VALUES (@transfer_type_id, @transfer_status_id, @account_from, @account_to, @amount);", conn);
                //cmd.Parameters.AddWithValue("@transfer_type_id", transfer.transferType);
                //cmd.Parameters.AddWithValue("@transfer_status_id", transfer.statusId);
                //cmd.Parameters.AddWithValue("@account_from", transfer.accountFrom);
                //cmd.Parameters.AddWithValue("@account_to", transfer.accountTo);
                //cmd.Parameters.AddWithValue("@amount", transfer.transferAmount);

                SqlCommand cmd = new SqlCommand(@"INSERT INTO transfer (transfer_type_id,transfer_status_id,account_from,account_to,amount)
                                                      OUTPUT inserted.transfer_id
                                                      VALUES (@transfer_type_id,@transfer_status_id,@account_from,@account_to,@amount)", conn);

                cmd.Parameters.AddWithValue("@transfer_type_id", transfer.transferTypeId);
                cmd.Parameters.AddWithValue("@transfer_status_id", transfer.statusId);
                cmd.Parameters.AddWithValue("@account_from", transfer.accountFrom);
                cmd.Parameters.AddWithValue("@account_to", transfer.accountTo);
                cmd.Parameters.AddWithValue("@amount", transfer.amount);

                transfer.transferId = (int)cmd.ExecuteScalar();



                //transfer.transferId = Convert.ToInt32(cmd.ExecuteScalar());

            }
            return ViewTransferById(transfer.transferId);
        }

        public Transfer ViewTransferById(int transferId)
        {
            Transfer transfer = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount " +
                                                "FROM transfer " +
                                                "WHERE transfer_id = @transfer_id;", conn);
                cmd.Parameters.AddWithValue("@transfer_id", transferId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    transfer = GetTransferFromReader(reader);
                }
            }

            return transfer;
        }

        public List<Transfer> ViewAllTransfers()
        {
            List<Transfer> transfers = new List<Transfer>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd  = new SqlCommand("SELECT transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount " +
                                                "FROM transfer " +
                                                "ORDER BY transfer_id;", conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Transfer transfer = GetTransferFromReader(reader);
                    transfers.Add(transfer);
                }
            }
            return transfers;
        }

        
                    
        private TenmoAccount GetAccountFromReader(SqlDataReader reader)
        {
            TenmoAccount tenmoAccount = new TenmoAccount()
            {
                accountId = Convert.ToInt32(reader["account_id"]),
                userId = Convert.ToInt32(reader["user_Id"]),
                accountBalance = Convert.ToDecimal(reader["balance"]),
            };


            return tenmoAccount;
        }

        private Transfer GetTransferFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer()
            {
                accountTo = Convert.ToInt32(reader["account_to"]),
                accountFrom = Convert.ToInt32(reader["account_from"]),
                amount = Convert.ToDecimal(reader["amount"]),
                transferId = Convert.ToInt32(reader["transfer_id"]),
                statusId = Convert.ToInt32(reader["transfer_status_id"]),
                transferTypeId = Convert.ToInt32(reader["transfer_type_id"])
            };


            return transfer;
        }
        public Transfer SendMoney(int accountTo, int accountFrom, decimal amount)
        {
            Transfer transfer = new Transfer {

                transferTypeId = 2,
                statusId = 2,
                accountFrom = accountFrom,
                accountTo = accountTo,
                amount = amount,
                
            
            };

            //Transfer transfer1 = Create(transfer);
            
            

            TenmoAccount sendMoneyAccount = new TenmoAccount();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE account SET balance += @amount WHERE account_id = @accountTo", conn);
                    cmd.Parameters.AddWithValue("@accountTo", accountTo);
                    //  SqlDataReader reader = cmd.ExecuteReader();
                    cmd.Parameters.AddWithValue("@amount", amount);

                    SqlCommand cmd1 = new SqlCommand("UPDATE account SET balance -= @amount WHERE account_id = @accountFrom", conn);
                    cmd1.Parameters.AddWithValue("@accountFrom", accountFrom);
                    //SqlDataReader reader1 = cmd1.ExecuteReader();
                    cmd1.Parameters.AddWithValue("@amount", amount);

                    cmd.ExecuteNonQuery();
                    cmd1.ExecuteNonQuery();

                }
            }
            catch (SqlException)
            {
                throw;
            }

            return transfer;
        }





    }
}
