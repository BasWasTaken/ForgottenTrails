using Godot;
using GodotInk;
using System.Collections.Generic;
using Godot.Collections;

public partial class ink_story_processor : Node
{	
	[Export]
	private InkStory story;  
	
	public string latest_state;
	
	private Array choices_array;

	public Array GetChoices()
	{
		return choices_array;
	}
		
	[Signal]
	public delegate void continued_storyEventHandler(string content);
	
	//[Signal]
	//public delegate void encountered_choicesEventHandler(InkChoice[] choices);
	
	[Signal]
	public delegate void fetch_my_choices_plxEventHandler();

	[Signal]
	public delegate void encountered_no_choicesEventHandler();
	
	[Signal]
	public delegate void saved_stateEventHandler(string json);
	
	[Signal]
	public delegate void loaded_stateEventHandler(string json);
	
	// external functions
	[Signal]
	public delegate void ink_function_printEventHandler(string content, bool is_warning);
	
	[Signal]
	public delegate void ink_function_spdEventHandler(float speed);
	
	[Signal]
	public delegate void ink_function_fade_to_colorEventHandler(string color, float delay);
	
	[Signal]
	public delegate void ink_function_fade_inEventHandler(float delay);
	
	[Signal]
	public delegate void ink_function_fade_outEventHandler(bool black, float delay);

	[Signal]
	public delegate void ink_function_effectEventHandler(string effect);

	[Signal]
	public delegate void ink_function_backdrop_imageEventHandler(string image, float delay);

	public override void _Ready()
	{
		story.BindExternalFunction("print", (string text) => EmitSignal(SignalName.ink_function_print, text, false));
		story.BindExternalFunction("print_warning", (string text) => EmitSignal(SignalName.ink_function_print, text, true));
		story.BindExternalFunction("_spd", (float speed) => EmitSignal(SignalName.ink_function_spd,speed));
		story.BindExternalFunction("_clear", (float speed) => EmitSignal(SignalName.ink_function_spd,speed));
		story.BindExternalFunction("_BackdropImage", (string image, float delay) => EmitSignal(SignalName.ink_function_backdrop_image, image, delay));
		story.BindExternalFunction("_FadeToColor", (string color, float delay) => EmitSignal(SignalName.ink_function_backdrop_color, color, delay));
		story.BindExternalFunction("_FadeIn", (float delay) => EmitSignal(SignalName.ink_function_fade_in, delay));
		story.BindExternalFunction("_FadeOut", (bool black, float delay) => EmitSignal(SignalName.ink_function_fade_out, black, delay));
		story.BindExternalFunction("_Effect", (string effect) => EmitSignal(SignalName.ink_function_effect, effect));
	}
	
	public void ContinueStory()
	{
		if(story.CanContinue) // extra validation
		{
			SaveState(); // save state before continuing
			//GD.Print("continueing story");
			string content = story.Continue();
			//GD.Print(content);
			content = content.Replace('<', '[').Replace('>', ']');

			EmitSignal(SignalName.continued_story, content);
			if(story.CanContinue)
			{
				GD.Print("Still continuing, no choices.");
				EmitSignal(SignalName.encountered_no_choices); // does nothing as of now?
			}
			else
			{
				GD.Print("Checking choices: " + story.CurrentChoices.Count);
				//convert ireadonlylist to array
				InkChoice[] seeSharpArray = new InkChoice[story.CurrentChoices.Count];
				for(int i = 0; i < story.CurrentChoices.Count; i++)
				{
					seeSharpArray[i] = story.CurrentChoices[i];
				}
				GD.Print("Choices: " + seeSharpArray.Length);
				// convert c# array to godot array
				Array variantArray = new Array(seeSharpArray);
				// test array conversion
				foreach(InkChoice choice in seeSharpArray)
				{
					GD.Print(choice.Text);
				}
				GD.Print(variantArray);
				
				choices_array = variantArray;
				EmitSignal(SignalName.fetch_my_choices_plx);
			}
		}
		else
		{
			GD.PushWarning("Illegal Continue attempt.");
		}
	}
	
	public void FeedChoice(int index)
	{
		GD.Print("story_getter received choice, feeding now");
		story.ChooseChoiceIndex(index);
	}

	public string SaveState()
	{
		// get story state
		latest_state = story.SaveState();
		GD.Print("getting story state: " + latest_state);
		
		return latest_state;
		// TODO also save log
	}

	public void LoadState(string saveState)
	{
		// load story state
		GD.Print("loading story state: " + saveState);
		//TODO Also set other objects
		story.LoadState(saveState);
		// if(story.CanContinue)
		// {
		// 	ContinueStory();
		// }
		// else
		// {

		// }
		EmitSignal(SignalName.loaded_state);

		// TODO also load log
	}
}
