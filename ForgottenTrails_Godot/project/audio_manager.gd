extends Node

@onready var vox_player = $VOXPlayer
@onready var sfx_player = $SFXPlayer
@onready var ambient_player = $AmbiencePlayer
@onready var music_player = $MusicPlayer
@onready var sysfx_player = $SystemPlayer

func play_audio(stream: AudioStream, source: String):
	var player: AudioStreamPlayer
	match source:
		"vox":
			player = vox_player
		"sfx":
			player = sfx_player
		"ambient":
			player = ambient_player
		"music":
			player = music_player
		"sysfx":
			player = sysfx_player
	# Stop any currently playing audio #TODO only sometimes, sometimes you'll want it overlapping
	player.stop()
	# Set the new stream
	player.stream = stream
	# Play the audio
	player.play()