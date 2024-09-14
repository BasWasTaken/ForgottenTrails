// Tracking numeric value for relationship between pc and other characters, and using cutoff points to determine friendly, hostile etc.
  // Can be compared simply by asking e.g.: {AFFHenry >= friendly}



LIST Attitudes = devoted = 100, alligned = 75, friendly = 60, ambivalent = 50, begrudging = 40, hostile = 25, spiteful = 0

=== function ConvertAttitude(value) ===
{
-   value >= LIST_VALUE(devoted):
    ~ return devoted
-   value >= LIST_VALUE(alligned):
    ~ return alligned
-   value >= LIST_VALUE(friendly):
    ~ return friendly
-   value >= LIST_VALUE(ambivalent):
    ~ return ambivalent
-   value >= LIST_VALUE(begrudging):
    ~ return begrudging
-   value >= LIST_VALUE(hostile):
    ~ return hostile
-   else:
    ~ return spiteful
}

  
  
   /* ---------------------------------
   ##### List: Affection values
   ----------------------------------*/
   
VAR AffEdgar = 50
VAR AffHenry = 50
VAR AffAlice = 50
VAR AffRobert = 50
VAR AffEdie = 50  