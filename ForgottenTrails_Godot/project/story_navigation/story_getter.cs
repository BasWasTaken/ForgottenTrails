using Godot;
using GodotInk;

public partial class story_getter : Node
{	
	[Export]
	private InkStory story; 
		
	[Signal]
	public delegate void continued_storyEventHandler(string content);
	
	[Signal]
	public delegate void encountered_no_choicesEventHandler();
	
	[Signal]
	public delegate void encountered_choiceEventHandler(InkChoice choice);
	
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
				foreach (InkChoice choice in story.CurrentChoices)
				{
					GD.Print("Choice: " + choice.Text);
					EmitSignal(SignalName.encountered_choice, choice);
				}
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
		//TODO: Call autosave via signal 
		GetSaveState();
		ContinueStory();
	}

	public string GetSaveState()
	{
		// get story state
		var state = story.SaveState();
		GD.Print("saving story state: " + state);
		return state;
		// TODO also save log
	}

	public void LoadSaveState(string saveState)
	{
		// load story state
		GD.Print("saving story state: " + saveState);
		story.LoadState(saveState);
		// TODO also load log
	}
}
