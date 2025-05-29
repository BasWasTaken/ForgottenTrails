// --------- Vugs  ---------
=== Awakening ===
~TimeOfDay = Dawn
~SetLocation(LOC_ScotlandEntranceRoad)
You awaken with a start. A dream after all. Of course it was, now that you look back on it. 
You turn on your back, the small canvas tent that shields you from the elements coming into view. You can smell the morning forest and the smouldering remains of your campfire.
->Awakening.Tent

=Tent

You decide to
<- AllowMap(->Tent)
<- AllowPartyScreen(-> Tent)

*(PackUpEarly)[...pack up]{aglue} pack up.
Or you would, but the grumbling of your stomach tells you that it's not going to be a fun hike without something to eat.

You can check your current hunger level on the right. As time passes, your need for food will increase. You wouldn't be the first adventurer to die of starvation, so keep an eye on it! [Vugs note: not yet implemented] 
->Tent
*[...make some breakfast first]{aglue} make some breakfast.
Your stomach rumbles, and what poor sort would head off without a proper meal first anyway? 
The campfire has yet to go out completely and should be easy to light. With the help of some kindling you gathered last night, it doesn't take you long to get a nice flame going.
The next step would be to hang your pot over the fire, but where did you leave the damn thing?

~item_add(Pot)
You can find your belongings by clicking on the backpack icon on the right. You can then right click an item and select 'use' to put it into action.
    **[{ItemChoice("cooking")}]
    You set up the small iron stakes and hang the pot on it, placing it nice and snug over the fire. Now, to put some food in. 
        ***[{ItemChoice("food")}]
        {item_remove_last_used()}
        You drop the {UsedItem} into the pot, resulting in a satisfying sizzle. Good thing master Pedrál went through that herbology phase last semester, or you would have left them by the wayside in fear of poison.
        A few minutes of stirring and a sprinkle of salt later, your woodland meal is ready to eat. It's not something you'd serve to a king or worse, a mother-in-law, but your stomach is grateful for it nevertheless. 
        {Tent.PackUpEarly: |You can check your current hunger level on the right. As time passes, your need for food will increase. You wouldn't be the first adventurer to die of starvation, so keep an eye on it! [Vugs note: not yet implemented]}
        Fully fed, it's about time to head off if you still want to make some decent progress today. 
        While outside, you can get a general sense of what time it is by looking at the time indicator in the top right. Most actions and events will cause time to progress. The world will differ depending on the day-night cycle, but do not worry on missing out. A Keeper's quest is as much -if not more so- about the journey than it is about the destination. And if Fate wants certain people to meet, she always finds a way. [Vugs note: not yet implemented, time is currently tracked in Inky but needs a coding addition] 
            ~TimeOfDay = Morning
            ****[Pack up]
            You gather your belongings and make sure the fire is thorougly smothered by a heap of sand. With a few steps you move from the small clearing where you made your camp back to the road. Looking north, it slopes gently upward.
            ->Awakening.PackUp
            
*...sleep in[]. One way to shake a nightmare is with a new dream but sadly, sleep doesn't come.
->Tent

=PackUp
*So you take the road North.
->ScotlandEntranceRoad
+But you decide to head south.
{Southbound is the way you came. The goal of your journey is the other way.|You came from there only yesterday. It would be such a waste to turn back now.|Your master would be sorely dissappointed if you came back without accomplishing what you set out to do.|Because somehow, you feel compelled to do so. You feel like if you take one step in that direction, you cannot help but walk all the way back home.|And end your northern adventures right here, only to begin the long journey home.->END} ->Awakening.PackUp