using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ForgottenTrails.InkFacilitation;
using Ink.Runtime;
using MyGUI;
using DataService;

namespace Items
{

    /// <summary>
    /// <para>Summary not provided.</para>
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        [Header("Prefab")]
        [SerializeField]
        ItemContainer itemContainerPrefab;

        public GUISlideIn book;

        

        Dictionary<InkListItem, ItemContainer> UnityInventory = new();


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
                    UnityInventory.Add(inventoryItem.InkListItem, obj);
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
    }
}
