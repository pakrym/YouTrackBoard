using RestSharp;

namespace YoutrackBoard
{
    internal class GetAllProjectsRequest : RestRequest
    {
        public GetAllProjectsRequest(bool verbose)
            : base("/rest/project/all")
        {
            AddParameter("verbose", verbose);
        }
    }
}