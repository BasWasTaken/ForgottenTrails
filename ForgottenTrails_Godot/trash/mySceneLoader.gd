extends Node

var scene = preload("res://project/main_gameplay_scene.tscn")

func _ready():
	var instance = scene.instantiate()
	get_tree().change_scene_to_packed(scene)
