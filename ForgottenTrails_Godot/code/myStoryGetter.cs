using Godot;
using GodotInk;

public partial class myStoryGetter : VBoxContainer
{	
	[Export]
	private InkStory story; 
	
	[Export]
	private Button myContinueButton;

	public override void _Ready()
	{
		myContinueButton.Connect("continueSignal", Callable.From(ContinueStory));
		
		story.BindExternalFunction("Print", (string text) => PresentConsoleMessage(text, false));
		story.BindExternalFunction("PrintWarning", (string text) => PresentConsoleMessage(text, true));
		ContinueStory();
	}
	
	private void ContinueStory()
	{
		if(!story.CanContinue)
		{
			GD.Print("Cannot Continue");
			return;
		}
		
		PresentStory(story.Continue());

		if(story.CanContinue)
		{
			PresentPrompt();
		}
		else
		{
			foreach (InkChoice choice in story.CurrentChoices)
			{
				PresentChoice(choice); // wordt later moeilijker vanwege hidden keuzes
			}
		}
	}
	
	#region Presenter // this just feels like it should be another sript file...
	// well maybe i can help that by later writing the code so that from here it goes to something that displays it and it does not need to fire back so much.
	// or! split it, and create events for the continue button.
	private void PresentStory(string text)
	{
		foreach (Node child in GetChildren())
		{
			child.QueueFree();
		}
		Label content = new() { Text =  text}; // later just use existing label instead of making new?
		AddChild(content);
	}
	
	private void PresentPrompt()
	{
		Button button = new() {Text = ">"};
		button.Pressed += delegate
		{
			ContinueStory();
		};
		AddChild(button);		
	}
	
	private void PresentChoice(InkChoice choice)
	{
		Button button = new() { Text = choice.Text };
		button.Pressed += delegate
		{
			story.ChooseChoiceIndex(choice.Index);
			ContinueStory();
		};
		AddChild(button);
	}
	
	private void PresentConsoleMessage(string text, bool warning = false)
	{
		if (warning)
		{
				GD.PushWarning("Warning from INK Script: " + text);
		}
		else
		{
			GD.Print("Message from INK Script: " + text);
		}
	}
	#endregion Presenter
}




