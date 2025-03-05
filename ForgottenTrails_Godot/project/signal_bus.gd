extends Node

# Define global signals
# signal story_continued
# signal story_printed
# signal story_skipped
# signal background_changed(path)
# signal vn_state_changed(state)
# signal choice_selected(index)
# signal game_saved
# signal game_loaded
# signal game_reset
# FORMAT: actor_topic_action
# FORMAT: subject_object_verb
# FORMAT: who_which_what
# hm actually i don't think the formats are helpful. i'm just going to try to have clear namings. 
# the fact that most of the signals are collected in one place, should already help prevent losing track of what exists. i can easily see if i am naming things too similarly

# Ignore the warning about unused signals- they are used in other scripts

#signal menu_open_requested(window:Control) leaving this signal for later, when|if i change how windows are management, or the current implementation starts causing problems

signal printer_text_finished
signal continue_button_pressed
signal choice_button_pressed(index:int) 
signal skip_key_pressed

signal inkfunc_print(text: String)
signal inkfunc_spd(speed: int)