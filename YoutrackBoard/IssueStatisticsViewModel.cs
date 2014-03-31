using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace YoutrackBoard
{
    internal class IssueStatisticsViewModel
    {
        private readonly IEnumerable<Issue> _issues;
        private readonly Func<Issue, IssueViewModel> _issueViewModelFactory;

        public IssueStatisticsViewModel(IEnumerable<Issue> issues, 
            Func<Issue, IssueViewModel> issueViewModelFactory )
        {
            _issues = issues;
            _issueViewModelFactory = issueViewModelFactory;
            Open = new ObservableCollection<IssueViewModel>(issues.Select(_issueViewModelFactory));
        }

        public ObservableCollection<IssueViewModel> Done { get; set; }
        public ObservableCollection<IssueViewModel> InProgress { get; set; }
        public ObservableCollection<IssueViewModel> Open { get; set; }
    }
}