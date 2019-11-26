# ShowMe_POOA

The project consists in creating an Android application that allows a user to manage the watching of his shows:

* The user can make a list of shows that he is watching.
Note : When adding a show to the list of watching shows the user has access to the summary of the show, the list of episodes, the actors present in the show. The user can also indicate the last episode he watched.

* The user can follow the last episode watched of each show and increment it easily

* The user can see the next television transmissions of his shows and be notified of these transmissions.
Note : The user can indicate his wish not to be notified.

* The user can also make a list of favorite shows and track his watching statistics

## Getting Started

 The instructions will get you a copy of the application on your Android device

 * Enable developer options and debugging on your Android device
 1. Open the Settings app
 2. Select System (on Android 8.0 or higher)
 3. Scroll to the bottom and select About phone
 4. Scroll to the bottom and tap Build number 7 times
 5. Return to the previous screen to find Developer options near the bottom

 * Build the application on your device
 1. Connect your mobile device to your computer via USB
 2. In the ```ShowMe``` folder : Open the ```ShowMe.sln``` file with Visual Studio
 3. Wait until visual studio recognizes your device (once it's good your device's name will be displayed on the top of Visual Studio Windows)
 4. Build the application on your device by clicking on the green arrow on the top of Visual Studio windows

 * Play with the application on your device

### Prerequisites

 Packages : 
 * FirebaseDatabase.net (4.0.1)
 * Newtonsoft.Json (12.0.2)
 * Rg.Plugings.Popup (1.2.0.223)
 * Xam.Plugins.Forms.ImageCircle (3.0.0.5)
 * Xam.Plugins.Notifier (3.0.0)
 * Xamarin.Auth (1.7.0)
 * Xamarin.Essentials (1.1.0)
 * Xamarin.FFImageLoading.Forms (2.4.11.982)
 * Xamarin.Froms (4.1.0.618606)

### Installing 

 Install and manage packages in Visual Studio using the NuGet Package Manager:

* Find and install a package
1. In Solution Explorer, right-click either References or a project and select ```Manage NuGet Packages...```
2. Search for a specific package using the search box on the upper left. Select a package from the list to display its information, which also enables the ```Install``` button along with a version-selection drop-down.
3. Select the desired version from the drop-down and select ```Install```. Visual Studio installs the packages and its dependencies into the project. You may be asked to accept license terms. 


## Playing with ShowMe Android application

### ShowMe application

* Authentification with a Google account

* ShowMe application is divided into four tab pages:
1. Home TV 
```
Home TV page is divided into two tab pages: 
a. To Watch - The user can follow the last episode watched of each show and increment it easily
b. Upcoming - The user can see the next television transmissions of his shows
```

2. Browse
```
Browse page is divided into two sections:
a. Search bar - To search for specific shows
b. Browsing section - To discover shows by swiping up
```

2.bis. Clicking on a show, the user accesses to the Show Details Page
```
The user can : 
a. Access the shows details (summary, cast, episodes list)
b. Add the show to his watch list
c. Indicate if he started to watch the show, and indicate the last episode watched
d. Indicate that the show is one of his favorites (by clicking on the heart button)
e. Indicate that he does not want to be notified of this show transmissions on TV (by click on the bell button)
f. Change the last episode watched
g. Delete the show from his watch list
```

3. Browse my shows
```
Browse my shows page is divided into three sections:
a. Selection bar - To filter the shows of the watching list by advancement criterias (Not started - In progress - Finished)
b. Favorites button - To`filter favorite shows
c. Browsing section - To see the status of each show of the watching list
```

4. Profile page
```
Profile page is divided into three sections:
a. User section - Display the user picture and name
b. User statistics - Display the time spent watching shows and the number of episodes watched
c. Log out button
```

### Running the tests

* Browse through the pages to verify initialization:
1. Home TV page : Verify that you get the message ```Click on the Discover tab to start adding shows to your list !``` on the To Watch tab, and ```No shows on your list is scheduled on TV, try adding another show!```on the upcoming tab.
2. Browse : Verify that you are able to discover more shows by swiping up (note: a loading image should appear if the API call is too long)
3. Browse my show : Empty
4. Profile page : Verify that your profile picture and user name are displayed and that your statistics are up to 0

* Add a show to your watch list:
1. Click on a show (for example ```Under the Dome```) 
1.a. Verify that you see the Summary, the Cast (with a list of actors - name and picture), the Episodes list (picture and title)
2. Click on the button ```Add to my shows```
2.a Click on ```Yes``` to indicate that you already started to watch the show
2.b Click on ```Choose season``` and choose a number (do the same to choose the episode)
2.c Click on ```Save```

* Verify that the show has been added to your Watch list:
1. Click on the ```Browse my show``` page 
2. Verify that the show is displayed

* Verify that you can increment the last episode seen:
1. Click on the ```Home TV``` page 
2. Verify that the show is displayed 
3. Click on the check-button
4. Verify that the check-button goes green
5. Verify that the last episode number has been incremented

* Verify that your statistics have been incremented:
1. Click on the ```Profile``` page 
2. Verify that your statistics have been incremented

* Add several shows to your watch list:
1. Add a show and indicate that it's one of your favorites by clicking on the heart button
2. Add a show and indicate that you did not start watching it
3. Add a show and indicate that you finished it (select the last episode of the last season)
4. Add a show that is scheduled on TV (examples : Big City Greens (on Saturdays), General Hospital (on weekdays), PBS NewsHour (on weekdays))

* Verify several properties:
1. Verify that you can filter by ```Favorites``` in the ```Browse my show``` page
2. Verify that you can filter by ```Progress status``` (All, Not started, In progress, Finished)
3. Verify that you get the ```UpComing``` on the ```Home TV``` page
4. Verify that you can delete a show, by going on the ```Browse my shows``` page, clicking on a show then clicking on the trash button
5. Verify that you can modify your ```Last episode watched```, by going on the ```Browse my shows``` page, clicking on a show then clicking on the pencil button

* Verify that you get the notifications:
1. Close the application on your device
2. Open the application (you should get the notification 24 hours in advance)

* Verify that you can log out:
1. Click on the ```Profile``` page 
2. Click on the ```Log out``` button

## Code Structure 

1. ShowMe :
* Models : The classes defining the objects we use in the application (show, episode etc.)
* Services : Additional services to access API, Firebase, to schedule notifications etc.
* ViewModels : The backend code corresponding to the views
* Views : The pages and frontend code
* Constants.cs : Environment constants for google login and firebase

2. ShowMe.Android : 
* Resources/drawable : all the images and icons used in the app 
* BackgroundReceiver.cs : service that wakes up the phone at given time-interval to schedule notifications
* CustomUrlSchemeInterceptorActivity.cs : activity redirecting the user after log in
* ToastMessage.cs : allows to send toast messages on Android
