namespace YoutrackBoard
{
    internal class IssueViewModel
    {
        public Issue Issue { get; set; }

        public IssueViewModel(Issue issue)
        {
            Issue = issue;
        }
    }
}