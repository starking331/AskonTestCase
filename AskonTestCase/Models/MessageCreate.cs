namespace AskonTestCase.Models
{
    public class MessageCreate
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<int> Recipients { get; set; }
    }
}
