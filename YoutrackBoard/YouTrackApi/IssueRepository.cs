using System.Collections.Generic;
using System.Threading.Tasks;

namespace YoutrackBoard
{
    using System;
    using System.Reactive.Linq;
    using System.Threading;

    class IssueRepository: IRepository
    {

        public event EventHandler Refresh;

        private readonly YouTrackClientFactory youTrackClientFactory;

        public IssueRepository(YouTrackClientFactory youTrackClientFactory)
        {
            this.youTrackClientFactory = youTrackClientFactory;
        }

        public List<Issue> Search(Project project, Sprint sprint)
        {
            var connection = this.youTrackClientFactory.CreateConnection();
            var result = connection.Execute<IssueResponce>(
                new SearchIssueRequest(string.Format("#{0} Fix versions: {1}", project.Name, sprint.Name)));
            return result.Data.Issue;      
        }

        public List<WorkItem> GetWorkItems(Issue issue)
        {
           var connection = this.youTrackClientFactory.CreateConnection();
           var result = connection.Execute<List<WorkItem>>(new GetIssueWorkItemsRequest(issue));
           return result.Data;      
        }


        public void RefreshData()
        {
            if (Refresh != null)
            {
                Refresh(this, EventArgs.Empty);
            }
        }

        public IObservable<List<Issue>> SearchObservable(Project project, Sprint sprint)
        {
            return TaskObservable.Create(() => this.Search(project, sprint), this);
        }


        public IObservable<List<WorkItem>> GetWorkItemsObservable(Issue issue)
        {
            return TaskObservable.Create(() => this.GetWorkItems(issue), this);
        }
    }

    internal interface IRepository
    {
        event EventHandler Refresh;
    }

    internal sealed class TaskObservable
    {
        public static IObservable<T> Create<T>(Func<T> func, IRepository issueRepository)
        {
            return new TaskObservable<T>(func,issueRepository);
        }
    }
    internal sealed class TaskObservable<TResult> : IObservable<TResult>
    {
        private readonly Func<TResult> func;
        private readonly IRepository repository;

        
        public TaskObservable(Func<TResult> func, IRepository repository)
        {
            this.func = func;
            this.repository = repository;
        }

        public IDisposable Subscribe(IObserver<TResult> observer)
        {

            EventHandler run = (s, e) => this.Run(observer);

            repository.Refresh += run;
            var cts = this.Run(observer);

            return new CancelOnDispose { Source = cts, Repository = repository, Handler = run };
        }

        private CancellationTokenSource Run(IObserver<TResult> observer)
        {
            var cts = new CancellationTokenSource();
            var task = new Task<TResult>(this.func);

            task.ContinueWith(
                t =>
                    {
                        switch (t.Status)
                        {
                            case TaskStatus.RanToCompletion:
                                observer.OnNext(task.Result);
                                break;
                            case TaskStatus.Faulted:
                                observer.OnError(task.Exception);
                                break;
                            case TaskStatus.Canceled:
                                observer.OnError(new TaskCanceledException(t));
                                break;
                        }
                    },
                cts.Token);
            task.Start();
            return cts;
        }

        private class CancelOnDispose : IDisposable
        {
            internal CancellationTokenSource Source;
            internal IRepository Repository;
            internal EventHandler Handler;

            #region IDisposable Members

            void IDisposable.Dispose()
            {
                Source.Cancel();
                Repository.Refresh -= this.Handler;
            }

            #endregion
        }

    }

}