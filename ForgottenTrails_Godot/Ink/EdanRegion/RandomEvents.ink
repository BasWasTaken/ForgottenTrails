// --------- Shared ---------
=== RandomEventsEdanArea ===
//To do: add event content
{~->MerchantSiblings1|->Deer|->Downpour | ->FindRations} 

// --------- Vugs  ---------
=== MerchantSiblings1 ===
// Note from Bas: This is all happening on the location "ontheroad". I think this is fine but I don't know what your plans are with regards to what should and should not be a location. Like we discussed yesterday, I can imagine if events reach a certain length, and/or there are characters that we want to be able to return to, it might make sense to make a dedicated location for them that we divert into, instead of having it all happen "on the road". But we'll have to think on how exactly to do that. It will involve changing the "TargetLocation" variable to a new location, but I don't know if it needs anything else. 
// Perhaps you could write/think of an example scenario where that would apply (such as a crumbling bridge or cave in) and then we can see what makes sense to us.
~Party_AddMember(Alice) // Bas@Vugs: replaced "Party += Alice" with the "add" function".~ Party += Alice

The road you're following steadily climbs a gentle hill. Gazing over the hilltop, you can see a small stream of smoke rising upward. 
Once at the top, you spot the source: a cooking fire set by the roadside. A man seems to be stirring something in a pot, while a woman is tending to a horse that's lazily grazing on some grass. A wagon stands a little ways of the road, most likely the animal's burden. 

*[Continue on your way]
    You decide to just continue on your way. However as you descend the hill, the woman spots you. She seems to say something to the man, prompting him to look up. Both give you a pleasant wave. 
    **[Wave back]
        You return their gesture. By now you're near enough to see their expressions and they seem to be smiling at you. The pair appears to be in their early twenties, both with the same brown hair. The man's is a tussled affair, while the woman's is tied in a loose braid that's hanging over her right shoulder. 
        ->MerchantSiblings1a
        
    **[Ignore them]
        You ignore their gesture. By now you're near enough to see their expressions and they exchange a worried glace. The pair appears to be in their early twenties, both with the same brown hair. The man's is a tussled affair, while the woman's is tied in a loose braid that's hanging over her right shoulder. 
        
        {Party has Alice or Robert: -> MerchantSiblingsPartyConvo1}
        
        ->MerchantSiblings1a
        
