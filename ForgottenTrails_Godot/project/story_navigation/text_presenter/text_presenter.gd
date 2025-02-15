extends RichTextLabel

@onready var box:ColorRect= get_parent()

#--- should this be here? definitions


var typing_speed_base: float = 1
var typing_speed_modifier: float = 1

var typing_speed_net: float:
	get:
		return typing_speed_base * typing_speed_modifier 

func _on_speed_applied():
	typing_speed_base = ConfigHandler.get_live_value(ConfigHandler.choose.keys()[ConfigHandler.choose.text_speed])		
	

var typing_delay: float:
	get:		
		var delay:float = 0
		if typing_speed_net > 0:
			delay = 1/typing_speed_net
		#print("delay: ", delay) 
		return delay

@export var timer: Timer  

@onready var audio_player: AudioStreamPlayer =$AudioStreamPlayer

signal finished_typing
var typing: bool = false

func _ready():
	ConfigHandler.setting_changed.connect(
		func(id, _value):
			if id == ConfigHandler.choose.keys()[ConfigHandler.choose.textbox_opacity]:
				_on_opacity_change_applied()
	)
	ConfigHandler.setting_changed.connect(
		func(id, _value):
			if id == ConfigHandler.choose.keys()[ConfigHandler.choose.text_speed]:
				_on_speed_applied()
	)
	
	_on_opacity_change_applied()
	_on_speed_applied()
	
	present_story("Press Continue To Start the Story.")

var opacity:
	get:
		return ConfigHandler.get_live_value(ConfigHandler.choose.keys()[ConfigHandler.choose.textbox_opacity])

func _on_opacity_change_applied():
	var scaled = opacity * 2.55 #convert 0-100 to 0-255
	#print(scaled)
	box.self_modulate=Color8(0,0,0,scaled as int)
	

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
			audio_player.play() # play some audio #TODO make not all sound at once with instant text
			if(typing_delay>0):	
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
	print("new speed: ",typing_speed_base, " times ", typing_speed_modifier, " = ", typing_speed_net, " seconds per character") 
