 
LIST Backdrop = (none), BG_Road1, BG_VaultHall, BG_VaultLibrary, BG_VaultHall2, BG_VaultOffice, BG_CastleGate, swamp_house, flower_gates // list of BackGrounds
  
=== function FadeToImage(image, duration) // name of image and duration in seconds (e.g. 0.5)
    ~ Backdrop = image //update background 
~ (_FadeToImage(image, duration)) 

=== function _FadeToImage(listItem, float)
    
<<i>Godot background: {listItem}</i>> 
EXTERNAL _FadeToImage(listItem, float) 

