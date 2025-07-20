extends OptionButton

func choose_visible(value: bool):
	if value:
		show()
	else:
		hide()

func toggle_hidden():
	if visible:
		#_depopulate()
		hide()
	else:
		show()
		#populate()

func _ready():
	visibility_changed.connect(_on_visibility_changed)

func _on_visibility_changed():
	if visible:
		populate()
	else:
		_depopulate()

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
	hide()
	#TODO hide elements from code after a succesful load has been confirmed, not by grossly emmittiny a signal
