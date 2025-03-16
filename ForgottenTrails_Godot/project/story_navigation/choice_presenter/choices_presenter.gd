extends VBoxContainer

@onready var continue_button_scene = preload("res://project/story_navigation/choice_presenter/continue_button.tscn")#$ContinueButton
@onready var choice_button_scene = preload("res://project/story_navigation/choice_presenter/choice_button.tscn")#$ChoiceButton

func _ready():
	#continue_button_scene = preload("res://UI/continue_button.tscn")
	#choice_button_scene = preload("res://UI/choice_button.tscn")
	#SignalBus.ink_sent_story.connect(_clear)
	SignalBus.ink_sent_choices.connect(present_choices)
	SignalBus.ink_sent_no_choices.connect(present_continue_button)
	SignalBus.control_requests_continue.connect(_on_input)
	SignalBus.control_requests_choice.connect(_on_input2)
	
func _on_input():
	_clear()
func _on_input2(_index: int):
	_clear()
func clear():
	_clear()
func _clear():
	for child in get_children():
		child.queue_free()


func present_continue_button() -> void: 
	#await SignalBus.printer_text_finished 
	var continue_button = continue_button_scene.instantiate() #create object
	add_child(continue_button) #place in hierarchy #could also activate and de-activate as needed, but it makes sense to me to do the same as with the choice buttons, because then you can very easily just destroy all children to remove choices
	continue_button.grab_focus() #set focus to this button

func present_choices(choices: Array) -> void: #TODO: connext to signal
	#await SignalBus.printer_text_finished # this is here because it needs to wait for the text to finish printing before it can present the choices, but i belive this is what is causing issues with the loading sometimes. i believe it should rather just check the state of the printer
	var i: int = 0
	var first: Button
	var prev: Button
	var current: Button
	for choice in choices:
		if false:
			pass #TODO: process invisible choices
			continue
		else:
			if(current!=null): #if this is not the first choice
				prev = current #save the current choice as the previous choice
				if(first==null): #if this is the second choice
					first = prev #save the previous choice as the first choice
			current = present_choice(choice) #present the new choice and save it as the current choice
			if(prev!=null): #if this is not the first choice
				current.focus_neighbor_top = prev.get_path()
				prev.focus_neighbor_bottom = current.get_path()
			if i<10:
				pass #TODO: add keyboard shortcuts for the first 10 choices (using index i-1)
			i=i+1
	current.focus_neighbor_bottom = first.get_path()
	first.focus_neighbor_top = current.get_path()
	first.grab_focus()
	#set the focus neighbors for the first and last choice


func present_choice(choice: InkChoice) -> Button:
	#await SignalBus.printer_text_finished
	#TODO: check if null first?
	var choice_button: Button = choice_button_scene.instantiate()
	choice_button.text = choice.Text
	choice_button.index = choice.Index
	choice_button.name = "choice_"+str(choice.Index)
	
	add_child(choice_button)
	return choice_button
