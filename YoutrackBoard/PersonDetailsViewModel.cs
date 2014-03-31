using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Caliburn.Micro;

namespace YoutrackBoard
{
    using System.Linq;
    using System.Windows.Media;

    internal class PersonDetailsViewModel:PropertyChangedBase
    {
        private readonly UserRepository _userRepository;
        private readonly Func<IEnumerable<Issue>, IssueStatisticsViewModel> _issueStatisticsFactory;
        private string _avatar;
        private IssueStatisticsViewModel _issueStatistics;

        static PersonDetailsViewModel()
        {
            TileColors = new Brush[] {
                 new SolidColorBrush(Color.FromRgb((byte)111,(byte)189,(byte)69)),
                new SolidColorBrush(Color.FromRgb((byte)75,(byte)179,(byte)221)),
                new SolidColorBrush(Color.FromRgb((byte)65,(byte)100,(byte)165)),
                new SolidColorBrush(Color.FromRgb((byte)225,(byte)32,(byte)38)),
                new SolidColorBrush(Color.FromRgb((byte)128,(byte)0,(byte)128)),
                new SolidColorBrush(Color.FromRgb((byte)0,(byte)128,(byte)64)),
                new SolidColorBrush(Color.FromRgb((byte)0,(byte)148,(byte)255)),
                new SolidColorBrush(Color.FromRgb((byte)255,(byte)0,(byte)199)),
                new SolidColorBrush(Color.FromRgb((byte)255,(byte)135,(byte)15)),
                new SolidColorBrush(Color.FromRgb((byte)45,(byte)255,(byte)87)),
                new SolidColorBrush(Color.FromRgb((byte)127,(byte)0,(byte)55))
            };
             ColorRandom = new Random();
        }

        public Brush GetColor(string name)
        {
            return TileColors[Math.Abs(name.GetHashCode()) % TileColors.Length];
        }

        public static Random ColorRandom { get; set; }

        public static Brush[] TileColors { get; set; }

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

        public string IconText
        {
            get { return  new string(Person.Name.Where(char.IsUpper).ToArray());}
        }

        public Brush IconBrush
        {
            get
            {
                return this.GetColor(Person.Name);
            }
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