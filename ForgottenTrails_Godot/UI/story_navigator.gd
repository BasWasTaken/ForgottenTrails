extends Node

#@onready var my_csharp_script = load("res://UI/story_getter.cs")
#@onready var my_csharp_node = my_csharp_script.new()
@onready var story_getter = get_node("StoryGetter")
@onready var text_presenter = get_node("TextPresenter")

signal skip

func _process(_delta):
	# manually start the story (because it cannot do so automatically yet)
	if Input.is_action_just_pressed("start"): 
		print("navigator got enter")
		_on_continue_pressed()
	elif Input.is_action_just_pressed("continue"): 
		print("got spacebar")
		_on_continue_pressed()

func _on_continue_pressed():
	print("story_navigator received request to continue, evaluating...");
	if text_presenter.busy:
		print("Typer Busy. Skipping to end of Line.");
		skip.emit();
	elif story_getter.story.CanContinue:
		print("validated. Continueing Story.");
		story_getter.ContinueStory();
	else:
		print("Cannot Continue");

func _on_choice_pressed(index):
	print("navigator received choice " + str(index))
	story_getter.FeedChoice(index);
