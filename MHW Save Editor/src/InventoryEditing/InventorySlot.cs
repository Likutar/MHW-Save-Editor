using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
                if (value.Default)//Should be any special item and the item should be checking it's own minima and maxima
                {
                    _amount = 0;
                    RaisePropertyChanged("Amount");
                }
                else if (_amount == 0)
                {
                    _amount = 1;
                    RaisePropertyChanged("Amount");
                }
                _item = value;
                RaisePropertyChanged();
            } 
        }

        private int _type;
        private ItemList _slotchoices;
        public CollectionView SlotChoicesDropDown { get => _slotchoices.GetItemsView();}

        public InventorySlot(int type, int amount = 0, uint item = 0)
        {
            _type = type;
            _slotchoices = ItemList.TypeToListing(type);
            
            if (amount == 0 || item == 0)
            {
                this._item = SingletonMasterItemList.Instance.EmptyItem();
                this._amount = 0;
            }
            else
            {
                this._item = _slotchoices.FromCode(item);
                this._amount = amount;
            }
        }
        
        public byte[] Serialize()
        {
            return BitConverter.GetBytes(_item.id).Concat(BitConverter.GetBytes(_amount)).ToArray();
        }
    }
    
}