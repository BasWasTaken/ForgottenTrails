extends VBoxContainer

@export var continue_button_scene = preload("res://project/scenes/components/continue_button.tscn")
@export var choice_button_scene = preload("res://project/scenes/components/choice_button.tscn")

func _ready():
	#continue_button_scene = preload("res://UI/continue_button.tscn")
	#choice_button_scene = preload("res://UI/choice_button.tscn")
	#SignalBus.ink_sent_story.connect(_clear)
	SignalBus.ink_sent_choices.connect(present_choices)
	SignalBus.ink_sent_no_choices.connect(present_continue_button)
	SignalBus.request_clear_buttons.connect(_clear)

func _clear():
	print("clearing buttons")
	for child in get_children():
		child.queue_free()


func present_continue_button() -> void: 
	SignalBus.buttons_ready.emit()
	print("continue button ready, waiting for signal")
	await SignalBus.printer_requests_buttons 
	var continue_button = continue_button_scene.instantiate() #create object
	print(continue_button)
	add_child(continue_button) #place in hierarchy #could also activate and de-activate as needed, but it makes sense to me to do the same as with the choice buttons, because then you can very easily just destroy all children to remove choices
	
	print(continue_button.get_parent())
	# Ok so the object EXISTS, but it gets disappeared???
	continue_button.grab_focus() #set focus to this button

#TODO: catch event for end of script better. now if no continue it just assumes there is a choice
func present_choices(choices: Array) -> void: #TODO: connext to signal
	SignalBus.buttons_ready.emit()
	print("choice buttons ready, waiting for signal") #NOTE you could probably move this, and generate the buttons first, just not show them yet
	await SignalBus.printer_requests_buttons # NOTE: since you've reactivated this (2025-06-25), you should expect issues again with saveloading. but the solution is not to comment this oput. rather, you shhould make it so that old signals are cut off or checked for releance, to prevent old signals causing behaviour after reloading.
	var i: int = 0
	var first: Button = null
	var prev: Button = null
	var current: Button = null
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
	if(current==null): # no choice exists
		push_warning("no (visible) choices have been provided. likely this thread cannot be further explored in godot")
		return
	if(first==null): # only 1 choice exists
		first = current
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
