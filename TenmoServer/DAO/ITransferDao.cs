using System;
using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {
        /*
         * what are some methods we need for the account?
         * 
         * check balance yes
         * send money yes
         * see history yes 
         * see transfer details yes
         * 
         */
        

        Transfer Create(Transfer transfer);

        //Transfer GetTransfer(int transferId);

        List<Transfer> ViewAllTransfers();
        
        
        Transfer ViewTransferById(int transferId);

        Transfer SendMoney(int accountTo, int accountFrom, decimal amount);


    }
}
