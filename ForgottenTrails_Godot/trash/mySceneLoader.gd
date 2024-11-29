extends Node

var scene = preload("res://project/main_gameplay_scene.tscn")

var instance

func _ready(): 
	instance = scene.instantiate()

func launch_game():
	get_tree().change_scene_to_packed(scene)


func _on_start_game_pressed():
	launch_game()
