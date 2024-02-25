# ForgottenTrails
Our little project.
Branch flow as of 2024-02-25:
/main: Should only contain stable versions. No changes should be made on this directedly.
/release/\*: Will be used as soon as we exit the prototyping phase. Will contain merged packets ready for deployment to /main at set points of time or progress. Multiple can be active at once (e.g. doing final bugfixes on 1.1 while making new features for 1.2). After being pushed to mnain, a release branch should be tagged and closed.
/meeting/yyyymmdd: Made as branches of the /develop or /meeting branches. Used for preserving the state of the project as tested before a meeting. After the meeting these are transferred to tags and preserved for prosterity. 
/develop: Will contain changes from all feature/ branches as well as changes made by bas on the fly.
/feature/\*: Individual branches on which we can work on separate features, isolated from other changes. When done, they can be merged to develop and closed. If active for a long time (such as in the case of /feature/story) should also receive merges from /develop occasionally (e.g. when pulling there) to prevent large discrepancies between versions.
/feature/story: Branch where both Vugs and Bas write to ink. Frequently bi-merged with develop to keep both feature and story updates synched up, while avoiding unstable updates that are written on the other "feature" branches.

---

# Documentation of Ink functions
V1, written 2024-02-25 by Bas. (Draft made with Chatgtp)

This documentation covers all the functions and variables implemented for extra features in the project. 
As a self-imposed rule, functions prefixed with an underscore ("_") are ONLY used for back-end handling or pass-throughs to Unity. As such, they should NEVER have to be called from knots written by Vugs. If you ever feel like you do, either you have misunderstood something, or I have made a mistake in my code. Contact me to resolve such matters.

## Core Ink Utility

These are functions taken directly from Ink's documentation or examples, being officially sanctioned functionality.

## Incremental Knowledge Tracking

Allows tracking of what the player has observed throughout the game, using distinct lists called "Knowledge Chains."

- **KnowledgeState:** This variable serves as a list containing all acquired knowledge.
- **Knows(fact):** Check if a fact is present in the knowledge state.
- **KnowledgeStateBetween(factX, factY):** Check if the knowledge state is between two specific points.
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

Enables the display of available locations on a map screen in Unity. The map will consist of a ink knot that contains choices to unlocked locations. All of these will be caught by Unity and displayed as visual items on a map screen, which is opened and closed as the knot is accessed and left. The first time the player learns of a location, it is added to the map. But, it is only made available to travel to after they've visited it once before. And, all travel is locked during dialogue sections.

- **AllowMap(->returnTo):** Include this a list of choice options as a thread, to open up the possibility for the player to use the map screeen on this choice.
- **MapScreen(->returnTo):** Directly open the map screen for the player. This will rarely be needed, since usually it is sufficient to give the player the option to open the map, and the code will take care of the rest when they choose to. But in some cases you may want to take control and forcibly open the map.

### Track Inventory

Keeps track of player inventory and items.

Items are defined in the Items LIST.

- **Item_Add(item):** Add an item to the inventory.
- **Item_Remove(item):** Remove a given from the inventory.
- **ItemChoice(itemOrAffordance):** Present an in-game choice using an inventory item. This will be hidden in Unity, instead enabling an item to be used from the inventory screen. In Ink however this will just show up as a sample choice without indicating a specific item. NOTE: as of 2024-02-25, soon to be expanded with multiple parameter options.
- **UsedItem:** Variable to contain most recently used item. Useful in cases of multiple options where you as a writer don't know what item the player will choose. Note that this function cannot really be used this way in Ink (i.e. without playing in Unity) since the item use action takes place in Unity and cannot take place in Ink- you can only choose what affordance you used. 
- **Item_RemoveLstUsed():** Remove the item which was most recently used by the player, from inventory. Equivalent to calling Item_Remove() with the UsedItem parameter. This can be used to remove whatever item the player chose to use during a item puzzle. As noted above, it does not work without Unity. Thus this presents one of the few ways our game isn't entirely playable in Inky- the inventory won't have such items removed on use.

