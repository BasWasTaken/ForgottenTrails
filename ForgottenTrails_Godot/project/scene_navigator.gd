extends Node

@export var main_gameplay = preload("res://project/main_gameplay_scene.tscn")
@export var main_menu = preload("res://project/main_menu/main_menu_scene.tscn")
var instance_menu
var instance_gameplay

func _ready(): 
	instance_menu = main_menu.instantiate()
	instance_gameplay = main_gameplay.instantiate()

func _process(_delta):
	if Input.is_action_just_pressed("quit"):
		quit_game()

	if Input.is_action_just_pressed("reset"):
		reset_game()


func launch_game(file = null):
	get_tree().change_scene_to_packed(SceneChanger.main_gameplay) # why scene and not instance..?
	if file:
		#TODO: better resetting of text presenter states
		DataManager.load_game(file)
	else:
		pass
		#TODO: start a new game file


func quit_game():
	get_tree().quit()

func reset_game():
	get_tree().change_scene_to_packed(main_menu) # why scene and not instance..?
