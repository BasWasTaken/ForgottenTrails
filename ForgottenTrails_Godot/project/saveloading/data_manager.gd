extends Node

@export_dir var saving_directory = "user://saves"
var player_name = "dev"

var file_path:
	get:
		return saving_directory + "/"+player_name+"/"

func _ready():
	# Ensure the root save directory exists
	if not DirAccess.dir_exists_absolute(saving_directory):
		DirAccess.make_dir_absolute(saving_directory)

	# Now we can safely open it
	var dir = DirAccess.open(saving_directory)
	if dir == null:
		push_error("Failed to open directory: " + saving_directory)
		return

	# Now create subdirectories
	dir.make_dir_recursive(file_path)


# need a quicksave, autosave and manual save


# Script - Globally accessible
# Note: This can be called from anywhere inside the tree. This function is
# path independent.
# Go through everything in the persist category and ask them to return a
# dict of relevant variables.

func quicksave_game(state:String):
	save_game(state, "quick")

func autosave_game(state:String):
	save_game(state, "auto")

func save_game(state:String, method):
	print("starting " + method + " save" + " with state: " + state)
	# determine timestamp

	# determine file path	
	var datetime = Time.get_date_string_from_system()

	var filename = file_path + method + "_" + datetime + ".save"
	var save_file = FileAccess.open(filename, FileAccess.WRITE)

	assert(save_file, "Failed to open file for writing: " + filename + " with error " + str(FileAccess.get_open_error()))

	var save_nodes = get_tree().get_nodes_in_group("persist")
	for node in save_nodes:
		# Check the node is an instanced scene so it can be instanced again during load.
		#if node.scene_file_path.is_empty():
			#print("persistent node '%s' is not an instanced scene, skipped" % node.name)
			#continue

		# Check the node has a save function.
		if !node.has_method("save"):
			print("persistent node '%s' is missing a save() function, skipped" % node.name)
			continue

		# Call the node's save function.
		var node_data = node.call("save")

		# JSON provides a static method to serialized JSON string.
		var json_string = JSON.stringify(node_data)

		# Store the save dictionary as a new line in the save file.
		save_file.store_line(json_string)

	# store the ink file state
	save_file.store_line(state)

	save_file.close()
	print("Game saved")

func get_files(player: String = player_name, type: String = "any") -> Array:
	var files = []
	var dir = DirAccess.open(saving_directory)
	if dir == null:
		push_error("Failed to open directory: " + saving_directory)
		return files

	dir.list_dir_begin()
	var file_name = dir.get_next()
	while file_name != "":
		if file_name.ends_with(".save"):
			# Use full path
			var full_path = saving_directory + "/" + file_name
			
			# Extract player and type based on assumed file structure
			var parts = file_name.get_basename().split("_")  # Adjust if structure differs
			if parts.size() >= 2:
				var file_player = parts[0]
				var file_type = parts[1]
				
				if (player == "any" or file_player == player) and (type == "any" or file_type == type):
					files.append(full_path)
					
		file_name = dir.get_next()
	dir.list_dir_end()

	# Sort the files chronologically by modification time
	files.sort_custom(_compare_file_modification_time)
	return files

func _compare_file_modification_time(a: String, b: String) -> int:
	var time_a = FileAccess.get_modified_time(a)
	var time_b = FileAccess.get_modified_time(b)
	return sign(time_a - time_b)  # Ensures proper sorting


func load_most_recent_quicksavefile():
	load_game(get_files(player_name,"quick")[0])

func load_most_recent_autosavefile():
	load_game(get_files(player_name,"auto")[0])

func load_most_recent_savefile():
	load_game(get_files(player_name, "any")[0])

func load_game(file):
	print("Loading game")
	#TODO create behaviour based on enum
	
	if not FileAccess.file_exists(file):
		return # Error! We don't have a save to load.

	# CONTINUE HERE AFTER 20250205221645
	# OK I REALLY QUESTION THE NEED FOR THE BELOW, AT LEAST FOR NOW
	# I WANT TO at least do the story state for json, ie 	StoryNavigator.story_state = save_file.get_line()

	# for now there are some issues with needing static and such which i also need to look at, as i'm running up against what statics can and can;'t do. i have globals for this, i should use those
	


	# We need to revert the game state so we're not cloning objects
	# during loading. This will vary wildly depending on the needs of a
	# project, so take care with this step.
	# For our example, we will accomplish this by deleting saveable objects.
	var save_nodes = get_tree().get_nodes_in_group("persist")
	for i in save_nodes:
		i.queue_free()

	# Load the file line by line and process that dictionary to restore
	# the object it represents.
	var save_file = FileAccess.open(file, FileAccess.READ)
	while save_file.get_position() < save_file.get_length():
		var json_string = save_file.get_line()

		# Creates the helper class to interact with JSON.
		var json = JSON.new()

		# Check if there is any error while parsing the JSON string, skip in case of failure.
		var parse_result = json.parse(json_string)
		if not parse_result == OK:
			print("JSON Parse Error: ", json.get_error_message(), " in ", json_string, " at line ", json.get_error_line())
			continue

		# Get the data from the JSON object.
		var node_data = json.data

		# Firstly, we need to create the object and add it to the tree and set its position.
		var new_object = load(node_data["filename"]).instantiate()
		get_node(node_data["parent"]).add_child(new_object)
		new_object.position = Vector2(node_data["pos_x"], node_data["pos_y"])

		# Now we set the remaining variables.
		for i in node_data.keys():
			if i == "filename" or i == "parent" or i == "pos_x" or i == "pos_y":
				continue
			new_object.set(i, node_data[i])
		
	print("Game loaded")
