namespace LtiProvider.Models
{
    public class RootObject
    {
        public string Context { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
        public PageOf PageOf { get; set; }
    }
}