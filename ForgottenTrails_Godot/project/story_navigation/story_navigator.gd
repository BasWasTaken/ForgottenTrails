extends Node

#@onready var my_csharp_script = load("res://UI/story_getter.cs")
#@onready var my_csharp_node = my_csharp_script.new()
@onready var story_getter = get_node("StoryGetter")
@onready var text_presenter = get_node("TextPresenterPanel/TextPresenter")
@onready var choices_presenter = get_node("ChoicePresenter")

signal skip

var selectedChoice = -1

func _ready():
	pass

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

func _on_continue_pressed():	
	#print("story_navigator received request to continue, evaluating...");
	if text_presenter.typing:
		#print("Typer Busy. Skipping to end of Line.");
		skip.emit();
	elif story_getter.story.CanContinue:
		#print("validated. Continueing Story.");
		_send_continue()
	elif choices_presenter.get_child_count()>1:#if there are choice buttons
		if selectedChoice == -1:
			print("Select a Choice first");
		else:
			#print("validated. Fed selected choice.");
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
