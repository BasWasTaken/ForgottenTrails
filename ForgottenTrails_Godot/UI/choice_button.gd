extends Button

signal pressed_choice(int)

var index: int

#func _ready():
	#pressed.connect(_on_pressed)

func _process(_delta):
	# This connects a number key to the button:
	if Input.is_action_just_pressed("choice "+str(index+1)): 
		_on_pressed()

func _on_pressed():
	pressed_choice.emit(index)
