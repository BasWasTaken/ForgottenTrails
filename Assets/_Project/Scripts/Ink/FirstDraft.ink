INCLUDE System.ink
INCLUDE Stories
INCLUDE BasTest.ink


// starting inventory
~ Inventory = (Knife, Rope, Lantern, ForagedMushrooms)
-> Start

=== Start ===
    + [Proceed with Vugs' sequence]
    -> MerchantSiblings1
    + [Try Bas' Travel Example:]
    -> BasTravelTest
    + [Proceed to Character Creation]
    -> Preamble
    
=== Death ===
And so ends this tale. Another apprentice that would never return, their findings lost to the wind.
->END
