extends RichTextLabel

const _box_opacity_default:float = 0.5# TODO: move to settings file
@export var _box_opacity:float = _box_opacity_default # TODO: move to settings file
var box_opacity = _box_opacity


@onready var box:TextureRect= get_node("TextPresenterBackground")

const _typing_delay_base_default:float = 0.01# TODO: move to settings file
@export var _typing_delay_base = 0.01 # TODO: move to settings file
var typing_delay_base = _typing_delay_base
#TODO: make signal for changed settings, and update


var typing_speed_modifier = 1

var typing_delay: float:
	get:
		return typing_delay_base/typing_speed_modifier

@export var timer: Timer  

@onready var audio_player: AudioStreamPlayer =$AudioStreamPlayer

signal finished_typing
var typing: bool = false

func _ready():
	present_story("Press Continue To Start the Story.")
	_apply_settings()

func _apply_settings():
	box_opacity = clamp(_box_opacity, 0, 1)
	box.modulate.a=box_opacity
	typing_delay_base = clamp(_typing_delay_base, 0, 999999999)

func present_console_message(content: String, warning: bool = false) -> void:
	if warning:
		push_warning("Warning from INK Script: " + content)
	else:
		print("Message from INK Script: " + content)

func present_story(content: String) -> void:
	# Prep Textbox
	self.clear()
	self.visible_characters = 0
	
	# Load Text
	self.set_text(content)
	
	# Type Text
	typing=true
	var level = 0
	for n in self.get_parsed_text():
		# Evaluate 'n'
		# the bracket check is not needed anymore- we're getting parsed text! Alleluya Godot
		if n=='[': 
			level +=1 # we are one more layer into brackets
		elif n==']':
			level -=1 # we are one less layer into brackets
		
		self.visible_characters+=1# show the letter
		
		if level>0:
			continue #skip delay, go to next character
		elif level<0: 
			push_warning("bracket depth error")
		else: # level==0			
			audio_player.play() # play some audio
			timer.start(typing_delay) # start the delay TODO:make dependent on 'n'
			await timer.timeout # wait for the typing delay
			if(!typing):
				break # exit loop if we have been skipped
	
	# Finish Text
	finish_text()

func finish_text():
	#TODO: Add finish line sound?
	visible_characters = -1 # set all visible
	
	# stop typing
	timer.stop()
	typing=false
	finished_typing.emit() #give signal

func _spd(new):
	typing_speed_modifier = new
