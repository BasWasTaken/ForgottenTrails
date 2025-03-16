extends Node
func _ready():
	SignalBus.control_requests_up.connect(navigate_up)
	SignalBus.control_requests_down.connect(navigate_down)

func navigate_up():
	print("up")

func navigate_down():
	print("down")
	
# this seems to not be necesary,as godot has this functionality baked in