namespace YoutrackBoard
{
    internal class Subvalue
    {
        public string Value { get; set; }

        public static implicit operator string(Subvalue v)
        {
            return v.Value;
        }
    }
}