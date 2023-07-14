I'll be documenting how i will go about building the technical aspect of the game as well as any bug discoveries to try and understand in the future.
In short i will discussing the **hypothetical architecture**

### Main Menu
1. Quit function
```GetTree().Quit();```
i always forget it for some reason so here it is, you access GetTree and use that built-in function.

**Everything else is self-explanatory, at best you'll have to check the comments! future me**

### The Game
The game will be tile-based, turn based
## The tiles
The tiles are only there for the player to step on, when that happens they execute specific functions depending on their situation/condition.

~~Booleans obviously.~~ that method would require 10 billion booleans so no, we're using integers 0,1,2 or something and run a check on it with an if/switch statemnt to see its state against an enum, saves resources and my own brain
1. **Unowned**:
	When a player steps on an unowned tile, they will be prompted with its card that shows the stats (eg. how much its priced at) and options like buy and auction

2. **Owned**:
	If the owner steps on it, nothing happens, it will just proceed with normal protocol.
	If its anyone other than the owner, they will execute the rent function. which charges the person who stepped on it.

3. **Mortgage**
	2008 flashbacks


**I might have to add a new property in properties.JSON called boardID for the tiles** and if that happens i'll have to change the file named to something like tiles.JSON and have "cards.cs" just filter out stuff by the name property (eg. property1, property2 instead of brown1, brown2).

THERE ARE 40 TILES IN A MONOPOLY BOARD!!!!
(foreshadowing?: BoardID is probably gonna be pointless)

# Game.cs 
i mean there's probably a better way to do it but according to the godot docs, the best way to go about storing nodes in an array is by making individual Node2D's first and then storing them in an array, however i opted to assign individually first and then assigning them to an array so idk and idc 

**BOARD**
yeah i have no idea if there's even a good part of organisation on this doc aanymore and frankly idc
so i ran some tests, turns out you cannot store structs in an array, it simply isn't a thing, structs are pretty much their "own" data structure that stores whatever you cant throw at it really, it barely has any limits and according to a few docs its way faster than a class soooooooooooooooooooo yep

board ideas for now are either storing it in a nested dictionary
``` Dictionary<Dictionary<int,int>,Dictionary<int,int>> Registry = new Dictionary<Dictionary<int,int>,Dictionary<int,int>>() ```
and this isnt safe practice (atleast i dont think so)

- other method would be a matrix data type, something that goes from point x --> and point y (cant find the down arrow lmao) and then goes ok this is coordinate (x,y) get me the property data on this but this might take some time to theorise on my part. 

- heard of other stuff like buckets and trees but i have no idea what they are exactly or how to do them. 

--> this certainly is proving to be a challenge, bigger than any other one i've done and im excited for it.

## The cards
I made a JSON file with every single bit of info thats needed for the properties, ~~only thing needed is parsing~~
i made the parser with datamanager.cs and even made specific options for each type of variable u can use, only thing im worrying about now is the optimisation
since everytime i'm gonna have to open the JSON file and read this data, again i stress the question, what about the performance?

- Just found out you can use bytes for checking stuff, less memory usage since you're using 8 bits instead of 32 bits to store data so uses less ram (atleast i think so)

## The tilemap
I just thought of an amazing idea, instead of making the tilemap the actual "game scene" aka. when you step on tile it executes code, it should be just there for visuals sake
nothing matters in the tilemap, its just there for convenience

the game instead simulates everything seperately from the "visuals"

## The player
player will be a seperate node obviously, that rolls dice and has an inventory with owned property, money and special cards (get ouf of jail card)
this will help later on with LAN and local multiplayer. (or will it?)

# music player
duh, just an array full of songs that gets chosen with a random number and checks if that same song played before
//Did i literally make a note for music player? lmao



