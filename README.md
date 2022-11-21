# CirclesVsBlocks
Made with Unity 2019.4.21f1. I’m happy to discuss about decisions made.  
Some of the designs are moved and changed while creating the game, like moving the block to the top so it’s easier to click on circles while holding the phone in hand.  And using integer as a value for coins and prices and not floating-point value
Due to limited time that was had, some parts were skipped but planned:
-	Better structure of events, (Observer pattern).
-	Possible change to the parsing of formula and creation of a custom one, that would be simpler and that will support what is needed for the project.
-	Better downloading of configuration, possible with some service like firebase or similar.
-	Lots of graphic can and should be upgraded but it was skipped first.
-	Formatting large numbers (e.g. 1,000,000 in 1mil).
-	Adding audio to coins, background music, attack, damage etc. 
-	Visual upgrades for levels.
-	Move formula getting for Circles into config file.
-	Improve effects pool to avoid reparenting.
-	Documentation, comments and summaries.
-	Refactor GameController into smaller classes
