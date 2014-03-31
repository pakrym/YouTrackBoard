namespace YoutrackBoard.ReviewboardApi
{
    internal class ReviewRequest
    {
        [RestSharp.Deserializers.DeserializeAs(Name = "absolute_url")]
        public string AbsoluteUrl { get; set; }

        public string Description { get; set; }
    }
}