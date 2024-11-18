
LIST Vox =  NA   

// Vox. Done with external function.
=== function Vox_Play(clip, volume)// use volume between 0.0 and 1.0
~ Vox = clip // assign clip to vox list to ensure fit of argument
~ _Vox_Play(clip, volume)

=== function _Vox_Play(listItem, float) // plays audio on voice channel, unlooped
<<i>Vox: {listItem} at {float} volume</i>>   
EXTERNAL _Vox_Play(listItem, float)

LIST Sfx = gong
// SFX. Done with external function.
=== function Sfx_Play(clip, volume)// use volume between 0.0 and 1.0
~ Sfx = clip // assign clip to sfx list to ensure fit of argument
~ _Sfx_Play(clip, volume)

=== function _Sfx_Play(listItem, float) // plays audio on sfx channel, unlooped
<<i>Sfx: {listItem}</i>>
EXTERNAL _Sfx_Play(listItem, float)


LIST Ambiance = (none), chatter
// Ambiance. Handled with external function.
=== function Ambiance_Add(clip, volume) //use volume between 0.0 and 1.0
~ Ambiance += clip
~ _Ambiance_Play(clip, volume)

=== function _Ambiance_Play(listItem, float) // adds audio on an ambiance channel, looping
<<i>Ambiance: {Ambiance} </i>>
EXTERNAL _Ambiance_Play(listItem, float)

=== function Ambiance_Adjust(clip, newVolume)
{ Ambiance ? clip:
    ~ _Ambiance_Play(clip, newVolume)
- else:
    ~ print_warning("{clip} Not found")
}

=== function Ambiance_Remove(clip)
~ Ambiance -= clip 
~ _Ambiance_Remove(clip)

=== function _Ambiance_Remove(listItem)
<<i>Ambiance: {Ambiance} </i>>
EXTERNAL _Ambiance_Remove(listItem)

=== function Ambiance_RemoveAll()
~ Ambiance = Ambiance.none
~ _Ambiance_RemoveAll()

=== function _Ambiance_RemoveAll()
<<i>Removed all ambiance. </i>>
EXTERNAL _Ambiance_RemoveAll()

=== function Ambiance_Play(clip, volume)
~_Ambiance_RemoveAll()
~Ambiance_Add(clip, volume)

LIST Music = (none), theStreetsOfWhiteRun, TabiNoTochuu
// Music. Handled with external function.
=== function Music_Play(clip, volume) // play new clip or adjust volume of existing clip
~ Music = clip
~ _Music_Play(clip, volume)

=== function _Music_Play(listItem, float) // plays audio on music channel, looping // use volume between 0.0 and 1.0
<<i>Music: {Music}</i>>
EXTERNAL _Music_Play(listItem, float) 

=== function Music_Stop()
~ Music = Music.none
~ _Music_Play(Music.none, 0)
  