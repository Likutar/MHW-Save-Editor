using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace MHW_Save_Editor.InventoryEditing
{
    public sealed class SingletonMasterItemList
    {
        public readonly Dictionary<UInt32, Item> ItemList;
        private SingletonMasterItemList()
        {
            List<Item> Listing = JsonConvert.DeserializeObject<List<Item>>(Properties.Resources.MasterItemList);
            ItemList = new Dictionary<UInt32, Item>();
            foreach (Item _item in Listing) ItemList.Add(_item.id,_item);
        }
        
        public static SingletonMasterItemList Instance { get { return Nested.instance; } }
        
        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly SingletonMasterItemList instance = new SingletonMasterItemList();
        }

        public Item EmptyItem()
        {
            return ItemList[0];
        }
    }
}