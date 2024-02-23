->EdanInn
=== SYSTEM ===


LIST RubertLocations = RuLEdanInnCommonRoom, RuLEdanInnFirstFloorLanding
LIST AliceLocations = AliceParty, AlLEdanInnCommonRoom, AlLEdanInnFirstFloorLanding
LIST EdieLocations = EdLEdanInnCommonRoom, EdLEdanInnFirstFloorLanding

//Hacky solution due to an issue in the System.ink file.
VAR KnowsAlice = 0

VAR AliveEdie = 1
VAR AliveAlice = 1
VAR AliveRubert = 1

->DONE
=== EdanInn ===
*[Set time dawn]
~TimeOfDay = Dawn
->EdanInnCommonRoom
*[Set time morning]
~TimeOfDay = Morning
->EdanInnCommonRoom

*[Set time Midday]
~TimeOfDay = Midday
->EdanInnCommonRoom

*[Set time Afternoon]
~TimeOfDay = Afternoon
->EdanInnCommonRoom

*[Set time Dusk]
~TimeOfDay = Dusk
->EdanInnCommonRoom

*[Set time Evening]
~TimeOfDay = Evening
->EdanInnCommonRoom

*[Set time Night]
~TimeOfDay = Night
->EdanInnCommonRoom

=EdanInnCommonRoom
{TimeOfDay == Dusk or TimeOfDay == Evening:
~AliceLocations = AlLEdanInnCommonRoom
}
{TimeOfDay >= Morning and TimeOfDay <= Evening:
~RubertLocations = RuLEdanInnCommonRoom
}
~Weather = ClearSkies
~EdieLocations = EdLEdanInnCommonRoom

