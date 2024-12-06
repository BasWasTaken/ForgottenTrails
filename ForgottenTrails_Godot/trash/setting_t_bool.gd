#@tool
#extends SettingHandler
#
#@export var default:bool=false:
	#set(value):
		#default_value = value
	#get:
		#return default_value
