#extends Node
#class_name Setting
#
#var default_value
#var _stored_value #assigned from the config handler
#
#var value:
	#get:
		#if _stored_value!=null:
			#return _stored_value
		#else:  
			#return default_value
