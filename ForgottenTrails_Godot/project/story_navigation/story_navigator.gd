extends Node
class_name StoryNavigator #todo remove, confusing

#@onready var my_csharp_script = load("res://UI/story_getter.cs")
#@onready var my_csharp_node = my_csharp_script.new()
@export var story_getter: Node
@export var text_presenter: RichTextLabel #TODO according to refinement 20250301135148 this could be replaced by signals, but it's very low priority.
@export var choices_presenter: VBoxContainer #todo remove reference
@onready var history_log #= get_node("HistoryLog")
var story_state_json: String:
	get:
		return story_getter.latest_state # PREVIOUSLY saved state
	# set(value):
	# 	story_getter.LoadState(value)


var selectedChoice = -1

func _ready():

	DataManager.load_story_state.connect(story_getter.LoadState)
	# TODO make this less confusing:
		# currently datamanger emits a signal when it has read the json line
		# then storynavigater receives that and fires story_getter.LoadState to load it in ink
		# then when that is done story gettter fires again and navigator catches it back and then calls upon textpresenter via another signal
		# and tbh i think this is all done because i am too lazy to declare and assign nodes in my scripts. or it's best practise. either or.
	#story_getter.loaded_state.connect(text_presenter.clear)
	#story_getter.loaded_state.connect(choices_presenter.clear)
	# actually, i think these can be removed, because the story_getter should be able to handle this itself by continueing
	story_getter.loaded_state.connect(_send_continue)


	story_getter.ink_function_print.connect(SignalBus.inkfunc_print.emit)
	story_getter.ink_function_spd.connect(SignalBus.inkfunc_spd.emit)

	#SignalBus.control_requests_continue.connect(_on_continue_pressed)
	#SignalBus.control_requests_choice.connect(_on_choice_pressed)

# func _process(_delta):
# 	#TODO process this
# 	# manually start the story (because it cannot do so automatically yet)
# 	if InputEvent.is_action_just_pressed("start") || Input.is_action_just_pressed("continue"): 
# 		print("got keystroke for continue")
# 		SignalBus.control_requests_continue.emit()
# 	elif input_next || input_prev:
# 		if input_next:
# 			selectedChoice+=1
# 			if selectedChoice>choices_presenter.get_child_count()-1:
# 				selectedChoice=0
# 		elif input_prev:
# 			selectedChoice-=1
# 			if selectedChoice<0:
# 				selectedChoice=choices_presenter.get_child_count()-1
# 		print("selected choice " + str(selectedChoice))
# 	for index in range(choices_presenter.get_child_count()):
# 		if Input.is_action_just_pressed("choice "+str(index+1)): 
# 			SignalBus.choice_button_pressed.emit(index)

# 	if Input.is_action_just_pressed("quickload"):
# 		text_presenter.finish_text()
# 		text_presenter.clear()
# 		choices_presenter.clear()
# 		DataManager.load_most_recent_quicksavefile()
# 	elif Input.is_action_just_pressed("quicksave"):
# 		DataManager.quicksave_game(story_state_json) # TODO since this is always the same, just replace, by making a reference to story naviagator from datamanager


# func _on_continue_pressed():	
# 	print("story_navigator received request to continue, evaluating...");
# 	if text_presenter.typing:
# 		print("Typer Busy. Skipping to end of Line.");
# 		SignalBus.control_requests_skip.emit();
# 	elif story_getter.story.CanContinue:
# 		print("validated. Continueing Story.");
# 		_send_continue()
# 	elif choices_presenter.get_child_count()>1:#if there are choice buttons
# 		if selectedChoice == -1:
# 			print("Select a Choice first");
# 		else:
# 			print("validated. requesting selected choice.");
# 			SignalBus.choice_button_pressed.emit(selectedChoice)
# 	else:
# 		push_warning("Cannot Parse Continue");


# func _on_choice_pressed(index:int):
# 	_send_choice(index)	

func _send_continue():
	story_getter.ContinueStory();
	selectedChoice = -1 # reset selection so next required an action to select



func _send_choice(index:int):
	print("printer received and now sending choice.", index);
	story_getter.FeedChoice(index);
	selectedChoice = -1 # reset selection so next required an action to select
	save_state()
	DataManager.autosave_game(story_state_json) # save now
	_send_continue()

func load_state(state: String):
	story_getter.LoadState(state)

func save_state():
	return story_getter.SaveState()
