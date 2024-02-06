// --------- Shared  ---------
INCLUDE System.ink
INCLUDE Stories
INCLUDE BasTest.ink

// starting inventory
~ Inventory = (Knife, Rope, Lantern, ForagedMushrooms)
-> Start

=== Start ===
    + [Proceed with Vugs' sequence]
    -> CastleGatehouseCourtyard
    + [Try Bas' Travel and party Example:]
    -> BasTravelTest
    + [I wanna test the linebreaks]
    -> lineBreakTest
    + [Proceed to Character Creation]
    -> Preamble
    
// --------- Vugs  ---------
    
=== Death ===
And so ends this tale. Another apprentice that would never return, their findings lost to the wind.
->END
