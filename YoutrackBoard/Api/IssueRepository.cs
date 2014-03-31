﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace YoutrackBoard
{
    class IssueRepository

    {
        private readonly ConnectionFactory _connectionFactory;

        public IssueRepository(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<Issue>> Search(Project project, Sprint sprint)
        {
            var connection = _connectionFactory.CreateConnection();
            var result = await connection.ExecuteTaskAsync<IssueResponce>(
                new SearchIssueRequest(string.Format("#{0} Fix versions: {1}", project.Name, sprint.Name)));

            return result.Data.Issue;      
        }

        public async Task<List<WorkItem>> GetWorkItems(Issue issue)
        {
           var connection = _connectionFactory.CreateConnection();
           var result = await connection.ExecuteTaskAsync < List<WorkItem>>(new GetIssueWorkItemsRequest(issue));

            return result.Data;      
        }
    }

}