# ForgottenTrails
Our little project.
Branch flow as of 2025-06-01:
/main: Latest released (stable) version. Only accepts merges from /release/ and /hotfix/ branches.
/hotfix/~: Used for addressing critical bugs on /main, skipping the usual workflow described below.
/develop: Development version. Most /feature/ and /bugfix/ branches fork from here and ultimately merge back into here, requiring a pull request. 
/release/~: Contains latest features, to test before they are released. Bugs may be addressed on and forked from this branch but not features. Forked from /develop/. Folds into /main/. Mostly relevant after we go live, as the main goal of these safeguards is preserving /main/ (preventing untested features from going there). Only the author of a feature branch should make a pull request.
/contrib/~: Continous changes. Rather than /feature/ branches, these are continuously committed to and kept up to date with /develop/.

Tags:
/meeting/yyyymmdd: Made as branches of the /develop or /meeting branches. Used for preserving the state of the project as tested before a meeting. After the meeting these are transferred to tags and preserved for prosterity. 
/release/~: made along with a release branch.
