extends VBoxContainer

@onready var continue_button_scene = preload("res://UI/continue_button.tscn")#$ContinueButton
@onready var choice_button_scene = preload("res://UI/choice_button.tscn")#$ChoiceButton
#@onready var story_navigator = get_node("../StoryNavigator")#TODO:fix this reference
@export var story_navigator: Node
@onready var text_presenter = get_node("../TextPresenter")

var selected = 0

signal choice_pressed(index) # is dit een onnodig tussensignaal?

func _ready():
	#continue_button_scene = preload("res://UI/continue_button.tscn")
	#choice_button_scene = preload("res://UI/choice_button.tscn")
	pass

func _clear():
	for child in get_children():
		child.queue_free()

func _on_continue(_dump: String):
	_clear()

func present_continue_button() -> void: 
	await text_presenter.finished_typing
	var continue_button = continue_button_scene.instantiate() #create object
	continue_button.pressed_continue.connect(story_navigator._on_continue_pressed) # link to continue signal
	add_child(continue_button) #place in hierarchy #TODO: figure out why create a button here and link ir rather than keep the same one and activate / de-activate

func present_choice(choice: InkChoice) -> void:
	await text_presenter.finished_typing
	#TODO: check if null first?
	var choice_button = choice_button_scene.instantiate()
	choice_button.text = choice.Text
	choice_button.index = choice.Index
	
	# Connect the button's pressed signal to choose the choice and continue the story
	choice_button.pressed_choice.connect(story_navigator._on_choice_pressed)
	add_child(choice_button)
