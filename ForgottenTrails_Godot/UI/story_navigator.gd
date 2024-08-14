extends Node

#@onready var my_csharp_script = load("res://UI/story_getter.cs")
#@onready var my_csharp_node = my_csharp_script.new()
@onready var story_getter = get_node("StoryGetter")

#remove: signal continued_story(text)
#remove: signal encountered_no_choices()
#remove: signal encountered_choice(choice)

#remove: func _ready(): # catch various events to convert and retransmit them via godot signals. 
#remove: 	story_getter.continued_story_EventHandler.connect(_on_continued_story)
#remove: 	story_getter.encountered_no_choices_EventHandler.connect(_on_encountered_no_choices)
#remove: 	story_getter.encountered_choice_EventHandler.connect(_on_encountered_choice)
	

func _on_continue_button_pressed(): # connecting here is more visible than doing sofrom a c# script
	story_getter.TryContinue();

#remove: func _on_continued_story(text):
#remove: 	continued_story.emit(text)

#remove: func _on_encountered_no_choices():
#remove: 	encountered_no_choices.emit()

#remove: func _on_encountered_choice(choice):
#remove: 	encountered_choice.emit(choice)


func _on_choices_presenter_choice_pressed(index):
	story_getter.FeedChoice(index);
