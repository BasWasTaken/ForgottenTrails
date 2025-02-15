extends Node



func get_files()->Array:
	return DataManager.get_files()

func _on_new_game_pressed():
	SceneChanger.launch_game()

func _on_continue_pressed():
	SceneChanger.launch_game(get_files()[0])

func load_game(file):
	SceneChanger.launch_game(file)


	
