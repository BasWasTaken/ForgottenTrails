extends Node

@export_dir var saving_directory = "user://saves"
var player_name = "dev"

# Construct the full path for a player's save directory
var file_path:
	get:
		return saving_directory + "/" + player_name + "/"

func _ready():
	# Ensure the root save directory exists
	if not DirAccess.dir_exists_absolute(saving_directory):
		DirAccess.make_dir_absolute(saving_directory)

	# Ensure player-specific directory exists
	if not DirAccess.dir_exists_absolute(file_path):
		DirAccess.make_dir_absolute(file_path)

	# connect signals
	SignalBus.control_requests_quicksave.connect(quicksave_game)
	SignalBus.control_requests_quickload.connect(load_most_recent_quicksavefile)

# Quick, Auto, and Manual Save Functions
func quicksave_game(state: String):
	save_game(state, "quick")

func autosave_game(state: String):
	save_game(state, "auto")

func save_game(state: String, method: String):
	print("Starting " + method + " save with state: " + state)
	

	# Get timestamp (safe filename format)
	var datetime = Time.get_datetime_string_from_system(true).replace(":", "").replace(" ", "_").replace(".", "")
	var filename = file_path + datetime + "_" + method + ".save"

	# Attempt to open file for writing
	var save_file = FileAccess.open(filename, FileAccess.WRITE)
	assert(save_file, "Failed to open file for writing: " + filename + " with error " + str(FileAccess.get_open_error()))

	# Store the ink story state at the start of the file
	save_file.store_line(state)

	# Iterate over all nodes in the "persist" group and save their data
	var save_nodes = get_tree().get_nodes_in_group("persist")
	for node in save_nodes:
		if !node.has_method("save"):
			print("Persistent node '%s' is missing a save() function, skipped" % node.name)
			continue

		# Call the node's save function and serialize as JSON
		var node_data = node.call("save")
		var json_string = JSON.stringify(node_data)

		# Write data to file
		save_file.store_line(json_string)

	save_file.close()
	print("Game saved")

# Retrieve saved files
func get_files(_player: String = player_name, type: String = "any") -> Array:
	var files = []
	
	# Ensure the directory exists before opening
	if not DirAccess.dir_exists_absolute(file_path):
		push_error("Player save directory does not exist: " + file_path)
		return files

	var dir = DirAccess.open(file_path)
	if dir == null:
		push_error("Failed to open directory: " + file_path)
		return files

	dir.list_dir_begin()
	var file_name = dir.get_next()
	while file_name != "":
		if file_name.ends_with(".save"):
			# Extract metadata from filename
			var parts = file_name.get_basename().split("_")
			if parts.size() >= 2:
				var file_type = parts[1]  # Assuming format is YYYYMMDD_HHMMSS_type.save
				
				if type == "any" or file_type == type:
					files.append(file_path + file_name)
					
		file_name = dir.get_next()
	dir.list_dir_end()

	# Sort the files alphabetically (by timestamp)
	files.sort()
	files.reverse()
	return files

# get and load the most recent save file of each type
func get_most_recent_quicksavefile():
	var files = get_files(player_name, "quick")
	if files.size() > 0:
		return files[0]
	return null

func load_most_recent_quicksavefile():
	load_game(get_most_recent_quicksavefile())

func get_most_recent_autosavefile():
	var files = get_files(player_name, "auto")
	if files.size() > 0:
		return files[0]
	return null

func load_most_recent_autosavefile():
	load_game(get_most_recent_autosavefile())

func get_most_recent_savefile():
	var files = get_files(player_name, "any")
	if files.size() > 0:
		return files[0]
	return null

func load_most_recent_savefile():
	load_game(get_most_recent_savefile())

# signal for emitting storystat to story_navigator
signal load_story_state(story_state: String)
# Load a save file
func load_game(file: String):
	
	print("Loading game from file: " + file)	
	# TODO: if needed(?) first ensure we're in a save state for loading
	#text_presenter.finish_text()
	#text_presenter.clear()
	#choices_presenter.clear()


	# Ensure the file exists before attempting to read
	assert(FileAccess.file_exists(file), "File does not exist: " + file)

	var save_file = FileAccess.open(file, FileAccess.READ)

	# Clear existing objects in the "persist" group to prevent duplication
	# var save_nodes = get_tree().get_nodes_in_group("persist")
	# for node in save_nodes:
	# 	node.queue_free()

	# Read and process saved data

 	# Load the ink story state
	load_story_state.emit(save_file.get_line())
	#TODO: Can be improved with bugfixing - currently a bit volatile after loading, clickling l keeps continueing in a sense.
	# should really make sure that all processes are ahalted before loading, like its a still system and a clean start

	while save_file.get_position() < save_file.get_length():
		var json_string = save_file.get_line()

		# Parse JSON string
		var json = JSON.new()
		var parse_result = json.parse(json_string)
		if parse_result != OK:
			print("JSON Parse Error: ", json.get_error_message(), " in ", json_string, " at line ", json.get_error_line())
			continue

		# Extract node data
		var node_data = json.data

		# # Recreate the object in the scene tree
		# var new_object = load(node_data["filename"]).instantiate()
		# get_node(node_data["parent"]).add_child(new_object)
		# new_object.position = Vector2(node_data["pos_x"], node_data["pos_y"])

		# # Restore object properties
		# for key in node_data.keys():
		# 	if key not in ["filename", "parent", "pos_x", "pos_y"]:
		# 		new_object.set(key, node_data[key])
		
	print("Game loaded")
