===ScotlandEntranceRoad===
~SetLocation(LOC_ScotlandEntranceRoad)
{PreviousLocation ? LOC_EdinburghCrossroads: Ah, a return visit! Too bad this isn't implemented yet.->ScotlandEntranceRoad.ReturnVisit|->ScotlandEntranceRoad.FirstVisit}
=ReturnVisit
*[Go back the way you came]

~PreviousLocation = LOC_ScotlandEntranceRoad
->EdinburghCrossroads
=FirstVisit
Step by step, you climb the hill. A worn path guides your feet, a pleasant change of pace from the overgrown woodland you found yourself in only two days ago.
*[Put another foot forward]You take another step. And another. Your legs have carried you all the way from Barralon to here: the far North of the Forgotten Isles. 
    **[Readjust your backpack]You shift the weight of the pack on your shoulders. It's heavy, but that's a small burden to bear for the essentials you carry.
        
        Of course, your most important possession is not carried on your back. Chained to your belt is a large, leatherbound book: your journal. You give it a reaffirming tap. A record of all the things you've seen so far and, equally important, plenty of empty pages left to fill with discoveries of the North. 
        
        You can check the contents of your journal by clicking its corresponding icon, on the right. [Vugs note: not yet implemented]
        
        No keeper has set foot here in two hundred years, and even those long dead explorers never ventured deeply into the wilds. Without a doubt, you will be able to complete your task here; to find knowledge not yet stored in the Vault of Barralon. From local folklore to a survey of the wildlife, anything will do. But you did not venture all this way to write down the mundane. You came to find something old. Something ancient, from before the sundering. 
        
        As you contemplate your quest, you realize your feet have carried you to the top of the hill. 
        ***[Survey the landscape]Beneath a bright morning sky fields of flowers roll out before you. The road winds down the hill, slowly making its descent before starting to climb again far in the distance. Its destination: a castle on a rocky outcrop. From your vantage point, you can see the land flattening out beyond it, eventually meeting the inlet sea. 
            ****[Continue following the road towards the castle]
            You continue your journey, the earth crunching beneath your feet. Almost at the halfway point between your hilltop outlook and the castle you find yourself at a crossroads. 
            ~ PreviousLocation = LOC_ScotlandEntranceRoad
            ->EdinburghCrossroads
        
            //As your gaze returns to the path before you, you realize you missed something on your first viewing: a person. Still far in the distance, but unmistakenbly a fellow traveler. They seem to be approaching at a fair pace, aided by a walking stick.
           // ****[Continue following the road]
            //****[Wait for the traveler]
            //****[Turn back]
            //****[Set up an ambush]
            
=== EdinburghCrossroads ===
~ SetLocation(LOC_EdinburghCrossroads)
The road splits here into four directions. The northbound road {!HasVisited(LOC_EdanCastle) and Knows(edanca.Exists): presumably |}leads to {Knows(EdanCastleKnow.IsCastleOnHill):Edan Castle|the castle on the hill}{PreviousLocation ? LOC_RoadToEdanCastle:, from which you came|.} The road South would carry you away from the Northern Lands, perhaps even all the way back home{PreviousLocation ? LOC_ScotlandEntranceRoad:, but you just came from there.|.} You're unsure where the roads leading East and West would take you.
At the center of the crossing you spot a decorated boulder: a Waystone.
->EdinburghCrossroads.Crossing
=Crossing
+[Take the North Road]
You decide to take the North road{PreviousLocation == LOC_EdinburghCastleEntrance: and go and go back the way you came.|  leading to {Knows(EdanCastleKnow.Exists):Edan Castle |the Hilltop Castle.}}
->RoadToEdanCastle
+[Take the East Road]
Sorry buddy, no content East yet!
->EdinburghCrossroads.Crossing
+[Take the South Road]
You decide to {PreviousLocation ? ScotlandEntranceRoad:go back the way you came.|take the Southern Road.}
->ScotlandEntranceRoad
+[Take the West Road]
Sorry buddy, no content West yet!
->EdinburghCrossroads.Crossing
+[Inspect the Waystone]

~ Learn(EdanCastleKnow.Exists)
{You decide to take a closer look at the Waystone in the middle of the crossing. It's decorated in a blocky script, which thankfully matches the sources you were able to study back in Barralon. In the Northern Tongue it reads:|You decide to take another look at the Waystone. It reads:}
"May the blessings of Crìsdaen be upon the honorable traveler

From this stone to Edan Castle, 7 miles Northbound
From this stone to the Sea, 8 miles Eastbound
From this stone to Thahnford, 107 miles Southbound"

A fourth line is also there, but the markings are scratched out. Carved beneath it in a freeform script -that barely passes as legible- is written a single word: "daemons".
->EdinburghCrossroads.Crossing
* [Consult your map]
    You can open your map by clicking on the relevant tab above!
    \[This is where the player learns about the map, and that they can also always open it with in the menu. But, currently, using this option caused a softlock! So sorry buddy, your game is softlocked now. You won't be able to go advance the story after the map closes. I'm working on it, al right?
    {stop}
    (Hope the quickload functionality works at the moment...)
    {OpenMap()}
    ->MapScreen (-> EdinburghCrossroads.Crossing)

+  [{AllowMap()}] -> MapScreen(-> EdinburghCrossroads.Crossing) // dit moet toch meer consise kunnen...?

=== RoadToEdanCastle
//Add first time content, repeated content and randomizer element
~SetLocation(LOC_RoadToEdanCastle)
{~->RandomEventsEdanArea|->CastleEntrance}

=== CastleEntranceFromRoad
// Bas: removed manual edit of previouslocation, als het goed is wordt dit nu bijgehouden door setlocation. ~PreviousLocation = RoadToEdanCastle
// Vugs: Noted. Ik heb de verwijzing naar deze locatie weggehaald gezien hij dan overbodig is, maar ik laat het stukje code nog even staan voor als het onverhoopt ontploft zodat we weten wat er wellicht is misgegaan. 
->CastleEntrance