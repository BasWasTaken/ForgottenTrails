
VAR speed = 1.0 

=== function spd(value) // change speed of textflow based on positive number, expressed as multiplier (eg .8 for 80%)
~ speed = value
~ _spd(value)

=== function _spd(float) 
// nothing
EXTERNAL _spd(float) 

=== function clear()  // Clears the textbox and moves that text to log
~ _clear()

=== function _clear()
<<i>Clear Page</i>>

EXTERNAL _clear()

VAR glue = "\{glue\}" // used to connect this line to the next one.

VAR aglue = "\{aglue\}" // used to connect the previous line to this one. 
