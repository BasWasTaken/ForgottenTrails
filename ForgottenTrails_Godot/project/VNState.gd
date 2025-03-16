extends Node
enum { PRINTING, WAITING, PROCESSING, LOCKED }
var _current_state = LOCKED

var get_state:
    get:
        return _current_state

func set_state(new_state):
    print("VN | printer State changed to: ", new_state) #een groter finite state machine pattern heeft entry and exit methods, maar dat is hier niet nodig
    _current_state = new_state