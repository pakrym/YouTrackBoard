using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Caliburn.Micro;

namespace YoutrackBoard
{
    internal class PersonDetailsViewModel:PropertyChangedBase
    {
        private readonly UserRepository _userRepository;
        private readonly Func<IEnumerable<Issue>, IssueStatisticsViewModel> _issueStatisticsFactory;
        private string _avatar;
        private IssueStatisticsViewModel _issueStatistics;

        public PersonDetailsViewModel(Person person,
            UserRepository userRepository,
            Func<IEnumerable<Issue>,IssueStatisticsViewModel> issueStatisticsFactory)
        {
            _userRepository = userRepository;
            _issueStatisticsFactory = issueStatisticsFactory;
            Person = person;
            Issues = new ObservableCollection<Issue>();
            WorkItems = new ObservableCollection<WorkItem>();
            
        }


        public string Avatar
        {
            get { return _avatar; }
            set
            {
                if (value == _avatar) return;
                _avatar = value;
                NotifyOfPropertyChange(() => Avatar);
            }
        }

        public Person Person { get; set; }

        public ObservableCollection<Issue> Issues { get; private set; }
        public ObservableCollection<WorkItem> WorkItems { get; private set; }

        public IssueStatisticsViewModel IssueStatistics
        {
            get { return _issueStatistics; }
            set
            {
                if (Equals(value, _issueStatistics)) return;
                _issueStatistics = value;
                NotifyOfPropertyChange(() => IssueStatistics);
            }
        }

        public void SetIssues(Issue[] issues)
        {
            Issues.Clear();
            foreach (var issue in issues)
            {
                Issues.Add(issue);
            }
            IssueStatistics = _issueStatisticsFactory(issues);
        }

        public void SetWorkItems(WorkItem[] workItems)
        {

            WorkItems.Clear();
            foreach (var workItem in workItems)
            {
                WorkItems.Add(workItem);
            }
        }
    }
}