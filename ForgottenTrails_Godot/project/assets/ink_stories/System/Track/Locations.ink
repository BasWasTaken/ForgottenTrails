// Tracking the players previous, current, and intended location, for use in backtraveling, intermitting random encounters, and more.

LIST Locations = LOC_EdanCastle, LOC_RoadToEdanCastle, LOC_EdinburghCrossroads, LOC_DreamState, LOC_ScotlandEntranceRoad, LOC_EdinburghCastleEntrance, LOC_EdanCastleEntrance, LOC_EdanCastleGatehouse, LOC_SampleCave, LOC_OnTheRoad, LOC_RuinedCoast, LOC_SeaBreezePath, LOC_EdanCastlePrison // Vugs may add items to this list.

~ Locations = LIST_ALL(Locations)  
    
VAR KnownLocations = () // all known locations
~ KnownLocations = Locations() // limit var to locations defined in Lists

=== function KnowsLoc(location) === //check if the player knows a location exists (NOTE: confusing overlap here with knows(fact) function)
~return KnownLocations has location

=== function LearnLoc(location) === /// add location to list
{ 
- !KnowsLoc(location):// if unknown
    ~ KnownLocations += location // add to known
    ~ print("{location} added to travel log") // inform in console
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