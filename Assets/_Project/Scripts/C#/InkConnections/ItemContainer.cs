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

        public bool isHovered;

        // check if contained in contextmenu.hovered?

        private float timeLeft = 0f;

        #endregion Fields

        #region Public Methods

        private float activationDelay = .5f;

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

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovered = true;
            activationDelay = .5f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovered = false;
            timeLeft = .1f; // ToDO: improve with fadeout
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
            StoryController.Instance.InterfaceBroker.InGameMenu.Supplemental.FeedText(definition.description + "\n" + "Worth: " + definition.coinValue);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            //hoverText.transform.position = Input.mousePosition;
        }

        #endregion Public Methods

        #region Private Methods

        private void Update()
        {
            if (isHovered)
            {
                if (activationDelay > 0)
                {
                    activationDelay -= Time.deltaTime;
                }
                else if (activationDelay != -99)
                {
                    ContextMenu.Instance.ShowHoverText(Input.mousePosition, definition.name);
                    activationDelay = -99;
                }
            }
            else
            {
                if (timeLeft > 0)
                {
                    timeLeft -= Time.deltaTime;
                }
                else
                {
                    ContextMenu.Instance.RemoveHoverText(definition.name);
                }
            }
        }

        #endregion Private Methods
    }
}