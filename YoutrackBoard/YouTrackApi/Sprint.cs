namespace YoutrackBoard
{
    internal class Sprint
    {
        public string Name { get; set; }

        public string Start { get; set; }
        public string Finish { get; set; }

        public Sprint()
        {
        }

        public Sprint(string name)
        {
            Name = name;
        }
    }
}