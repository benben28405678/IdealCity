README

>> Welcome to the Ideal City Builder Unity Project <<

Planning Document - https://docs.google.com/document/d/1q26zhTeUj3PCv8-XQbrE0N1pR6vu_W4UlEdI-UwNXYw/edit?usp=sharing

[!] How the Folder System Works

Imports - Imported assets. Inside there is the City Models pack and some other stuff.
Materials - Materials. Most materials we create should go here.
Plugins - Plugins for the Unity project. I use GitHub with Unity, so that's what it's for.
Resources - Please be careful with removing stuff from this folder. Anything in this folder can be accessed by the C# code while running.
Scenes - The folder for managing the scenes. I think we will only have one main scene for now; maybe a few later for the storyline and whatnot.
Scripts - The code. CameraControl manages the camera's movement. GridAnimation is useless. PlaceNew manages the placing of new buildings. PlaceNewPlaneAnimation manages the changing colors/sizes of the plane that shows under the building you're placing.
SettingManager is placeholder code to manage the game's global settings. Right now it just manages the graphics level. But it's kinda glitchy. StateManager is very important file. It holds two global variables: isBuilding and currentlyPlacingName. The first is a boolean. If it's true, a new building will show up to be placed. The second doesn't do anything as of now.
Textures - We're gonna keep textures in here. Obviously.

[!] How the Hierarchy works

BaseElements - Essential stuff like the camera, sun, ground.
MapElements - All buildings/decorations.
StateManager - An empty object that holds global variables.
CurrentlyPlacing - All the parts of the building you're about to place down.
Grid - The grid that shows up when isBuilding is set to true.
And the particle systems... don't mess with those :)

[!] Unity Collab

Please make descriptive comments in the Collab uploader when you submit an upload.