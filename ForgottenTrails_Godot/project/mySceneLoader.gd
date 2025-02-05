extends Node

@export var scene = preload("res://project/main_gameplay_scene.tscn")

var instance

func _ready(): 
	instance = scene.instantiate()

func launch_game():
	get_tree().change_scene_to_packed(scene) # why scene and not instance..?


func _on_start_game_pressed():
	launch_game()
	DataManager.load_game(DataManager.data_method.auto)
