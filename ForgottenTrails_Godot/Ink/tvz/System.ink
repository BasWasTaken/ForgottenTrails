INCLUDE PartyDialogues.ink




   === Section_Travelling ===
   /* ---------------------------------
   #### System: Travel & Travel Events
  ----------------------------------*/
   -> DONE
   // Allows for travel between locations, with a chance for a random event to pop up in between
   
VAR TargetLocation = () // contains location to travel to in map traversal
~ TargetLocation = Locations()  // limit var to locations defined in Lists
   
VAR RollRandomEvents = true // wether or not to roll for random events during travel

VAR SucceededRandomEvent = false // boolean to check after travel event

VAR LowRationsLimit = 3 // amount of rations that causes party to complain if you go under it.


=== InsertComplaint === // the complaint scene to insert on low rations
    You {LIST_COUNT(Party)>1:and your party} are growing {|ever more } wary of the low amount of remaining rations.
    ->->

=== Starvation ===
    You look at the meager rations still laft in your packs.
    // TODO: incldue fail forward here
    // if you do have a non-0 amount of rations left, you can divvy them out. everyone who did not get a portion grows more hungry. and then as some point they should just die or go of by themselves i guess?
    ->->

=== TravelingTo(targetLocation, ->targetScene) === // used for traveling from a to b. instead of immediately warping, there will be some animation, chance for encounter, use of rations, etc.
~ TargetLocation = targetLocation
~ temp OriginLocation = CurrentLocation
~ SetLocation(LOC_OnTheRoad)
{
- TravelRations < LIST_COUNT(Party): // if not enough rations left to feed everyone
    -> Starvation -> //grow more hungry
- TravelRations < LowRationsLimit * LIST_COUNT(Party): // check if low on rations
    -> InsertComplaint -> // grow more wary
} 
    ~ TravelRations-=1*LIST_COUNT(Party)  // each party member eats some rations
    You {LIST_COUNT(Party)>1:all }eat rations.

~ temp Encounter = false // default to false

{
- RollRandomEvents: // if random events are active
    {
    - RANDOM(0,1): // flip a coin
        ~ Encounter = false
    - else:
        ~ Encounter = true
    }
}

~ SucceededRandomEvent = true // default  to true

// we gaan er in eerste instantie dus van uit dat de speler succeed, maar hieronder geven we de event de kans om daat een stokje voor te steken:

{
- Encounter:
    ~Print("Heads! Encounter was true!") // who knows what might happen now
    -> RandomTravelEvent -> 
- else:
    ~Print("Tails! Encounter was false!") // succeed automatically
 }

{
- SucceededRandomEvent:
        ~Print("Proceeding with traversal.")
    ~_ArriveAt(TargetLocation)
    ->targetScene
- else:
        ~Print("Aborting traversal. Returning to previous location.")
    ~_ArriveAt(OriginLocation)
}
-    ->->
=== function _ArriveAt(location) === 
~SetLocation(location) // update curentlocation etc.


=== RandomTravelEvent ===
->RandomEventsEdanArea->
~Print("A Random event happened!")
- ->->
/*Continue, or head back anyway?
    + [Continue]
        ->->
    + [Go back]
        ~ SucceededRandomEvent = false // set to false anyway
        ->->*/


    === Section_Map ===
    /* ---------------------------------
   ##### System: Map Screen
  ----------------------------------*/
    -> DONE
   // he map will consist of a ink knot that contains choices to unlocked locations. All of these will be caught by Unity and displayed as visual items on a map screen, which is opened and closed as the knot is accessed and left.
   // The first time the player learns of a location, it is added to the map. But, it is only made available to travel to after they've visited it once before. And, all travel is locked during dialogue sections.

=== AllowMap(-> returnTo) === // including this in the list of choices allows the player to open their map in order to travel to any applicable locations (and exit the current conversation)

+  [\{UNITY:OpenMap\}] -> MapScreen(returnTo) //WITHOUT "()"

=== MapScreen(-> returnTo) // the map knot. visit to open the map in unity. This knot should include all possible items on the player's map
~ OpenMap()
+ { HasVisited(LOC_EdanCastle)} [{_MapChoice(LOC_EdanCastle)}] 
    -> TravelingTo(LOC_EdanCastle, ->ScotlandEntranceRoad)->returnTo
+ { HasVisited(LOC_SampleCave)} [{_MapChoice(LOC_SampleCave)}] 
    -> TravelingTo(LOC_SampleCave, ->SampleSampleCaveScene)->returnTo
+ { HasVisited(LOC_SeaBreezePath)} [{_MapChoice(LOC_SeaBreezePath)}] 
    -> TravelingTo(LOC_SeaBreezePath, ->SampleSeaBreesePathScene)->returnTo
