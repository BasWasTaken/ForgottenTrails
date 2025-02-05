extends Node

@export_dir var saving_directory
var player_name = "dev"

func _process(_delta):
	if Input.is_action_pressed("quickload"):
		load_game(data_method.quick)
	elif Input.is_action_pressed("quicksave"):
		save_game(data_method.quick)

# need a quicksave, autosave and manual save
enum data_method{
	quick,
	auto,
	manual
}

# Script - Globally accessible
# Note: This can be called from anywhere inside the tree. This function is
# path independent.
# Go through everything in the persist category and ask them to return a
# dict of relevant variables.
func save_game(method:data_method):
	print("Saving game")
	#TODO create behaviour based on enum
	
	var save_file = FileAccess.open("user://savegame.save", FileAccess.WRITE)
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

		if method != data_method.auto:
			#StoryNavigator.save_state()
			pass #TODO implement this #make a new save not just from checkpoints, but from the current state of the story
		
		# store the ink file state
		save_file.store_line(StoryNavigator.story_state)

		save_file.close()
	print("Game saved")

func load_game(method:data_method):
	print("Loading game")
	#TODO create behaviour based on enum
	
	if not FileAccess.file_exists("user://savegame.save"):
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
	var save_file = FileAccess.open("user://savegame.save", FileAccess.READ)
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
