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
    ~print("Heads! Encounter was true!") // who knows what might happen now
    -> RandomTravelEvent -> 
- else:
    ~print("Tails! Encounter was false!") // succeed automatically
 }

{
- SucceededRandomEvent:
        ~print("Proceeding with traversal.")
    ~_ArriveAt(TargetLocation)
    ->targetScene
- else:
        ~print("Aborting traversal. Returning to previous location.")
    ~_ArriveAt(OriginLocation)
}
-    ->->
=== function _ArriveAt(location) === 
~SetLocation(location) // update curentlocation etc.


=== RandomTravelEvent ===
->RandomEventsEdanArea->
~print("A Random event happened!")
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
//+ { HasVisited(LOC_SampleCave)} [{_MapChoice(LOC_SampleCave)}] 
//    -> TravelingTo(LOC_SampleCave, ->SampleSampleCaveScene)->returnTo
//+ { HasVisited(LOC_SeaBreezePath)} [{_MapChoice(LOC_SeaBreezePath)}] 
//    -> TravelingTo(LOC_SeaBreezePath, ->SampleSeaBreesePathScene)->returnTo
+ [\{UNITY:CloseMap\}]    
    \{UNITY:CloseMap()\}
-     (done) -> returnTo

=== function OpenMap() // call to open the map screen in unity
~ _OpenMap()
    
=== function _OpenMap() === // opens the map screen in unity
\{UNITY:OpenMap()\}

EXTERNAL _OpenMap()
    
=== function _MapChoice(destination) === // used to present an inky choice that will be represented visually on a map in unity. (in ink it simply lists as a normal choice)
\{MapChoice({destination})\in unity.