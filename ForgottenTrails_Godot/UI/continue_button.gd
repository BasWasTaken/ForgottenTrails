extends Button

signal pressed_continue

#func _ready():
	#pressed.connect(_on_pressed)

func _process(_delta):
	# This connects the spacebar to the button:
	if Input.is_action_just_pressed("continue"): 
		print("got spacebar")
		#_on_pressed() not needed because we do this from story_navigator

func _on_pressed():
	print("pressed continue")
	pressed_continue.emit()
