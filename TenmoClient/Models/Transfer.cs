using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class Transfer
    {

        public int accountTo { get; set; }

        public int accountFrom { get; set; }

        public decimal amount { get; set; }

        public int transferId { get; set; }

        public int statusId { get; set; }

        public int transferTypeId { get; set; }

        public Transfer()
        {

        }
        
        public Transfer(int accountTo, int accountFrom, decimal transferAmount, int transferId, int statusId, int transferTypeId)
        {
            this.accountTo = accountTo;
            this.accountFrom = accountFrom;
            this.amount = transferAmount;
            this.transferId = transferId;
            this.statusId = statusId;
            this.transferTypeId = transferTypeId;
        }

    }
}
