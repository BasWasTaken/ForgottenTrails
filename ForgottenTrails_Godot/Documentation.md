# Documentation of Ink functions
V2, rewritten 2025-05-05 by Bas. First update. For version in Godot.
V1 written 2024-02-25 by Bas. (Draft made with Chatgtp)

This documentation covers all the functions and variables implemented for extra features in the project. 
As a self-imposed rule, functions prefixed with an underscore ("_") are ONLY used for back-end handling or pass-throughs to Unity. As such, they should NEVER have to be called from knots written by Vugs. If you ever feel like you do, either you have misunderstood something, or I have made a mistake in my code. Contact me to resolve such matters.

---

Ik twijfel steeds weer of het niet handiger is om deze documentate in de ink files zelf te zetten, maar ik vind dat die files snel onoverzichtelijk worden...

## Core Ink Utility

These are functions taken directly from Ink's documentation or examples, being officially sanctioned functionality.

## Incremental Knowledge Tracking

Allows tracking of what the player has observed throughout the game, using distinct lists called "Knowledge Chains."

- **KnowledgeState:** This variable serves as a list containing all acquired knowledge.
- **Knows(fact):** Check if a fact is present in the knowledge state.
- **KnowsAbout(subject):** Check if ANY of the facts associated with a given subject are present in the knowledge state.
- **Learn(facts):** Learn a new fact and add it to the knowledge state.

## Custom Utility

These functions and lists are built for the project specifically by me.

### Looping Time of Day

Tracks the time of day in the game, progressing through different stages. All functions that progress time, loop around from night to morning if needed, incrementing the day count by 1.

- **TimeOfDay:** The list which holds the defined values for day sections, used for the time parameters in the below functions.
- **DaysPassed:** Number counting number of days passed ingame since start. 
- **Time_Advance():** Progress time by one unit.
- **Time_WaitUntil(time):** Wait until a specific time to progress.
- **Time_WaitUntilBetween(min, max):** Wait until a time range to progress.
- **Time_AdvanceUntil(time):** Progress time until reaching a specific time. Equivalent to calling Time_Advance() and then Time_WaitUntil().
- **Time_AdvanceUntilBetween(min, max):** Progress time until reaching a time range. Equivalent to calling Time_Advance() and then Time_AdvanceUntilBetween().

### Location
#### Tracks Locations Player Visited

Tracks the player's previous, current, and intended locations for various game mechanics. Similar in essense to Knowledge Tracking.

Location parameters are taken from the Locations LIST.

- **KnowsLoc(location):** Check if the player knows a location exists.
- **LearnLoc(location):** Add a new location to the list of known locations.
- **HasVisited(location):** Check if the player has visited a location before.
- **SetLocation(location):** Update current, previous, and visited locations.

#### Travel & Travel Events

Allows travel between locations with a chance for random events.

- **TravelingTo(targetLocation, targetScene):** Initiate travel from one location to another. Automatically updates location tracker when arriving. Has a chance for random events.

#### Map Screen
NOTE: MAP module has not yet been implemented in Godot. If you run an ink file that uses this in godot expect weird behaviour. I plan to rebuild this in godot eventuelly, and in the short term replace it with a mock-functionality to at least avoid crashes.

---

Enables the display of available locations on a map screen in Unity. The map will consist of a ink knot that contains choices to unlocked locations. All of these will be caught by Unity and displayed as visual items on a map screen, which is opened and closed as the knot is accessed and left. The first time the player learns of a location, it is added to the map. But, it is only made available to travel to after they've visited it once before. And, all travel is locked during dialogue sections.

- **AllowMap(->returnTo):** Include this a list of choice options as a thread, to open up the possibility for the player to use the map screeen on this choice.
- **MapScreen(->returnTo):** Directly open the map screen for the player. This will rarely be needed, since usually it is sufficient to give the player the option to open the map, and the code will take care of the rest when they choose to. But in some cases you may want to take control and forcibly open the map.

### Track Inventory
NOTE: Inventory module has not yet been implemented in Godot, nor has hidden item options. If you run an ink file that uses this in godot expect weird behaviour. I plan to rebuild this in godot eventuelly, and in the short term replace it with a mock-functionality to at least avoid crashes.

---

Keeps track of player inventory and items.

Items are defined in the Items LIST.

Affordances are NOT defined in Ink- they are defined in Unity, and in ink, we just use strings, which it just assumed we input correctly.

- **Item_Add(item):** Add an item to the inventory.
- **Item_Remove(item):** Remove a given from the inventory.
- **ItemChoice(itemOrAffordances):** Present an in-game choice using an inventory item. This will be hidden in Unity, instead enabling an item to be used from the inventory screen. In Ink however this will just show up as a sample choice without indicating a specific item. As of 2024-03-17, also accepts multiple affordances to match per choice, separated with "&"s. E.G.: "tool&sharp"
- **UsedItem:** Variable to contain most recently used item. Useful in cases of multiple options where you as a writer don't know what item the player will choose. Note that this function cannot really be used this way in Ink (i.e. without playing in Unity) since the item use action takes place in Unity and cannot take place in Ink- you can only choose what affordance you used. 
- **Item_RemoveLstUsed():** Remove the item which was most recently used by the player, from inventory. Equivalent to calling Item_Remove() with the UsedItem parameter. This can be used to remove whatever item the player chose to use during a item puzzle. As noted above, it does not work without Unity. Thus this presents one of the few ways our game isn't entirely playable in Inky- the inventory won't have such items removed on use.

