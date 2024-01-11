using VVGames.ForgottenTrails.UI;
using UnityEngine;
using UnityEngine.UI;

namespace VVGames.ForgottenTrails.InkConnections.Items
{
    /// <summary>
    /// <para>Summary not provided.</para>
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class ItemContainer : MonoBehaviour, IMouseOverOption
    {
        #region Fields

        [SerializeField]
        public InventoryItem definition;

        private TMPro.TextMeshProUGUI prompt;

        #endregion Fields

        #region Properties

        public bool IsMouseOver { get; set; }

        #endregion Properties

        #region Public Methods

        public void Construct(InventoryItem inventoryItem)
        {
            definition = inventoryItem;
            GetComponent<Image>().sprite = definition.image;
            prompt = GetComponentInChildren<TMPro.TextMeshProUGUI>();
            prompt.text = definition.description;
        }

        public void ActivateFromButton()
        {
            StoryController.Instance.InterfaceBroker.TryUseItem(definition);
        }

        #endregion Public Methods

        #region Private Methods

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

        // Explicitly implementing the interface methods
        void IMouseOverOption.OnMouseEnter()
        {
            // You can call the default implementation if needed
            OnMouseEnter();
        }

        #endregion Private Methods

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

        private void UpdateWhenMouseOver()
        {
            prompt.transform.position = Input.mousePosition;
        }
    }
}