using System.Diagnostics;

namespace YoutrackBoard
{
    [DebuggerDisplay("{Name}: {Value}")]
    internal class IssueField
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}