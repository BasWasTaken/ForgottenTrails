=== RandomEventsEdanArea ===
//To do: add event content
{~!->MerchantSiblings|!->Deer|!->Downpour}

=MerchantSiblings
TestMerchant
{CurrentLocation == roadToEdanCastleLoc and PreviousLocation == EdinburghCrossroadsLoc: -> CastleEntrance}
If you're seeing this something went wrong with the random event bit in Inky!
->END
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