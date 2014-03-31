namespace YoutrackBoard.ReviewboardApi
{
    using System;
    using System.Text;

    using RestSharp;

    internal class ReviewboardAuthentificateRequest : RestRequest
    {
        public ReviewboardAuthentificateRequest(string user, string password)
        {
            this.AddHeader("Authorization", Convert.ToBase64String(Encoding.ASCII.GetBytes(user + ":" + password)));
        }
    }
}