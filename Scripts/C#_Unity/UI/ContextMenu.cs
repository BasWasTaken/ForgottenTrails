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

        #endregion Fields

        private float timeLeft = 0f;

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
        }

        public void ShowContextMenu(Vector2 position)
        {
            ContextMenuObject.SetActive(true);
            ContextMenuObject.transform.position = hoverText.gameObject.activeSelf ? hoverText.transform.position : position;
            RemoveHoverText(hoverText.GetComponentInChildren<TextMeshProUGUI>().text);
            timeLeft = 1f;
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

        private void RemoveContextMenu()
        {
            ContextMenuObject.SetActive(false);
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
                if (timeLeft > 0)
                {
                    timeLeft -= Time.deltaTime; //tick
                }

                // if outside the UI element...
                if (!IsMouseOverUIElement(ContextMenuObject.GetComponent<RectTransform>()))
                {
                    // if a second has passed or on right click, close the context menu
                    if (timeLeft < 0 | Input.GetMouseButtonDown(1))
                    {
                        RemoveContextMenu();
                    }
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