using Bas.ForgottenTrails.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Bas.ForgottenTrails.InkConnections.Items
{

    /// <summary>
    /// <para>Summary not provided.</para>
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class ItemContainer : MonoBehaviour, IMouseOverOption
    {
        public bool IsMouseOver { get; set; }
        private TMPro.TextMeshProUGUI prompt;
        [SerializeField]
        public InventoryItem definition;

        public void Construct(InventoryItem inventoryItem)
        {
            definition = inventoryItem;
            GetComponent<Image>().sprite = definition.image;
            prompt = GetComponentInChildren<TMPro.TextMeshProUGUI>();
            prompt.text = definition.description;
        }

        // Default implementation for the interface methods
        private void OnMouseEnter()
        {
            IsMouseOver = true;
            prompt.gameObject.SetActive(true);
            prompt.text = "";
        }

        private void OnMouseExit()
        {
            IsMouseOver = false;
            prompt.gameObject.SetActive(false);
        }
        private void OnMouseOver()
        {
            UpdateWhenMouseOver();
        }
        public void ActivateFromButton()
        {
            StoryController.Instance.InterfaceBroker.TryUseItem(definition);
        }
        private void UpdateWhenMouseOver()
        {
            prompt.transform.position = Input.mousePosition;
        }

        // Explicitly implementing the interface methods
        void IMouseOverOption.OnMouseEnter()
        {
            // You can call the default implementation if needed
            OnMouseEnter();
        }

        void IMouseOverOption.OnMouseExit()
        {
            // You can call the default implementation if needed
            OnMouseExit();
        }

        void IMouseOverOption.UpdateWhenMouseOver()
        {
            // You can call the default implementation if needed
            UpdateWhenMouseOver();
        }
    }
}