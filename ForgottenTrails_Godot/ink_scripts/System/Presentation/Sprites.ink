 
LIST Portraits = (none), Alice1, Robert1
=== function Portraits_Set(images)
~ Portraits = images
<<i>Unity now shows {images}</i>> 

=== function Portraits_Add(image)
~ Portraits += image
<<i>Unity now shows {image}</i>> 

=== function Portraits_Remove(image)
~ Portraits -= image
<<i>Unity removes {image} if present</i>> 

=== function Portraits_RemoveAll()
~ Portraits = ()
<<i>Unity removes all portraits</i>> 
