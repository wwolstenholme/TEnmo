using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class TenmoAccount
    {
        public decimal accountBalance { get; set; } = 1000M;

        public int accountId { get; private set; }

        public TenmoAccount()
        {

        }

        public TenmoAccount(decimal accountBalance, int accountId)
        {
            this.accountBalance = accountBalance;
            this.accountId = accountId;
        }
    }
}
