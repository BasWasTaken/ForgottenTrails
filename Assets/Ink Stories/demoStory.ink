-> Start
// reminder to self: check out https://github.com/inkle/ink/blob/master/Documentation/WritingWithInk.md#5-functions and https://github.com/inkle/ink/blob/master/Documentation/WritingWithInk.md#part-5-advanced-state-tracking and https://www.patreon.com/posts/tips-and-tricks-18637020

#Backdrop sampleBG.png // This should set the background

#Music sampleMusic.mp3 // This should set the background music
#Music nothing

#Ambiance sampleAmbiance.mp3 // This should set the ambiance sound
#Ambiance nothing

#SFX sampleSFX.mp3 // This should play a one-off sound effect.
#Spd slow // Sets speed to slow, normal or fast.

#Log Hello World! // This prints the text to the unity console.
#LogWarning WARNING MESSAGE // This prints the text to the unity console in a warning format.




=== function VoorbeeldFunctie(VoorbeeldParameter) ===
	~ return VoorbeeldVariabele
	
=== Start ===
VAR VoorbeeldVariabele = "VoorbeeldWaarde"

Aan Joris de vraag, welke manier van een functie callen hem het fijnst lijkt werken:
~ VoorbeeldFunctie("VoorbeeldParameter") // hiervan zie je in de tekst niets.
{VoorbeeldFunctie("VoorbeeldParameter")} // Hiervan zie je zo mogelijk direct de waarde inline.
>>> VoorbeeldFunctie VoorbeeldParameter // Deze zie je in de ink preview, alsof het text is. 
#VoorbeeldFunctie VoorbeeldParameter//Deze zie je in de Ink preview rechts, als tags, tot er een paragraph break komt)
<br> // <br> Wordt als Inky laten zien als paragraph spacing, en interpreteert mijn unity code ook als paragraaf!
Bas: Kijk, op deze manier kan je een spreker aanduiden. Op dit moment wordt daar nog niets mee gedaan behalve dat de naam weg wordt gehaald in Unity. Maar dit zou je dus kunnen gebruiken voor nametags etc. Wsl voor ons niet per se relevant meer, maar als je het wel wil hebben, laat vooral even weten. Betekent dus wel dat je niet zomaar een dubbepunt kan gebruiken in je schrijven verder. Wsl zou ik er een escape functionaliteit voor moeten schrijven.
<br> 
The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. 
The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. 
<br>
(Internal): Op deze manier zou je gedachten aan kunnen duiden.

Aan het eind van deze zin wordt een command doorgegeven #voorbeeld van een command
Je kan er ook meerdere doorgeven # dit is commando 1 en # dit is commando 2 

- I looked at Monsieur Fogg. 
*   ... and I could contain myself no longer.
    'What is the purpose of our journey, Monsieur?'
    'A wager,' he replied.
    * *     'A wager!'[] I returned.
            He nodded. 
            * * *   'But surely that is foolishness!'
            * * *  'A most serious matter then!'
            - - -   He nodded again.
            * * *   'But can we win?'
                    'That is what we will endeavour to find out,' he answered.
            * * *   'A modest wager, I trust?'
                    'Twenty thousand pounds,' he replied, quite flatly.
            * * *   I asked nothing further of him then[.], and after a final, polite cough, he offered nothing more to me. <>
    * *     'Ah[.'],' I replied, uncertain what I thought.
    - -     After that, <>
*   ... but I said nothing[] and <>
- we passed the day in silence.
- -> END