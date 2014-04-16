using System.Collections.Generic;

namespace YoutrackBoard
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Reactive.Linq;
    using System.Threading.Tasks;

    using SharpSvn;
    using SharpSvn.Implementation;
    using SharpSvn.Security;

    class SvnCommitRepository : RepositoryBase
    {
        public SvnCommitRepository()
        {
            this.RepositoryUri = new Uri(ConfigurationManager.AppSettings["svn"]);
            this.Login = ConfigurationManager.AppSettings["svn.login"];
            this.Password = ConfigurationManager.AppSettings["svn.password"];
            this.Mappings = ConfigurationManager.AppSettings["svn.mapping"].Split(new [] {';'}, StringSplitOptions.RemoveEmptyEntries).Select(
                s =>
                    {
                        var parts = s.Split(new [] {'='}, StringSplitOptions.RemoveEmptyEntries);
                        return new { SvnName = parts[0], Name = parts[1] };
                    }).ToDictionary(m=>m.SvnName, m=>m.Name);
        }

        public Dictionary<string, string> Mappings { get; set; }

        internal string MapName(string name)
        {
            string newName;
            if (Mappings.TryGetValue(name, out newName))
            {
                return newName;
            };
            return name;
        }

        public string Login { get; set; }

        public string Password { get; set; }

        public Uri RepositoryUri { get; set; }


        protected SvnClient CreateClient()
        {
            var a = new SvnClient();
            a.Authentication.Clear(); 
            a.Authentication.DefaultCredentials = new System.Net.NetworkCredential(Login,Password);
            a.Authentication.SslServerTrustHandlers += delegate(object sender, SvnSslServerTrustEventArgs e)
            {
                e.AcceptedFailures = e.Failures;
                e.Save = true; // Save acceptance to authentication store
            };
            return a;
        }
        public IEnumerable<SvnCommit> ListCommits(TimeSpan span)
        {
            using (var a = this.CreateClient())
            {
                var results = new List<SvnCommit>();
                var arg = new SvnLogArgs()
                              {
                                  RetrieveAllProperties = true,
                                  RetrieveChangedPaths = true,
                                  Range = new SvnRevisionRange(new SvnRevision(DateTime.Now - span), new SvnRevision(DateTime.Now))
                              };
                
                a.Log(
                    this.RepositoryUri,
                        arg,
                        (sender, args) => results.Add(new SvnCommit()
                                                          {
                                                              Revision = args.Revision,
                                                              LogMessage = args.LogMessage.Replace("\r", "").Replace("\n", " "),
                                                              Author = MapName(args.Author),
                                                              Root = CommonPrefix(args.ChangedPaths.Select(p=>p.Path).ToArray()),
                                                              FilesChanged = args.ChangedPaths.Count
                                                          })


                    );

                return results;
            }
           
        }

        private static string CommonPrefix(string[] ss)
        {
            if (ss.Length == 0)
            {
                return "";
            }

            if (ss.Length == 1)
            {
                return ss[0];
            }

            int prefixLength = 0;

            foreach (char c in ss[0])
            {
                foreach (string s in ss)
                {
                    if (s.Length <= prefixLength || s[prefixLength] != c)
                    {
                        return ss[0].Substring(0, prefixLength);
                    }
                }
                prefixLength++;
            }

            return ss[0]; // all strings identical
        }

        public IObservable<IEnumerable<SvnCommit>> ListCommitsObservable(TimeSpan span)
        {
            return this.CacheCallObservable(() => this.ListCommits(span));
        }
    }

    internal class SvnCommit
    {
        public string LogMessage { get; set; }

        public long Revision { get; set; }

        public string Author { get; set; }

        public string Root { get; set; }

        public int FilesChanged { get; set; }
    }

    class IssueRepository: RepositoryBase
    {

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


        public IObservable<List<Issue>> SearchObservable(Project project, Sprint sprint)
        {
            return this.CacheCallObservable(() => this.Search(project, sprint));
        }


        public IObservable<List<WorkItem>> GetWorkItemsObservable(Issue issue)
        {
            return this.CacheCallObservable(() => this.GetWorkItems(issue));
        }
    }
}