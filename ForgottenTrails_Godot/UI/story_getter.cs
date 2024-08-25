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
		//TODO: enable starting automatically (need to figure out timing)
	}
	
	public void RequestContinue()
	{
		GD.Print("sotry_getter received request to continue, evaluating...");
		if(story.CanContinue)
		{
			GD.Print("validated. Continueing Story.");
			ContinueStory();
		}
		else
		{
			GD.Print("Cannot Continue");
		}
	}
	
	private void ContinueStory()
	{
		if(story.CanContinue) // extra validation
		{
			//GD.Print("continueing story")
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
			GD.PushWarning("Illegal Continue attempt.");
		}
	}
	
	public void FeedChoice(int index)
	{
		GD.Print("story_getter received choice, feeding now");
		story.ChooseChoiceIndex(index);
		ContinueStory();
	}
}

