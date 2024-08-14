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

	public override void _Ready()
	{
		story.BindExternalFunction("Print", (string text) => EmitSignal(SignalName.ink_function_print, text, false));
		story.BindExternalFunction("PrintWarning", (string text) => EmitSignal(SignalName.ink_function_print, text, true));
		RequestContinue();
	}
	
	public void RequestContinue()
	{
		// room for checking if the script is ready
		ContinueStory();
	}
	
	private void ContinueStory()
	{
		if(story.CanContinue)
		{
			string content = story.Continue();
			
			EmitSignal(SignalName.continued_story, content);
			
			if(story.CanContinue)
			{
				EmitSignal(SignalName.encountered_no_choices);
			}
			else
			{
				foreach (InkChoice choice in story.CurrentChoices)
				{
					EmitSignal(SignalName.encountered_choice, choice);
				}
			}
		}
		else
		{
			GD.Print("Cannot Continue");
		}
	}
	
	public void FeedChoice(int index)
	{
		story.ChooseChoiceIndex(index);
		ContinueStory();
	}
}

