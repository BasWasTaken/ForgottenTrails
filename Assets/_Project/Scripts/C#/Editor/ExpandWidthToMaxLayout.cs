using UnityEngine;
 
[ExecuteInEditMode]
public class ExpandWidthToMaxLayout: MonoBehaviour
{
    RectTransform rect;
    RectTransform parentRect;
    [SerializeField] float maxWidth;
 
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        parentRect = transform.parent.GetComponent<RectTransform>();
    }
 
    void Update()
    {
        rect.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            Mathf.Min(parentRect.rect.width, maxWidth)
        );
    }
}