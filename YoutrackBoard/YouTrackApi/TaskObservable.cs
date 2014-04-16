namespace YoutrackBoard
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class TaskObservable<TResult> : IObservable<TResult>
    {
        private readonly Func<TResult> func;
        private readonly IRepository repository;
        private readonly CancellationTokenSource tokenSource;

        public TaskObservable(Func<TResult> func, IRepository repository)
        {
            this.func = func;
            this.repository = repository;
            this.tokenSource = new CancellationTokenSource();
        }

        public IDisposable Subscribe(IObserver<TResult> observer)
        {

            EventHandler run = (s, e) => this.Run(this.tokenSource, observer);

            this.repository.Refresh += run;
            this.Run(this.tokenSource, observer);

            return new CancelOnDispose { Source = this.tokenSource, Repository = this.repository, Handler = run };
        }

        private void Run(CancellationTokenSource cts, IObserver<TResult> observer)
        {
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
        }

        private class CancelOnDispose : IDisposable
        {
            internal CancellationTokenSource Source;
            internal IRepository Repository;
            internal EventHandler Handler;

            #region IDisposable Members

            void IDisposable.Dispose()
            {
                this.Source.Cancel();
                this.Repository.Refresh -= this.Handler;
            }

            #endregion
        }

    }

    internal sealed class TaskObservable
    {
        public static IObservable<T> Create<T>(Func<T> func, IRepository issueRepository)
        {
            return new TaskObservable<T>(func,issueRepository);
        }
    }
}