+ [\{UNITY:CloseMap\}]    
    \{UNITY:CloseMap()\}
-     (done) -> returnTo

=== function OpenMap() // call to open the map screen in unity
~ _OpenMap()
    
=== function _OpenMap() === // opens the map screen in unity
\{UNITY:OpenMap()\}

EXTERNAL _OpenMap()
    
=== function _MapChoice(destination) === // used to present an inky choice that will be represented visually on a map in unity. (in ink it simply lists as a normal choice)
\{MapChoice({destination})\

  === Section_TrackInventory ===
  /* ---------------------------------
   ### System: Keeps track of player inventory and items
  ----------------------------------*/
  -> DONE
  // Inventory is managed by the LIST variable in Ink, which is observed by Unity and matched accordingly.

LIST Items = Knife, Pot, Rope, Lantern, Torch, ForagedMushrooms, WornSword, EdanInnRoomKey1, EdanInnRoomKey2, EdanInnRoomKey3, EdanInnMasterKey, BasicFishingRod // existing items // Vugs may add items to this list.

~ Items = LIST_ALL(Items)  // Full list for Unity syncing. Note Bas: I should maybe  prefix with underscore

// rations and money are currently not in inventory sytem:
VAR TravelRations = 107

VAR Money = 10

  
VAR Inventory = () // list of items the player has.
~ Inventory = Items() // restrict to items defined in list

=== function Item_Add(item) // Add item to inventory.
    ~ Inventory += item

=== function ItemChoice(itemOrAffordances) // include an ink choice which can only be taken by using an item from the inventory (in unity. in ink, it'll show as normal). 
\{ItemChoice({itemOrAffordances})\} 

VAR UsedItem = () // container for unity to tell ink what item it just used
~ UsedItem = Items()

=== function _Item_Remove(item) // used to remove an item from the inventory
    {
    - Inventory has item: 
        ~ Inventory -= item
        ~ Print("Removed {item}. Items still in possesion: {Inventory}")
    - else:
        ~ PrintWarning("Attempted to remove an item that wasn't there!")
    }
    
=== function Item_Remove(item)
~ _Item_Remove(item)

=== function Item_RemoveLastUsed() // remove item that was just used
~ Item_Remove(UsedItem)

  
  
  === Section_TrackParty ===
  /* ---------------------------------
   ### System: Tracking party members and speaking to them, potentially triggering leave conditions
  ----------------------------------*/
  -> DONE
  // WIP
  
   /* ---------------------------------
   #### List: Characters
   ----------------------------------*/
LIST PartyCandidates = Player, Alice, Robert // potential party members // Vugs may add items to this list.
VAR Party = () // list of characters in party

~ Party = PartyCandidates() // restrict to characters defined in list

=== function Party_AddMember(member) // Add character to party
    ~ Party += member
//System: {member} joined the party.
~Print("{member} joined the party.")
    
=== function Party_RemoveMember(member) // Add character to party
    ~ Party -= member
//System: {member} left the party.
~Print("{member} left the party.")

=== AllowPartyScreen(->returnTo) === // including this in the list of choices as a "thread statement" allows the player to open their party screen in order to start dialogues with party members.  Outside of these moments, party members can still be examined but not changed.
+  [\{UNITY:OpenPartyScreen\}] -> PartyScreen(returnTo) //WITHOUT "()"

=== function OpenPartyScreen() // call to open the map screen in unity
~ _OpenPartyScreen()

=== function _OpenPartyScreen() === // opens the map screen in unity
\{UNITY:OpenPartyScreen()\}

EXTERNAL _OpenPartyScreen()


=== PartyScreen(-> returnTo) // the party knot. visit to open the party screen in unity. 
~ _OpenPartyScreen()
<- PartyDialogues(returnTo)
+ [\{UNITY:ClosePartyScreen\}]    
    \{UNITY:ClosePartyScreen()\}
-     (done) -> returnTo
    

    
=== function _PartyChoice(character) === // used to present an inky choice that will be represented by a portrait in unity. in inky, it will just be an ormal option to click
\{PartyChoice({character})\}





   === Section_TrackAffection ===
  /* ---------------------------------
   #### System: Track opinion of NPCs regarding the player
  ----------------------------------*/
   -> DONE
  // Tracking numeric value for relationship between pc and other characters, and using cutoff points to determine friendly, hostile etc.
  // Can be compared simply by asking e.g.: {AFFHenry >= friendly}






LIST Attitudes = devoted = 100, alligned = 75, friendly = 60, ambivalent = 50, begrudging = 40, hostile = 25, spiteful = 0

=== function ConvertAttitude(value) ===
{
-   value >= LIST_VALUE(devoted):
    ~ return devoted
-   value >= LIST_VALUE(alligned):
    ~ return alligned
-   value >= LIST_VALUE(friendly):
    ~ return friendly
-   value >= LIST_VALUE(ambivalent):
    ~ return ambivalent
-   value >= LIST_VALUE(begrudging):
    ~ return begrudging
-   value >= LIST_VALUE(hostile):
    ~ return hostile
-   else:
    ~ return spiteful
}

  
  
   /* ---------------------------------
   ##### List: Affection values
   ----------------------------------*/
   
VAR AffEdgar = 50
VAR AffHenry = 50
VAR AffAlice = 50
VAR AffRobert = 50
VAR AffEdie = 50  
  
  
  === Section_Effects ===
  /* ---------------------------------
   ### System: Visual and Audio effects ("special effects") for the VN
  ----------------------------------*/
  -> DONE
  // 
  
   === Section_Backgrounds ===
  /* ---------------------------------
   #### FEATURE: Changing (and fading to) backgrounds. Done with external functions.
  ----------------------------------*/
   -> DONE
   
   
LIST Background = (none), BG_Road1, BG_VaultHall, BG_VaultLibrary, BG_VaultHall2, BG_VaultOffice, BG_CastleGate // list of BackGrounds
  
=== function FadeToImage(image, duration) // name of image and duration in seconds (e.g. 0.5)
    ~ Background = image //update background 
~ (_FadeToImage(image, duration)) 

=== function _FadeToImage(listItem, float)
    
<<i>Unity background: {listItem}</i>> 
EXTERNAL _FadeToImage(listItem, float) 

=== function FadeToColor(color, duration)   // duration in seconds (e.g. 0.5)
~ _FadeToColor(color, duration)

=== function _FadeToColor(string, float) // fade to a color.
<<i>Fade to {string}</i>> 
EXTERNAL _FadeToColor(string, float)


   === Section_Portraits ===
  /* ---------------------------------
   #### FEATURE: Changing what character portraits appear on screen. Done by observing variable.
  ----------------------------------*/
   -> DONE
   // NOTE: I have to test whether this allows explicit ordering
   
   
LIST Portraits = (none), Alice1, Robert1
=== function Portraits_Set(images)
~ Portraits = images
<<i>Unity now shows {images}</i>> 

=== function Portraits_Add(image)
~ Portraits += image
<<i>Unity now shows {image}</i>> 

=== function Portraits_Remove(image)
~ Portraits -= image
<<i>Unity removes {image} if present</i>> 

=== function Portraits_RemoveAll()
~ Portraits = ()
<<i>Unity removes all portraits</i>> 

   === Section_Audio ===
  /* ---------------------------------
   #### FEATURE: Audio effects
  ----------------------------------*/
   -> DONE

LIST Vox =  NA   

// Vox. Done with external function.
=== function Vox_Play(clip, volume)// use volume between 0.0 and 1.0
~ Vox = clip // assign clip to vox list to ensure fit of argument
~ _Vox_Play(clip, volume)

=== function _Vox_Play(listItem, float) // plays audio on voice channel, unlooped
<<i>Vox: {listItem} at {float} volume</i>>   
EXTERNAL _Vox_Play(listItem, float)

LIST Sfx = gong
// SFX. Done with external function.
=== function Sfx_Play(clip, volume)// use volume between 0.0 and 1.0
~ Sfx = clip // assign clip to sfx list to ensure fit of argument
~ _Sfx_Play(clip, volume)

=== function _Sfx_Play(listItem, float) // plays audio on sfx channel, unlooped
<<i>Sfx: {listItem}</i>>
EXTERNAL _Sfx_Play(listItem, float)


LIST Ambiance = (none), chatter
// Ambiance. Handled with external function.
=== function Ambiance_Add(clip, volume) //use volume between 0.0 and 1.0
~ Ambiance += clip
~ _Ambiance_Play(clip, volume)

=== function _Ambiance_Play(listItem, float) // adds audio on an ambiance channel, looping
<<i>Ambiance: {Ambiance} </i>>
EXTERNAL _Ambiance_Play(listItem, float)

=== function Ambiance_Adjust(clip, newVolume)
{ Ambiance ? clip:
    ~ _Ambiance_Play(clip, newVolume)
- else:
    ~ PrintWarning("{clip} Not found")
}

=== function Ambiance_Remove(clip)
~ Ambiance -= clip 
~ _Ambiance_Remove(clip)

=== function _Ambiance_Remove(listItem)
<<i>Ambiance: {Ambiance} </i>>
EXTERNAL _Ambiance_Remove(listItem)

=== function Ambiance_RemoveAll()
~ Ambiance = Ambiance.none
~ _Ambiance_RemoveAll()

=== function _Ambiance_RemoveAll()
<<i>Removed all ambiance. </i>>
EXTERNAL _Ambiance_RemoveAll()

=== function Ambiance_Play(clip, volume)
~_Ambiance_RemoveAll()
~Ambiance_Add(clip, volume)

LIST Music = (none), theStreetsOfWhiteRun, TabiNoTochuu
// Music. Handled with external function.
=== function Music_Play(clip, volume) // play new clip or adjust volume of existing clip
~ Music = clip
~ _Music_Play(clip, volume)

=== function _Music_Play(listItem, float) // plays audio on music channel, looping // use volume between 0.0 and 1.0
<<i>Music: {Music}</i>>
EXTERNAL _Music_Play(listItem, float) 

=== function Music_Stop()
~ Music = Music.none
~ _Music_Play(Music.none, 0)
  

   === Section_TextFlow ===
  /* ---------------------------------
   #### Feature: Control the rate at which text appears in unity.
  ----------------------------------*/
   -> DONE
  
VAR Speed = 1.0 

=== function Spd(value) // change speed of textflow based on positive number, expressed as multiplier (eg .8 for 80%)
~ Speed = value
~ _Spd(value)
=== function _Spd(float) 
// nothing
EXTERNAL _Spd(float) 

=== function Halt(duration)
~ _Halt(duration)

=== function _Halt(float) // [EXPERIMENTAL] pause text for x seconds 
<<i>Unity halts for: {float}</i>> 
EXTERNAL _Halt(float) 

=== function Clear()  // Clears the textbox and moves that text to log
~ _Clear()

=== function _Clear()
<<i>Clear Page</i>>
EXTERNAL _Clear()

VAR stop = "\{stop\}" // Used in Unity for stopping the continue loop

VAR glue = "\{glue\}" // used to glue next line to this.

VAR aglue = "\{aglue\}" // used to glue this to previous line


  === Section_VariableLanguage ===
  /* ---------------------------------
   ### System: Variable language for dealing with plural vs singular, multiple possible player genders, etc.

  ----------------------------------*/
  -> DONE
  // 


=== function IsAre(list)
	{LIST_COUNT(list) == 1:is|are}

=== function ListWithCommas(list, if_empty)
    {LIST_COUNT(list):
    - 2:
            {LIST_MIN(list)} and {ListWithCommas(list - LIST_MIN(list), if_empty)}
    - 1:
            {list}
    - 0:
            {if_empty}
    - else:
              {LIST_MIN(list)}, {ListWithCommas(list - LIST_MIN(list), if_empty)}
    }



VAR PlayerName = "PlayerName"

EXTERNAL PromptName()
=== function PromptName()
~PlayerName = "Player"

// This is the list of vars for the gender stuff, but that's also not really well implemented yet of course.

LIST players_gender = (undefined), nonbinary, male, female

// TODO: reform the below to lists
VAR players_eyecolor = "Undefined"
VAR players_hair = "Undefined"
VAR players_hair_color = "Undefined"
VAR players_hair_style = "Undefined"

VAR androgynous = "androgynous"
VAR they = "they"
VAR them = "them"
VAR their = "their"
VAR theirs = "theirs"
VAR Mx = "Mx"
VAR master = "master"
VAR person = "person"
VAR kid = "kid"
VAR lad = "lass"
VAR guy = "guy"

=== function SetPronouns(gender)
~ players_gender = gender
{
	- players_gender == male:
	    ~ androgynous = "masculine"
		~ they = "he"
        ~ them = "him"
        ~ their = "his"
        ~ theirs = "his"
        ~ Mx = "Mr"
        ~ master = "mister"
        ~ person = "man"
        ~ kid = "boy"
        ~ lad = "lad"
        ~ guy = "guy"        
        
    - players_gender == female:
        ~ androgynous = "feminine"
        ~ they = "she"
        ~ them = "her"
        ~ their = "her"
        ~ theirs = "hers"
        ~ Mx = "Ms"
        ~ master = "missus"
        ~ person = "woman"
        ~ kid = "girl"
        ~ lad = "lass"
        ~ guy = "gal"
        
	- else:
	    ~ androgynous = "androgynous"
        ~ they = "they"
        ~ them = "them"
        ~ their = "their"
        ~ theirs = "theirs"
        ~ Mx = "Mx"
        ~ master = "master"
        ~ person = "person"
        ~ kid = "kid"
        ~ lad = "lad"
        ~ guy = "guy"
}


  === Section_Logs ===
  /* ---------------------------------
   ### System: Communicating with Unity
  ----------------------------------*/
  -> DONE

EXTERNAL Print(string) 
=== function Print(message) // send text to unity console as message
<<i>Log: {message}</i>> 

EXTERNAL PrintWarning(string)
=== function PrintWarning(message) // send text to unity console as warning
<<i>LogWarning: {message}</i>>







