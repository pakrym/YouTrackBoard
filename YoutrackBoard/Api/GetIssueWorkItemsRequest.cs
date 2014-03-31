using RestSharp;

namespace YoutrackBoard
{
    internal class GetAvatarRequest : RestRequest
    {
        public GetAvatarRequest(Person person)
            : base("/user/{user}")
        {
            AddUrlSegment("user", person.Login);
        }
        
    }
    internal class GetIssueWorkItemsRequest : RestRequest
    {
        public GetIssueWorkItemsRequest(Issue issue)
            : base("/rest/issue/{issue}/timetracking/workitem/")
        {
            AddUrlSegment("issue", issue.Id);
        }
    }
}