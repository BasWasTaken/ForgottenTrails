using TMPro;
using UnityEngine;

public class RightPage : MonoBehaviour
{
    private TMPro.TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void FeedText(string input)
    {
        text.text = input;
    }

    public void Clear()
    {
        text.text = "";
    }
}