### NPCS

#### Track Party
NOTE: Party module has not yet been implemented in Godot. If you run an ink file that uses this in godot expect weird behaviour. I plan to rebuild this in godot eventuelly, and in the short term replace it with a mock-functionality to at least avoid crashes.

---

Tracks party members and interactions with them.

- **Party_AddMember(member):** Add a character to the party.
- **Party_RemoveMember(member):** Remove a character from the party.
- **AllowPartyScreen(-> returnTo):** Include this a list of choice options as a thread, to open up the possibility for the player to use the party screeen on this choice.

#### Track Affection

Tracks the opinion of NPCs regarding the player. Affection values are stored in variables with the format AffName and range from 0 to 100.

- **ConvertAttitude(value):** convert a numeric attitude value (usually an existing variable) to a human-readable ordinal value. 

### Visual and Audio Effects

#### Visual Effects
The Visual Effects system handles changing backgrounds and character portraits on the screen, as well as controlling the rate at which text appears. This section provides a seamless integration of visual elements to enhance the player's experience.

##### Changing Backgrounds & Foregrounds
Change the background picture and fade to and from colours. Later also will enable effects such as rain. 
- **FadeToImage(image, duration):** Initiates a fade transition to the specified image over a given duration.
- **FadeToColor(color, duration):** Fade the foreground to a given colour. See https://docs.godotengine.org/en/stable/classes/class_color.html for a list of colours defined in Godot.
- **FadeToBlack(duration):** Preset for FadeToColor
- **FadeToWhite(duration):** Preset for FadeToColor
- **FadeIn(duration):** Fade from curront color to transparent to reveal the scene.
- **Flash(color, amount):** Preset for quickly flashing to and from a color.



##### Spriteboard
SHow sprites on a panel. Sprites have 3 parameters: their subject, their variant, and their position. Positions are given in "x,y" coordinate system.
Functions:
- **Spriteboard_Present(subject, variant, position):** Place image of subject_variant at position. If another image of subject was already on display, the old one is removed.
- **Spriteboard_Move(subject, position):** Place image of subject at position, using whatever variant is currently visible, then removes that old one. Fails if subject is not already displayed. (Or could use a neutral.)
- **Spriteboard_Alter(subject, variant):** Place image of subject with variant, at whatever position the subject currently stands, then removes that old one. If subject is not already displayed, places subject in middle.
- **Spriteboard_Remove(image):** Removes a specific character portrait.
- **Spriteboard_RemoveAll():** Clears all character portraits from the screen.

##### Text Flow Control
The text flow control functions regulate the pacing of dialogue and narration, allowing for more nuanced storytelling and player engagement.

Functions:
- **Spd(value)**: Adjust the normal speed of the text by applying a multiplyer. This is separate from the preferred speed setting in the options menu, i.e., this multiplier will be applied on top of whatever the player's default speed is.
- **Halt(duration)**: Pauses text display for a specified duration.
- **Clear()**: Clears the text box and moves the text to the log.

#### Audio Effects
Play audio effects. Each type of sound is played over its own channel ingodot.
Volume parameters accept numers from a range between 0.0 and 1.0 inclusive.
Sounds can overlap to a point, playing a next one while there are too many active stops the first. This limit can be set per channel.

##### Voiceovers (vox)
Play voice bits, which are by default set to play once. 
Functions:
- **Vox_Play(clip, volume):** Plays the specified voiceover clip at the designated volume.

##### Sound Effects (Sfx)
Play sfx bits, which are by default set to play once. 
Functions:
- **Sfx_Play(clip, volume):** Plays the specified sound effect clip at the designated volume.

##### Ambiance
Play or stop ambience, which are by default set to loop. 
Functions:
- **Ambiance_Play(clip, volume):** Adds an ambient audio clip to the scene with the specified volume.
- **Ambiance_Stop(clip):** Removes a specific ambient audio clip from the scene.
- **Ambiance_StopAll():** Clears all ambient audio clips from the scene.

##### Music
Play or stop music clips, which are by default set to loop.
Functions:
- **Music_Play(clip, volume):** Plays the specified music track at the designated volume.
- **Music_Stop():** Stops the playback of the current music track.

### Variable Language

Handles variable language for dealing with plural vs. singular, multiple genders, etc.

- **IsAre(list)**: insert an "is" or an "are" depending on whether there are 1 or multiple entries in this list.
- **ListWithCommas(list)**: write out all of the items in this list, each separated by a comma, except the last which is preceeded by an "and".


### Communicating with Godot

Functions for communicating with Unity for logging and warnings.

- **Print()**: Send a text message to the Unity console log.
- **PrintWarning()**: Send a text message to the Unity console log, formatted as a warning.
