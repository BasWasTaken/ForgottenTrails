using Godot;
using GodotInk;

public partial class story_getter : Node
{	
	[Export]
	private InkStory story; 

	public string latest_state;
		
	[Signal]
	public delegate void continued_storyEventHandler(string content);
	
	[Signal]
	public delegate void encountered_choicesEventHandler(InkChoice[] choice);
	
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

	public override void _Ready()
	{
		story.BindExternalFunction("print", (string text) => EmitSignal(SignalName.ink_function_print, text, false));
		story.BindExternalFunction("print_warning", (string text) => EmitSignal(SignalName.ink_function_print, text, true));
		story.BindExternalFunction("_spd", (float speed) => EmitSignal(SignalName.ink_function_spd,speed));
		story.BindExternalFunction("_clear", (float speed) => EmitSignal(SignalName.ink_function_spd,speed));
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
				EmitSignal(SignalName.encountered_choices, story.CurrentChoices); //TODO: somwhere in the code later (another script) take out the hidden choices
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
