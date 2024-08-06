// The following are functions taken directly from Ink's documentation. May be altered, but the core is official functionality.

=== function _Pop(ref list) // Helper function: popping elements from lists
   ~ temp x = LIST_MIN(list) 
   ~ list -= x 
   ~ return x

=== function came_from(-> x)
	~ return TURNS_SINCE(x) == 0
	
/*
	Tests if the flow has reached a particular gather "this scene". This is an extension of "seen_more_recently_than", but it's so useful it's worth having separately.

	Usage: 

	// define where the start of the scene is
	~ sceneStart = -> start_of_scene

	- (start_of_scene)
		"Welcome!"

	- (opts)	
		<- cough_politely(-> opts)

		*	{ seen_this_scene(-> cough_politely.cough) }
			"Hello!"
		
		+	{ not seen_this_scene(-> cough_politely.cough) }
			["Hello!"]
			I try to speak, but I can't get the words out!
			-> opts


		
	=== cough_politely(-> go_to)
		*	(cough) [Cough politely]
			I clear my throat. 
			-> go_to
		
*/


VAR sceneStart = -> seen_this_scene 
VAR buffer = -> seen_this_scene

=== function init_sceneStart(-> link)
~ sceneStart = link

=== function seen(->x)
    ~ return x

=== function seen_very_recently(-> x)
    ~ return TURNS_SINCE(x) >= 0 && TURNS_SINCE(x) <= 3
    
=== function seen_this_scene(-> link)
	{  sceneStart == -> seen_this_scene:
		[ERROR] - you need to initialise the sceneStart variable before using "seen_this_scene"!
		~ return false
	}
	~ buffer = sceneStart
	~ sceneStart = -> seen_this_scene 
	~ return seen_more_recently_than(link, sceneStart)
	

=== function seen_more_recently_than(-> link, -> marker)
	{ TURNS_SINCE(link) >= 0: 
        { TURNS_SINCE(marker) == -1: 
            ~ return true 
        } 
        ~ return TURNS_SINCE(link) < TURNS_SINCE(marker) 
    }
    ~ return false 





  /* ---------------------------------
   ### System: Incremental knowledge tracking.
  ----------------------------------*/

  //  Allows tracking of what the player has observed throughout the game. Used by distinct LISTs. Each LIST is a chain of facts. Each fact supersedes the fact before. LISTs used this way are also called "Knowledge Chains".
  
VAR KnowledgeState = () // VAR that will serve as list containing all acquired knowledge

=== function Knows(fact) // check if fact is present in knowledge state, i.e. is it known information?
   ~ return KnowledgeState ? fact 
   
=== function Learn(facts)  // used to "learn" a fact   
    ~ KnowledgeState += facts
    
=== function KnowsAbout(subject)
    ~ return KnowledgeState ^ subject // see if any overlap between subject and knowledge base

/* This function simplified on 2024-05-06 as part of the pivot awai from incremental knowledge states. We are opting instead for the ability to use nonlineair logic (as in a player can Know A, not B, as well as B, not A) and estimate that the paradoxical results (not knowing A before B when that is nonsensical) are mitigatable by manual additions. Should we see that we often have to manually learn a lot of steps to prevent paradoxes and/or not really benefit from the nonlineair knowledge states, we can reinstate the following logic.:

=== function KnowledgeStateBetween(factX, factY) // used to check if knowledge state is between two specific points, i.e. does the player know x, but also not know y?
   ~ return KnowledgeState? factX && not (KnowledgeState ^ factY)

=== function Learn(facts)  // used to "learn" a fact (i.e. add it to knowledge state). This also learns all facts before it (because they are incremental).
   ~ temp x = _Pop(facts)
   {
   - not x: 
      ~ return false 

   - not Knows(x):
      ~ temp chain = LIST_ALL(x)
      ~ temp statesGained = LIST_RANGE(chain, LIST_MIN(chain), x)
      ~ KnowledgeState += statesGained
      ~ Learn (facts) 	// set any other states left to set
      ~ return true  	       // and we set this state, so true
 
    - else:
      ~ return false || Learn(facts) 
    }	
*/
    
   /* ---------------------------------
   #### List of Knowledge Chains
   ----------------------------------*/
// WIP: Move to different file
LIST EdanCastleKnowState = Reputation, Location, Name 

LIST EdgarKnowState = Reputation, Face, Name
LIST HenryKnowState = Reputation, Face, Name
LIST TomasKnowState = Reputation, Face, Name
LIST EileenKnowState = Reputation, Face, Name
LIST AliceKnowState = Reputation, Face, Name
LIST RobertKnowState = Reputation, Face, Name
LIST EdieKnowState = Reputation, Face, Name