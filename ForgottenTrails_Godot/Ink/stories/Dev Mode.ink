VAR DEBUG = false
=== DeveloperModeToggle ===
// Developer mode adds a few shortcuts - remember to set to false in release!
~DEBUG = true
{DEBUG:
	IN DEVELOPER MODE!
    + [Proceed with Vugs' sequence]
        -> Excerptfromthecoastalruins
    + [Try Bas' Travel and party Example:]
        -> BasTravelTest
    + [I wanna test the linebreaks]
        -> lineBreakTest
    + [Give me item examples]
        -> ItemUses
    + [To Bas' writings]
        //++ [Heist]
            //-> Antecedent
        ++ [Puzzle]
            -> InventoryPuzzleWelcome
    + [Proceed to Character Creation]
    -> Opening
- else:
    -> Start
}