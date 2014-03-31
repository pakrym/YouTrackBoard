using System;
using System.Configuration;
using System.Net;
using RestSharp;


namespace YoutrackBoard
{
    class ConnectionFactory
    {
        private RestClient _client;
        private bool _hasAuth = false;

        public ConnectionFactory()
        {
            _client = new RestClient(ConfigurationManager.AppSettings["url"]);
            _client.CookieContainer = new CookieContainer();
        }

        
        public IRestClient  CreateConnection()
        {
            if (!_hasAuth)
            {
                Authentificate();
            }
            return _client;
        }

        private void Authentificate()
        {
            var result = _client.Execute(new AuthentificateRequest(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]));

            _hasAuth = result.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }

    internal class AuthentificateRequest : RestRequest
    {
        public AuthentificateRequest(string login, string password)
            : base("/rest/user/login", Method.POST)
        {
            AddParameter("login", login);
            AddParameter("password", password);
        }
    }
}