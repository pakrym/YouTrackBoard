using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace YoutrackBoard
{
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

        public int Estimation
        {
            get { return GetFieldAsFirst<int>("Estimation"); }
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

        private int? GetFieldAsInt(string name)
        {
            var value = GetField(name);
            if (string.IsNullOrWhiteSpace(value)) return null;

            else return int.Parse(value.Trim('[', '"',']'));
        }
    }

    internal class Assignee
    {
        public string FullName { get; set; }
        [JsonProperty("value")]
        public string Login { get; set; }
    }
}