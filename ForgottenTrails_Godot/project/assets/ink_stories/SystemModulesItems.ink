//Light Related Items and System
=== ItemStateLightSource ===
VAR LightSource = 0
->END

=LightSourceToggleOff
~LightSource = 0
{DEBUG == true:{LightSource}}
->->

=== ItemStateTorch ===
VAR TorchState = 0
{TorchState == 0:->TorchToggleOn|->TorchToggleOff}

=TorchToggleOn
~LightSource = 1
~TorchState = 1
{DEBUG == true:{LightSource}}
->TorchToggleResolution

=TorchToggleOff
~TorchState = 0 
{DEBUG == true:{LightSource}}
{TorchState == 0: ->TorchToggleLightCheck|->TorchToggleResolution}

=TorchToggleLightCheck
~LightSource = 0
{DEBUG == true:{LightSource}}
->TorchToggleResolution

=TorchToggleResolution
{TorchState == 1:You light your torch|You extinguish your torch}
->->

=== ItemStateLantern ===
VAR LanternState = 0
{LanternState == 0:->LanternToggleOn|->LanternToggleOff}

=LanternToggleOn
~LightSource = 1
~LanternState = 1
{DEBUG == true:{LightSource}}
->LanternToggleResolution

=LanternToggleOff
~LanternState = 0 
{DEBUG == true:{LightSource}}
{TorchState == 0: ->LanternToggleLightCheck|->LanternToggleResolution}

=LanternToggleLightCheck
~LightSource = 0
{DEBUG == true:{LightSource}}
->LanternToggleResolution

=LanternToggleResolution
//Lists variable specific resolutions. If none are met, defaults to generic description
{CurrentLocation == LOC_CrumblingMonasteryChurchCrypt && LitCrypt == 0: ->  CrumblingMonasteryChurchCryptFirstLighting}
{LanternState == 1:You light your lantern|You extinguish your lantern}
->->


