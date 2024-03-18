using Dog_Books_BackEnd.Interfaces;
using Dog_Books_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;

namespace Dog_Books_BackEnd.Controllers
{
    public class DogBookController : Controller, IDogBookConroller
    {
        public readonly Settings _settings;
        private readonly IConfiguration _configuration;

        public DogBookController(IOptions<Settings> options, IConfiguration configuration)
        {
            _settings = options.Value;
            _configuration = configuration;
        }

        #region Dog API Endpoint Calls

        [HttpGet]
        [Route("api/[controller]/[action]")]
        /// <summary>
        /// Populates the list of breeds for the user to select.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <exception cref="Exception"></exception>
        public async Task<List<string>> GetDogBreeds(string apiKey)
        {
            if (apiKey == _settings.ApiKey)
            {
                List<string> allBreeds = await GetDogBreedsAsync();
                return allBreeds;
            }
            else
            {
                throw new Exception("Invalid API Key");
            }
        }

        [HttpGet]
        [Route("api/[controller]/[action]")]
        /// <summary>
        /// This endpoint is used by itself, and within the joint call with the OpenLibraryAPI.
        /// </summary>
        /// <param name="breed"></param>
        /// <param name="apiKey"></param>
        /// <exception cref="Exception"></exception>
        public async Task<string> GetDogPictureURLByBreed(string breed, string apiKey)
        {
            if (apiKey == _settings.ApiKey)
            {
                string dogPictureUrl = await GetDogPictureURLByBreedAsync(breed);
                return dogPictureUrl;
            }
            else
            {
                throw new Exception("Invalid API Key");
            }
        }

        [HttpGet]
        [Route("api/[controller]/[action]")]
        /// <summary>
        /// Random Dog Picture for the "Saved Books" page if they have no books saved.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <exception cref="Exception"></exception>
        public async Task<string> GetRandomDogPicture(string apiKey)
        {
            if (apiKey == _settings.ApiKey)
            {
                string dogPictureUrl = await GetRandomDogPictureAsync();
                return dogPictureUrl;
            }
            else
            {
                throw new Exception("Invalid API Key");
            }
        }

        #endregion


        #region Book API Endpoint Calls (Not currently using)
        //These are unecessary to include as seperate accessible calls. 
        //This is just what I originally created to debug/test through swagger. 
        //Then I created the 'GET' endpoint utilizing both API's that the front end can call

        //[HttpGet]
        //[Route("api/[controller]/[action]")]
        //public async Task<string> SearchBooksByDog(string breed, string apiKey)
        //{
        //    if (apiKey == _settings.ApiKey)
        //    {
        //        string bookId = await SearchBooksByDogAsync(breed);
        //        return bookId;
        //    }
        //    else
        //    {
        //        throw new Exception("Invalid API Key");
        //    }
        //}

        //[HttpGet]
        //[Route("api/[controller]/[action]")]
        //public async Task<ParsedBookResponse> GetAuthorKeyandTitlebyID(string bookId, string apiKey)
        //{
        //    if (apiKey == _settings.ApiKey)
        //    {
        //        return await GetAuthorKeyandTitlebyIDAsync(bookId);
        //    }
        //    else
        //    {
        //        throw new Exception("Invalid API Key");
        //    }
        //}

        //[HttpGet]
        //[Route("api/[controller]/[action]")]
        //public async Task<string> GetAuthorbyAuthorID(string authorId, string apiKey)
        //{
        //    if (apiKey == _settings.ApiKey)
        //    {
        //        return await GetAuthorbyAuthorIDAsync(authorId);
        //    }
        //    else
        //    {
        //        throw new Exception("Invalid API Key");
        //    }
        //}


        #endregion


        #region Joint API Calls

        [HttpGet]
        [Route("api/[controller]/[action]")]
        /// <summary>
        /// This includes 4 total calls to the backend API's.
        /// Three calls to the OpenLibrary API: SearchBooksByDog, GetAuthorandTitlebyID, and GetAuthorbyAuthorID
        /// One call to the Dog API: GetDogPictureByBreed
        /// 
        /// This returns a formatted version of all of the book/picture information so the front end can simply call, and display the info.
        /// </summary>
        /// <param name="breed"></param>
        /// <param name="apiKey"></param>
        /// <exception cref="Exception"></exception>
        public async Task<DogAndBookResponse> GetBookAndDogPictureByBreed(string breed, string apiKey)
        {
            if (apiKey == _settings.ApiKey)
            {
                DogAndBookResponse dogAndBookResponse = await GetBookAndDogPictureByBreedAsync(breed);
                //Adding my artificial delay here.
                //Want to make it seem complex operations are happening to make sure the user knows we're handpicking solid selections for the breed they selected
                //Loading spinner and picture of their selected breeds dog will be displayed in the meantime
                Thread.Sleep(TimeSpan.FromSeconds(3));
                return dogAndBookResponse;
            }
            else
            {
                throw new Exception("Invalid API Key");
            }
        }

