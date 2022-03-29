using Microsoft.AspNetCore.Mvc;
using TenmoServer.Models;
using System;
using System.Collections.Generic;
using TenmoServer.DAO;
using TenmoServer.Security;


namespace TenmoServer.Controllers
{
    [Route("account")]
    [ApiController]

    public class TenmoAccountController : ControllerBase
    {
        private readonly ITenmoAccountDao tenmoAccountDao;
      

        public TenmoAccountController(ITenmoAccountDao tenmoAccountDao)
        {

            this.tenmoAccountDao = tenmoAccountDao;

        }

        [HttpPost()]
        public ActionResult<TenmoAccount> Create(int userId)
        {
            TenmoAccount tenmoAccount1 = tenmoAccountDao.Create(userId);
            return Created($"/Account/{tenmoAccount1.accountId}", tenmoAccount1);
        }

        [HttpGet()]
        public ActionResult<decimal> ViewBalance()
        {
           
            TenmoAccount tenmoAccount = tenmoAccountDao.ViewBalance(GetLoggedInUserId());
            
            if(tenmoAccount == null)
            {
                return NotFound();
            }
            return Ok(tenmoAccount.accountBalance);

        }

        [HttpPut()]
        public ActionResult TransferMoney(Transfer transfer)
        {
            tenmoAccountDao.AddMoneyToAccount(transfer.accountTo, transfer.amount);
            tenmoAccountDao.SubtractMoneyToAccount(transfer.accountFrom, transfer.amount);
            return Ok();
        }
        





        private int GetLoggedInUserId()
        {

            int userId = int.Parse(User.FindFirst("sub")?.Value);

            return userId;




         
        }




    }
}
