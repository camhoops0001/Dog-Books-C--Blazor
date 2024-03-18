## Project setup
This is setup in Microsoft SQL Server Management Studio 18
Run the two SQL files within the main folder
![image](https://github.com/camhoops0001/Dog-Books-C--Blazor/assets/95889699/0876c39f-6690-450f-9996-ed0281aaa13f)
```
step1_fresh_DogBooks-db
```
And
```
step2_add-StoredProc
```
##Running the Project
You will need to build and run the two .sln files.
```
Dog-Books-Backend.sln
```
And
```
Dog-Books-Frontend.sln
```
This will run the backend API and the front end Blazor application

##Things to consider
My local instance of SSMS is setup for (LocalDb)\MSSQLLocalDB
![image](https://github.com/camhoops0001/Dog-Books-C--Blazor/assets/95889699/b1d00cc8-e3f6-4dc9-95c6-b80bd28dcb06)

If you connect like this instead:
![image](https://github.com/camhoops0001/Dog-Books-C--Blazor/assets/95889699/3721a41f-fb2a-4f9e-bbb6-b344180fc390)

You will need to change your DefaultConnection within appsettings.json to Server=.\\SQLEXPRESS
![image](https://github.com/camhoops0001/Dog-Books-C--Blazor/assets/95889699/66d50c82-ca64-41b3-9483-b48d420fd0a7)


# This is a WebApi meant to utilize two separate backend API's.

Dog API  

https://dog.ceo/dog-api/  

Utilizing Dog API to populate a breed list, grab relevant pictures of dogs from user breed selection, and random image generation as well

-Breed list  

-Images by breed  

-Random dog image generator  


OpenLibrary  

https://openlibrary.org/developers/api  

Utilizing OpenLibraries API to gather information about books that relate to different breed types.  

-Book Search  

-Search Inside Text Search  

-Author Search  

Packages Used:
-NewtonSoft.Json
-RestSharp
-System.Text.Json
-System.Data.SqlClient
-Swashbuckle.AspNetCore (default)

You pick your favorite dog breed, and the application will give you a book that includes the specific breed you selected. It's built to eventually be able to let the user choose between "X" amount of books that we display. 

All of the breeds listed have a direct match of some sort with mentions in a book. 
(Right now Beagles, and one or two others are still erroring when trying to retrieve a book to save, something to look into if taking this further. The structure of a couple of the breeds returns must be slightly different then the rest)

It is currently hooked up to give you the ability to select a breed and return a single book that you have the option to save to your collection.

A few of the breeds have 100+ results, so if I wanted to spend the time on it later, we could implement a paginator and display all of the books related specific breeds!

We could specify searching by author, by the breed being mentioned somewhere in the test of the book, in the title... etc.. 

A lot of cool possibilities here.  

##Things I would update
- I have very little error management at the moment, I would love to add more exception handling on both ends of the project backend/front end.
- If this app went live it would be be very beneficial to utilize log4net or NLog for logging as well.
- Would move all of these services to Azure.. The Front End, Back End, and the database.
- Would secure the Api Key and any other sensitive information in the Azure Key Vault.
- First time ever using Blazor so I would get more familiar with it's strengths and weaknesses and refactor the front end to better fit best practices in the framework
- Didn't spend much time on the UI.. I would populate the dog breeds you can select into an actual dropdown list, or seperating them so you could see all of the breed options in 3 columns as opposed to listing them single file.
- Would add ability for people to pick from 5-10 books instead of the one random one. Set it up so that would be an easy addition later down the line
