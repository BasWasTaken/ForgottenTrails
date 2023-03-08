#region namespaces
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;
using System.Collections;
using Ink;
using NaughtyAttributes;
#endregion

/// <summary>
/// Behaviour for object that can receive text (predominantly INK/json objects) to display on screen, along with dialogue options.
/// </summary>
public class DialogueWindow : MonoBehaviour 
{
	#region variables
	[BoxGroup("Child Objects")]
	public TextMeshProUGUI textPanel;
	[BoxGroup("Child Objects")]
	public Image nameTag;
	[BoxGroup("Child Objects")]
	public HorizontalLayoutGroup portraitPanel;
	[BoxGroup("Child Objects")]
	public Image[] portraits;
	[BoxGroup("Child Objects")]
	public Image bgImage;
	[BoxGroup("Child Objects")]
	public Transform buttonAnchor;
	[BoxGroup("Child Objects")]
	public Vector2 buttonOffset = new (0, 1);
	[BoxGroup("Child Objects")]
	public TextMeshProUGUI textPanelAlt;
	[BoxGroup("Child Objects")]
	public Image nameTagAlt;
	[BoxGroup("Child Objects")]
	public Image bgImageAlt;
	[BoxGroup("Child Objects")]
	public Animator triangle;


	[BoxGroup("Prefabs")]
	public Button buttonPrefab;
	[BoxGroup("Scene References")]
	[SerializeField] protected Canvas canvas;
	[BoxGroup("Tweaks")]
	[Tooltip("Rate of typewriter effect in nr of characters per second.")] 
	public float textSpeed = 50;
	public TextMeshProUGUI ActiveTextPanel 
	{ 
		get 
		{
			if (NovelManager.Instance.State.displayState==NovelManager.NovelState.DisplayState.Dialogue)
			{
				return textPanel;
			}
			else if (NovelManager.Instance.State.displayState == NovelManager.NovelState.DisplayState.Narration)
			{
				return textPanelAlt;
            }
            else
            {
				throw new System.Exception("Not in any of the two states I know.");
            }
		} 
	}
	public bool CompletedText => ActiveTextPanel.maxVisibleCharacters == ActiveTextPanel.text.Length;
	private bool completeText = false;

	private float timeSinceAdvance = 0;
	private float advanceDialogueDelay;

	#endregion
	#region loop
	protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CompletedText == false & timeSinceAdvance>advanceDialogueDelay)
            {
				completeText = true;
			}
        }
		timeSinceAdvance += Time.unscaledDeltaTime;
	}
	#endregion
	#region init
	public void Init()
    {
		ClearUI();
		canvas = transform.parent.GetComponent<Canvas>();
		
		transform.localPosition = new Vector2(Camera.main.transform.position.x,Camera.main.transform.position.y);
		advanceDialogueDelay = FindObjectOfType<NovelManager>().AdvanceDialogueDelay;
		textPanelAlt.color = Color.white;
		nameTagAlt.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
	}
    #endregion

    #region methods
	public void ClearUI(NovelManager.NovelState.DisplayState mode = NovelManager.NovelState.DisplayState.Dialogue)/// Destroys all the buttons of this gameobject (clear UI)
	{
        if (mode !=NovelManager.NovelState.DisplayState.Dialogue)
        {
			Debug.LogError("wrong mode");
			return;
        }
        else
        {
			int childCount = buttonAnchor.childCount;
			for (int i = childCount - 1; i >= 0; --i)
			{
				Transform child = buttonAnchor.GetChild(i);
				if (child.TryGetComponent(out Button button))
				{
					Destroy(button.gameObject);
				}
			}

		}
	}
	public IEnumerator DisplayContent(string text, NovelManager.NovelState.DisplayState mode) // Creates a textbox showing the the line of text
	{
		TextMeshProUGUI panel;
        if (mode == NovelManager.NovelState.DisplayState.Dialogue)
        {
			panel = textPanel;
        }
        else if(mode== NovelManager.NovelState.DisplayState.Narration)
        {
			panel = textPanelAlt;
        }
		else
		{
			Debug.LogError("Unknown mode.");
			yield break;
		}
		timeSinceAdvance = 0;
		panel.text = text;
		for (int i = 0; i < text.Length+1; i++)
		{
			panel.maxVisibleCharacters = i;
			yield return new WaitForSecondsRealtime(1 / textSpeed);
			yield return new WaitUntil(()=>isActiveAndEnabled);
			if (completeText)
            {
				panel.maxVisibleCharacters = text.Length;
				completeText = false;
				yield break;
            }
		}
	}
	public Button PresentButtons(string text, int n = 0, NovelManager.NovelState.DisplayState mode = NovelManager.NovelState.DisplayState.Dialogue)    // Creates a button showing the choice text
	{
        if (mode != NovelManager.NovelState.DisplayState.Dialogue)
		{
			Debug.LogError("wrong mode");
			return null;
        }
		else
		{
			// TODO: make fancy fadein spawning 
			Button choice = Instantiate(buttonPrefab, buttonAnchor);        // Creates the button from a prefab

			TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI>();        // Gets the text from the button prefab
			choiceText.text = text;

			// Make the button expand to fit the text
			HorizontalLayoutGroup layoutGroup = choice.GetComponent<HorizontalLayoutGroup>();
			layoutGroup.childForceExpandHeight = false;

			//choice.transform.position = buttonAnchor.position + (Vector3)buttonOffset * n;

			return choice;

		}
	}
	#endregion


}