*[Give a travelers' greeting]
    You wave at the pair with one hand, and cup the other around your mouth to amplify your shout. "Hail friends!" you bellow down the hill, "How does Crìsdaen's wind blow?"
    Both look up from their tasks, their gaze directed at you. They return your wave and {players_gender == male:the woman}{players_gender == female:the man} {players_gender == nonbinary: {~the man|the woman}} returns your greeting: "it blows fair, friend!", implying that the road ahead is safe. 
        **[Approach them]
            You continue your way down the road, approaching the pair. By now you're near enough to see their expressions and they seem to be smiling at you. Both appear to be in their early twenties, sharing the same brown hair colour. The man's is a tussled affair, while the woman's is tied in a loose braid that's hanging over her right shoulder.
        ->MerchantSiblings1b
*Ambush them
    Just two travelers on the road? Might be easy pickings. 
    -> MerchantSiblingsAmbush
*Turn back around
    For whatever reason, you decide you'd rather not be spotted by the pair and turn back the way you came. 
    ~ SucceededRandomEvent = false
    ->->
    
=MerchantSiblingsPartyConvo1
        {Party has Alice + Robert: still needs text| {Party has Alice: Alice gently nudges you. "Maybe we should say something? Wouldn't want them to think we're wraiths or have ill intent."}{Party has Robert: Robert speaks up. "{lad}, maybe we should return their greeting, mhm?"}}
                *"You're right."
                Deciding to heed {Party has Alice + Robert: their|{Party has Alice:her}{Party has Robert:his}} counsel, you give the pair a wave back. They visibly relax and move to meet you on the path. 
                *"Perhaps, but I don't trust them."
                *"Would be funny to scare them though."
                *"I just really don't feel like dealing with people right now."

=MerchantSiblings1a
{KnowsAbout(EileenKnowState): test }
    As you near a distance in which you would no longer need to shout, {players_gender == male:the woman}{players_gender == female:the man}{~!the man|the woman} speaks up: "Hello friend! How does Crìsdaen's wind blow?
    - (test)
        <- UnprovokedAttackMerchants(->test)
        ***Imply the road is safe.
        ->->
        ***Imply the road is dangerous
        ->->
        ***Keep ignoring them. 
        ->->
        
        
        //This is going to provide issues. Discuss for meeting.
=UnprovokedAttackMerchants(->returnTo)
//How to account for multiple traits? Say, weapon and ranged/melee?
+[{ItemChoice("weapon")}]
Your hand drifts to your weapon. For a moment you hesitate
    ++and you think the better of it. 
    ->returnTo 
    ++but then you strike.  
    ->DONE
=MerchantSiblings1b
->->

    
 =MerchantSiblingsAmbush
    [Insert Ambush Scene Here]
     ->->
    
==AliceEvent1==
~ FadeToImage(BG_Road1, 1)
The morning trek unfolds beneath a canvas of muted hues, the sun hidden behind a cloudy veil. As you march, a melody piques your ears: Alice has started humming an unfamiliar tune. You realize it's been a while since you last spoke. 
*[Start a conversation]
You open your mouth to speak, but before you get any words out Alice's humming turns into song. 
->TravelersSong1A
*[Remain quiet]
You decide not to break the silence. Whether she sees this as approval of her humming or another silence to fill you do not know. Regardless, she begins to add words to the song. 
->TravelersSong1A
==TravelersSong1A==
~ Music_Play(TabiNoTochuu, 1)
As I wander down
this endless road,
on a journey all alone.

I wonder if
I'll find my way;
reach the path back home.

On this lonely road
I met a traveler
who sang a broken song.

And somehow I knew
to mend their tune,
and I sang along:

"In an Eastern land down by the sea
there stands a tower made of gold,
where a Northern queen blessed the moon 
and received blackened keys threefold"
~ Music_Stop()

Her song reaches a small crescendo, which she seemingly deems a good point to stop her performance. 

*[Clap excitedly]
You clap excitedly, to which she responds with a comically extravagant bow. "Why thank you, dear audience! You're too kind" she says with a laugh. 
~ AffAlice += 5// How to do this? 
// @Vugs zo!
->TravelersSong1B
*[Compliment her]
You compliment her singing. She smiles, "Thanks, {PlayerName}."
->TravelersSong1B
*[Express discomfort with her singing]
You tell her you would appreciate it if she would refrain from bursting out into song. 
{AffAlice =< 30: "Sure, can you do me a favour in return?" she asks "If you could reach up your arse and pull out that giant stick you got wedged in there- No, wait, better yet: shove it up further so it can hit that tiny little peanut you have for a brain. Shouldn't take much with how far up it is already!" ->AliceEvent1Fight | "Oh. Sorry." comes her soft response. ~ ->->}
*[Carry on]
You continue on your path in the same silence as before. You're not entirely sure, but from the corner of your eye you might have seen Alice pout.
//~AffAlice - 5
->->

=AliceEvent1Fight
*["Now hang on a minute"]
"Now hang on-" "No, you hang on!" she cuts you off. "I've had it up to here with you." 
    **["I... You're right. Sorry, that was rude of me."]
    "...Thank you" she replies. "And sorry for my outburst"
            ***[Carry on]
            ->TravelersSong1B
            ***["I'm sorry too. I'll try to loosen the stick a bit]
            She chuckles "Yes, you better."
            ->TravelersSong1B
    **"Look, I just don't like singing, ok?"
    you say. "And I'm a bard" she replies "does that sound like a good combination to you?"
        ***["Maybe not"]
            "Then maybe it's best if we part ways here." she replies. 
            ****["Yes, maybe that's for the best."]
                She smiles wryly. "Alright then, farewell {PlayerName}." She pauses. {AffAlice >=20:"...Don't die out there, alright?"|"Hope you find what you're looking for."}
                            ~ Party -= Alice
                            ->->
            ****["I'd still like to make this work"]
                {AffAlice >=20: She looks you in the eyes for a bit, then sighs. "Alright, but if you want to make this work, my singing is just something you'll have to put up with.->AliceEvent1FightA|"You've had that chance, I don't see this ever being a pleasant partnership.->AliceEvent1FightB}
        ***["You're right, that was insensitive of me."] +5
        "...Thank you" she replies. "And sorry for my outburst"
            ****[Carry on]
            ->TravelersSong1B
            ****["I'm sorry too. I'll try to loosen the stick a bit]
            She chuckles "Yes, you better."
            ->TravelersSong1B
//*"How about I take it out and start hitting you with it?" [you say.] 
//She looks at you coldly. "Violence is your answer to everything isn't it?"
*["I... You're right. Sorry, that was rude of me."]
    "...Thank you" she replies. "And sorry for my outburst"
            **[Carry on]
            ->TravelersSong1B
            **["I'm sorry too. I'll try to loosen the stick a bit]
            She chuckles "Yes, you better."
            ->TravelersSong1B
*[Ignore her]
You continue walking, paying her outburst no heed. Alice isn't following, however. 
    **Whatever, that's her problem. 
    ~ Party -= Alice
    ->->
    **[Turn around]
    You turn around to look for her. She hasn't moved since she started shouting, her arms folded and her gaze directed some ways in the distance. 
        ***[Walk back towards her]
        You turn around and approach her. She doesn't look at you. 
            ****["Look, if you can't handle some constructive criticism we might have a problem."]
            "Look, if you-" "No, you look here {Mx}!" she cuts you off. "I've had it up to here with you." 
                *****["Sorry, that was rude of me."]
                "...Thank you" she replies. "And sorry for my outburst"
                        ******[Carry on]
                          ->TravelersSong1B
                        ******["I'm sorry too. I'll try to loosen the stick a bit]
                        She chuckles "Yes, you better."
                        ->TravelersSong1B
                *****[Carry on]
                ->TravelersSong1B
                *****["I'm sorry too. I'll try to loosen the stick a bit]
                She chuckles "Yes, you better."
                ->TravelersSong1B
            ****"Look, I just don't like singing, ok?"
            <> you say. 
            "And I'm a bard" she replies "does that sound like a good combination to you?"
                *****["Maybe not"]
                "Then maybe it's best if we part ways here." she replies. 
                    ******["Yes, maybe that's for the best."]
                        She smiles wryly. "Alright then, farewell {PlayerName}." She pauses. {AffAlice >=20:"...Don't die out there, alright?"|"Hope you find what you're looking for."}
                            ~ Party -= Alice
                            ->->
                    ******["I'd still like to make this work"]
                    {AffAlice >=20: She looks you in the eyes for a bit, then sighs. "Alright, but if you want to make this work, my singing is just something you'll have to put up with.->AliceEvent1FightA|"You've had that chance, I don't see this ever being a pleasant partnership.->AliceEvent1FightB}
                *****["You're right, that was insensitive of me."] +5
                "...Thank you" she replies. "And sorry for my outburst"
                    ******[Carry on]
                    ->TravelersSong1B
                    ******["I'm sorry too. I'll try to loosen the stick a bit]
                    She chuckles "Yes, you better."
                    ->TravelersSong1B

->END
=AliceEvent1FightA
*["If that what it takes."]
She nods and the both of you continue your journey, now with slightly less humming. 
->->
*["Yes, sorry about that."]
Alice smiles faintly as you apologize. "Yes, well, let's not dwell any more on the matter shall we?"
->TravelersSong1B
*["Urgh, can you give it a rest you screeching banshee?"]
If looks could kill, you'd be very, very dead. Fortunately for you, all she does is storm off. 
~ Party -= Alice
->->
=AliceEvent1FightB
"Goodbye." she says coldly. And with that remark, your traveling days together come to an end.
~ Party -= Alice
->->
==TravelersSong1B
*"What's the song called?"
<> you ask.
"'The Traveler's Road'... I think. That's what I learned it by anyway, but I've also heard people calling it 'Journey's Neverend' and 'Crisdain's Song', although the latter is only used this far North. ->TravelersSong1B
*"So what's the song about?" 
<> you ask.
"Well, actually I'm not quite sure. It's a traveler's song, certainly, but I haven't a clue what the tower or keys are supposed to mean. Or what queen it's referencing, for that matter. I once met a man who told me it was originally written in some strange tongue and that the meaning had been lost in translation. But that same man was deep in his cups and emptied his stomach on the poor bartender just a little later, so I wouldn't attribute too much value to it. ->TravelersSong1B
*"Was that the whole song?"
<> you ask.
She shakes her head, "There are a few more verses{AffAlice >=50:." And with a wink she adds: "If you're good, I might sing them for you someday."|, but I don't feel like singing those right now.} ->TravelersSong1B
*[Continue your journey]
    You decide to carry on.
    ->->
    
    
    
// --------- Bas ---------
=== FindRations ===
    You come across a field where you notice numerous animal tracks and dropppings. It seems like much game took to living around these parts, meaning this might be a goood opportunity for some hunting to top up your rations- and more to the point, enjoy something other than dried jerky for once.
    * [Take the time to hunt some game.]
        ~Time_Advance()
        You take some time to hunt small game. After a few hours you have decent bit of meat to go around. You light a fire and decide to each have some freshly cooked meat and preserve the rest for later consumption. 
        ~TravelRations+=2*LIST_COUNT(Party)
        ->->

    * [We don't have the time or energy for a hunting expedition right now.]
        text text
        ->->


=== Deer ===
TestDeer
->->

VAR rainStart = -1
VAR rainStop = -1
=== function turnsTilRaining
~temp turns = TURNS_SINCE(->Downpour) + ExtraTime
{
-rainStart < turns && turns < rainStop:
0 // return 0 meaning it is now raining
- rainStart > turns:
{rainStart-turns} // return turns remaining until rain starts
- turns > rainStop:
{rainStop-turns}// return how many turns ago the rain stopped
}

=== Downpour ===
- (calc) //set raining false
~ rainStart = RANDOM(0, 28) // determine rain start 
~ rainStop = RANDOM(rainStart, rainStart+28) // determine rain end
~Print("Randomly determined rain will start {rainStart} turns from now, and last {rainStop-rainStart} turns")
- (tell)
As you're traveling, you start to notice dark clouds gathering overhead.

*(dontSeekShelter)[Press on]
    {seekShelter:A|It's probably nothing. And even so, a} little rain can't stop you, right?
    VAR shelterFound = false
    -> pressOn
*(seekShelter)[Seek shelter]
    You decide not to risk getting drenched and find some cover. 
    VAR shelterDistance = 0
    ~ shelterDistance = RANDOM(0,2)
    -> lookForShelter

= pressOn
+ {turnsTilRaining>0} [stubbornly keep walking] You stubbornly keep walking.
    ->checkTime->pressOn
+ {turnsTilRaining==0} [live with your decision] You stubbornly keep walking.
    ->-> // escape!
+ {turnsTilRaining<0} [keep walking but now you're dry]
    ->-> // escape!

// now keep redirectinguntil the rain is passed or we give up, and at that point, continue out of the tunnel back to the travel
VAR i = 0
= checkTime
{
-i>7:
~Time_Advance()
~i=0
- else:
~i ++
}
-(describeWeather)
{
- turnsTilRaining>0:
    Skies go {|ever} darker...
- turnsTilRaining == 0:
    {Sure enough, it starts too rain.|The rain pours on.}
- turnsTilRaining<0:
    {The rain lets up.|The air feels damp from the {recent|} rain.}
}

-(describeSituation)
{
- shelterFound:
->WaitInShelter
- seekShelter && !dontSeekShelter:
->lookForShelter
- else:
    {
    - seekShelter && dontSeekShelter:
    (if only you had not given up on shelter)
    - else:
    (if only you had found shelter)
    }
(describe whether you are wet or not.)
}
->-> // does this go to press on?


= lookForShelter // go to checkrain
{ 
    - shelterDistance<=0:
    -> findShelter
    - else:
    ~shelterDistance-=1
    You {|still} can't find any decent cover to shield you from the rain {turnsTilRaining>0:looming overhead|drenching your clothes}
}

+ [keep searching]
    ->checkTime
+ [Give up and continue your journey]
    ->dontSeekShelter // go to the press on option anyway
    
= findShelter
~ shelterFound = true
You {TURNS_SINCE(->seekShelter)>1:finally} find some shelther: (describe shelter)//REPLACE WITH ->naturalShelter
- (inShelter)
Grateful for the little cover you found, you{glue}
- (rest)
* (backpackoff) [Take of your backpack]
    take of your pack{glue} ->rest
* (comfortable)[Find a comfortable position]
    ** {backpackoff}[Lie down on the soft moss]
    lie down on the soft moss,{glue} ->rest
    ** [Sit down against (a surface depending on the shelter)]
    sit down, lean back,{glue}->rest
+ {comfortable}[rest your eyes]
    and rest your eyes. ->checkTime
+ [Wait]
    {|and} wait. ->checkTime
+ {!comfortable} [change your mind and continue your journey]
    ->dontSeekShelter
  
= WaitInShelter
You are in your shelter.
    + {turnsTilRaining>=0}[continue waiting]
        VAR ExtraTime = 0 
        ++[wait just a few minutes]
        ~ ExtraTime+=1
        ->checkTime
        ++[zone out for a while]
        ~ ExtraTime+=RANDOM(3, 6)
        ->checkTime
        ++[take a nap]
        ~ ExtraTime+=RANDOM(8, 36)
        ->checkTime
    * {turnsTilRaining>=0} [Give up and continue your journey]
        ->dontSeekShelter
    * {turnsTilRaining<0} [Gather your things and set out.]
    After waiting a while, the rain lets up and you proceed with your journey.
    - ->->
// WIP UNDER THIS LINE ______


    
    





    
== naturalShelter
//ToDO: make this dependent on environment
{~->Overhang|->Leaves|->Cave}
->->

== Overhang
VAR Shelter = ->Overhang
A nearby mountainside offers a bit of dry soil by way of an overhang a few meters off the ground. It doesn't look like the most comfortable place to pass the time, but anything beats getting soaked. {turnsTilRaining<2:Feeling the clouds grow thicker, you quickly|You} make your way over to the rocky wall. 
* [Enter]
Taking care not to hit your head on the ceiling, which at points is rather closer to the ground then you hoped for, you squeeze your way into your dry haven. 
{
-LIST_COUNT(Party)>2:
You help your companions in as well, although each subsequent fellow makes the room less and less comfortable.
-LIST_COUNT(Party)>1:
You help your companion in as well.
- else:
You are glad to be on your own here, as you doubt this place would have accomodated any more travelers.
} 
->->
== Leaves
~Shelter = ->Leaves
Some big ol' trees provide some meager cover against the elements.
//ToDoBas: elaborate
->->
== Cave
~Shelter = ->Cave
You find a cave to hide in.
//ToDoBas: elaborate
->->