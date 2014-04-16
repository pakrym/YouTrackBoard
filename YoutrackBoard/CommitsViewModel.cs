namespace YoutrackBoard
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Windows;

    using Caliburn.Micro;

    internal class funcitonfac
    {
        
    }

    internal class CommitsViewModel: PropertyChangedBase
    {
        private Visibility showHeader;

        public CommitsViewModel(
            Person person,
            Sprint sprint,
            SvnCommitRepository commitRepository,
            Func<SvnCommit, CommitViewModel> commitViewModelFactory
            )
        {
            this.Commits = new ObservableCollection<CommitViewModel>();
            this.ShowHeader = Visibility.Collapsed;
            commitRepository.ListCommitsObservable(TimeSpan.FromDays(7)).ObserveOnDispatcher().Subscribe(
                result =>
                    {
                        this.Commits.Clear();
                        foreach (var eventArg in result.Where(r=>r.Author.Contains(person.Login)))
                        {
                            this.Commits.Add(commitViewModelFactory(eventArg));
                        }
                        ShowHeader = this.Commits.Any() ? Visibility.Visible : Visibility.Collapsed;
                    });
        }

        public ObservableCollection<CommitViewModel> Commits { get; private set; }

        public Visibility ShowHeader
        {
            get
            {
                return this.showHeader;
            }
            set
            {
                if (value == this.showHeader) return;
                this.showHeader = value;
                this.NotifyOfPropertyChange(() => this.ShowHeader);
            }
        }
    }
}