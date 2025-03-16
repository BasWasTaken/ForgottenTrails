extends Node
enum { PRINTING, WAITING, PROCESSING, LOCKED }
var current_state = PROCESSING

func set_state(new_state):
    print("VN State changed to: ", new_state) #een groter finite state machine pattern heeft entry and exit methods, maar dat is hier niet nodig
    current_state = new_state
