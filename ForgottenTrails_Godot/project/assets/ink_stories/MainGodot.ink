INCLUDE Functions.ink
INCLUDE Stories.ink
INCLUDE SystemDevMode.ink

//Files that need to be included to not break something else, but need to be replaced
INCLUDE ArchiveEdanRegionRandomEvents.ink
INCLUDE ArchiveIntroductionAwakening.ink
INCLUDE ArchiveEdanRegionRuinedCoast.ink
INCLUDE ArchiveEdanRegionSeaBreezePath.ink
INCLUDE ArchiveEdanRegionSouthernRoads.ink
INCLUDE ArchiveEdanGate.ink
INCLUDE ArchiveEdanInn.ink
INCLUDE ArchiveEdanMarket.ink
INCLUDE ArchiveEdanPrison.ink



//Set inventory
~ Inventory = (Knife, Rope, Lantern, ForagedMushrooms)

//Set party
~ Party = (Player)

->Start

=== Start ===
Do you want to toggle developer mode?
+[Yes]
-> developer_mode_toggle
+[No]
-> Opening

// Placeholder. Need to move this and make it context specific in several cases.     
=== Death ===
And so ends this tale. Another apprentice that would never return, their findings lost to the wind.
->END
