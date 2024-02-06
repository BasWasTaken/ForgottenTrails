// --------- Bas  ---------
=== Section_Top ===
/* ---------------------------------
    # Custom Features
----------------------------------*/
-> DONE
// This file contains all of the functions and variables for the extra features we use.
// Documentation is included where relevant.
// Functions starting with an underscore ("_") should NEVER be called from knots written by VUGS.
// This file contains seemingly blank knots labeled "Section"s. These help Ink's new "collapsable knots" feature actually collapse in a helpful way. (Without them, my headers get lumped in with each previous block.) 
 === Section_Core ===
 /* ---------------------------------
    ## Core Ink Utility 
 ----------------------------------*/
 -> DONE
 // The following are functions taken directly from Ink's documentation. May be altered, but the core is official functionality.

=== function _Pop(ref list) // Helper function: popping elements from lists
   ~ temp x = LIST_MIN(list) 
   ~ list -= x 
   ~ return x

  === Section_TrackKnowledge ===
  /* ---------------------------------
   ### System: Incremental knowledge tracking.
  ----------------------------------*/
  -> DONE
  //  Allows tracking of what the player has observed throughout the game. Used by distinct LISTs. Each LIST is a chain of facts. Each fact supersedes the fact before. LISTs used this way are also called "Knowledge Chains".
  
VAR KnowledgeState = () // VAR that will serve as list containing all acquired knowledge

=== function Knows (fact) // check if fact is present in knowledge state, i.e. is it known information?
   ~ return KnowledgeState ? fact 

=== function KnowledgeStateBetween(factX, factY) // used to check if knowledge state is between two specific points, i.e. does the player know x, but also not know y?
   ~ return KnowledgeState? factX && not (KnowledgeState ^ factY)

=== function Learn(facts)  // used to "learn" a fact (i.e. add it to knowledge state). This also learns all facts before it (because they are incremental).
   ~ temp x = _Pop(facts)
   {
   - not x: 
      ~ return false 

   - not Knows(x):
      ~ temp chain = LIST_ALL(x)
      ~ temp statesGained = LIST_RANGE(chain, LIST_MIN(chain), x)
      ~ KnowledgeState += statesGained
      ~ Learn (facts) 	// set any other states left to set
      ~ return true  	       // and we set this state, so true
 
    - else:
      ~ return false || Learn(facts) 
    }	
    
   /* ---------------------------------
   #### List of Knowledge Chains
   ----------------------------------*/
//Vugs: kunnen we het een keer hebben over de exists, name flow? Weet niet of ik daar helemaal happy mee ben atm. 
// Bas: Absoluut, laat maar weten. 
// Overigens is dit trouwens een voorbeeld van iets dat me misschien handig lijkt om in een andere file te defineren: jij zult wsl veel nieuwe knowledge chains aan moeten maken, en dan is het miss fijn als je niet steeds dit hele systeem document door hoef te spitten. (Idem voor items etc.)
LIST EdanCastleKnow = (none), Exists, IsCastleOnHill // wat is dit nou weer voor een verschikkelijke variabelnaam die ik heb gemaakt wtf
LIST Edgar = (none), Exists, Name
LIST Henry = (none), Exists, Name
LIST Tomas = (none), Exists, Name
LIST Eileen = (none), Exists, Name
    
 === Section_Extended ===
 /* ---------------------------------
    ## Custom Utility
 ----------------------------------*/
 -> DONE
 // The following is not official Ink utility: they are functions and lists we build for this project.
 
  === Section_TrackTime ===
  /* ---------------------------------
   ### System: Looping Time of Day.
  ----------------------------------*/
  -> DONE
  
LIST TimeOfDay = (Dawn), Morning, Midday, Afternoon, Dusk, Evening, Night
//(Here we consider the day to start at dawn and end at night. Admittedly a large part of day 1's night is technically part of day 2, the alternatives are either saying that the day ends in evening, making night part of the next day entirely, which complicates the condition "TimeOfDay>=Dusk", or splitting the night up further in before or after midnight. Of these three I find the current option to be least unsatisfactory.)
//@Vugs: thoughts on the above? Do you agree with these parts of day, or should Night be considered part of the next day or split up?

VAR DaysPassed = 0

=== function Time_Advance() // used to progress time 1 or loop back around and progress dayspasse 1.
{ TimeOfDay == LIST_MAX(LIST_ALL(TimeOfDay)):
    ~ DaysPassed ++
	~ TimeOfDay = LIST_MIN(LIST_ALL(TimeOfDay))
- else:
	~ TimeOfDay ++ 
}   
~Print("It is now {TimeOfDay}, Day {DaysPassed+1}")

