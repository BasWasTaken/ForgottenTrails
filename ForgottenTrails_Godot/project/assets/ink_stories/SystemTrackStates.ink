// ItemStates

=== ItemStateLightSource ===
VAR LightSource = 0
->END

=LightSourceToggleOff
~LightSource = 0
{DEBUG == true:{LightSource}}
->->

=== ItemStateTorch ===
VAR TorchState = 0

{TorchState == 0:You light your torch. ->TorchToggleOn|You extinguish your torch. ->TorchToggleOff}

=TorchToggleOn
~LightSource = 1
~TorchState = 1
{DEBUG == true:{LightSource}}
->->

=TorchToggleOff
~TorchState = 0 
{LanternState == 0: ->ItemStateLightSource.LightSourceToggleOff}
->->

=== ItemStateLantern ===
VAR LanternState = 0

{LanternState == 0:You light your lantern. ->LanternToggleOn|You extinguish your lantern. ->LanternToggleOff}

=LanternToggleOn
~LightSource = 1
~LanternState = 1
{DEBUG == true:{LightSource}}
->->

=LanternToggleOff
~LanternState = 0 
{TorchState == 0: ->ItemStateLightSource.LightSourceToggleOff}
->->


// other States

