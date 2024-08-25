extends Node

#@onready var my_csharp_script = load("res://UI/story_getter.cs")
#@onready var my_csharp_node = my_csharp_script.new()
@onready var story_getter = get_node("StoryGetter")

func _on_continue_pressed():
	print("navigator received continue")
	story_getter.RequestContinue();

func _on_choice_pressed(index):
	print("navigator received choice " + str(index))
	story_getter.FeedChoice(index);

func _process(_delta):
	# manually start the story (because it cannot do so automatically yet)
	if Input.is_action_just_pressed("start"): 
		print("got enter")
		story_getter.RequestContinue()
