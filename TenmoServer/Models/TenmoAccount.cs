using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoServer.Models
{
    public class TenmoAccount
    {
        public decimal accountBalance { get; set; } = 1000M;

        public int accountId { get;  set; }
        public int userId { get; set; }

        /*
        public override string ToString()
        {
            return ($"UserId: {userId}   AccountId: {accountId}   Balance: {accountBalance}");
        }
        */
    }
}
