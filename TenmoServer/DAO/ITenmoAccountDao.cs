using System;
using System.Collections.Generic;
using TenmoServer.Models;


namespace TenmoServer.DAO
{
    public interface ITenmoAccountDao
    {

        TenmoAccount ViewBalance(int accountId);

        
        TenmoAccount Create(int userId);

        TenmoAccount GetAccount(int accountId);


        void AddMoneyToAccount(int accountId, decimal amount);

        void SubtractMoneyToAccount(int accountId, decimal amount);




    }
}
