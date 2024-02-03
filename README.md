MusicApp - API and Client 

_________________________________



About this project :

This project is a simple Music Application that includes building an API, using an external Api, and a console client. 




  

*** Web API Application ***

  

 #Features 

-Store users, genres, songs and artists. 

-Connect users to genres, artists, and songs. 

-Testable application using IoC. 

  

  

#REST API 


.Endpoints: 

- GET /users: Retrieve all users in the system. 

- GET /genre/{userId}: Retrieve genres linked to a specific user. 

- GET /artist/{userId}: Retrieve artists linked to a specific user. 

- GET /song/{userId}: Retrieve songs linked to a specific user. 

- POST/newUser: Add users 

- POST /user/{userId}/genre/{genreId}: Connect a user to a genre. 

- POST /user/{userId}/artist/{artistId}: Connect a user to an artist. 

- POST /users/{userId}/song/{songId}: Connect a user to a song. 

- POST /song: Add song. 

  

. External API Integration 


- Utilizes an external API (https://www.theaudiodb.com/api_guide.php) with (https://www.theaudiodb.com/api/v1/json/2/discography.php?s=coldplay) to fetch additional data, such as Artist with album names and year. 

- GET /album/{artist}: Get recommended artists. 

 


  
*** Console Client ***

  

#Features 

  

-Console-based user interface. 

-Option to choose or create a user at the start. 

-Options to list and add genres, artists, and songs. 

-Options for API endpoints, including recommendations. 

  

*** Getting Started ***

  

1. Install .NET Core:  

Ensure that .NET Core 6 is installed on your machine. You can download it (https://dotnet.microsoft.com/en-us/download) . 

  

2. Clone the Project: 

 Clone this project to your local machine. 

(bash) 

  git clone <https://github.com/xammax1337/MusicProjekt.git> 

3-Build and Run  

(bash) 

.dotnet build 

.dotnet run 

------------------------------------------------------------------------------------------------------------------------------------------------------------------------  

Documentation of calls to API 


1. http://localhost:XXXX/newUser 

This POST call will create a new user, by posting a new username in the JSON text field. Exception messages will occur, if no or an already existing username is chosen. 

  

2. http://localhost:XXXX/user 

This GET call will list all existing users, by userId and username, in the database. 

  

3. http://localhost:XXXX/artist/{userId} 

This GET call will find the chosen user's, by user ID, connected artists. If a non-existing user is typed in, an exception message will display this. 

  

4. http://localhost:XXXX/user/{userId}/artist/{artistId} 

This POST call will connect the chosen user, by user ID, with the chosen artist, by artist ID. If a non-existing user ID or artist ID is typed in, exception messages will occur. 

  

5. http://localhost:XXXX/genre/{userId} 

This GET call will list the chosen user's connected genres, by user ID. If a non-existing user ID is given, an exception will occur. 

  

6. http://localhost:XXXX/user/{userId}/genre/{genreId} 

This POST call will connect the chosen user, by user ID,  to the chosen genre, by genre ID. If a non-existing user or genre is chosen, exception messages will occur. 



7. http://localhost:XXXX/song/{userId} 

This GET call will list the chosen user's connected songs. If a non-existing user ID is chosen, an exception message will occur. 



8. http://localhost:XXXX/user/{userId}/song/{songId} 

This POST call will connect the user, by user ID, to a song, by song ID. If a non-existing user or song is chosen, exception messages for this will be displayed. 


  
9. http://localhost:XXXX/song 

This POST call will create a new song, if the user types in the fields for title, artist ID and genre ID in the JSON text field. If the title field is empty, or if the chosen artist ID or genre ID is non-existing, exception messages will occur. 

  

10. http://localhost:XXXX/album/{artist} 

This GET call will list the chosen artist's albums. 

 



