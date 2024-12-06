#@tool
#extends SettingHandler
#
#@export var default:int=0:
	#set(value):
		#default_value = value
	#get:
		#return default_value
