using System.Text.Json;
namespace Dog_Books_BackEnd.Models
{
    public class DogBreedApiResponse
    {
        public Dictionary<string, string[]> Message { get; set; }
    };

    public class DogImageApiResponse
    {
        public string Message { get; set; }
        public string Status { get; set; }
    }
}