=== RandomEventsEdanArea ===
//To do: add event content
{~->MerchantSiblings1|->Deer|->Downpour} // NOTE FROM BAS: ! removed the "!" because they were not working as intended, i.e., they wer not marking these options as once-only, they were just showing up as text. Maybe "once-only"(!) and "shuffle"(~) are not compatible?

=== MerchantSiblings1 ===
// Note from Bas: This is all happening on the location "ontheroad". I think this is fine but I don't know what your plans are with regards to what should and should not be a location. Like we discussed yesterday, I can imagine if events reach a certain length, and/or there are characters that we want to be able to return to, it might make sense to make a dedicated location for them that we divert into, instead of having it all happen "on the road". But we'll have to think on how exactly to do that. It will involve changing the "TargetLocation" variable to a new location, but I don't know if it needs anything else. 
// Perhaps you could write/think of an example scenario where that would apply (such as a crumbling bridge or cave in) and then we can see what makes sense to us.
~ Party += Alice

The road you're following steadily climbs a gentle hill. As you gaze over the hilltop, you see a small stream of smoke rising upward. 
Once at the top, you spot the source: a cooking fire set by the roadside. A man seems to be stirring something in a pot, while a woman is tending to a horse that's lazily grazing on some grass. The animal is probably responsible for pulling the wagon, which currently stands parked a little ways off the road. 

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
     {Knows(Eileen.Exists): test }
        As you near a distance in which you would no longer need to shout, {players_gender == male:the woman}{players_gender == female:the man}{~!the man|the woman} speaks up: "Hello friend! How does Crìsdaen's wind blow?"
        ***Imply the road is safe.
        ->->
        ***Imply the road is dangerous
        ->->
        ***Keep ignoring them. 
        ->->
        +[{ItemChoice(weapon)}]
        //How to account for multiple traits? Say, weapon and ranged/melee?
Your hand drifts to your weapon. For a moment you hesitate
    ++and you think the better of it. 
    ++but then you strike.
        
=MerchantSiblings1b
->->

    
 =MerchantSiblingsAmbush
    [Insert Ambush Scene Here]
     ->->

=== Deer ===
TestDeer
->->

=== Downpour ===
VAR RemainingRain = 0 
~ RemainingRain = RANDOM(0,3)
//Add companion dependent dialogue
As you're traveling, you start to notice dark clouds gathering overhead.
*Press on
    It's probably nothing. And even so, a little rain can't stop you, right?
    ->CheckRain
*Seek shelter
    You decide not to risk getting drenched and find some cover.
-> CheckRain->Shelter

= CheckRain
{
-RemainingRain>0:
    Sure enough, before too long it starts too rain.
-else:
    Soon enough the dark clouds part away.
}
    ->->

= Shelter
~RemainingRain--
~AdvanceTime()

    {
    -RemainingRain>0:
        The rain pours on.
        + [Sit and wait]
        ->Shelter
        + [Decide to start up again despite the rain.]
        ->->
    -else:
        After waiting a while, the rain lets up and you proceed with your journey.
        ->->
    }
    
==AliceEvent1==
The morning trek unfolds beneath a canvas of muted hues, the sun hidden behind a cloudy veil. As you march, a melody piques your ears: Alice has started humming an unfamiliar tune. You realize it's been a while since you last spoke. 
*[Start a conversation]
You open your mouth to speak, but before you get any words out Alice's humming turns into song. 
->TravelersSong1A
*[Remain quiet]
You decide not to break the silence. Whether she sees this as approval of her humming or another silence to fill you do not know. Regardless, she begins to add words to the song. 
->TravelersSong1A
==TravelersSong1A==

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

Her song reaches a small crescendo, which she seemingly deems a good point to stop her performance. 

*[Clap excitedly]
You clap excitedly, to which she responds with a comically extravagant bow. "Why thank you, dear audience! You're too kind" she says with a laugh. 
->TravelersSong1B
*[Compliment her]
*[Make a neutral remark]
*[Express discomfort with her singing]
You tell her you would appreciate it if she would refrain from bursting out into song. 
{AffAlice =< 30: "Sure, can you do me a favour in return?" she asks "If you could reach up your arse and pull out that giant stick you got wedged in there- No, wait, better yet: shove it up further so it can hit that tiny little peanut you have for a brain. Shouldn't take much with how far up it is already!" ->AliceEvent1Fight | "Oh. Sorry." comes her soft response. ~}
-> END

->END
*[Carry on]
You continue on your path in the same silence as before. You're not entirely sure, but from the corner of your eye you might have seen Alice pout.

=AliceEvent1Fight
*["Now hang on a minute"]
"Now hang on-" "No, you hang on!" she cuts you off. "I've had it up to here with you." 
    **[Apologize]
    **"Look, I just don't like singing, ok?"
    you say. "And I'm a bard" she replies "does that sound like a good combination to you?"
        ***["Maybe not"]
        ***["You're right, that was insensitive of me."] +5
        "...Thank you" she replies. "And sorry for my outburst"
            ****[Carry on]
            ->TravelersSong1B
            ****["I'm sorry too. I'll try to loosen the stick a bit]
            She chuckles "Yes, you better."
            ->TravelersSong1B
*"How about I take it out and start hitting you with it?"
you say. She looks at you coldly. "Violence is your answer to everything isn't it?"
*[Apologize]
*[Ignore her]
You continue walking, paying her outburst no heed. Alice isn't following, however. 

->END
==TravelersSong1B
->END