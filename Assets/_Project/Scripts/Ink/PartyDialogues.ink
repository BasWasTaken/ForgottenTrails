// --------- Shared ---------
=== PartyDialogues(-> returnTo)
+ { Party?Alice} [{_PartyChoice(Alice)}] 
    ->AliceDialogue->returnTo
+ { Party?Robert} [{_PartyChoice(Robert)}] 
    ->RobertDialogue->returnTo
        
// --------- Shared ---------
== AliceDialogue
"Alice..."
+ [Compliment]
    ->Compliment
+ [Dismiss this party member]
    ->Dismiss
+ [Remark on a thing that just happened.]
    ->Sample
    
= Compliment
+ "I think you're doing great."
    "Cool thanks."
-    ->->
+ "Cool face."
    "Yeah thanks."
-    ->->
    
= Sample
* Witty remark relevant to the thing we just witnessed.
    Fitting response.
-    ->->
* Another witty remark relevant to the other thing we just witnessed.
    Fitting response.
-    ->->
* -> 
    "Actually, I got nothing."
-    ->->
    
= Dismiss
You sure?
    + (confirm)[Y]"I think you should just leave."
    -> Leaving
    + [N]"Nevermind"
-    ->->
    
= Leaving
    Alice is fucking pissed, dude.
    ~Party_RemoveMember(Alice)
    ->->
== RobertDialogue
"Robbie..."
+ [Compliment]
    ->Compliment
+ [Dismiss this party member]
    ->Dismiss
    
= Compliment
+ "I think you're doing great."
    "Cool thanks."
-    ->->
+ "Cool face."
    "Yeah thanks."
-    ->->
    
= Dismiss
You sure?
    + (confirm)[Y]
    -> Leaving
    + [N]
    Ah, we cool then.
-    ->->

= Leaving
    Rob is okay with it.
    ~Party_RemoveMember(Robert)
    But <>
    -> AliceDialogue.Leaving