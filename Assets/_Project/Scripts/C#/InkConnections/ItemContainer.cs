using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VVGames.ForgottenTrails.InkConnections.Items
{
    /// <summary>
    /// <para>Summary not provided.</para>
    /// </summary>
    public class ItemContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerMoveHandler
    {
        #region Fields

        [SerializeField]
        public InventoryItem definition;

        #endregion Fields


        public bool isHovered; // check if contained in contextmenu.hovered?


        #region Public Methods


        public void Construct(InventoryItem inventoryItem)
        {
            definition = inventoryItem;
            GetComponentInChildren<Image>().sprite = definition.image;
            GetComponentInChildren<TextMeshProUGUI>().text = definition.name;
        }

        public void UseItemInDefinition()
        {
            StoryController.Instance.InterfaceBroker.TryUseItem(definition);
        }

        #region PointerEvents

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovered = true;
            ContextMenu.Instance.ShowHoverText(eventData.position, definition.name);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovered = false;
            ContextMenu.Instance.RemoveHoverText(definition.name);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Inspect();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                ContextMenu.Instance.Clear();
                ContextMenu.Instance.AddOption("Use " + definition.name, UseItemInDefinition);
                ContextMenu.Instance.AddOption("Inspect " + definition.name, Inspect);
                ContextMenu.Instance.ShowContextMenu(eventData.position);
            }
        }

        public void Inspect()
        {
            StoryController.Instance.InterfaceBroker.book.RightPage.FeedText(definition.description);
        }


        #endregion PointerEvents


        #endregion Public Methods

        public void OnPointerMove(PointerEventData eventData)
        {
            //hoverText.transform.position = Input.mousePosition;
        }
    }
}