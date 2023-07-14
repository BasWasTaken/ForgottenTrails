using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookMarkClicked : MonoBehaviour
{
    private Rect startingValues;
    private void Awake()
    {
        startingValues = transform.GetChild(0).GetComponent<RectTransform>().rect;
    }
    public void OnClick(Button button)
    {
        foreach(RectTransform rectTransform in transform.GetComponentInChildren<RectTransform>())
        {
            if (rectTransform.gameObject == button.gameObject)
            {
                rectTransform.sizeDelta = new(startingValues.size.x, startingValues.size.y/2);
            }
            else
            {
                rectTransform.sizeDelta = new(startingValues.size.x, startingValues.size.y);
            }
        }
    }
}
