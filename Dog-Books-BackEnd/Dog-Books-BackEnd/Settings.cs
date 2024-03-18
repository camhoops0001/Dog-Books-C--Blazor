namespace Dog_Books_BackEnd
{
    public class Settings
    {
        //I like setting up this type of class for scalability and ease of access. Not super beneficial with three variables but it's easily scalable
        public string ApiKey { get; set; }
        public string DogApiBaseUrl { get; set; }
        public string BookApiBaseUrl { get; set; }

    }
}
