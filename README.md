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
