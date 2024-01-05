=== RandomEventsEdanArea ===
//To do: add event content
{~->MerchantSiblings|->Deer|->Downpour} // NOTE FROM BAS: ! removed the "!" because they were not working as intended, i.e., they wer not marking these options as once-only, they were just showing up as text. Maybe "once-only"(!) and "shuffle"(~) are not compatible?

=MerchantSiblings
// Note from Bas: This is all happening on the location "ontheroad". I think this is fine but I don't know what your plans are with regards to what should and should not be a location. Like we discussed yesterday, I can imagine if events reach a certain length, and/or there are characters that we want to be able to return to, it might make sense to make a dedicated location for them that we divert into, instead of having it all happen "on the road". But we'll have to think on how exactly to do that. It will involve changing the "TargetLocation" variable to a new location, but I don't know if it needs anything else. 
// Perhaps you could write/think of an example scenario where that would apply (such as a crumbling bridge or cave in) and then we can see what makes sense to us.
~ FadeToImage(BG_SketchTest, 1)
The road you're following steadily climbs a gentle hill. As you gaze over the hilltop, you see a small stream of smoke rising upward. 
Once at the top, you spot the source: a cooking fire set by the roadside. A man seems to be stirring something in a pot, while a woman is tending to a horse that's lazily grazing on some grass. The animal is probably responsible for pulling the wagon, which currently stands parked a little ways off the road. 

*[Continue on your way]
    You decide to just continue on your way. However as you descend the hill, the woman spots you. She seems to say something to the man, prompting him to look up. Both give you a pleasant wave. 
    **[Wave back]
        You return their gesture. By now you're near enough to see their expressions and they seem to be smiling at you. The pair appears to be in their early twenties, both with the same brown hair. The man's is a tussled affair, while the woman's is tied in a loose braid that's hanging over her right shoulder. 
        ->MerchantSiblings1a
        
    **[Ignore them]
        You ignore their gesture. By now you're near enough to see their expressions and they exchange a worried glace. The pair appears to be in their early twenties, both with the same brown hair. The man's is a tussled affair, while the woman's is tied in a loose braid that's hanging over her right shoulder. 
        ->MerchantSiblings1a
        
*[Give a travelers' greeting]
    You wave at the pair with one hand, and cup the other around your mouth to amplify your shout. "Hail friends!" you bellow down the hill, "How does Crìsdaen's wind blow?"
    Both look up from their tasks, their gaze directed at you. They return your wave and {players_gender == male:the woman}{players_gender == female:the man} {players_gender == nonbinary: {~the man|the woman}} returns your greeting: "fair winds blew on our path friend!"
    ->->
*Ambush them
    -> MerchantSiblingsAmbush
*Turn back around
    ~ SucceededRandomEvent = false
    ->->

=MerchantSiblings1a
     {Knows(Eileen.Exists): test }
        As you near a distance in which you would no longer need to shout, {players_gender == male:the woman}{players_gender == female:the man}{~!the man|the woman} speaks up: "Hello friend! How does Crìsdaen's wind blow?"
        ***"Fair and true!"
        ->->
        ***"A bit harsh at first, but quite alright now"
        ->->
        ***"
        ->->
 =MerchantSiblingsAmbush
    [Insert Ambush Scene Here]
     ->->

=Deer
TestDeer
->->

=Downpour
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
    
    