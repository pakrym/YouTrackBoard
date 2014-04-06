using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace YoutrackBoard
{
    using System;

    class UserRepository
    {
        private Regex _avatarRegex = new Regex("(\\/_persistent\\/.*?)\"");
        private YouTrackClientFactory youTrackClientFactory;

        public UserRepository(YouTrackClientFactory youTrackClientFactory)
        {
            this.youTrackClientFactory = youTrackClientFactory;
        }

        public async Task<string> GetAvatar(Person person)
        {
            return null;
            var clietn = this.youTrackClientFactory.CreateConnection();
            var result = await clietn.ExecuteTaskAsync(new GetAvatarRequest(person));
            var match = _avatarRegex.Match(result.Content);
            if (match.Success)
            {
                return clietn.BaseUrl + match.Groups[1].Value;
            }
            return null;
        }


    }

    public static class RestClientExtensions
    {
        internal static Task<T> ExecuteAsyncTask<T>(this IRestClient client, IRestRequest request) where T : new()
        {
            var tcs = new TaskCompletionSource<T>();
            var loginResponse = client.ExecuteAsync<T>(request, r =>
            {
                if (r.ErrorException == null)
                {
                    tcs.SetResult(r.Data);
                }
                else
                {
                    tcs.SetException(r.ErrorException);
                }
            });
            return tcs.Task;
        }
    }

    class ProjectRepository : IRepository
    {
        private readonly YouTrackClientFactory youTrackClientFactory;

        public ProjectRepository(YouTrackClientFactory youTrackClientFactory)
        {
            this.youTrackClientFactory = youTrackClientFactory;
        }

        public List<Project> GetAll()
        {
            var connection = this.youTrackClientFactory.CreateConnection();
            var result = connection.Execute<List<Project>>(new GetAllProjectsRequest(false));
            return result.Data;
        }

        public IObservable<List<Project>> GetAllObservable()
        {
            return TaskObservable.Create(this.GetAll, this);
        }

        public List<Sprint> GetSprints(Project project)
        {
            var connection = this.youTrackClientFactory.CreateConnection();
            var result = connection.Execute<ProjectSprints>(new GetProjectsSprintsRequest(project));

            return result.Data.Sprint;
        }

        public IObservable<List<Sprint>> GetSprintsObservable(Project project)
        {
            return TaskObservable.Create(()=>GetSprints(project), this);
        }


        public event EventHandler Refresh;

        public void RefreshData()
        {
            if (this.Refresh != null)
            {
                this.Refresh(this, EventArgs.Empty);
            }
        }
    }


    internal class GetProjectsSprintsRequest : RestRequest
    {
        public GetProjectsSprintsRequest(Project project)
            : base("/rest/agile/{project}/sprints")
        {
            AddUrlSegment("project", project.Name);
        }
    }
}