=== function Time_AdvanceUntil(time)
~ Time_AdvanceUntilBetween(time, time)

=== function Time_AdvanceUntilBetween(min, max)
~ Time_Advance() // first call at least 1 advancement
~ Time_WaitUntilBetween(min, max) // then check and loop if needed

=== function Time_WaitUntil(time)
~ Time_WaitUntilBetween(time, time)

=== function Time_WaitUntilBetween(min, max) // used to progress time to point, iterating dayspassed by 1 if appropriate. Does NOT progress if time is already fit.
{
- min <= TimeOfDay && TimeOfDay <= max:
// do nothing
- else:
~ Time_Advance() // call function
~ Time_WaitUntilBetween(min, max) // and go again
}




  === Section_TrackLocations ===
  /* ---------------------------------
   ### System: Tracks Locations Player visited
  ----------------------------------*/
  -> DONE
  // Tracking the players previous, current, and intended location, for use in backtraveling, intermitting random encounters, and more.
  
LIST Locations = LOC_EdanCastle, LOC_RoadToEdanCastle, LOC_EdinburghCrossroads, LOC_DreamState, LOC_ScotlandEntranceRoad, LOC_EdinburghCastleEntrance, LOC_EdanCastleEntrance, LOC_EdanCastleGatehouse, LOC_SampleCave, LOC_OnTheRoad, LOC_RuinedCoast, LOC_SeaBreezePath

~ Locations = LIST_ALL(Locations)  
    
VAR KnownLocations = () // all known locations
~ KnownLocations = Locations() // limit var to locations defined in Lists

=== function KnowsLoc(location) === //check if the player knows a location exists (NOTE: confusing overlap here with Knows(fact) function)
~return KnownLocations has location

=== function LearnLoc(location) === /// add location to list
{ 
- !KnowsLoc(location):// if unknown
    ~ KnownLocations += location // add to known
    ~ Print("{location} added to travel log") // inform in console
}

VAR VisitedLocations = () // lists all visited locations (NOTE: does NOT work as breadcrumbs/history- a location is only added once, on its first visit)
~ VisitedLocations = Locations() // limit var to locations defined in Lists

=== function HasVisited(location) === // check list of visited locations
~return VisitedLocations has location
    
VAR CurrentLocation = () // contains current location
~ CurrentLocation = Locations() // limit var to locations defined in Lists

VAR PreviousLocation = ()  // contains most recently visited location
~ PreviousLocation = Locations () // limit var to locations defined in Lists

=== function SetLocation(location) === // update current, previous, and all visited locations 
~ LearnLoc(location)

//~ PreviousLocation = Locations()
//~ PreviousLocation += CurrentLocation
~ PreviousLocation = CurrentLocation
//~ CurrentLocation = Locations()
//~ CurrentLocation += location
~ CurrentLocation = location
~ VisitedLocations += location

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

VAR LowRationsLimit = 9 // amount of rations that causes party to complain if you go under it.


=== InsertComplaint === // the complaint scene to insert on low rations
You {LIST_COUNT(Party)>1:and your party} are growing {|ever more }hungry.
->->

=== Starvation ===
You need food to survive idiot.
-> Death

=== TravelingTo(targetLocation) === // used for traveling from a to b. instead of immediately warping, there will be some animation, chance for encounter, use of rations, etc.
~ TargetLocation = targetLocation
~ temp OriginLocation = CurrentLocation
~ SetLocation(LOC_OnTheRoad)
{
- TravelRations == 0: // check if rations left
    -> Starvation
- TravelRations < LowRationsLimit: // check if low on rations
    ~ TravelRations-- // eat the rations
    -> InsertComplaint
- else:
    ~ TravelRations-- // eat the rations
    You eat yummy rations.
} 

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
    -> _ArriveAt(TargetLocation)
- else:
        ~Print("Aborting traversal. Returning to previous location.")
    -> _ArriveAt(OriginLocation)
}
=== _ArriveAt(location) === 
~SetLocation(location) // update curentlocation etc.
{
- location == LOC_ScotlandEntranceRoad:
You arrive at scotland Entrance!
-> ScotlandEntranceRoad
- location == LOC_EdanCastle:
->  CastleEntrance
- location == LOC_SampleCave:
-> SampleSampleCaveScene
- location == LOC_SeaBreezePath:
-> SampleSeaBreesePathScene

- else:
-> DONE// wsl willen we hier een of ander default. ik weet niet eens of deze "DONE"werkt, ik snap nog niet wanneer ik een DONE nodig heb vs een tunnel ("->->").
}

