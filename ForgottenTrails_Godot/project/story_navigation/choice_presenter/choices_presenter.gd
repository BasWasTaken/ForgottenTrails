extends VBoxContainer

@onready var continue_button_scene = preload("res://project/story_navigation/choice_presenter/continue_button.tscn")#$ContinueButton
@onready var choice_button_scene = preload("res://project/story_navigation/choice_presenter/choice_button.tscn")#$ChoiceButton

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
	add_child(continue_button) #place in hierarchy #could also activate and de-activate as needed, but it makes sense to me to do the same as with the choice buttons, because then you can very easily just destroy all children to remove choices

func present_choices(choices: Array[InkChoice]) -> void: #TODO: connext to signal
	await SignalBus.printer_text_finished # this is here because it needs to wait for the text to finish printing before it can present the choices, but i belive this is what is causing issues with the loading sometimes. i believe it should rather just check the state of the printer
	var i: int = 0
	for choice in choices:
		if false:
			pass #TODO: process invisible choices
			continue
		var button: Button = await present_choice(choice)
		if i>0:
			button.focus_neighbor_top = get_child(i-1).get_path()
			get_child(i-1).focus_neighbor_bottom = button.get_path()
		if i<10:
			pass #TODO: add keyboard shortcuts for the first 10 choices (using index i-1)
		i=i+1


func present_choice(choice: InkChoice) -> Button:
	await SignalBus.printer_text_finished
	#TODO: check if null first?
	var choice_button: Button = choice_button_scene.instantiate()
	choice_button.text = choice.Text
	choice_button.index = choice.Index
	
	add_child(choice_button)
	return choice_button
