extends Node
class_name StoryNavigator

#@onready var my_csharp_script = load("res://UI/story_getter.cs")
#@onready var my_csharp_node = my_csharp_script.new()
@export var story_getter: Node
@export var text_presenter: RichTextLabel
@export var choices_presenter: VBoxContainer 
@onready var history_log #= get_node("HistoryLog")
var story_state_json: String:
	get:
		return story_getter.latest_state # PREVIOUSLY saved state
	# set(value):
	# 	story_getter.LoadState(value)

signal skip

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

func _process(_delta):
	# manually start the story (because it cannot do so automatically yet)
	var input_next = Input.is_action_just_pressed("select_next_choice")
	var input_prev = Input.is_action_just_pressed("select_previous_choice")
	if Input.is_action_just_pressed("start") || Input.is_action_just_pressed("continue"): 
		print("got keystroke for continue")
		_on_continue_pressed()
	elif input_next || input_prev:
		if input_next:
			selectedChoice+=1
			if selectedChoice>choices_presenter.get_child_count()-1:
				selectedChoice=0
		elif input_prev:
			selectedChoice-=1
			if selectedChoice<0:
				selectedChoice=choices_presenter.get_child_count()-1
		print("selected choice " + str(selectedChoice))

	if Input.is_action_just_pressed("quickload"):
		text_presenter.finish_text()
		text_presenter.clear()
		choices_presenter.clear()
		DataManager.load_most_recent_quicksavefile()
	elif Input.is_action_just_pressed("quicksave"):
		DataManager.quicksave_game(story_state_json) # TODO since this is always the same, just replace, by making a reference to story naviagator from datamanager


func _on_continue_pressed():	
	print("story_navigator received request to continue, evaluating...");
	if text_presenter.typing:
		print("Typer Busy. Skipping to end of Line.");
		skip.emit();
	elif story_getter.story.CanContinue:
		print("validated. Continueing Story.");
		_send_continue()
	elif choices_presenter.get_child_count()>1:#if there are choice buttons
		if selectedChoice == -1:
			print("Select a Choice first");
		else:
			print("validated. Fed selected choice.");
			_send_choice(selectedChoice)
	else:
		push_warning("Cannot Parse Continue");
	
func _send_continue():
	story_getter.ContinueStory();
	selectedChoice = -1 # reset selection so next required an action to select

func _on_choice_pressed(index):
	_send_choice(index)

func _send_choice(index):
	#print("navigator received choice " + str(index))
	story_getter.FeedChoice(index);
	selectedChoice = -1 # reset selection so next required an action to select
	save_state()
	DataManager.autosave_game(story_state_json) # save now
	_send_continue()

func load_state(state: String):
	story_getter.LoadState(state)

func save_state():
	return story_getter.SaveState()
