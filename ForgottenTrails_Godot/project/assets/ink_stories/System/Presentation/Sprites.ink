LIST Portraits = (none), Alice1, Robert1, Brian, Gabriel
LIST Variants = (none), Angry, Sad, Happy
LIST Positions = (none), top_left, top_right, bottom_left, bottom_right, center, random, not_specified

=== function Spriteboard_Present(character, variant, position)
~ _Spriteboard_Present(character, variant, position)

=== function _Spriteboard_Present(character, variant, position)
<<i>Godot now shows {character} being {variant} at {position}.</i>> 
EXTERNAL _Spriteboard_Present(character, variant, position)

=== function Spriteboard_Move(character, position)
~ Spriteboard_Present(character, "not_specified", position)

=== function Spriteboard_Alter(character, variant)
~ Spriteboard_Present(character, variant, "not_specified")

=== function Spriteboard_Remove(character)
~ _Spriteboard_Remove(character)

=== function _Spriteboard_Remove(string)
<<i>Godot removes {string} if present</i>> 
EXTERNAL _Spriteboard_Remove(string)

=== function Spriteboard_Remove_All()
~ _Spriteboard_Remove_All()

=== function _Spriteboard_Remove_All()
<<i>Godot removes all spritres currently presented</i>> 
EXTERNAL _Spriteboard_Remove_All()