### NPCS

#### Track Party

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

##### Changing Backgrounds
The FadeToImage function enables the smooth transition between different background images, creating a visually immersive environment. It's complemented by the Background list, which contains a range of background options for diverse settings.
To instead have instant transitions, simply give the duration parameter a value of 0.

- **FadeToImage(image, duration):** Initiates a fade transition to the specified image over a given duration.
- **FadeToColor(color, duration):** Fades the background to a solid predefined color over a specified duration. See http://www.flounder.com/csharp_color_table.htmv for a list of colours defined in C# (our unity coding language).

##### Character Portraits 
With the Portraits system, the game dynamically adjusts character portraits based on the narrative context.

Functions:
- **Portraits_Set(images):** Sets the character portraits to the specified images. Note: ordering not fully tested yet.
- **Portraits_Add(image):** Adds a character portrait to the existing set.
- **Portraits_Remove(image):** Removes a specific character portrait.
- **Portraits_RemoveAll():** Clears all character portraits from the screen.

##### Text Flow Control
The text flow control functions regulate the pacing of dialogue and narration, allowing for more nuanced storytelling and player engagement.

Functions:
- **Spd(value)**: Adjust the normal speed of the text by applying a multiplyer. This is separate from the preferred speed setting in the options menu, i.e., this multiplier will be applied on top of whatever the player's default speed is.
- **Halt(duration)**: Pauses text display for a specified duration.
- **Clear()**: Clears the text box and moves the text to the log.
- **{stop}**: Require a continue action from the player (i.e. click or spacebar) before moving on from this line.  (In unity)
- **{glue}**: used to glue next line to this in Unity.
- **{aglue}**: used to glue this to previous line in Unity.

#### Audio Effects
The Audio Effects system manages the playback of various audio elements, including voiceovers, sound effects, ambiance, and music. It enriches the game environment with immersive auditory experiences.

##### Voiceovers (vox)
The Vox system handles voiceover playback, ensuring clear and synchronized audio delivery during dialogue sequences.

Volume parameters accept numers from a range between 0.0 and 1.0 inclusive.

Functions:
- **Vox_Play(clip, volume):** Plays the specified voiceover clip at the designated volume.

##### Sound Effects (Sfx)
The Sfx system triggers various sound effects to complement in-game actions and events, enhancing gameplay feedback and immersion.

Functions:
- **Sfx_Play(clip, volume):** Plays the specified sound effect clip at the designated volume.

##### Ambiance
Ambiance adds atmospheric audio layers to different scenes, setting the mood and enhancing the player's sense of presence within the game world.

Functions:
- **Ambiance_Add(clip, volume):** Adds an ambient audio clip to the scene with the specified volume.
- **Ambiance_Adjust(clip, newVolume):** Adjusts the volume of an existing ambient audio clip.
- **Ambiance_Remove(clip):** Removes a specific ambient audio clip from the scene.
- **Ambiance_RemoveAll():** Clears all ambient audio clips from the scene.

##### Music
The Music system controls the playback of background music tracks, enriching the player's experience with thematic compositions tailored to the game's narrative and setting.

Functions:
- **Music_Play(clip, volume):** Plays the specified music track at the designated volume.
- **Music_Stop():** Stops the playback of the current music track.

### Variable Language

Handles variable language for dealing with plural vs. singular, multiple genders, etc.

- **IsAre(list)**: insert an "is" or an "are" depending on whether there are 1 or multiple entries in this list.
- **ListWithCommas(list)**: write out all of the items in this list, each separated by a comma, except the last which is preceeded by an "and".


### Communicating with Unity

Functions for communicating with Unity for logging and warnings.

- **Print()**: Send a text message to the Unity console log.
- **PrintWarning()**: Send a text message to the Unity console log, formatted as a warning.
