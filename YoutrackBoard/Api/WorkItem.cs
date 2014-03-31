using System;

namespace YoutrackBoard
{
    internal class WorkItem
    {
        public string Id { get; set; }
        public long Date { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }

        public WorkItemAuthor Author { get; set; }

    }
}