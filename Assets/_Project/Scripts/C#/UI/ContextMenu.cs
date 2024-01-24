using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VVGames.Common;
using Button = UnityEngine.UI.Button;

namespace VVGames.ForgottenTrails
{
    public class ContextMenu : MonoSingleton<ContextMenu>
    {
        [SerializeField] private TextMeshProUGUI hoverText;
        [SerializeField] private GameObject ContextMenuObject;
        [SerializeField] private Button buttonPrefab;

        protected override void Awake()
        {
            base.Awake();
            hoverText = GetComponent<TextMeshProUGUI>();
        }
        private void Start()
        {
            // Disable the context menu initially
            if (ContextMenuObject != null)
            {
                ContextMenuObject.SetActive(false);
            }
        }

        public void AddOption(string text, UnityAction onClick)
        {
            var button = Instantiate(buttonPrefab, ContextMenuObject.transform);
            button.onClick.AddListener(delegate
            {
                onClick();
                Clear();
            });
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
        }


        public void Clear()
        {
            foreach (Transform child in ContextMenuObject.transform)
            {
                Destroy(child.gameObject);
            }

            ContextMenuObject.SetActive(false);
        }
        // Update is called once per frame
        private void Update()
        {
            // Close context menu on right-click outside the UI element
            if (ContextMenuObject != null && Input.GetMouseButtonDown(1) && !IsMouseOverUIElement(ContextMenuObject.GetComponent<RectTransform>()))
            {
                ContextMenuObject.SetActive(false);
            }
        }
        bool IsMouseOverUIElement(RectTransform rectTransform)
        {
            // Get the position of the mouse in screen space
            Vector2 mousePosition = Input.mousePosition;

            // Check if the mouse position is within the RectTransform of the object
            return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition);
        }

        public void ShowHoverText(Vector2 position, string text)
        {
            if (hoverText == null) return;
            hoverText.gameObject.SetActive(true);
            hoverText.transform.position = position;
            hoverText.text = text;
        }

        public void ShowContextMenu(Vector2 position)
        {
            ContextMenuObject.SetActive(true);
            ContextMenuObject.transform.position = position;

        }
        public void RemoveHoverText(string text)
        {
            if (hoverText == null) return;
            if (hoverText.text == text)
            {
                hoverText.text = "";
                hoverText.gameObject.SetActive(false);
            }
        }
    }
}