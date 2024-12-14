#extends Control
#class_name SettingHandler
#
#@export var reference:Settings.setting
#
#@export var value: #can i even do this, or is this not possible because we're dealing with variant variables..?
	#get:
		#return Settings.values[reference]
#
#@export var default:
	#get:
		#return Settings.values[reference]
	#
## TODO write a func that changes the default value too
