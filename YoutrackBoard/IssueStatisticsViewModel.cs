namespace YoutrackBoard
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    internal class IssueStatisticsViewModel
    {
        private readonly Func<Issue, IssueViewModel> _issueViewModelFactory;
        

        public IssueStatisticsViewModel(IEnumerable<Issue> issues, Func<Issue, IssueViewModel> issueViewModelFactory)
        {
            var doneStates = new[] { "To Verify", "Done" };
            var inProgressStates = new[] { "In Progress" };
            var openStates = new[] { "Open", "Reopened" };

            
            this._issueViewModelFactory = issueViewModelFactory;
            this.Groups =
                new ObservableCollection<IssueStatisticsGroupViewModel>(
                    new[]
                        {
                            new IssueStatisticsGroupViewModel("Open", issues.Where(i => openStates.Contains(i.State)).Select(this._issueViewModelFactory)),
                            new IssueStatisticsGroupViewModel("In Progress", issues.Where(i => inProgressStates.Contains(i.State)).Select(this._issueViewModelFactory)),
                            new IssueStatisticsGroupViewModel("Done", issues.Where(i => doneStates.Contains(i.State)).Select(this._issueViewModelFactory)),
                            new IssueStatisticsGroupViewModel(
                                "Other",
                                issues.Where(i => !(openStates.Contains(i.State) || doneStates.Contains(i.State) || inProgressStates.Contains(i.State)))
                                .Select(this._issueViewModelFactory)),
                        });
        }

        public ObservableCollection<IssueStatisticsGroupViewModel> Groups { get; set; }

        
    }
}