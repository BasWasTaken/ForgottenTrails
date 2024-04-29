// --------- Shared  ---------
INCLUDE System.ink
INCLUDE Stories




// starting inventory
~ Inventory = (Knife, Rope, Lantern, ForagedMushrooms)
~ Party = (Player)

// Developper mode adds a few shortcuts - remember to set to false in release!
VAR DEBUG = true
{DEBUG:
	IN DEVELOPER MODE!
    + [Proceed with Vugs' sequence]
        -> EdanInn
    + [Try Bas' Travel and party Example:]
        -> BasTravelTest
    + [I wanna test the linebreaks]
        -> lineBreakTest
    + [Give me item examples]
        -> ItemUses
    + [To Bas' writings]
        ++ [Heist]
            -> Antecedent
        ++ [Puzzle]
            -> InventoryPuzzleWelcome
    + [Proceed to Character Creation]
    -> Start
- else:
    -> Start
}

=== Start ===
-> Preamble
    
// --------- Vugs  ---------
    
=== Death ===
And so ends this tale. Another apprentice that would never return, their findings lost to the wind.
->END
