namespace YoutrackBoard.ReviewboardApi
{
    using RestSharp;

    internal class GetReviewRequests : RestRequest
    {
        public GetReviewRequests(string from, string to)
            : base("/api/review-requests/")
        {
            if (!string.IsNullOrEmpty(from))
            {
                this.AddParameter("from-user", from);
            }
            if (!string.IsNullOrEmpty(to))
            {
                this.AddParameter("to-user", to);
            }
        }
    }
}