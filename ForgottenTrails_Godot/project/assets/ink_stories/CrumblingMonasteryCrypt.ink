=== CrumblingMonasteryChurchCrypt
~SetLocation(LOC_CrumblingMonasteryChurchCrypt)
{->CrumblingMonasteryChurchCryptIntroduction|}
//Return visit text
->Actions

=== CrumblingMonasteryChurchCryptIntroduction
VAR LeftCrypt = 0

You slowly wake. A dream after all. You find yourself face down on a damp, stone floor. Your head is pounding something fierce, as if to drive home the harsh divide between the realm of slumber and the one you currently find yourself in... Where are you anyway? The darkness stretches every way you look. 
->Actions

=== CrumblingMonasteryChurchCryptTalk
->Actions

=== CrumblingMonasteryChurchCryptUse
->Actions

=== CrumblingMonasteryChurchExamine
{LeftCrypt == 0 && LightSource == 0:The room is obscured in deep darkness. As you carefully feel around, you feel the damp stone walls of the room pass by your fingers.}
->Actions

=== CrumblingMonasteryChurchCryptMove
->Actions
