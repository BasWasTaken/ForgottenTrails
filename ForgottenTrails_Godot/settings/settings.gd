extends Control

const _box_opacity_default:float = 0.5
@export_range(0,1) var _box_opacity:float = _box_opacity_default:
	set(new_value):
		if _box_opacity != new_value:
			_box_opacity = new_value
			settings_changed.emit()
	get:
		return _box_opacity

const _typing_delay_base_default:float = 0.01
@export_range(0,1) var _typing_delay_base:float = _typing_delay_base_default:
	set(new_value):
		if _typing_delay_base != new_value:
			_typing_delay_base = new_value
			settings_changed.emit()
	get:
		return _typing_delay_base


signal settings_changed
