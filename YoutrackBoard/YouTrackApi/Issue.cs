using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace YoutrackBoard
{
    using System;

    internal class Issue
    {
        public string Id { get; set; }

        public List<IssueField> Field { get; set; }

        public Assignee Assignee
        {
            get { return GetFieldAsFirst<Assignee>("Assignee"); }
        }

        public string Summary
        {
            get { return GetField("summary"); }
        }

        public DateTime Resolved
        {
            get { return this.GetFieldAs<long>("resolved").ToDateTime(); }
        }
        public DateTime Created
        {
            get { return GetFieldAs<long>("created").ToDateTime(); }
        }
        public DateTime Updated
        {
            get { return GetFieldAs<long>("updated").ToDateTime();; }
        }

        public TimeSpan Estimation
        {
            get { return TimeSpan.FromMinutes(GetFieldAsFirst<int>("Estimation")); }
        }

        public string State
        {
            get
            {
                return this.GetFieldAsFirst<string>("State");
            } 
        }

        public TimeSpan TimeSpent
        {
            get { return TimeSpan.FromMinutes(GetFieldAsFirst<int>("Spent Time")); }
        }

        private T GetFieldAs<T>(string name) 
        {
            var value = GetField(name);
            if (string.IsNullOrWhiteSpace(value)) return default(T);

            return JsonConvert.DeserializeObject<T>(value);
        }
        private T GetFieldAsFirst<T>(string name)
        {
            var dv = GetFieldAs<IEnumerable<T>>(name);
            if (dv == null) return default(T);
            return dv.FirstOrDefault();
        }

        private string GetField(string name)
        {
            var filed = Field.FirstOrDefault(f => f.Name == name);
            return filed == null? string.Empty: filed.Value;
        }

    }

    internal class Assignee
    {
        public string FullName { get; set; }
        [JsonProperty("value")]
        public string Login { get; set; }
    }
}