namespace YoutrackBoard
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;

    internal class RepositoryBase: IRepository
    {

        public void RefreshData()
        {
            this.ClearCache();
            if (this.Refresh != null)
            {
                this.Refresh(this, EventArgs.Empty);
            }
        }

        public event EventHandler Refresh;

        private ReaderWriterLockSlim cacheLockSlim = new ReaderWriterLockSlim();
        private Dictionary<string, object> _value = new Dictionary<string, object>();

        protected void ClearCache()
        {
            this._value.Clear();
        }

        public Func<T> CacheCall<T>(Expression<Func<T>> expression)
        {
            var keyParameters = MethodParameterExtractor.GetObjects(expression);
            var key = MethodParameterExtractor.ToKey(keyParameters);
            var lambda = expression.Compile();

            return () =>
                {
                    object v;
                    try
                    {
                        cacheLockSlim.EnterUpgradeableReadLock();
                        _value.TryGetValue(key, out v);
                        if (v == null)
                        {
                            try
                            {
                                cacheLockSlim.EnterWriteLock();
                                if (_value.ContainsKey(key))
                                {
                                    v = _value[key];
                                }
                                else
                                {
                                    v = lambda();    
                                }
                                
                                _value.Add(key, v);
                                
                            }
                            finally
                            {
                                cacheLockSlim.ExitWriteLock();
                            }
                        }
                    }
                    finally
                    {
                        cacheLockSlim.ExitUpgradeableReadLock();    
                    }
                    return (T)v;
                };
        }

        public IObservable<T> CacheCallObservable<T>(Expression<Func<T>> expression)
        {
            return TaskObservable.Create(this.CacheCall(expression),this);
        }

    }
}