using Microsoft.AspNetCore.Mvc;
using TenmoServer.Models;
using TenmoServer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Controllers
{
    [Route("Transfer")]
    [ApiController]
    public class TransferController : Controller
    {
        private readonly ITransferDao transferDao;
        private readonly ITenmoAccountDao tenmoAccountDao;

        public TransferController (ITransferDao transferDao, ITenmoAccountDao tenmoAccountDao)
        {
            this.transferDao = transferDao;
            this.tenmoAccountDao = tenmoAccountDao;
        }


        [HttpPost("{transferId}")]
        public ActionResult<Transfer> Create(Transfer transfer)
        {
            Transfer transfer1 = transferDao.Create(transfer);
            return Created($"/Transfer/{transfer1.transferId}", transfer1);
        }



        [HttpGet()]

        public ActionResult<List<Transfer>> ViewAllTransfers()
        {
            List<Transfer> allTransfers = transferDao.ViewAllTransfers();
            return Ok(allTransfers);
        }

        //fix this method 
        [HttpGet("{Id}")]

        public ActionResult<Transfer> ViewTransferById(int transferId)
        {
            Transfer transfer = transferDao.ViewTransferById(transferId);
            if (transfer == null)
            {
                return NotFound();
            }
            return Ok(transferId);


        }


        /*
        [HttpPost()]

        public ActionResult<Transfer> SendMoney(int accountTo, int accountFrom, decimal amount)
        {
            TenmoAccount receivingAccount = tenmoAccountDao.GetAccount(accountTo);
            TenmoAccount sendingAccount = tenmoAccountDao.GetAccount(accountFrom);

            if (receivingAccount == null || sendingAccount == null)
            {
                return NotFound();
            }

            //else if(amount <= 0 || amount > sendingAccount.accountBalance)
            //{
            //    return BadRequest();
            //}
            else
            {
                Transfer transfer = transferDao.SendMoney(receivingAccount.accountId, sendingAccount.accountId, amount);

                tenmoAccountDao.AddMoneyToAccount(receivingAccount.accountId, amount);
                tenmoAccountDao.SubtractMoneyToAccount(sendingAccount.accountId, amount);
                return transfer;     //(tenmoAccount.accountBalance);
            }
            

        }
        */

        private int GetLoggedInUserId()
        {
            
            int userId =  int.Parse(User.FindFirst("sub")?.Value);

            return userId;




            //this is a method that can be used by all endpoints within the controller to return the user id for the current logged in authenticated user.
        }



    }
}
