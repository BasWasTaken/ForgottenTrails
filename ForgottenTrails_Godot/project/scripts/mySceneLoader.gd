extends Node



func get_files()->Array:
	return DataManager.get_files()

func _on_new_game_pressed():
	SceneNavigator.launch_game()

func _on_continue_pressed():
	SceneNavigator.launch_game(DataManager.get_most_recent_savefile())

func load_game(file):
	SceneNavigator.launch_game(file)


	
