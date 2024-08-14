extends VBoxContainer

@onready var continue_button = load("res://UI/continue_button.tscn") as PackedScene#$ContinueButton
@onready var choice_button = load("res://UI/choiceButton.tscn") as PackedScene#$ChoiceButton

signal choice_pressed(index)

func _ready():
	pass

func _clear():
	for child in get_children():
		child.queue_free()

func _on_finished_continue(_dump: String):
	_clear()

func present_continue_button() -> void: 	
	var button = continue_button.Instance()
	button.connect("pressed", )
	
	add_child(button)

func present_choice(choice: InkChoice) -> void:
	var button = choice_button.Instance()
	button.text = choice.text
	
	# Connect the button's pressed signal to choose the choice and continue the story
	button.connect("pressed", self, "_on_choice_pressed", [choice.index])
	
	add_child(button)

func _on_choice_pressed(index: int) -> void:
	_clear()
	choice_pressed.emit(index)