{{TimeOfDay == Dawn or TimeOfDay == Night:You try the door which, thankfully, turns out to be unlocked. It opens with a slight creak and| The door} gives way to a long, L-shaped room, stretching some 40 yards back and 15 across. In the L's hook, to the right of the entrance, an ashwood bar stands worn but well kept. {RubertLocations == RuLEdanInnCommonRoom:Behind it, {Knows(Rubert.Name):Rubert|a burly man} is cleaning a mug. He nods at you in welcome as you enter.} The rest of the room is filled with all manner of tables and chairs, made from a variety of materials such as metal, wood and stone{TimeOfDay >= Morning and TimeOfDay <= Dusk:, which are occupied by a few patrons. {Knows(Edie.Name):Adelaine|A servant girl} is busily cleaning an empty table in the back}{TimeOfDay == Evening:, which are almost all occupied by patrons. {Knows(Edie.Name):Adelaine|A servant girl} is busily running to and fro to bring people their orders}. In the center of the left wall a hearth is burning, providing the room with warmth and a flickering of orange light{Weather >= LightRain:, offsetting the clatter of rain against the windows}. {TimeOfDay == Morning:The morning sun beams through the windows that are set into the right wall}{TimeOfDay == Midday and Weather <= LightClouds:Sunlight pours in through the windows that are set into the front and right sides of the room}{TimeOfDay == Dusk and Weather <= LightClouds:The Twilight gleam seeps in through the windows that are set into the front side of the building}{TimeOfDay == Evening:Lit chandeliers hang from the suprisingly high ceiling, giving the room some extra light. }{TimeOfDay == Dusk or TimeOfDay == Evening and AliceLocations == AlLEdanInnCommonRoom:{TimeOfDay == Dusk:The conversations between the few early visitors|the patrons' banter} is accompanied by the gentle strum of a guitar. Its source is found in the back corner: {KnowsAlice == 1:Alice|a young woman with goldenbrown hair} is seated on a stool, playing her instrument}{TimeOfDay == Dawn or TimeOfDay == Night:{Knows(Edie.Name):Adelaine|A girl} sits crouched in front of the fire, stoking it with a poker. As you enter, she looks up at you in surprise. -> EdanInnEntranceConvoEdie}.|You step into the Edani Inn. The hearth is burning merrily{TimeOfDay == Midday and Weather <= LightClouds:, even though the sun is at its zenith,}{TimeOfDay >= Morning and TimeOfDay <= Dusk: and a few patrons are having a drink or bite to eat.}{TimeOfDay == Evening:, to the joy of the many patrons packed into the place.}{TimeOfDay == Night or TimeOfDay == Dawn and EdieLocations == EdLEdanInnCommonRoom:, undoubtedly kept going by {Knows(Edie.Name):Adelaide,|the servant girl} who's currently tending to it.}}

->EdanInnCommonRoomChoices
=EdanInnEntranceConvoEdie
<BR>
{Knows(Edie.Name):"Oh, it's you {PlayerName}! Sorry, I wasn't expecting anyone at this hour{AffEdie >= 50: but please, do come in!|.}"|"Oh, I'm sorry Sir/Madam/They, I wasn't expecting anyone at this hour."}
//Missing a sir/madam option in the pronouns. 
->EdanInnCommonRoomChoices

=EdanInnCommonRoomChoices
+[Look at...]
    ++{AliceLocations == AlLEdanInnCommonRoom} \ [{KnowsAlice == 1:Alice|The bard}]
    ->EdanInnCommonRoomConvoAlice
    ++{RubertLocations == RuLEdanInnCommonRoom} \ [{Knows(Rubert.Name):Rubert|The bartender}]
    ->EdanInnCommonRoomChoices
    ++{EdieLocations == EdLEdanInnCommonRoom} \ [{Knows(Edie.Name):Adelaine|The servant girl}]
    ->EdanInnCommonRoomConvoEdie
    ++[Actually, nevermind.] 
    ->EdanInnCommonRoomChoices
+[Talk to...]
    ++{AliceLocations == AlLEdanInnCommonRoom} \ [{KnowsAlice == 1:Alice|The bard}]
    ->EdanInnCommonRoomChoices
    ++{RubertLocations == RuLEdanInnCommonRoom} \ [{Knows(Rubert.Name):Rubert|The bartender}]
    ->EdanInnCommonRoomConvoRubert
    ++{EdieLocations == EdLEdanInnCommonRoom} \ [{Knows(Edie.Name):Adelaine|The servant girl}]
    ->EdanInnCommonRoomChoices
    ++[Actually, nevermind.] 
    ->EdanInnCommonRoomChoices
+[Go...]
    ++[to the first floor]
    ->EdanInnFirstFloor
    ++[outside]
    ->EdanMarketSquare
    ++[Actually, nevermind.]
    ->EdanInnCommonRoomChoices

=EdanInnCommonRoomLookAt
+{AliceLocations == AlLEdanInnCommonRoom} \ [{KnowsAlice == 1:Alice|The bard}]
->EdanInnCommonRoomLookAt
+{RubertLocations == RuLEdanInnCommonRoom} \ [{KnowsAlice == 1:Rubert|The bartender}]
->EdanInnCommonRoomLookAt
+{EdieLocations == EdLEdanInnCommonRoom} \ [{Knows(Edie.Name):Adelaine|The servant girl}]
->EdanInnCommonRoomLookAt
+[Actually, nevermind.] 
->EdanInnCommonRoomChoices

+[Actually, nevermind.] 
->EdanInnCommonRoomChoices

=EdanInnCommonRoomConvoAlice
->EdanInnCommonRoomChoices

=EdanInnCommonRoomConvoEdie
->EdanInnCommonRoomChoices

=EdanInnCommonRoomConvoRubert
->EdanInnCommonRoomChoices

=EdanInnFirstFloor
{You climb the stairs and find yourself on a landing that runs the length of the building. On your right hand side you find three doors that, presumably, lead to the guest rooms|You find yourself on the first floor landing}.
->EdanInnFirstFloorChoices

=EdanInnFirstFloorChoices
+(Look)[Look at...]
    ++[The landing]
    The landing appears well kept. A painting hangs on the left wall. On the opposite side there are three doors that are currently closed.
    ->EdanInnFirstFloorChoices
    ++{AliceLocations == AlLEdanInnFirstFloorLanding} \ [{KnowsAlice == 1:Alice|The bard}]
    ->EdanInnFirstFloorChoices
    ++{RubertLocations == RuLEdanInnFirstFloorLanding} \ [{KnowsAlice == 1:Rubert|The bartender}]
    ->EdanInnFirstFloorChoices
    ++{EdieLocations == EdLEdanInnFirstFloorLanding} \ [{Knows(Edie.Name):Adelaine|The servant girl}]
    ->EdanInnFirstFloorChoices
    ->END
+(Talk)[Talk to...]
    ++{AliceLocations == AlLEdanInnFirstFloorLanding} \ [{KnowsAlice == 1:Alice|The bard}]
    ->EdanInnFirstFloorChoices
    ++{RubertLocations == RuLEdanInnFirstFloorLanding} \ [{KnowsAlice == 1:Rubert|The bartender}]
    ->EdanInnFirstFloorChoices
    ++{EdieLocations == EdLEdanInnFirstFloorLanding} \ [{Knows(Edie.Name):Adelaine|The servant girl}]
    ->EdanInnFirstFloorChoices
    ++ ->
        There's no one here to talk to. -> EdanInnFirstFloorChoices
+[Go...]
    ++into the first room.
    You try the door, but it seems to be locked.
    ->EdanInnFirstFloorChoices
    ++into the second room.
    You try the door, but it seems to be locked.
    ->EdanInnFirstFloorChoices
    ++into the third room. 
    You try the door, but it seems to be locked.
    ->EdanInnFirstFloorChoices
    ++[downstairs.]
    ->EdanInnCommonRoom
->END

=EdanMarketSquare
You step outside onto the market square.
+[Go back inside]
->EdanInnCommonRoom
->END