using System;
using System.Collections.Generic;

using Caliburn.Micro;

namespace YoutrackBoard
{
    using System.Linq;
    using System.Reactive.Concurrency;
    using System.Threading;
    using System.Windows.Media;

    using SharpSvn;

    internal class PersonDetailsViewModel:PropertyChangedBase
    {
        private readonly UserRepository _userRepository;
        private readonly IssueRepository issueRepository;
        private readonly Func<IEnumerable<Issue>, IssueStatisticsViewModel> _issueStatisticsFactory;
        private string _avatar;
        private IssueStatisticsViewModel _issueStatistics;
        private CommitsViewModel commits;

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

        public PersonDetailsViewModel(Project project, 
            Sprint sprint,
            Person person,

            UserRepository userRepository,
            IssueRepository issueRepository,
            Func<IEnumerable<Issue>, IssueStatisticsViewModel> issueStatisticsFactory,
            Func<Sprint, Person, CommitsViewModel> commitsFactory
            )

        {
            _userRepository = userRepository;
            this.issueRepository = issueRepository;
            _issueStatisticsFactory = issueStatisticsFactory;
            Person = person;

            issueRepository.SearchObservable(project, sprint)
                           .Subscribe(r =>
                                    IssueStatistics = issueStatisticsFactory(r.Where(issue =>issue.Assignee != null && issue.Assignee.Login == person.Login))
                            );

            Commits = commitsFactory(sprint, person);
        }

        public string IconText
        {
            get { return  new string(Person.Name.Where(char.IsUpper).ToArray()); }
        }

        public Brush IconBrush
        {
            get
            {
                return this.GetColor(Person.Name);
            }
        }
        
        public Person Person { get; set; }

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

        public CommitsViewModel Commits
        {
            get
            {
                return this.commits;
            }
            set
            {
                if (Equals(value, this.commits))
                {
                    return;
                }
                this.commits = value;
                this.NotifyOfPropertyChange(() => this.Commits);
            }
        }
    }
}