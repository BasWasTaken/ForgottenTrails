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
->CoastalRuinsB2
=CoastalRuinsB2
You descend the stairs and find yourself in a narrow corridor. 
- (top)
No {TimeOfDay >= Dusk and TimeOfDay <= Night:moonlight|daylight} can find its way in here{LightSource == 1:, but thankfully your {LanternState == 1:lantern|torch} illuminates your surroundings| and without a lightsource it is too difficult to see.}{LanternState == 1 or TorchState == 1:{. The hallway extends forward in a straight line, its end hidden behind the reach of your light. The walls reveal openings in set intervals, with many showing a small room behind them, while some appear to be obstructed by shutters or debris. The first rooms on your left and right are close enough to be peered into, but they appear to be full of rubble.|, revealing the ruined hallway.}}
+[{ItemChoice("lantern")}]
->ItemStateLantern->
-> top
+[{ItemChoice("torch")}]
->ItemStateTorch->
-> top
+{LightSource == 1}Test
->END
+{LightSource == 0}[Try to find your way through the dark]
    Despite a lack of vision you try to make your way forward. 