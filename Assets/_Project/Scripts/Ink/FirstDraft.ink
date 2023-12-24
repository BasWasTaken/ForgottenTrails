INCLUDE System.ink
INCLUDE Stories
INCLUDE BasTest.ink


// starting inventory
~ Inventory = (Knife, Rope, Lantern, ForagedMushrooms)
-> Start 

=== Start ===
    + [Proceed with Vugs' sequence]
    -> RandomEventsEdanArea.MerchantSiblings
    + [Try Bas' Travel Example:]
    -> BasTravelTest
    
=== Death ===
And so ends this tale. Another apprentice that would never return, their findings lost to the wind.
->END
