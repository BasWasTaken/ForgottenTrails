extends Node

func quicksave():
	SignalBus.control_requests_quicksave.emit()