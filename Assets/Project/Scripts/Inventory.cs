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
                itemDictionary.Add(item.name, item);
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
                    item.inkEquevalent = inkListItem;
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
                    if (!items.all.ContainsItemNamed(unityItem.inkEquevalent.itemName))
                    {
                        error += string.Format("\nItem \"{0}\" not present in ink list!", unityItem.inkEquevalent.itemName);
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
                //Debug.Log(item);
                if (!itemsInInventory.ContainsKey(item.itemName))
                {
                    AddItem(item);
                }
            }
        }
        public void AddItem(InkListItem itemName)
        {
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
