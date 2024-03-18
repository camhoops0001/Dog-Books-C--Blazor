using Dog_Books_BackEnd.Models;
using System.Net;

namespace Dog_Books_BackEnd.Interfaces
{
    public interface IDogBookConroller
    {
        Task<List<string>> GetDogBreeds(string apiKey);
        Task<string> GetDogPictureURLByBreed(string breed, string apiKey);
        Task<string> GetRandomDogPicture(string apiKey);
        Task<DogAndBookResponse> GetBookAndDogPictureByBreed(string breed, string apiKey);
        Task<List<DogAndBookResponse>> GetSavedBooks(string apiKey);
        Task<HttpStatusCode> SaveBookInformation(string apiKey);
    }
}
