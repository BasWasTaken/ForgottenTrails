# Ink
- AdvanceTime()
## Inventory:
- Item_Add(item) 
- Item_Remove(item)
- ItemOption(itemOrAffordance)
## Knowledge:
- Knows(fact)
	- Returns: boolean
- KnowledgeStateBetween(fact x, fact y)
	- Returns: boolean
- Learn(facts)
## Travel:
- SetLocation(location)
- HasVisited(location)
	- Returns: boolean
# StoryPresentation:
- Clear()
- Spd(float)
- Halt(duration)
- {stop}
- {glue}
- {aglue}
## SetDressing:
### Images:
- FadeToImage(image, duration)
- FadeToColor(color, duration)
- Portraits_Set(images)
- Portraits_Add(image)
- Portraits_Remove(image)
- Portraits_RemoveAll()
### Audio
- Sfx_Play(clip, volume)
- Vox_Play(clip, volume)
- Ambiance_Add(clip, volume)
- Ambiance_Adjust(clip, newVolume)
- Ambiance_Remove(clip)
- Ambiance_RemoveAll()
- Music_Play(clip, volume)
- Music_Stop()
# Misc
- Print(message)
- PrintWarning(message)
