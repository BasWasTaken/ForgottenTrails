using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ForgottenTrails.InkFacilitation;
using Ink.Runtime;
using MyGUI;
using DataService;

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

        public GUISlideIn book;

        

        Dictionary<string, ItemRepresentation> itemsInInventory = new();

        private void Awake()
        {
        }


        public void FetchItems(object items)
        {
            var inkInventory = items as Ink.Runtime.InkList;
            //Debug.Log("Populating inventory");
            foreach (ItemRepresentation item in itemsInInventory.Values)
            {
                bool contains = false;
                foreach (InkListItem item1 in inkInventory.Keys)
                {
                    if (item1.itemName == item.definition.InkListItem.itemName)
                    {
                        contains = true;
                        break;
                    }
                }
                if (!contains)
                {
                    RemoveItem(item.definition.InkListItem.itemName);
                }
            }

            foreach (InkListItem item in inkInventory.Keys)
            {
                //Debug.Log(item);
                if (!itemsInInventory.ContainsKey(item.itemName))
                {
                    AddItem(item);
                }
            }
        }
        public void AddItem(InkListItem item)
        {
            if (AssetManager.Instance.itemDictionary.TryGetValue(item.itemName, out InventoryItem inventoryItem))
            {
                if (!itemsInInventory.ContainsKey(inventoryItem.name))
                {
                    ItemRepresentation obj = Instantiate(itemContainerPrefab, transform);
                    obj.Construct(inventoryItem);
                    inventoryItem.InkListItem = item;
                    itemsInInventory.Add(inventoryItem.name, obj);
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
