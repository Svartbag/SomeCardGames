# Card Games application
Play various card games. For now only "War" game is available and can be selected. Have fun :)

## How to start the game: 
1. Open Visual Studio
2. Set CardGames.UI as startup project
3. Start program (Debug -> Start Debugging)

##  Game play
The 2 players have a pile of cards each. Each player pick the card on top of the pile and place it on the table (done by clicking on the card pile's with left mouse click). The 2 cards are then compared. The player with the highest card wins both of the cards. Cards are collected by clicking on the "Collect cards" button which appears. If the cards are similar (As default similar = +/- 1 in difference) there is "War". Now both players pick the 4 next cards from their card pile and place them on the table (done by clicking on the card piles). The cards to the very right on the table are now compared. The player who has the higest of these cards now wins all the cards on the table. In this way the game will continue until a player has won all the cards. The amount of cards needed in order to win can be adjusted in a config file in the code. 

## Some descriptions and thoughts about the game
This game has been made primarily to train my WPF skils. Focus has been on the UI part and how to create an MVVM pattern in a good way. Focus has not been on making a fully functional and robust game. For now start/stop game and game play works -for the "War" game. Users are predefined but can be changed via config file. Also possible to adjust some of the game play parameters as well in the config file. Not possible to save or open game for now.
- Game is made as a WPF project
- MVVM pattern with ViewModel First approach is used for the UI handling:
  - (Most) game logic is handled by the game engine in the CardGames.War project. 
  - Handling of events from user is handled by the Views models. 
  - Buttons are made available/disable via the “CanExecute” method in the ICommand interface
  - UI is handled by the views
  - Views are bound to the View models in a separate xaml file (View Model First approach).
- GameEngine is used by the view models. Same instance of the game engine is used for all View models. In this way they can all modify and get feedback from the same game engine.
- Dependency injection (using an IOC container) is used. For this app the Autofac framework is used. 
- Views are split up in several views (user controls), which are responsible for different areas on the screen. 
- Views + view models are reused (same view used for 2 different places on the screen. Same logic but with different data because different corresponding view model instances are used).
-	A mediator pattern is used for communication between classes. Prism EventAgregator is used to facilitate this. 
-	Prism is used for implementing the ICommand interface for buttons.
-	ObservableCollection is used for list properties which need to update on the screen.
-	A ViewModelBase class is created which all View models inherits from. This class implements the INotifyPropertyChanged interface. Whenever the PropertyChangedEventHandler event is invoked (via the OnPropertyChanged method in ViewModelBase) it will notify the view (binding to the property) that the value has changed and needs to be updated. 
-	The nuget package EluciusFTW.CardGames.Core.French is used for providing core functionality regarding cards.

## Some of the design guidelines I try to follow in this project
- Clean code approach:
  - LINQ is preferred over foreach/for loops 
  - Use small methods (and classes) with limited responsibility each (this approach is not followed all the time..)
  - Make method and parameter names as descriptive as possible
  - No Boolean input parameters used as input to methods

- SOLID:
  - Single responsibility principle (small methods and classes with only limited responsibility) is preferred. However not done all the time. 
  - Open/closed principle. Not using this approach as this is new code.
  - Liskov: This is used sometimes. For instance in the CardGameFactory. The factory can return several kind of classes. However as long as they all implements the   IVewModelBaseWrapper interface (which is the return type), they are all IVewModelBaseWrapper’s an can all be used as return values. Actually the IWarMainViewModel is casted to be a IviewModelBaseWrapper interface, but that works fine. Because warMainViewModel both implements the IwarMainViewModel and the IviewModelBaseWrapper interface. 
  - segregation: Good idea to split responsibilities between interfaces. However I prefer to only use 1 interface pr class. Makes it easier to do dependency injection. In one situation I therefore wrapped 2 interfaces into 1 interface in order to be able to use only 1 interface for my class. 
  - dependency injection / dependency inversion: Used that all the time. I use interfaces all the time in stead of instances. This makes the code more testable
   
## Ideas for improvement
-	Add unit tests. So far no unit tests are made for the app, which means that the quality and stability -and maintainability of the app is low. Unit tests have not been prioritized for this app since it is only made for educational purpose. 
-	A big and maybe a bit confusing game engine code which uses a lot of flags in order to work. Would be nice to make a state machine instead not using all these flags. Could perhaps make it possible to remove more logic from the ViewModels (to the game engine) as well -which is preferable. 
-	Use database for hosting users. Preparation for implementation started up but not finalized. 
-	Provide the possibility to create and edit users.
-	Save and open game
-	Create a nicer UI layout with animation, fancy buttons and this kind of stuff
-	Add more games like poker as well.
