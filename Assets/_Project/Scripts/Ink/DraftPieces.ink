=== AdelaideScene ===
The door opens with a creak. You see Adelaide enter and quickly slam it shut behind her. Resting with her back against the door and eyes closed, she lets out a deep sigh. 
*[Remain silent]
You stand in silence for a short while. 
->AdelaideScene1
*[Clear your throat]
You clear your throat. 
->AdelaideScene1
*"Good evening Adelaide."
->AdelaideScene1
*"Evening Edie, rough crowd?"
~AffEdie = AffEdie + 5
->AdelaideScene1
=AdelaideScene1
->END


=== Excerptfromthecoastalruins === 
You descend the stairs and find yourself in a narrow corridor. 
->CoastalRuinsB2Corridor
=CoastalRuinsB2Corridor
- (top)
No {TimeOfDay >= Dusk and TimeOfDay <= Night:moonlight|daylight} can find its way in here{LightSource == 1:, but thankfully your {LanternState == 1:lantern|torch} illuminates your surroundings| and without a lightsource it is too difficult to see.}{LightSource == 1:{. The hallway extends forward in a straight line, its end hidden behind the reach of your light. The walls reveal openings in set intervals. Many are open, but some appear to be obstructed by shutters or debris. The first rooms to your left and right are close enough to be peered into, but they appear to be full of rubble.|, revealing the ruined hallway.}}
+[{ItemChoice("lantern")}]
->ItemStateLantern->
-> top
+[{ItemChoice("torch")}]
->ItemStateTorch->
-> top
+{LightSource == 1}[Examine the room on your left.]
-> top
+{LightSource == 1}[Examine the room on your right.]
-> top
+(HallwayDark){LightSource == 0}[Try to find your way through the dark]
    Despite a lack of vision you try to make your way forward. 
    ->CoastalRuinsB2CorridorMiddle
+(HallwayLit){LightSource == 1}[Move further down the hallway.]
->CoastalRuinsB2CorridorMiddle
+{DEBUG == true}[Add Alice] 
~Party_AddMember(Alice)
-> top
+{DEBUG == true}[Add Robert] 
~Party_AddMember(Robert)
-> top

=CoastalRuinsB2CorridorMiddle
{LightSource == 1:You move further down the hallway. You pass what appear to be doorways, but they are blocked by either debris or metal sheets. Noticeably, the doorways alternate with large window frames. Most of these are shuttered by metal. However, the next window frame on your left seems to be open.|By keeping your hand against the wall, you manage to steady yourself and keep to the path. Every now and again, the concrete gives way to metal. You quickly find there's a rythm to it, two steps of concrete, ten steps of metal. Two steps of concrete, ten steps of metal. Two steps of concrete... Air. You stumble. You try to steady yourself but in the dark you can't find anything to hold on to.{Party has Alice && AffAlice =< 50: You feel something brush your back. Is Alice trying to catch you? If she is, she's too late.|}With a smack you collide with the ground. Your knee hurts and you think you've scratched your arm, but otherwise you seem relatively fine.}

//@Bas having a bit of a scuffle with the above section regarding party member presence. I can't manage to have it check for both Alice's presence in the party and her current affection. Mind taking a look?

- (top)
{LightSource == 0:You should be standing at about the midway point in the corridor, but it's too dark to know for sure.}
{LightSource == 1 && CoastalRuinsB2Corridor.HallwayDark:{once:With a light source in hand you manage to survey your surroundings. You're standing in the middle of the hallway. Ahead of you the light reflects from the end of the hallway, which seems to turn left. The metal sheets you passed earlier turn out to be shutters that block doorways. The empty space that caused your fall is right ahead of you and seems to be a large window into a room to your left.}}
    
+[{ItemChoice("lantern")}]
->ItemStateLantern->
-> top
+[{ItemChoice("torch")}]
->ItemStateTorch->
-> top
+{LightSource == 1}[Look through the window frame]
->END
+{LightSource == 1}[Move to the back end of the corridor]
->END
+{LightSource == 1}[Move to the entrance of the corridor]
//Add flavour text
->CoastalRuinsB2Corridor
+{LightSource == 0}[Reach around the dark]
->END
+{LightSource == 0}[Press on ahead]
->END
+{LightSource == 0}[Try to head back to the entrance of the corridor]
//Add flavour text
->CoastalRuinsB2Corridor







