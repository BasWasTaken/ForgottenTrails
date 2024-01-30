using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VVGames.Common;
using Button = UnityEngine.UI.Button;

namespace VVGames.ForgottenTrails
{
    ///
    public class ContextMenu : MonoSingleton<ContextMenu>
    {
        #region Fields

        [SerializeField] private GameObject hoverText;
        [SerializeField] private GameObject ContextMenuObject;
        [SerializeField] private Button buttonPrefab;

        private float timeLeft = 0;

        #endregion Fields

        #region Public Methods

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

        public void ShowHoverText(Vector2 position, string text)
        {
            if (hoverText == null) return;
            if (ContextMenuObject.activeSelf)
            {
                return; /// stop if thecontext menu is already active.
            }
            hoverText.SetActive(true);
            hoverText.transform.position = position;
            hoverText.GetComponentInChildren<TextMeshProUGUI>().text = text;
            timeLeft = .5f;
        }

        public void ShowContextMenu(Vector2 position)
        {
            if (hoverText.gameObject.activeSelf)
            {
                return; // stop if the hover tool is already active.
            }
            ContextMenuObject.SetActive(true);
            ContextMenuObject.transform.position = position;
        }

        public void RemoveHoverText(string text)
        {
            if (hoverText == null) return;
            if (hoverText.GetComponentInChildren<TextMeshProUGUI>().text == text)
            {
                hoverText.GetComponentInChildren<TextMeshProUGUI>().text = "";
                hoverText.gameObject.SetActive(false);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void Start()
        {
            ContextMenuObject.SetActive(false);

            hoverText.SetActive(false);
        }

        // Update is called once per frame
        private void Update()
        {
            if (ContextMenuObject != null)
            {
                // Close context menu on right-click outside the UI element
                if (Input.GetMouseButtonDown(1) && !IsMouseOverUIElement(ContextMenuObject.GetComponent<RectTransform>()))
                {
                    ContextMenuObject.SetActive(false);
                }
            }
        }

        private bool IsMouseOverUIElement(RectTransform rectTransform)
        {
            // Get the position of the mouse in screen space
            Vector2 mousePosition = Input.mousePosition;

            // Check if the mouse position is within the RectTransform of the object
            return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition);
        }

        #endregion Private Methods
    }
}