=== RandomTravelEvent ===
->RandomEventsEdanArea->
~Print("A Random event happened!")
->->
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

=== function AllowMap === // including this in the list of choices allows the player to open their map in order to travel to any applicable locations (and exit the current conversation)
/*
Unfortunately. the format for properly including this option is a bit esoteric as of the time of writing (2023-12-24). Brace yourself. It is as follows:

+  [{AllowMap()}] -> MapScreen(-> SceneToReturnTo) 

"SceneToReturnTo" is the scene this choice is IN, i.e. the scene to return to after closing the map without traveling. 
So you must copy past that whole line as a choice and replace the "SceneToReturnTo" bit with whatever knot you are making the choice from.
*/

\{UNITY:OpenMap\}

=== MapScreen (-> returnTo) // the map knot. visit to open the map in unity. This knot should include all possible items on the player's map
~ _OpenMap()
+ { HasVisited(LOC_EdanCastle)} [{_MapChoice(LOC_EdanCastle)}] 
    -> TravelingTo(->CastleEntrance)
+ { HasVisited(LOC_SampleCave)} [{_MapChoice(LOC_SampleCave)}] 
    -> TravelingTo(->SampleSampleCaveScene)
+ { HasVisited(LOC_SeaBreezePath)} [{_MapChoice(LOC_SeaBreezePath)}] 
    -> TravelingTo(->SampleSeaBreesePathScene)
+ [\{UNITY:CloseMap\}]    

\{UNITY:CloseMap()\}
    -> returnTo
    
=== function OpenMap() // call to open the map screen in unity
~ _OpenMap()
    
=== function _OpenMap() === // opens the map screen in unity
\{UNITY:OpenMap()\}

EXTERNAL _OpenMap()
    
=== function _MapChoice(destination) === // used to present an inky choice that will be represented visually on a map in unity. (in ink it simply lists as a normal choice)
\{MapChoice({destination})\}

  === Section_TrackInventory ===
  /* ---------------------------------
   ### System: Keeps track of player inventory and items
  ----------------------------------*/
  -> DONE
  // Inventory is managed by the LIST variable in Ink, which is observed by Unity and matched accordingly.

LIST Items = Knife, Pot, Rope, Lantern, ForagedMushrooms, WornSword // existing items

~ Items = LIST_ALL(Items)  // Full list for Unity syncing. Note Bas: I should maybe  prefix with underscore

LIST Affordances = weapon, tool, cooking, cutting, stabbing, food

~ Affordances = LIST_ALL(Affordances) // Full list for Unity syncing. Note Bas: I should maybe prefix with underscore 

// NOTE BAS: Wait, why do I do that? Check this later. Volgens mij moet ik helemaal niet hier "List all" moeten gebruiken, dat kan ook gewoon vanuit code in unity... En de lijst is hier al gewoon gedefineerd als zo'n lijst.

// rations and money are currently not in inventory sytem:
VAR TravelRations = 107

VAR Money = 10

  
VAR Inventory = () // list of items the player has.
~ Inventory = Items() // restrict to items defined in list

=== function Item_Add(item) // Add item to inventory.
    ~ Inventory += item

=== function ItemChoice(itemOrAffordance) // include an ink choice which can only be taken by using an item from the inventory (in unity. in ink, it'll show as normal). 
\{ItemChoice({itemOrAffordance})\}

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

=== function Item_Consume() // remove item that was just used
~ Item_Remove(UsedItem)

=== function Item_UseAndRemove(item)
~ UsedItem = item
~ Item_Consume()
  
  
  === Section_TrackParty ===
  /* ---------------------------------
   ### System: 
  ----------------------------------*/
  -> DONE
  // WIP
  
   /* ---------------------------------
   #### List: Characters
   ----------------------------------*/
LIST Party = (Player), Alice, Robert

   === Section_TrackAffection ===
  /* ---------------------------------
   #### System: Track opinion of NPCs regarding the player
  ----------------------------------*/
   -> DONE
  // Tracking numeric value for relationship between pc and other characters, and using cutoff points to determine friendly, hostile etc.
  // Can be compared simply by asking e.g.: {AFFHenry >= friendly}





LIST Attitudes = devoted = 100, alligned = 75, friendly = 60, ambivalent = 50, begrudging = 40, hostile = 25, spiteful = 0

=== function GetAttitude(value) ===
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


=== function isAre(list)
	{LIST_COUNT(list) == 1:is|are}


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







