extends OptionButton

func toggle_hidden():
	if visible:
		_depopulate()
		hide()
	else:
		show()
		populate()

func populate():
	clear()
	var save_slots = DataManager.get_files(DataManager.player_name,"any")
	for save_slot in save_slots:
		add_item(save_slot)#.get_file().get_basename())

func _depopulate():
	clear()


func _on_confirm_load_game_pressed():
	print("loading game from save slot: " + get_item_text(get_selected()))
	DataManager.load_game(get_item_text(get_selected()))
