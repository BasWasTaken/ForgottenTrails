extends RichTextLabel
#TTODO rename this to printer or something
@export var box:ColorRect # TODO: als je het ooit helemaal netjes wil doen (lage prio), kun je het aansturen van de popacity van it object loshalen uit dit script en in een eigen script gooien, aan die node vast. daarmee haal je de noodzaak van deze referentie hier helemaal weg. zie note marked 20250301140143.

#--- should this be here? definitions


var typing_speed_base: float = 1
var typing_speed_modifier: float = 1

var typing_speed_net: float:
	get:
		return typing_speed_base * typing_speed_modifier 

func _on_speed_applied():
	typing_speed_base = ConfigHandler.get_live_value(ConfigHandler.choose.keys()[ConfigHandler.choose.text_speed]) 


	print("new speed: ",typing_speed_base, " times ", typing_speed_modifier, " = ", typing_speed_net, " characters per second") 		
	

var typing_delay: float:
	get:		
		var delay:float = 0
		if typing_speed_net > 0:
			delay = 1/typing_speed_net
		#print("delay: ", delay) 
		return delay

@onready var timer: Timer = $Timer

@onready var audio_player: AudioStreamPlayer =$AudioStreamPlayer

func _ready():
	SignalBus.ink_sent_story.connect(present_story)
	SignalBus.control_requests_skip.connect(skip_to_printed)

	SignalBus.ink_func_print.connect(present_console_message)
	SignalBus.ink_func_spd.connect(_spd)

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
	_init()
	printer_state.set_state(printer_state.VN_State.WAITING)
	present_story("Press Continue To Start/Continue the Story.")

func _init():
	clear()

var opacity:
	get:
		return ConfigHandler.get_live_value(ConfigHandler.choose.keys()[ConfigHandler.choose.textbox_opacity])

func _on_opacity_change_applied():
	var scaled = opacity * 2.55 #convert 0-100 to 0-255
	#print(scaled)
	box.color=Color8(0,0,0,scaled as int)
	

func present_console_message(content: String, warning: bool = false) -> void:
	if warning:
		push_warning("Warning from INK Script: " + content)
	else:
		print("Message from INK Script: " + content)

func present_story(content: String) -> void:
	# set state
	printer_state.set_state(printer_state.VN_State.PRINTING)

	# Prep Textbox
	self.clear()
	self.visible_characters = 0
	
	# Load Text
	self.set_text(content)
	
	# Type Text
	var level = 0
	for n in self.get_parsed_text():
		if visible_characters == -1: # exit loop if state is no longer printing (such as if the user skips)
			break 
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
	
	# Finish Text
	finish_text()

func skip_to_printed():
	visible_characters = -1 # set all visible
	# wait for the loop to exit, and it should automatically enter the finish_text() function
	timer.stop()

func finish_text():
	#TODO: Add finish line sound?
	#TODO pas hier de knoppen laten verschijnen
	visible_characters = -1 # set all visible
	
	# stop typing
	timer.stop()
	printer_state.set_state(printer_state.VN_State.WAITING)
	SignalBus.printer_text_finished.emit() #give signal

func _spd(new):
	typing_speed_modifier = new
	_on_speed_applied()
