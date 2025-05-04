

// WIP
  
   /* ---------------------------------
   #### List: Characters
   ----------------------------------*/
LIST PartyCandidates = Player, Alice, Robert // potential party members // Vugs may add items to this list.
VAR Party = () // list of characters in party

~ Party = PartyCandidates() // restrict to characters defined in list

=== function Party_AddMember(member) // Add character to party
    ~ Party += member
//System: {member} joined the party.
~print("{member} joined the party.")
    
=== function Party_RemoveMember(member) // Add character to party
    ~ Party -= member
//System: {member} left the party.
~print("{member} left the party.")

=== AllowPartyScreen(->returnTo) === // including this in the list of choices as a "thread statement" allows the player to open their party screen in order to start dialogues with party members.  Outside of these moments, party members can still be examined but not changed.
+  [\{UNITY:OpenPartyScreen\}] -> PartyScreen(returnTo) //WITHOUT "()"

=== function OpenPartyScreen() // call to open the map screen in unity
~ _OpenPartyScreen()

=== function _OpenPartyScreen() === // opens the map screen in unity
\{UNITY:OpenPartyScreen()\}

EXTERNAL _OpenPartyScreen()


=== PartyScreen(-> returnTo) // the party knot. visit to open the party screen in unity. 
~ _OpenPartyScreen()
<- PartyDialogues(returnTo)
+ [\{UNITY:ClosePartyScreen\}]    
    \{UNITY:ClosePartyScreen()\}
-     (done) -> returnTo
    

    
=== function _PartyChoice(character) === // used to present an inky choice that will be represented by a portrait in unity. in inky, it will just be an ormal option to click
\{PartyChoice({character})\}