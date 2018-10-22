using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using MHW_Save_Editor.Data;

namespace MHW_Save_Editor.InventoryEditing
{
    
    //Type: 0=Item, 1=Material, 2=Account Item, 3=Ammo/Coating, 4=Decoration
    public class ItemList :  NotifyUIBase
    {
        
        private readonly Dictionary<UInt32, Item> Items;
        private readonly ObservableCollection<Item> Manifest;
        private ItemList(UInt32 Type)
        {
            Items =
                SingletonMasterItemList.Instance.ItemList.Where(s => (s.Value.storageID==Type && !s.Value.Default)|| s.Value.id == 0x0)
                    .ToDictionary(dict => dict.Key, dict => dict.Value);
            Manifest = new ObservableCollection<Item>(Items.Values);
        }
        public ListCollectionView GetItemsView() => (ListCollectionView)new CollectionViewSource { Source = Manifest }.View;

        public Item FromCode(UInt32 code){try{return Items[code];}catch{return Items[0];}}
        
        public static ItemList ItemListing = new ItemList(0);
        public static ItemList MaterialListing = new ItemList(1);
        public static ItemList AmmoListing = new ItemList(3);
        public static ItemList DecorationListing = new ItemList(4);

        public static ItemList TypeToListing(int type)
        {
            switch (type)
            {
                case 0:
                    return ItemListing;
                case 1:
                    return MaterialListing;
                case 3:
                    return AmmoListing;
                case 4:
                    return DecorationListing;
                default:
                    throw new KeyNotFoundException();
            }
        }
    }
}