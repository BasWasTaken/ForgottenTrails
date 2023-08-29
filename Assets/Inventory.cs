using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ForgottenTrails.InkFacilitation;
using Ink.Runtime;

namespace items
{

    /// <summary>
    /// <para>Summary not provided.</para>
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        [Header("Prefab")]
        [SerializeField]
        ItemRepresentation itemContainerPrefab;

        [SerializeField]
        List<InventoryItem> possibleItems = new();
        readonly Dictionary<string, InventoryItem> itemDictionary = new();

        Dictionary<string, ItemRepresentation> itemsInInventory = new();

        private void Awake()
        {
            itemDictionary.Clear();
            foreach (InventoryItem item in possibleItems)
            {
                itemDictionary.Add(item.name, item);
            }
        }
        public void FetchItems(object items)
        {
            var inkInventory = items as Ink.Runtime.InkList;
            Debug.Log("Populating inventory");
            foreach (ItemRepresentation item in itemsInInventory.Values)
            {
                bool contains = false;
                foreach (InkListItem item1 in inkInventory.Keys)
                {
                    if (item1.itemName == item.definition.inkEquevalent.itemName)
                    {
                        contains = true;
                        break;
                    }
                }
                if (!contains)
                {
                    RemoveItem(item.name);
                }
            }

            foreach (InkListItem item in inkInventory.Keys)
            {
                Debug.Log(item);
                if (!itemsInInventory.ContainsKey(item.itemName))
                {
                    AddItem(item);
                }
            }
        }
        public void AddItem(InkListItem itemName)
        {
            // check if known
            if (itemDictionary.TryGetValue(itemName.itemName, out InventoryItem inventoryItem))
            {
                if (!itemsInInventory.ContainsKey(inventoryItem.name))
                {
                    ItemRepresentation obj = Instantiate(itemContainerPrefab, transform);
                    obj.Construct(inventoryItem);
                    inventoryItem.inkEquevalent = itemName;
                    itemsInInventory.Add(inventoryItem.name, obj);
                }
                else
                {
                    Debug.LogError("item already in inventory");
                }
            }
            else
            {
                Debug.LogError(string.Format("Item \"{0}\" not recognised!", itemName));
            }
        }
        public void RemoveItem(string itemName)
        {
            if (itemsInInventory.ContainsKey(itemName))
            {
                itemsInInventory.Remove(itemName);
            }
            else
            {
                Debug.LogError(string.Format("Item \"{0}\" not found in inventory!", itemName));
            }

        }
    }
}
