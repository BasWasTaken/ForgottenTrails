// Inventory is managed by the LIST variable in Ink, which is observed by Unity and matched accordingly.

LIST Items = Knife, Pot, Rope, Lantern, Torch, ForagedMushrooms, WornSword, EdanInnRoomKey1, EdanInnRoomKey2, EdanInnRoomKey3, EdanInnMasterKey, BasicFishingRod, Journal // existing items // Vugs may add items to this list.

~ Items = LIST_ALL(Items)  // Full list for Unity syncing. Note Bas: I should maybe  prefix with underscore

// rations and money are currently not in inventory sytem:
VAR TravelRations = 107

VAR Money = 10

  
VAR Inventory = () // list of items the player has.
~ Inventory = Items() // restrict to items defined in list

=== function item_add(item) // Add item to inventory.
    ~ Inventory += item

=== function ItemChoice(itemOrAffordances) // include an ink choice which can only be taken by using an item from the inventory (in unity. in ink, it'll show as normal). 
\{ItemChoice({itemOrAffordances})\} 

VAR UsedItem = () // container for unity to tell ink what item it just used
~ UsedItem = Items()

=== function _item_remove(item) // used to remove an item from the inventory
    {
    - Inventory has item: 
        ~ Inventory -= item
        ~ print("Removed {item}. Items still in possesion: {Inventory}")
    - else:
        ~ print_warning("Attempted to remove an item that wasn't there!")
    }
    
=== function item_remove(item)
~ _item_remove(item)

=== function item_remove_last_used() // remove item that was just used
~ item_remove(UsedItem)



