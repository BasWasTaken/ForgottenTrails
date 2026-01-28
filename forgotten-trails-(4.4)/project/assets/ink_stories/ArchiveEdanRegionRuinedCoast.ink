=== RuinedCoast ===
~SetLocation(LOC_RuinedCoast)
You find yourself at the shoreline. It's a broken affair; what must once have been some sort of harbour has been almost completely swallowed by the sea. The remaining structures have sunken into the ground, causing them to stand at odd angles. A lone quay still stands upright in defiance of the elements, but you fear that if you put any weight on it that it too will dissappear beneath the waves. 

As your gaze follows the coast in a Northerly direction, you see a large but dilapidated building standing at the water's edge. Looking to the South-East, you see little but jagged rocks being battered by the waves. 
->RuinedCoastChoices1

=RuinedCoastChoices1
+{DEBUG == true}[Add Alice] 
~Party_AddMember(Alice)
->RuinedCoastChoices1
+{DEBUG == true}[Add Robert] 
~Party_AddMember(Robert)
->RuinedCoastChoices1
+[Look around]
{You look around, hoping to find something of interest. Unfortunaly, there doesn't seem to be anything here but rocks and rubble. The quay might yield better results, but it looks unsafe.|You look around again, hoping to catch something you missed earlier. Despite your best efforts, you find nothing of value. Exploring the quay might be worthwile, but it looks unstable.|You look around the area once more, stubbornly hoping to find something, <i>anything</i>, but come up completely empty handed. The quay stands silently amongst the waves, a leering siren tempting you to come explore it; undoubtedly to pull you into the sea only a moment after.|As you look around yet again {{Party has Robert:Robert gently puts his hand on your shoulder, "Haven't we looked around here enough {lad}?"} {Party has Alice:Alice {Party has Robert:chimes in: |speaks up: }{PlayerName}, can we move on? I think I've seen every pebble twice over by now.}|you find nothing. You've turned over every pebble and every rock. You're starting to think the sea's mocking you. Maybe you should go do something else.}} 
    ++[Nevermind then]
    ->RuinedCoastChoices1
    ++[Explore the quay]
+[Take the road West]
->SeaBreezePath
+[Explore to the South-East]
->RuinedCoastSouth
+[Walk towards the Northern building]
->AbandonedShoppingMall
+[{ItemChoice("fishing")}]
~FishingWaters = FISH_FirthofForth
->Fishing
->DONE

=RuinedCoastSouth
->END
= AbandonedShoppingMall
->END