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

# Ignore the warning about unused signals- they are used in other scripts
signal user_skip_requested
signal printer_text_finished
#signal menu_open_requested(window:Control) leaving this signal for later, when|if i change how windows are management, or the current implementation starts causing problems