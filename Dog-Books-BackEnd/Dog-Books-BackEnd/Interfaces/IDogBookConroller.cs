using Dog_Books_BackEnd.Models;

namespace Dog_Books_BackEnd.Interfaces
{
    public interface IDogBookConroller
    {
        Task<List<string>> GetDogBreeds(string apiKey);
        Task<string> GetDogPictureURLByBreed(string breed, string apiKey);

        Task<string> GetRandomDogPicture(string apiKey);
        Task<string> SearchBooksByDog(string breed, string apiKey);

        Task<OpenLibraryBookResponse> GetAuthorandTitlebyID(string bookId, string apiKey);

        Task<OpenLibraryAuthor> GetAuthorbyAuthorID(string title, string apiKey);

    }
}
