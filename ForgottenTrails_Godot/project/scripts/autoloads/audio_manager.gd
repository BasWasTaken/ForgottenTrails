extends Node

@onready var vox_player = $VOXPlayer
@onready var sfx_player = $SFXPlayer
@onready var ambience_player = $AmbiencePlayer
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
		"ambience":
			player = ambience_player
		"music":
			player = music_player
		"system":
			player = sysfx_player
	# cast to pylophonic for ambience player
	if source == "ambience":
	#	stream = stream as AudioStreamPolyphonic
		print("polyphonic playing is not implemented yet")
	
	if (source == "sfx" or source == "vox" or source == "system") and player.playing:
		# play as soon as player is available
		await player.finished # wait for the player to be available #can this have a timeout?

	# Set the new stream
	player.stream = stream
	# Set the volume for the player
	player.volume_db = linear_to_db(volume) #AudioServer.get_bus_volume_db(AudioServer.get_bus_index(source)) + linear_to_db(volume)
	# NOTE: this sets the volume for the player, not the stream itself. thus you cannot have different volume levels for ambiences
 	
	# Play the audio
	player.play()
	print("Playing audio ", stream, " from source: ", source, " with volume: ", volume) #NOTE cannot figure out how to get name of the clip...

func remove_ambience(_stream: AudioStream):	
	# Stop the ambience player
	print("removing individual ambience clips is not implemented yet")
	ambience_player.stop()

func remove_ambience_by_string(stream: String):
	# Load the audio stream from the string path
	var audio_stream: AudioStream = load(stream)
	# Call the remove_ambience function with the loaded stream
	remove_ambience(audio_stream)

func remove_all_ambience():
	# Stop the ambience player
	ambience_player.stop()


func play_audio_by_string(clip: String, source: String, volume: float = 1.0):
	# Load the audio stream from the string path
	var audio_stream: AudioStream = load("res://project/assets/audio/" +source +"/snd_"+source+"_"+ clip+".wav")
	
	# TODO make a helper script for retreiving files, so that the various attempts can be handled there. reduces redundant code.
	if audio_stream == null:
		audio_stream = load("res://project/assets/audio/" +source +"/snd_"+source+"_"+ clip+".mp3")
		if audio_stream == null:
			audio_stream = load("res://project/assets/audio/" +source +"/snd_"+source+"_"+ clip+".ogg")
			if audio_stream == null:
				print("Audio stream not found at path: " + "res://project/assets/audio/" +source +"/snd_"+source+"_"+ clip)



	# Call the play_audio function with the loaded stream and source
	play_audio(audio_stream, source, volume)

var button_click_sound: AudioStream = preload("res://project/assets/audio/system/snd_system_ui_click.mp3")
var button_release_sound: AudioStream = preload("res://project/assets/audio/system/snd_system_ui_release.mp3") #NOTE dit legt eigenlijk blood dat de Naam "system" onzin is, het zou "UI" moeten zijn, dat maakt meteen deze redundance onnodig
# TODO rename system channel and bus to ui and update all references as well as naming convention

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
	SignalBus.ink_func_audio_vox_play.connect(func(stream, volume): play_audio_by_string(stream, "vox", volume))
	SignalBus.ink_func_audio_sfx_play.connect(func(stream, volume): play_audio_by_string(stream, "sfx", volume))
	SignalBus.ink_func_audio_ambience_play.connect(func(stream, volume): play_audio_by_string(stream, "ambience", volume))
	SignalBus.ink_func_audio_music_play.connect(func(stream, volume): play_audio_by_string(stream, "music", volume))

	SignalBus.ink_func_audio_ambience_rmv.connect(remove_ambience)
	SignalBus.ink_func_audio_ambience_rmv_all.connect(remove_all_ambience)
	SignalBus.ink_func_audio_music_stop.connect(func(): music_player.stop())
	

func _on_button_clicked():
	# Play the button click sound
	AudioManager.play_audio(button_click_sound, "system")
	# Optionally, you can add more logic here if needed
func _on_button_released():
	# Play the button release sound
	AudioManager.play_audio(button_release_sound, "system")
	# Optionally, you can add more logic here if needed
