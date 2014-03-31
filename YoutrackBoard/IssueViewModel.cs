namespace YoutrackBoard
{
    using System.Collections.Generic;
    using System.Windows.Documents;

    internal class IssueViewModel
    {
        public Issue Issue { get; set; }

        public List<string> Tags { get; set; }

        public string TagsString
        {
            get
            {
                return string.Join(", ", Tags);
            }
        }

        public IssueViewModel(Issue issue)
        {
            Issue = issue;
            Tags = new List<string>();

            if (Issue.Resolved.LessThenDays(1))
            {
                Tags.Add("Resolved today");
            }
            else if (Issue.Resolved.LessThenDays(2))
            {
                Tags.Add("Resolved yesterday");
            }
            else
            {

                if (Issue.Updated.LessThenDays(1))
                {
                    Tags.Add("Updated today");
                }
                else if (Issue.Updated.LessThenDays(2))
                {
                    Tags.Add("Updated yesterday");
                }
                else
                {
                    if (Issue.Created.LessThenDays(1))
                    {
                        Tags.Add("Created today");
                    }
                    else if (Issue.Created.LessThenDays(2))
                    {
                        Tags.Add("Created yesterday");
                    }
                }
            }

           





        }

        public string Estimation
        {
            get
            {
                return Issue.Estimation.ToReadableString();
            }
        }

        public string TimeSpent
        {
            get
            {
                return Issue.TimeSpent.ToReadableString();
            }
        }
    }
}