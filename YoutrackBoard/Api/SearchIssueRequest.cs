using RestSharp;

namespace YoutrackBoard
{
    internal class SearchIssueRequest : RestRequest
    {
        public SearchIssueRequest(string filter)
            : base("/rest/issue")
        {
            AddParameter("filter",filter);
            AddParameter("max", 1000);

        }
    }
}