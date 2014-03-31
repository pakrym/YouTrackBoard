using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoutrackBoard.ReviewboardApi
{
    using System.Configuration;
    using System.Net;

    using RestSharp;

    class ReviewRepository
    {
        private readonly ReviewboardClientFactory _reviewboardClientFactory;

        public ReviewRepository(ReviewboardClientFactory reviewboardClientFactory)
        {
            this._reviewboardClientFactory = reviewboardClientFactory;
        }

        public async Task<List<ReviewRequest>> GetReviewsTo()
        {
            var client = _reviewboardClientFactory.CreateConnection();
            var result = await client.ExecuteTaskAsync<ReviewRequestResponse>(new GetReviewRequests());
            return result.Data.ReviewRequests;
        }

        public async Task<List<ReviewRequest>> GetReviewsFrom()
        {
            
        }
    }

    internal class GetReviewRequests : RestRequest
    {
        public GetReviewRequests()
            : base("/api/review-requests/")
        {
        }
    }

    class ReviewboardClientFactory
    {
        private RestClient _client;
        private bool _hasAuth = false;

        public ReviewboardClientFactory()
        {
            _client = new RestClient(ConfigurationManager.AppSettings["reviewboard.url"]);
            _client.CookieContainer = new CookieContainer();
        }

        public IRestClient CreateConnection()
        {
            if (!_hasAuth)
            {
                Authentificate();
            }
            return _client;
        }

        private void Authentificate()
        {
            var result = _client.Execute(new ReviewboardAuthentificateRequest(ConfigurationManager.AppSettings["reviewboard.user"], ConfigurationManager.AppSettings["reviewboard.password"]));

            _hasAuth = result.StatusCode == HttpStatusCode.OK;
        }
    }
}
