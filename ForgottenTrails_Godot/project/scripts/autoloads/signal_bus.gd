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
# signal control_requests_accept
signal control_requests_continue
# signal control_requests_cancel
signal control_requests_choice(index: int)
signal control_requests_skip

signal ink_func_print(text: String)
signal ink_func_spd(speed: int)

signal control_requests_options
signal control_requests_quicksave
signal control_requests_quickload
signal control_requests_load

signal ui_button_clicked()
signal ui_button_released()

signal ink_sent_story(story: String)
signal ink_sent_choices(choices: Array)
signal ink_sent_no_choices

signal ink_func_backdrop_image(image: String, duration: float)
signal ink_func_fade_to_color(color: String, duration: float)
signal ink_func_fade_in(duration: float)
signal ink_func_fade_out(black: bool, duration: float)

signal ink_func_effect(effect: String)
signal ink_func_flash(color: String, amount: int)

signal ink_func_sprite_present_by_string(character : String, variant: String, coords: String)
signal ink_func_sprite_remove(character : String)
signal ink_func_sprite_remove_all()

signal ink_func_audio_vox_play(voice: String, volume: float)
signal ink_func_audio_sfx_play(sfx: String, volume: float)
signal ink_func_audio_ambience_play(ambience: String, volume: float)
signal ink_func_audio_ambience_rmv(ambience: String)
signal ink_func_audio_ambience_rmv_all()
signal ink_func_audio_music_play(music: String, volume: float)
signal ink_func_audio_music_stop()