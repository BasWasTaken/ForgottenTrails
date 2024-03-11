using Ink.Runtime;
using System.Collections.Generic;
using UnityEngine;
using VVGames.ForgottenTrails.InkConnections;
using VVGames.ForgottenTrails.InkConnections.Items;

namespace VVGames.ForgottenTrails.UI
{
    /// <summary>
    /// <para>Summary not provided.</para>
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        #region Fields

        public InGameMenuSwapper book;

        [Header("Prefab")]
        [SerializeField]
        private ItemContainer itemContainerPrefab;

        private Dictionary<InkListItem, ItemContainer> UnityInventory = new();

        #endregion Fields

        #region Public Methods

        // note: zou dit problemen gevven met dat items in unity verwijderd  worden en de inventroy vervvolgens weer rechtgetrokken wordt naar hoe ie in ink is?
        // eigenlijk zou deze functionaliteit in ink moeten zitten, niet?
        // oh wacht dat is het ook, dit adden en removen gaat vgm puur om de knopjes.
        public void FetchItems(InkList inkInventory)
        {
            List<InkListItem> dummy = new();
            dummy.AddRange(UnityInventory.Keys);
            //Debug.Log("Populating inventory");
            foreach (InkListItem item in dummy)
            {
                if (!inkInventory.ContainsKey(item))
                {
                    RemoveItem(item);
                }
            }

            foreach (InkListItem item in inkInventory.Keys)
            {
                //Debug.Log(item);
                if (!UnityInventory.ContainsKey(item))
                {
                    AddItem(item);
                }
            }
        }

        public void AddItem(InkListItem item)
        {
            if (AssetManager.Instance.ItemDictionary.TryGetValue(item, out InventoryItem inventoryItem))
            {
                if (!UnityInventory.ContainsKey(item))
                {
                    ItemContainer obj = Instantiate(itemContainerPrefab, transform);
                    obj.Construct(inventoryItem);
                    inventoryItem.InkListItem = item;
                    UnityInventory.Add(item, obj);
                }
                else
                {
                    //Debug.LogErrorFormat("item {0} already in inventory", item.itemName);
                }
            }
            else
            {
                Debug.LogError(string.Format("Item \"{0}\" not recognised!", item.itemName));
            }
        }

        public void RemoveItem(InkListItem item)
        {
            if (UnityInventory.TryGetValue(item, out ItemContainer value))
            {
                UnityInventory.Remove(item);
                Destroy(value.gameObject);
            }
            else
            {
                Debug.LogError(string.Format("Item \"{0}\" not found in inventory!", item.itemName));
            }
        }

        #endregion Public Methods
    }
}