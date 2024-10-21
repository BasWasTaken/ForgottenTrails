class_name setting_float#TODO also make for enums, etc
var default_value:float
var min_value:float=0
var max_value:float=1
var saved_value:float = default_value

func _init(default, min, max):
	default_value = default
	saved_value = default_value
	min_value = min
	max_value = max

enum speeds{
	slow=1,
	mid=50,
	fast=100
}
