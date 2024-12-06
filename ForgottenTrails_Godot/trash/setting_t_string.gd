#@tool
#extends SettingHandler
#
#@export var default:String="":
	#set(value):
		#default_value = value
	#get:
		#return default_value
