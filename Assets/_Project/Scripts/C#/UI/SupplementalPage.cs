using TMPro;
using UnityEngine;
[RequireComponent(typeof(TMPro.TextMeshProUGUI))]

public class SupplementalPage : MonoBehaviour
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
