namespace YoutrackBoard
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;

    internal class IssueStatisticsGroupViewModel
    {
        public IssueStatisticsGroupViewModel(string name, IEnumerable<IssueViewModel> issues)
        {
            this.Name = name;
            this.Issues = new ObservableCollection<IssueViewModel>(issues.OrderByDescending(i=>i.Tags.Count));
            this.Visible = this.Issues.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        public ObservableCollection<IssueViewModel> Issues { get; set; }

        public string Name { get; set; }

        public Visibility Visible { get; set; }
    }
}