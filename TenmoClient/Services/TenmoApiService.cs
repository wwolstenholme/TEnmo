using RestSharp;
using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient.Services
{
    public class TenmoApiService : AuthenticatedApiService
    {
        public readonly string ApiUrl;

        public TenmoApiService(string apiUrl) : base(apiUrl) { }

        public decimal GetBalance()
        {
            //no endpoint for account balance
            RestRequest request = new RestRequest($"account");
            IRestResponse<decimal> response = client.Get<decimal>(request);
            CheckForError(response);
            return response.Data;
        }

        //public List<TenmoAccount> GetAccountsSearchName(string searchTerm)

        //public TenmoAccount UpdateBalance(TenmoAccount accountToUpdate)
        //{
        //    RestRequest request = new RestRequest($"account/{accountToUpdate.accountId}");
        //    request.AddJsonBody(accountToUpdate);
        //    IRestResponse<TenmoAccount> response = client.Put<TenmoAccount>(request);
        //    CheckForError(response);
        //    return response.Data;
        //}

        public void UpdateBalance(Transfer transfer)
        {
            RestRequest request = new RestRequest($"account");
            request.AddJsonBody(transfer);
            IRestResponse response = client.Put(request);
            CheckForError(response);
        }

        public List<Transfer> ViewAllTransfers()
        {
            RestRequest request = new RestRequest($"transfer");
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);
            CheckForError(response);
            return response.Data;
        }

        public Transfer CreateTransfer(Transfer transfer)
        {
            RestRequest request = new RestRequest($"transfer/{transfer.transferId}");
            request.AddJsonBody(transfer);
            IRestResponse<Transfer> response = client.Post<Transfer>(request);
            CheckForError(response);
            return response.Data;
        }


        public List<ApiUser> GetUsers()
        {
            RestRequest request = new RestRequest($"User");
            IRestResponse<List<ApiUser>> response = client.Get<List<ApiUser>>(request);
            CheckForError(response);
            return response.Data;


        }

        public int GetAccountFromId(string userName)
        {
            RestRequest request = new RestRequest($"User");
            request.AddJsonBody(userName);
            IRestResponse<int> response = client.Get<int>(request);
            CheckForError(response);
            return response.Data;

        }


    }
}
