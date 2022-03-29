using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;
using System.Data.SqlClient;


namespace TenmoServer.DAO
{
    public class TenmoAccountSqlDao : ITenmoAccountDao
    {
        private readonly string connectionString;
        public TenmoAccountSqlDao(string dbConnectionString)
        {
            this.connectionString = dbConnectionString;
        }


        public TenmoAccount ViewBalance(int userId)
        {
            TenmoAccount returnTenmoAccount = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT account_id, user_id, balance FROM account WHERE user_Id = @userId", conn);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        returnTenmoAccount = GetAccountFromReader(reader);
                    }
                    
                }
            }

            catch (SqlException)
            {
                throw;
            }

            return returnTenmoAccount;
        }

      
        public TenmoAccount Create(int userId)
        {
            int newTenmoAccountId;
            TenmoAccount account = new TenmoAccount();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO account (user_id, balance) OUTPUT INSERTED.account_id VALUES (@user_Id, @balance)", conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    cmd.Parameters.AddWithValue("@balance", 1000M);

                    newTenmoAccountId = Convert.ToInt32(cmd.ExecuteScalar());
                   

                }
            }
            catch (SqlException)
            {
                throw;
            }

            return GetAccount(newTenmoAccountId);
        }

        public Transfer ViewTransferById(int transferId)
        {
            Transfer transfer = new Transfer();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT transfer_id, account_from, account_to, transfer_type_id, transfer_status_id, amount from transfer where transfer_id = @transfer_id ", conn);
                    cmd.Parameters.AddWithValue("@transfer_id", transferId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        transfer = GetTransferFromReader(reader);
                      
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return transfer;
        }

        public List<TenmoAccount> List()
        {
            List<TenmoAccount> allAccounts = new List<TenmoAccount>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT account_id, user_id, balance FROM account", conn);
                   
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                         TenmoAccount t = GetAccountFromReader(reader);
                          allAccounts.Add(t);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return allAccounts;
        }


        public TenmoAccount GetAccount(int accountId)
        {
            TenmoAccount tenmoAccount = new TenmoAccount();
             try
             {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT account_id, user_id, balance from account where account_id = @account_id ", conn);
                    cmd.Parameters.AddWithValue("@account_Id", accountId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if(reader.Read())
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

        public void AddMoneyToAccount(int accountId, decimal amount)
        {
            TenmoAccount tenmoAccount = GetAccount(accountId);

            tenmoAccount.accountBalance += amount;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE account SET balance = @balance WHERE account_id = @accountId", conn);
                    cmd.Parameters.AddWithValue("@balance", tenmoAccount.accountBalance);
                    cmd.Parameters.AddWithValue("@accountId", accountId);
                    cmd.ExecuteNonQuery();
                    
                }


            }
            catch (SqlException)
            {
                throw;
            }

           
        }

        public void SubtractMoneyToAccount(int accountId, decimal amount)
        {
            TenmoAccount tenmoAccount = GetAccount(accountId);

            tenmoAccount.accountBalance -= amount;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE account SET balance = @balance WHERE account_id = @accountId", conn);
                    cmd.Parameters.AddWithValue("@balance", tenmoAccount.accountBalance);
                    cmd.Parameters.AddWithValue("@accountId", accountId);
                    cmd.ExecuteNonQuery();

                }


            }
            catch (SqlException)
            {
                throw;
            }


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



    }
        
}

