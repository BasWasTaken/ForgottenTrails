extends Node
# Store settings here

enum setting{
	NULL,
	text_speed,
	box_opacity
}


var default_values:Dictionary = {
	setting.text_speed: speeds.mid,
}

var values:Dictionary = default_values
# in this or other file, check if file exists and if needed create it with the defaults?


enum speeds{
	slow=1,
	mid=50,
	fast=100
}

#var value:
	#get:
		#if _stored_value!=null:
			#return _stored_value
		#else:  
			#return default_value
