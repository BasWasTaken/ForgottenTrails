# ForgottenTrails
Our little project.
Branches as of 2023-08-28:
Bas-Experimental: Main branch where Bas works on large changes that may leave the project unusable in this branch. When changes are deemed stable, Bas should merge them into Bas-Stable.
Bas-Stable: Contains the furthest-developed features from Bas' side. Receives merges from Bas-Experimental and Vugs-Stable.
Vugs-Experimental: Branch where Vugs can test and make large changes to UI etc. It is on Vugs to keep this branch up to date before he starts this work, by mergin from main, Vugs-Stable, or Bas-Stable.
Vugs-Stable: Branch where Vugs does his writing. Usually will be at least stable enough to launch Unity and frequently merged into Bas-Stable by Bas.
Main: Should only contain stable versions. No changes should be made on this directedly. Instead, receives merges or pulls from Bas-Stable and Vugs-Stable. Handled by Bas.
