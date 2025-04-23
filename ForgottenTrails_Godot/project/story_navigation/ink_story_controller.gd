extends Node

#@onready var my_csharp_script = load("res://UI/my_story_getter.cs")
#@onready var my_csharp_node = my_csharp_script.new()
@export var my_story_getter: Node
#@export var text_presenter: RichTextLabel #TODO according to refinement 20250301135148 this could be replaced by signals, but it's very low priority.
#@export var choices_presenter: VBoxContainer #todo remove reference
@onready var history_log #= get_node("HistoryLog")
var story_state_json: String:
	get:
		return my_story_getter.latest_state # PREVIOUSLY saved state
	# set(value):
	# 	my_story_getter.LoadState(value)


func _ready():
	
	SignalBus.control_requests_continue.connect(input_continue)
	SignalBus.control_requests_choice.connect(input_choice)

	my_story_getter.continued_story.connect(SignalBus.ink_sent_story.emit)
#	my_story_getter.continued_story.connect(history_log.add_to_history) # TODO but in own script
#	my_story_getter.encountered_choices.connect(SignalBus.ink_sent_choices.emit) #does not work, have to do with hack
	my_story_getter.encountered_no_choices.connect(SignalBus.ink_sent_no_choices.emit)
	my_story_getter.fetch_my_choices_plx.connect(hack)


	DataManager.load_story_state.connect(my_story_getter.LoadState)
	# TODO make this less confusing:
		# currently datamanger emits a signal when it has read the json line
		# then storynavigater receives that and fires my_story_getter.LoadState to load it in ink
		# then when that is done story gettter fires again and navigator catches it back and then calls upon textpresenter via another signal
		# and tbh i think this is all done because i am too lazy to declare and assign nodes in my scripts. or it's best practise. either or.
	#my_story_getter.loaded_state.connect(text_presenter.clear)
	#my_story_getter.loaded_state.connect(choices_presenter.clear)
	# actually, i think these can be removed, because the my_story_getter should be able to handle this itself by continueing
	my_story_getter.loaded_state.connect(input_continue)


	my_story_getter.ink_function_print.connect(SignalBus.ink_func_print.emit)
	my_story_getter.ink_function_spd.connect(SignalBus.ink_func_spd.emit)
	my_story_getter.ink_function_fade_to_color.connect(SignalBus.ink_func_fade_to_color.emit)
	my_story_getter.ink_function_backdrop_image.connect(SignalBus.ink_func_backdrop_image.emit)
	my_story_getter.ink_function_fade_in.connect(SignalBus.ink_func_fade_in.emit)
	my_story_getter.ink_function_fade_out.connect(SignalBus.ink_func_fade_out.emit)
	my_story_getter.ink_function_effect.connect(SignalBus.ink_func_effect.emit)
	my_story_getter.ink_function_flash.connect(SignalBus.ink_func_flash.emit)
	my_story_getter.ink_function_sprite_present.connect(SignalBus.ink_func_sprite_present_by_string.emit)
	my_story_getter.ink_function_sprite_remove.connect(SignalBus.ink_func_sprite_remove.emit)
	my_story_getter.ink_function_sprite_remove_all.connect(SignalBus.ink_func_sprite_remove_all.emit)


func hack(): #needed because the variantarray cannot by itself be sent through a signal
	var converted_array: Array = Array(my_story_getter.GetChoices())
	SignalBus.ink_sent_choices.emit(converted_array)
	

func input_continue():
	printer_state.set_state(printer_state.PROCESSING)
	my_story_getter.ContinueStory();


func input_choice(index:int):
	my_story_getter.FeedChoice(index);
	save_state()
	DataManager.autosave_game(story_state_json) # save now #TODO instead regel dit met een signal?
	input_continue()

func load_state(state: String):
	my_story_getter.LoadState(state)

func save_state():
	return my_story_getter.SaveState()
