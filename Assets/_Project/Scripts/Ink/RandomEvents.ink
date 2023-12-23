=== RandomEventsEdanArea ===
//To do: add event content
{~!->MerchantSiblings|!->Deer|!->Downpour}

=MerchantSiblings
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
    Both look up from their tasks, their gaze directed at you. They return your wave and {players_gender == male:the woman}{players_gender == female:the man} returns your greeting: "fair winds blew on our path friend!"
    
    
*Ambush them
*Turn back around
//Vugs: hier moet een functie komen die je terugstuurt naar de plek waar je vandaan komt, maar ik snap het nieuwe travel systeem nog niet haha. 
{CurrentLocation == roadToEdanCastleLoc and PreviousLocation == EdinburghCrossroadsLoc: -> CastleEntrance}
If you're seeing this something went wrong with the random event bit in Inky!
->END

=MerchantSiblings1a
     {Knows(Eileen.Exists): test }
        As you near a distance in which you would no longer need to shout, {players_gender == male:the woman}{players_gender == female:the man}{~!the man|the woman} speaks up: "Hello friend! How does Crìsdaen's wind blow?"
        ***"Fair and true!"
        ***"A bit harsh at first, but quite alright now"
        ***"

=Deer
TestDeer
{CurrentLocation == roadToEdanCastleLoc and PreviousLocation == EdinburghCrossroadsLoc: -> CastleEntrance}
If you're seeing this something went wrong with the random event bit in Inky!
->END
=Downpour
//Add companion dependent dialogue
As you're traveling, you start to notice dark clouds gathering overhead.
*Press on
    It's probably nothing. And even so, a little rain can't stop you, right?
    [Bas Note: Something happens with the rain I guess? But anyway I'm diverting to the next bit.]
    -> CastleEntrance
*Seek shelter
    You decide not to risk getting drenched and find some cover. Unfortunately, you don't 
{CurrentLocation == roadToEdanCastleLoc and PreviousLocation == EdinburghCrossroadsLoc: -> CastleEntrance}
If you're seeing this something went wrong with the random event bit in Inky!
->END