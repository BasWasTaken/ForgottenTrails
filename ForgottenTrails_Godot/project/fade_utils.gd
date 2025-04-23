extends Node

class_name FadeUtils

static func fade_color(node: ColorRect, to_color: Color, duration: float = 1.0) -> void:
	var start_color = node.modulate
	var elapsed := 0.0
	while elapsed < duration:
		await node.get_tree().process_frame
		elapsed += node.get_process_delta_time()
		var t = clamp(elapsed / duration, 0.0, 1.0)
		node.modulate = start_color.lerp(to_color, t)
	#node.modulate = to_color this caused problems when multiple proceses overlapped

static func fade_alpha(node: CanvasItem, to_alpha: float, duration: float = 1.0) -> void:
	var current = node.modulate
	var target = Color(current.r, current.g, current.b, to_alpha)
	await fade_color(node, target, duration)
