extends TabContainer

func _ready():
	# Connect the signal to handle tab changes
	tab_changed.connect(_on_visibility_changed)
	visibility_changed.connect(_on_visibility_changed)
	current_tab = -1

func _on_visibility_changed():
	if current_tab == -1 || is_visible_in_tree(): # if no tab is selected, do nothing
		hide()
		mouse_filter = Control.MOUSE_FILTER_IGNORE
	else:
		show()
		mouse_filter = Control.MOUSE_FILTER_PASS