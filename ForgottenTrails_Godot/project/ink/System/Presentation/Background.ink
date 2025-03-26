 
LIST Background = (none), BG_Road1, BG_VaultHall, BG_VaultLibrary, BG_VaultHall2, BG_VaultOffice, BG_CastleGate, swamp_house, flower_gates // list of BackGrounds
  
=== function FadeToImage(image, duration) // name of image and duration in seconds (e.g. 0.5)
    ~ Background = image //update background 
~ (_FadeToImage(image, duration)) 

=== function _FadeToImage(listItem, float)
    
<<i>Godot background: {listItem}</i>> 
EXTERNAL _FadeToImage(listItem, float) 

=== function FadeToColor(color, duration)   // duration in seconds (e.g. 0.5)
~ _FadeToColor(color, duration)

=== function _FadeToColor(string, float) // fade to a color.
<<i>Fade to {string}</i>> 
EXTERNAL _FadeToColor(string, float)
function FadeIn(duration)   // duration in seconds (e.g. 0.5)
~ _FadeIn(duration)

=== function _FadeIn(float) // fade to a color.
<<i>Fade in</i>> 
EXTERNAL _FadeIn(float)

=== function FadeToBlack(duration)   // duration in seconds (e.g. 0.5)
~ FadeToColor("Black", duration)

=== function FadeToWhite(duration)   // duration in seconds (e.g. 0.5)
~ FadeToColor("White", duration)
