using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using MHW_Save_Editor.Data;
using Newtonsoft.Json;

namespace MHW_Save_Editor.InventoryEditing
{
    public class InventorySlot :  NotifyUIBase
    {
        private int _amount { get; set; }

        public int Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                RaisePropertyChanged();
            }
        }
        private Item _item { get; set; }
        
        public Item Item
        {
            get => _item;
            set
            {
                if (value == SingletonMasterItemList.Instance.EmptyItem())
                {
                    _amount = 0;
                    RaisePropertyChanged("Amount");
                }
                _item = value;
                RaisePropertyChanged();
            } 
        }

        public InventorySlot(int amount = 0, Item item = null)
        {
            if (amount == 0 || item==null || item.id == 0)
            {
                this._item = SingletonMasterItemList.Instance.EmptyItem();
                this._amount = 0;
            }
            else
            {
                this._item = item;
                this._amount = amount;
            }
        }
        
        public byte[] Serialize()
        {
            return BitConverter.GetBytes(_item.id).Concat(BitConverter.GetBytes(_amount)).ToArray();
        }
    }
    
    public sealed class SingletonMasterItemList
    {
        public readonly Dictionary<UInt32, Item> ItemList;
        private SingletonMasterItemList()
        {
            List<Item> Listing = 
            JsonConvert.DeserializeObject<List<Item>>(
                File.ReadAllText("./src/InventoryEditing/MasterItemList.json"));
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
    
    public class ItemList
    {
        public readonly Dictionary<UInt32, Item> Items;
        public readonly ObservableCollection<Item> Manifest;
        public ItemList(UInt32 Type)
        {
            Items =
            SingletonMasterItemList.Instance.ItemList.Where(s => s.Value.type == Type || s.Value.id == 0x0)
                .ToDictionary(dict => dict.Key, dict => dict.Value);
            ObservableCollection<Item> Manifest = new ObservableCollection<Item>(Items.Values);
        }
        public ListCollectionView GetItemsView() => (ListCollectionView)new CollectionViewSource { Source = Manifest }.View;
        public Item FromCode(UInt32 code) => Items[code];
    }
    
    public class Item
    {
        public UInt32 id  { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Byte  subType  { get; set; }
        public UInt32 type { get; set; }
        public Byte rarity  { get; set; }
        public Byte carryLimit  { get; set; }
        public Byte  unk  { get; set; }
        public UInt16 sortOrder { get; set; }
        public UInt32 flags { get; set; }
        public UInt32 iconID { get; set; }
        public Byte  iconColor { get; set; }
        public Byte  carryItem { get; set; }
        public UInt32 sellPrice { get; set; }
        public UInt32 buyPrice    { get; set; }

        public bool Default { get => (flags&0x1)!=0x0;}
        public bool EZ { get => (flags&(0x1<<1))!=0x0;}
        //public bool Unknown { get => (flags&(0x1<<2))!=0x0;}
        public bool Usable { get => (flags&(0x1<<3))!=0x0;}
        public bool Appraisal { get => (flags&(0x1<<4))!=0x0;}
        //public bool Unknown { get => (flags&(0x1<<5))!=0x0;}
        public bool Mega { get => (flags&(0x1<<6))!=0x0;}
        public bool Level1 { get => (flags&(0x1<<7))!=0x0;}
        public bool Level2 { get => (flags&(0x1<<8))!=0x0;}
        public bool Level3 { get => (flags&(0x1<<9))!=0x0;}
        public bool Glitter { get => (flags&(0x1<<10))!=0x0;}
        public bool Deliverable { get => (flags&(0x1<<11))!=0x0;}
        public bool PouchVisible { get => (flags&(0x1<<12))!=0x0;}
        //public bool Unknown { get => (flags&(0x1<<13))!=0x0;}...
    }
}