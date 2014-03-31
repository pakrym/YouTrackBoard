using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace YoutrackBoard
{
    class UserRepository
    {
        private Regex _avatarRegex = new Regex("(\\/_persistent\\/.*?)\"");
        private ConnectionFactory _connectionFactory;

        public UserRepository(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> GetAvatar(Person person)
        {
            return null;
            var clietn = _connectionFactory.CreateConnection();
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

    class ProjectRepository
    {
        private readonly ConnectionFactory _connectionFactory;

        public ProjectRepository(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<Project>> GetAll()
        {
            var connection = _connectionFactory.CreateConnection();
            var result = await connection.ExecuteTaskAsync<List<Project>>(new GetAllProjectsRequest(false));

            return result.Data;

        }

        public async Task<List<Sprint>> GetSprints(Project project)
        {
            var connection = _connectionFactory.CreateConnection();
            var result = await connection.ExecuteTaskAsync<ProjectSprints>(new GetProjectsSprintsRequest(project));

            return result.Data.Sprint;
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