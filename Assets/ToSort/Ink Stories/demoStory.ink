-> Start
    // reminder to self: check out https://github.com/inkle/ink/blob/master/Documentation/WritingWithInk.md#5-functions and https://github.com/inkle/ink/blob/master/Documentation/WritingWithInk.md#part-5-advanced-state-tracking and https://www.patreon.com/posts/tips-and-tricks-18637020
    // mandatory reading for both Bas and Joris: https://github.com/inkle/ink/blob/master/Documentation/RunningYourInk.md#marking-up-your-ink-content-with-tags

EXTERNAL Print(string)

=== function Print(a)
<i>Print to console:</i> {a}

=== Start ===
    #backdrop: whiterun // This should set the background
    #ambiance: chatter // this should set the ambiance
    #music: the streets of whiterun //this should set the music
    ~ Print("Hello world!")// This prints the text to the unity console
    This is the beginning of the ink story.
    -> sfx
=== sfx ===
    >>>Gong// this should play a sound //NOT YET IMPLEMENTED: #pause:2s
    Did you like the sound effect? Here, you can sound it as many times as you want.
    + [Hit it again.] -> sfx
    * [Continue] -> continue
=== continue ===
    Tekst komt binnen in ons tekst vakje, en vult het langzaamaan op.
    The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. 
    You can play sounds like so-
    #sfx:gong
    And you can play sounds like so-
    >>>gong
    <br>
    De linebreak geeft aan wanneer een nieuwe paragraaf begint. Het textvak wordt dan geleegd, de log erboven wordt aangevuld met de tekst die net weggehaald is, en het tekstvak wordt aangevuld met deze nieuwe tekst.
    Op dit moment zijn dit de enige twee manieren om tekst te splitten; newlines en linebreaks. Gezien alle newlines in één keer worden getoond (tot er een linebreak of ink keuze komt), is er nu nog geen manier om een paragraaf in een paar stukjes te splitten, zonder dat meteen de vorige stukken weggaan.
    <br>
    Het lijkt me in ieder geval, dat er voor nu genoeg functionaliteit is, dat de de geavanceerder linebreak methodes voor nu geen prioriteit zijn.
    -> next
=== next ===
    * [Agreed, what's next?] What's next?
    Bas: Door deze zin te beginnen met een naam gevolgd door een dubbele punt, heb ik een spreker aangeduid. Voor nu maakt dat alleen dat deze tekst binnen aanhalingstekens komt, maar hier kun je nog van alles mee doen.
    Ik moet alleen nog wel een escape clause schrijven om te voorkomen dat er geen gekke dingen gebeuren als je elders in je tekst een dubbele punt gebruikt.
    -> showme
=== showme ===
    <br>
    * [Show me inkle's demo.] Inkle's demo:
    <br>
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