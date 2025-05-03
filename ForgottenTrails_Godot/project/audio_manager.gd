extends Node

@onready var vox_player = $VOXPlayer
@onready var sfx_player = $SFXPlayer
@onready var ambient_player = $AmbiencePlayer
@onready var music_player = $MusicPlayer
@onready var sysfx_player = $SystemPlayer

func set_volume(bus: String, config):
	# Get the bus index
	var bus_index = AudioServer.get_bus_index(bus)
	var linear_volume:float = ConfigHandler.get_live_value(config) as float
	var db_volume = linear_to_db(linear_volume/100)	
	AudioServer.set_bus_volume_db(bus_index, db_volume)
	print("Volume for bus ", bus, " set to ", db_volume, " dB (", linear_volume, "%)")

func play_audio(stream: AudioStream, source: String, volume: float = 1.0):
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
	# Set the volume for the player
	player.volume_db = linear_to_db(volume) #AudioServer.get_bus_volume_db(AudioServer.get_bus_index(source)) + linear_to_db(volume)
	# Play the audio
	player.play()

func remove_ambience():	
	# Stop the ambience player
	print("removing individual ambience clips is not implemented yet")
	ambient_player.stop()

func remove_all_ambience():
	# Stop the ambience player
	ambient_player.stop()


func play_audio_by_string(stream: String, source: String, volume: float = 1.0):
	# Load the audio stream from the string path
	var audio_stream: AudioStream = load(stream)
	# Call the play_audio function with the loaded stream and source
	play_audio(audio_stream, source)

var button_click_sound: AudioStream = preload("res://project/system/button_click.mp3")
var button_release_sound: AudioStream = preload("res://project/system/button_release.mp3")

func _ready():
	# Connect the signal to the function
	# get the signal from godot's event system
	SignalBus.ui_button_clicked.connect(_on_button_clicked)
	SignalBus.ui_button_released.connect(_on_button_released)

	ConfigHandler.setting_changed.connect(
		func(id, _value):
			if id == ConfigHandler.choose.keys()[ConfigHandler.choose.master_volume]:
				set_volume("Master", ConfigHandler.choose.keys()[ConfigHandler.choose.master_volume]) #todo make less error prone by using enums or so
			elif id == ConfigHandler.choose.keys()[ConfigHandler.choose.voice_volume]:
				set_volume("VOX", ConfigHandler.choose.keys()[ConfigHandler.choose.voice_volume])
			elif id == ConfigHandler.choose.keys()[ConfigHandler.choose.sfx_volume]:
				set_volume("SFX", ConfigHandler.choose.keys()[ConfigHandler.choose.sfx_volume])
			elif id == ConfigHandler.choose.keys()[ConfigHandler.choose.ambiant_volume]:
				set_volume("Ambience", ConfigHandler.choose.keys()[ConfigHandler.choose.ambiant_volume])
			elif id == ConfigHandler.choose.keys()[ConfigHandler.choose.music_volume]:
				set_volume("Music", ConfigHandler.choose.keys()[ConfigHandler.choose.music_volume])
			elif id == ConfigHandler.choose.keys()[ConfigHandler.choose.system_volume]:
				set_volume("System", ConfigHandler.choose.keys()[ConfigHandler.choose.system_volume])
	)
	# Set the initial volume for each bus
	# beetje lelijk zoveel verschillende signalen te hebben, maar ach
	set_volume("Master", ConfigHandler.choose.keys()[ConfigHandler.choose.master_volume])
	set_volume("VOX", ConfigHandler.choose.keys()[ConfigHandler.choose.voice_volume])
	set_volume("SFX", ConfigHandler.choose.keys()[ConfigHandler.choose.sfx_volume])
	set_volume("Ambience", ConfigHandler.choose.keys()[ConfigHandler.choose.ambiant_volume])
	set_volume("Music", ConfigHandler.choose.keys()[ConfigHandler.choose.music_volume])
	set_volume("System", ConfigHandler.choose.keys()[ConfigHandler.choose.system_volume]) 

	# listen for audio signals from ink, with a proxy function to add the source parameter
	SignalBus.ink_func_audio_vox_play.connect(func(stream, volume): play_audio(stream, "vox", volume))
	SignalBus.ink_func_audio_sfx_play.connect(func(stream, volume): play_audio(stream, "sfx", volume))
	SignalBus.ink_func_audio_ambience_play.connect(func(stream, volume): play_audio(stream, "ambient", volume))
	SignalBus.ink_func_audio_music_play.connect(func(stream, volume): play_audio(stream, "music", volume))

	SignalBus.ink_func_audio_ambience_rmv.connect(remove_ambience)
	SignalBus.ink_func_audio_ambience_rmv_all.connect(remove_all_ambience)
	

func _on_button_clicked():
	# Play the button click sound
	AudioManager.play_audio(button_click_sound, "sfx")
	# Optionally, you can add more logic here if needed
func _on_button_released():
	# Play the button release sound
	AudioManager.play_audio(button_release_sound, "sfx")
	# Optionally, you can add more logic here if needed
