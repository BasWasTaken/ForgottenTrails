extends VBoxContainer

@onready var continue_button_scene = preload("res://UI/continue_button.tscn")#$ContinueButton
@onready var choice_button_scene = preload("res://UI/choice_button.tscn")#$ChoiceButton
#@onready var story_navigator = get_node("../StoryNavigator")#TODO:fix this reference
@export var story_navigator: Node

signal choice_pressed(index) # is dit een onnodig tussensignaal?

func _ready():
	#continue_button_scene = preload("res://UI/continue_button.tscn")
	#choice_button_scene = preload("res://UI/choice_button.tscn")
	pass

func _clear():
	for child in get_children():
		child.queue_free()

func _on_finished_continue(_dump: String):
	_clear()

func present_continue_button() -> void: 
	var continue_button = continue_button_scene.instantiate() #create object
	continue_button.pressed_continue.connect(story_navigator._on_continue_pressed) # link to continue signal
	continue_button.pressed_continue.connect(_on_continue_pressed)
	add_child(continue_button) #place in hierarchy

func _on_continue_pressed():
	print("choice presenter received continue")
	#_clear()

func present_choice(choice: InkChoice) -> void:
	#TODO: check if null first?
	var choice_button = choice_button_scene.instantiate()
	choice_button.text = choice.Text
	choice_button.index = choice.Index
	
	# Connect the button's pressed signal to choose the choice and continue the story
	choice_button.pressed_choice.connect(story_navigator._on_choice_pressed)
	choice_button.pressed_choice.connect(_on_choice_pressed)
	add_child(choice_button)

func _on_choice_pressed(index: int) -> void:
	print("choice presenter received choice " + str(index))
	#_clear()
