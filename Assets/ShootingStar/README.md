Shooting Star
-------------------------------------
[Asset Store Link](http://u3d.as/o0e)  
© 2017 Justin Garza

PLEASE LEAVE A REVIEW OR RATE THE PACKAGE IF YOU FIND IT USEFUL!
Enjoy! :)

Contact  
-------------------------------------
Questions, suggestions, help needed?  
Contact me at:  
Email: jgarza9788@gmail.com  
Cell: 1-818-251-0647  
Contact Info: [justingarza.info/contact](http://justingarza.info/contact/)
  
Description/Features
-------------------------------------
A simple game template with a Level Menu, Level Data Management, Level Unlocking, and more!

* LevelMenu
    * Touch Gestures
    * Level Unlocking
    * Level to Scene Mapping
* Scene Transition  
* 20 Pre-made levels
* Prefabs 
    * Blocks
    * ForceAreas
    * And MORE!
* Code Comments
…Reach out to me to request new features!



Terms of Use
-------------------------------------
You are free to add this asset to any game you’d like
However:  
please put my name in the credits, or in the special thanks section. :)  
please do not re-distribute.  

Table of Contents 
-------------------------------------
1. Overview
2. Systems
    * SetUp and GlobalObjects
    * Audio System
    * Scene Transition System 	
    * LevelMenu 
    * GameManager 
    * Script List


  
Overview
-------------------------------------
The point of the game is to get the star to the "StarGate" by pulling back and shooting it (pulling back on the star and letting it go).

There are 3 Parts to this game.  

**MainMenu**  
This is the basic intro to the game  
![Imgur](http://i.imgur.com/qt2utfwl.png)

**LevelMenu**  
This is where the player picks a level to play  
![Imgur](http://i.imgur.com/LP6B5yAl.png)

**Levels**  
![Imgur](http://i.imgur.com/BBZb9gel.png)

SetUp and GlobalObjects 
-------------------------------------
**SetUp.cs**  
The SetUp.cs script creates 4 objects, if they don't already exists. These 4 Objects will not be destroied (see DontDestroy.cs) and will last the entirety of the game...hence GlobalObjects.

![Imgur](http://i.imgur.com/wja56o3l.png)


Audio System
-------------------------------------
**MusicManager && SoundManager**  
These two gameobjects are created at the start of the game to play the music and any sound we would like to be played. Since these objects are not destroied during the scene change there is no stop in the music or the sound.

Please see the following scripts for more information:  
MusicManager.cs  
SoundManager.cs  
VolumeControl.cs  
PlaySound.cs  


Scene Transition System 
-------------------------------------
**SceneSwitchAnimator**  
This gameObject is used to switch the scenes in an elegant and easy way. 

 
~~~cs
//displays animation than switch scene...
SceneSwitchAnimator.Instance.ChangeScene(SceneName,gameObject.transform.position);

//Note: The position passed into the method is where the transition will start. 
~~~



LevelMenu 
-------------------------------------
The LevelMenu is it's own asset and it's documentation can be found in Assets/ShootingStar/LevelMenu/. 



GameManager
-------------------------------------
The **GameManager.cs** is the main piece of each level that controls The Level's State (StartGame, Play, Win, Reset, and Pause). The GameManger.cs will also interact with the AnimUI (the object that shows/hides menus), the DTTR (double tap to reset), and the PauseButton. 

The GameManager.cs and all the other scripts are commented in detail, but please reach out to me if you have any questions.



Script List
-------------------------------------
Below is a List of Scripts and a breif description.


**Border.cs**  
Sets a border based on the screen's edges.  

**BrakeArea.cs**  
This is used to slow down all objects that have a tag in the TagList

**BreakBlock.cs**  
Manages blocks that will break when the star hits it.  

**DestroyAfter.cs**  
Destorys a the GameObject it is attached to after time.  

**DisableNextLevel.cs**  
This will disable the NextLevel button if it's the last level.

**DontDestroy.cs**  
This script will allow an object to live on after scene transition.

**ForceArea.cs**  
this is used to add a force to all objects that have a tag in the TagList

**GameManager.cs**  
This script manages the game's state.
Possible states include; StartGame, Play, Win, Reset, and Pause.

**GoToScene.cs**  
Allows a button to change the Scene

**LevelTitle.cs**  
Sets the Title of the Level

**MoveObject.cs**  
This script manages the movement of objects

**MusicManager.cs**  
This script is used to manage the music.

**ObjectPool.cs**  
A pool of objects that can be reused. (used with PoolManager.cs)

**PlaySound.cs**  
This script is used to play a sound

**PoolManager.cs**  
This script manages pools of objects for Spawning and Recycling.
(spawning and recycling is more mobile friendly than creating and destorying)

**RecycleAfter.cs**  
This script Recycles an Object after t seconds. (used with PoolManager.cs)

**RecycleObject.cs**  
This script is used to recycle objects in the pool manager. (used with PoolManager.cs)

**SetUp.cs**  
Creates GameObjects on Awake...but don't create them if they exists.

**SoundManager.cs**  
This script is used to manage the Sounds.

**Star.cs**  
This script manages the Star

**VolumeControl.cs**  
This script can turn music or sound off/on.





