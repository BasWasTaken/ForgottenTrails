 
LIST Background = (none), BG_Road1, BG_VaultHall, BG_VaultLibrary, BG_VaultHall2, BG_VaultOffice, BG_CastleGate, swamp_house, flower_gates // list of BackGrounds
  
=== function BackdropImage(image, duration) // name of image and duration in seconds (e.g. 0.5)
    ~ Background = image //update background 
~ (_BackdropImage(image, duration)) 

=== function _BackdropImage(listItem, float)
    
<<i>Godot background: {listItem}</i>> 
EXTERNAL _BackdropImage(listItem, float) 

