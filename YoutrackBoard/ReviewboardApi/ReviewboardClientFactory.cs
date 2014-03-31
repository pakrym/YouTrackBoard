using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoutrackBoard.ReviewboardApi
{
    using System;
    using System.Configuration;
    using System.Linq.Expressions;
    using System.Net;
    using System.Runtime.Caching;

    using Castle.DynamicProxy;

    using RestSharp;

    class CacheInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var key = GenerateKey(invocation);
            var cachedValue = MemoryCache.Default[key];

            if (cachedValue == null)
            {     
                invocation.Proceed();
                var result = invocation.ReturnValue;
                var task = result as Task;
                if (task != null)
                {
                    task.ContinueWith(
                        (task1, o) =>
                            {
                                dynamic t = task1;
                                MemoryCache.Default[(string)o] = t.Result;
                            }, key);
                }
                else
                {
                    MemoryCache.Default[key] = invocation.ReturnValue;
                }
            }
            else
            {

                if (typeof(Task).IsAssignableFrom(invocation.Method.ReturnType))
                {
                    invocation.ReturnValue = Activator.CreateInstance(
                        typeof(Task<>).MakeGenericType(cachedValue.GetType()),
                        Expression.Lambda(typeof(Func<>).MakeGenericType(cachedValue.GetType()), Expression.Constant(cachedValue)).Compile());
                }
                else
                {
                    invocation.ReturnValue = cachedValue;
                }
            }
        }

        private string GenerateKey(IInvocation invocation)
        {
            return invocation.Method.DeclaringType.Name + invocation.Method.Name + string.Join(",", invocation.Arguments.Select(Convert.ToString));
        }
    }

    class ReviewRepository
    {
        private readonly ReviewboardClientFactory _reviewboardClientFactory;

        public ReviewRepository(ReviewboardClientFactory reviewboardClientFactory)
        {
            this._reviewboardClientFactory = reviewboardClientFactory;
        }

        public async Task<List<ReviewRequest>> GetReviewsTo(string user)
        {
            var client = _reviewboardClientFactory.CreateConnection();
            var result = await client.ExecuteTaskAsync<ReviewRequestResponse>(new GetReviewRequests(null, user));
            return result.Data.ReviewRequests;
        }

        public async Task<List<ReviewRequest>> GetReviewsFrom(string user)
        {
            var client = _reviewboardClientFactory.CreateConnection();
            var result = await client.ExecuteTaskAsync<ReviewRequestResponse>(new GetReviewRequests(user, null));
            return result.Data.ReviewRequests;
        }
    }

    class ReviewboardClientFactory
    {
        private RestClient _client;
        private bool _hasAuth = false;

        public ReviewboardClientFactory()
        {
            _client = new RestClient(ConfigurationManager.AppSettings["reviewboard.url"]);
            _client.CookieContainer = new CookieContainer();
        }

        public IRestClient CreateConnection()
        {
            if (!_hasAuth)
            {
                Authentificate();
            }
            return _client;
        }

        private void Authentificate()
        {
            var result = _client.Execute(new ReviewboardAuthentificateRequest(ConfigurationManager.AppSettings["reviewboard.user"], ConfigurationManager.AppSettings["reviewboard.password"]));

            _hasAuth = result.StatusCode == HttpStatusCode.OK;
        }
    }
}