        [HttpGet]
        [Route("api/[controller]/[action]")]
        /// <summary>
        /// This saves the specific book information we retrieved for the user
        /// </summary>
        /// <exception cref="Exception"></exception>
        public async Task<List<DogAndBookResponse>> GetSavedBooks(string apiKey)
        {
            if (apiKey == _settings.ApiKey)
            {
                string connString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("spGetSavedBooks", connection);
                    command.CommandType = CommandType.StoredProcedure;


                    var reader = command.ExecuteReader();
                    var dogAndBookList = new List<DogAndBookResponse>();

                    // Map data from reader to DogAndBookResponse objects
                    while (reader.Read())
                    {
                        dogAndBookList.Add(new DogAndBookResponse
                        {
                            Title = reader.GetString(1),   // Assuming Title is the second column (index 1)
                            Author = reader.GetString(2),  // Assuming Author is the third column (index 2)
                            DogPictureUrl = reader.GetString(3), // Assuming DogPictureUrl is the fourth column (index 3)
                        });
                    }
                    return dogAndBookList;
                }
            }
            else
            {
                throw new Exception("Invalid API Key");
            }


        }

        [HttpPost]
        [Route("api/[controller]/[action]")]
        /// <summary>
        /// This saves the specific book information we retrieved for the user
        /// </summary>
        /// <exception cref="Exception"></exception>
        public async Task<HttpStatusCode> SaveBookInformation(string apiKey)
        {
            if (apiKey == _settings.ApiKey)
            {
                string rawContent = string.Empty;
                using (var reader = new StreamReader(Request.Body,
                              encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false))
                {
                    rawContent = await reader.ReadToEndAsync();
                }
                DogAndBookResponse dogAndBookResponse = JsonConvert.DeserializeObject<DogAndBookResponse>(rawContent);


                string connString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("spSaveBook", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Title", dogAndBookResponse.Title);
                    command.Parameters.AddWithValue("@Author", dogAndBookResponse.Author);
                    command.Parameters.AddWithValue("@DogPictureUrl", dogAndBookResponse.DogPictureUrl);

                    var response = command.ExecuteNonQuery();
                    if (response == 1)
                    {
                        return HttpStatusCode.Created;
                    }
                    else
                    {
                        return HttpStatusCode.InternalServerError;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid API Key");
            };

        }

        #endregion


        #region Dog API Method Logic
        public async Task<List<string>> GetDogBreedsAsync()
        {
            string retPayLoad;
            string url = _settings.DogApiBaseUrl + "/breeds/list/all";
            RestClient client = new RestClient(url);
            RestRequest request = new RestRequest();
            client.AddDefaultHeader("Content-Type", "application/json");

            try
            {
                RestResponse response = await client.GetAsync(request);
                retPayLoad = response.Content;
                DogBreedApiResponse dogBreedReturn = JsonConvert.DeserializeObject<DogBreedApiResponse>(retPayLoad);
                //Filter down the return so it just returns a simple list of all the breeds
                List<string> allBreeds = dogBreedReturn.Message?.Keys.ToList();
                return allBreeds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<string> GetDogPictureURLByBreedAsync(string breed)
        {
            string retPayLoad;
            string url = _settings.DogApiBaseUrl + $"/breed/{breed}/images/random";
            RestClient client = new RestClient(url);
            RestRequest request = new RestRequest();
            client.AddDefaultHeader("Content-Type", "application/json");

            try
            {
                RestResponse response = await client.GetAsync(request);
                retPayLoad = response.Content;
                DogImageApiResponse dogBreedImageReturn = JsonConvert.DeserializeObject<DogImageApiResponse>(retPayLoad);
                //Just return the necessary image URL
                string breedImageUrl = dogBreedImageReturn.Message;
                return breedImageUrl;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<string> GetRandomDogPictureAsync()
        {
            string retPayLoad;
            string url = _settings.DogApiBaseUrl + "/breeds/image/random";
            RestClient client = new RestClient(url);
            RestRequest request = new RestRequest(url);
            client.AddDefaultHeader("Content-Type", "application/json");

            try
            {
                RestResponse response = await client.GetAsync(request);
                retPayLoad = response.Content;
                DogImageApiResponse dogBreedImageReturn = JsonConvert.DeserializeObject<DogImageApiResponse>(retPayLoad);
                //Just return the necessary image URL
                string breedImageUrl = dogBreedImageReturn.Message;
                return breedImageUrl;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        #endregion


        #region Book API Method Logic

        public async Task<string> SearchBooksByDogAsync(string breed)
        {
            string retPayLoad;
            string url = _settings.BookApiBaseUrl + $"/search.json?q={breed}";
            RestClient client = new RestClient(url);
            RestRequest request = new RestRequest();
            client.AddDefaultHeader("Content-Type", "application/json");

            //Extra logic needed to just grab the list of seeds. That's all we need
            List<string> FormattedbookIds = new List<string>();

            try
            {
                RestResponse response = await client.GetAsync(request);
                retPayLoad = response.Content;

                FormattedbookIds = FormatBookIds(ExtractAndFormatSeeds(retPayLoad));

                //Due to time constraints and complexity, I'm choosing to return one random book
                //I still set the logic up in a way where this would be easily scalable to display multiple books
                //to choose from if desired
                Random random = new Random();

                // Get a random index within the list bounds
                int randomIndex = random.Next(FormattedbookIds.Count);

                // Return the single book from the list of ID's
                return FormattedbookIds[randomIndex];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<ParsedBookResponse> GetAuthorKeyandTitlebyIDAsync(string bookId)
        {
            string retPayLoad;
            string url = _settings.BookApiBaseUrl + $"/books/{bookId}.json";
            RestClient client = new RestClient(url);
            RestRequest request = new RestRequest();
            client.AddDefaultHeader("Content-Type", "application/json");

            //Extra logic needed to just grab the list of seeds. That's all we need
            List<string> FormattedbookIds = new List<string>();

            try
            {
                RestResponse response = await client.GetAsync(request);
                retPayLoad = response.Content;

                OpenLibraryBookResponse openLibraryBookResponse = JsonConvert.DeserializeObject<OpenLibraryBookResponse>(retPayLoad);

                ParsedBookResponse parsedBookResponse = new ParsedBookResponse()
                {
                    Title = openLibraryBookResponse.Title,
                    FullTitle = openLibraryBookResponse.FullTitle,
                    AuthorKey = openLibraryBookResponse.Authors[0].Key
                };

                return parsedBookResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<string> GetAuthorbyAuthorKeyAsync(string authorId)
        {
            string retPayLoad;
            string url = _settings.BookApiBaseUrl + $"{authorId}.json";
            RestClient client = new RestClient(url);
            RestRequest request = new RestRequest();
            client.AddDefaultHeader("Content-Type", "application/json");

            try
            {
                RestResponse response = await client.GetAsync(request);
                retPayLoad = response.Content;

                OpenLibraryAuthor openLibraryAuthorResponse = JsonConvert.DeserializeObject<OpenLibraryAuthor>(retPayLoad);

                string author = openLibraryAuthorResponse.Name;

                return author;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        #endregion


        #region Joint API Method Logic

        public async Task<DogAndBookResponse> GetBookAndDogPictureByBreedAsync(string breed)
        {
            DogAndBookResponse dogAndBookResponse = new DogAndBookResponse();
            //Calling the 3 OpenLibrary endpoints required to return all of the relevant book information
            string bookId = await SearchBooksByDogAsync(breed);
            ParsedBookResponse parsedBookResponse = await GetAuthorKeyandTitlebyIDAsync(bookId);
            string author = await GetAuthorbyAuthorKeyAsync(parsedBookResponse.AuthorKey);
            //Normally would add some more if blocks/checks to make sure we're populating correct data, but for now we're assuming
            //these have populated correctly with minimal error handling
            dogAndBookResponse.Author = author;

            //Some of these books do not have a Full Title, default to Title if null
            if (parsedBookResponse.FullTitle != null)
            {
                dogAndBookResponse.Title = parsedBookResponse.FullTitle;
            }
            else
            {
                dogAndBookResponse.Title = parsedBookResponse.Title;
            }

            //Calling the Dog API Endpoint to return a picture with the book
            string dogPictureUrl = await GetDogPictureURLByBreedAsync(breed);
            dogAndBookResponse.DogPictureUrl = dogPictureUrl;


            return dogAndBookResponse;
        }
        #endregion


        #region General Methods

        //The books API is much less straightforward to work with then the DOG API. Alot of nested data, and sometimes
        //we only need a partial string of what they provide as an ID.
        public List<string> ExtractAndFormatSeeds(string json)
        {
            var seeds = new List<string>();

            // Deserialize JSON into JObject
            dynamic jObject = JsonConvert.DeserializeObject(json);

            // Access "docs" property as JArray
            var docs = jObject["docs"] as JArray;

            if (docs != null)
            {
                // Loop through each item in "docs" as JObject
                foreach (var item in docs)
                {
                    // Access "seed" property as JArray
                    var seedArray = item["seed"] as JArray;

                    if (seedArray != null)
                    {
                        // Use Select with ToList to convert JArray to List<string>
                        seeds.AddRange(seedArray.Select(s => (string)s).ToList());
                    }
                }
            }

            return seeds;
        }

        public List<string> FormatBookIds(List<string> unformattedBookIds)
        {
            List<string> formattedBookIds = new List<string>();
            foreach (string seed in unformattedBookIds)
            {
                //Certain breeds return quite a few seeds that are not books, and that would break our application.
                //This effectively filters it to only return data that we need, and returns the relevant ID.
                if (seed.StartsWith("/books/"))
                {
                    formattedBookIds.Add(seed.Substring(7));
                }
            }

            return formattedBookIds;
        }

        #endregion


    }
}
