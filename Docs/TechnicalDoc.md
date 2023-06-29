I'll be documenting how i will go about building the technical aspect of the game as well as any bug discoveries to try and understand in the future.
In short i will discussing the **hypothetical architecture**

### Main Menu
1. Quit function
``` 
if(Input.IsActionPressed("Quit"))
			GetTree().Quit(); 
```
i always forget it for some reason so here it is, you access GetTree and use that built-in function.

**Everything else is self-explanatory, at best you'll have to check the comments! future me**

### The Game
The game will be tile-based, turn based
## The tiles
The tiles are only there for the player to step on, when that happens they execute specific functions depending on their situation/condition.

Booleans obviously.
1. **Unowned**:
	When a player steps on an unowned tile, they will be prompted with its card that shows the stats (eg. how much its priced at) and options like buy and auction

2. **Owned**:
	If the owner steps on it, nothing happens, it will just proceed with normal protocol.
	If its anyone other than the owner, they will execute the rent function. which charges the person who stepped on it.

3. **Mortgage**
	2008 flashbacks


**I might have to add a new property in properties.JSON called boardID for the tiles** and if that happens i'll have to change the file named to something like tiles.JSON and have "cards.cs" just filter out stuff by the name property (eg. property1, property2 instead of brown1, brown2).

THERE ARE 40 TILES IN A MONOPOLY BOARD!!!!

# Game.cs 
i mean there's probably a better way to do it but according to the godot docs, the best way to go about storing nodes in an array is by making individual Node2D's first and then storing them in an array, however i opted to assign individually first and then assigning them to an array so idk and idc 

## The cards
I made a JSON file with every single bit of info thats needed for the properties, only thing needed is parsing

## The tilemap
I just thought of an amazing idea, instead of making the tilemap the actual "game scene" aka. when you step on tile it executes code, it should be just there for visuals sake
nothing matters in the tilemap, its just there for convenience

the game instead simulates everything seperately from the "visuals"

## The player
player will be a seperate node obviously, that rolls dice and has an inventory with owned property, money and special cards (get ouf of jail card)
this will help later on with LAN and local multiplayer.

# music player
duh, just an array full of songs that gets chosen with a random number and checks if that same song played before




