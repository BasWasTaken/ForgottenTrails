/*using Godot;
using GodotInk;

public partial class myStoryPresenter : VBoxContainer
{
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
}
*/




