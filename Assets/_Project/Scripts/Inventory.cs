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
        readonly public Dictionary<string, InventoryItem> itemDictionary = new();

        Dictionary<string, ItemRepresentation> itemsInInventory = new();

        private void Awake()
        {
            itemDictionary.Clear();
            foreach (InventoryItem item in possibleItems)
            {
                itemDictionary.Add(item.CanonicalName, item);
            }
        }

        public bool AssertSymmetry(InkList items, InkList affordances)
        {
            string error = "";

            // assert all items exist symmetrically
            // assert all items from ink exist in unity
            foreach (InkListItem inkListItem in items.all.Keys)
            {
                if (itemDictionary.TryGetValue(inkListItem.itemName, out InventoryItem item))
                {
                    item.InkListItem = inkListItem;
                }
                else
                {
                    error += string.Format("\nItem \"{0}\" not present in unity dictionary!", inkListItem.itemName);
                }
            }

            // assert all items from unity exist in ink
            //var newList = new Ink.Runtime.InkList("Items", StoryController.Instance.Story);
            foreach (InventoryItem unityItem in itemDictionary.Values)
            {
                try
                {
                    if (!items.all.ContainsItemNamed(unityItem.InkListItem.itemName))
                    {
                        error += string.Format("\nItem \"{0}\" not present in ink list!", unityItem.InkListItem.itemName);
                    }
                }
                catch (NullReferenceException e)
                {
                    error += "\n"+e;
                }
                //newList.AddItem(unityItem.inkEquevalent.itemName);
            }
            // items.Contains(newList)?debug;

            // assert all afforances are same

            // assert all affordances from ink exist in unity
            foreach (InkListItem affordance in affordances.all.Keys)
            {
                if (!Enum.IsDefined(typeof(Affordance), affordance.itemName))
                {
                    error += string.Format("\nAffordance \"{0}\" not present in unity enum definition!", affordance);
                }
            }

            // assert all affordances from unity exist in ink
            foreach (string affordance in Enum.GetNames(typeof(Affordance)))
            {
                if (!affordances.all.ContainsItemNamed(affordance))
                {
                    error += string.Format("\nAffordance \"{0}\" not present in ink list!", affordance);
                }
            }

            if (error == "")
            {
                return true;
            }
            else
            {
                Debug.LogAssertion("ITEMS OR AFFORDANCES NOT MATCHED UP"+error);    
                return false;
            }
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
            if (itemDictionary.TryGetValue(item.itemName, out InventoryItem inventoryItem))
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
                    Debug.LogErrorFormat("item {0} already in inventory", item.itemName);
                }
            }
            else
            {
                Debug.LogError(string.Format("Item \"{0}\" not recognised!", item));
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
