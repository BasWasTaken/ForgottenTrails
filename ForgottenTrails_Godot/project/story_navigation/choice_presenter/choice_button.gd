extends Button

var index: int

func _ready():
	pressed.connect(_on_pressed)

func _process(_delta):
	# This connects a number key to the button:
	if Input.is_action_just_pressed("choice "+str(index+1)): 
		_on_pressed()

func _on_pressed():
	SignalBus.choice_button_pressed.emit(index)
