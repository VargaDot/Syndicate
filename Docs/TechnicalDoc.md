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

## The cards
JSON file with all the attributes??

