namespace YoutrackBoard
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Media;

    internal class CommitViewModel
    {
        public SvnCommit Commit { get; set; }

        public CommitViewModel(SvnCommit commit)
        {
            var regex = new Regex("#(\\w+-\\d+)");
            this.Commit = commit;
            this.IssueIds = regex.Matches(commit.LogMessage).OfType<Match>().Select(m => m.Groups[1].Value).ToArray();
            
        }

        public string[] IssueIds { get; set; }

        public Color TextColor
        {
            get
            {
                return IssueIds.Length == 0? Colors.OrangeRed : Colors.Black;
            }
        }
    }
}