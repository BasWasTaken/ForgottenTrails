extends Button


func _process(delta):
	# This connects the spacebar to the button:
	if Input.is_action_just_pressed("continue"): 
		get_node("Button").emit("pressed")
