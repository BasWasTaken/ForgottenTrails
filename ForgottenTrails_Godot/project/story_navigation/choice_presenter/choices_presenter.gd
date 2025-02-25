extends VBoxContainer

@onready var continue_button_scene = preload("res://project/story_navigation/choice_presenter/continue_button.tscn")#$ContinueButton
@onready var choice_button_scene = preload("res://project/story_navigation/choice_presenter/choice_button.tscn")#$ChoiceButton
@export var story_navigator:StoryNavigator# = get_node("StoryNavigator") #TODO fix onready for this
@export var text_presenter:Node# = get_node("../TextPresenterPanel/TextPresenter")

func _ready():
	#continue_button_scene = preload("res://UI/continue_button.tscn")
	#choice_button_scene = preload("res://UI/choice_button.tscn")
	pass
func clear():
	_clear()
func _clear():
	for child in get_children():
		child.queue_free()

func _on_continue(_dump: String):
	_clear()

func present_continue_button() -> void: 
	await SignalBus.printer_text_finished
	var continue_button = continue_button_scene.instantiate() #create object
	continue_button.pressed_continue.connect(story_navigator._on_continue_pressed) # link to continue signal
	add_child(continue_button) #place in hierarchy #could also activate and de-activate as needed, but it makes sense to me to do the same as with the choice buttons, because then you can very easily just destroy all children to remove choices

func present_choice(choice: InkChoice) -> void:
	await SignalBus.printer_text_finished
	#TODO: check if null first?
	var choice_button = choice_button_scene.instantiate()
	choice_button.text = choice.Text
	choice_button.index = choice.Index
	
	# Connect the button's pressed signal to choose the choice and continue the story
	choice_button.pressed_choice.connect(story_navigator._on_choice_pressed)
	add_child(choice_button)
