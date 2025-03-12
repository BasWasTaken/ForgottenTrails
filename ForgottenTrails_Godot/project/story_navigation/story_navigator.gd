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
	story_getter.loaded_state.connect(input_continue)


	story_getter.ink_function_print.connect(SignalBus.inkfunc_print.emit)
	story_getter.ink_function_spd.connect(SignalBus.inkfunc_spd.emit)

	SignalBus.control_requests_continue.connect(input_continue)
	SignalBus.control_requests_choice.connect(input_choice)


func input_continue():
	story_getter.ContinueStory();

func input_choice(index:int):
	story_getter.FeedChoice(index);
	save_state()
	DataManager.autosave_game(story_state_json) # save now #TODO instead regel dit met een signal?
	input_continue()

func load_state(state: String):
	story_getter.LoadState(state)

func save_state():
	return story_getter.SaveState()
