using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VVGames.ForgottenTrails.InkConnections.Items
{
    /// <summary>
    /// <para>Summary not provided.</para>
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class ItemContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerMoveHandler
    {
        #region Fields

        [SerializeField]
        public InventoryItem definition;

        private TMPro.TextMeshProUGUI hoverText;
        public GameObject ContextMenu;

        #endregion Fields


        public bool isHovered = false;


        #region Public Methods


        public void Construct(InventoryItem inventoryItem)
        {
            definition = inventoryItem;
            GetComponent<Image>().sprite = definition.image;
            hoverText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        }

        public void ActivateFromButton()
        {
            StoryController.Instance.InterfaceBroker.TryUseItem(definition);
        }

        #region PointerEvents

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovered = true;
            ShowHoverText(definition.name);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovered = false;
            hoverText.gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                ShowContextMenu();
            }
        }


        #endregion PointerEvents


        #endregion Public Methods

        public void OnPointerMove(PointerEventData eventData)
        {
            hoverText.transform.position = Input.mousePosition;
        }
        private void Start()
        {
            // Disable the context menu initially
            if (ContextMenu != null)
            {
                ContextMenu.SetActive(false);
            }
        }
        // Update is called once per frame
        private void Update()
        {
            // Close context menu on right-click outside the UI element
            if (ContextMenu != null && Input.GetMouseButtonDown(1) && !isHovered)
            {
                ContextMenu.SetActive(false);
            }
        }
        private void ShowHoverText(string text)
        {
            if (hoverText != null)
            {
                hoverText.text = text;
            }
        }
        private void ShowContextMenu()
        {
            // Check if the context menu exists
            if (ContextMenu == null) return;
            // Set the context menu to active
            ContextMenu.SetActive(true);

            // Add options to the context menu
            //ContextMenu.AddOption("Use Item", definition);
            //ContextMenu.AddOption("Inspect Item", definition);
        }
    }
}