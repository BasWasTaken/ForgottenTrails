INCLUDE System.ink
INCLUDE Stories
INCLUDE BasTest.ink


// starting inventory
~ Inventory = (Knife, Rope, Lantern, ForagedMushrooms)
-> Start 

=== Start ===
    + [Proceed with Vugs' sequence]
    -> RandomEventsEdanArea.MerchantSiblings
    + [Try Bas' Travel Example]
    ~Party_AddMember(Alice)
    ~Party_AddMember(Robert)
    -> BasTravelTest
    + [Proceed to Character Creation]
    -> Preamble
    
=== Death ===
And so ends this tale. Another apprentice that would never return, their findings lost to the wind.
->END
