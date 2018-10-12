using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using MHW_Save_Editor.Data;

namespace MHW_Save_Editor.InventoryEditing
{
    public class InventoryArea : NotifyUIBase
    {
        public ListCollectionView SlotList { get; set; }
        private string _areaname;
        public override string ToString()
        {
            return _areaname;
        }
        public InventoryArea(byte[] inventoryslice, int type, string areaname)
        {
            _areaname = areaname;
            int size = inventoryslice.Length / 8;
            uint itemcode;
            int amount;
            ObservableCollection<InventorySlot> _SlotList = new ObservableCollection<InventorySlot>();
            for (int i = 0; i < size; i++)
            {
                itemcode = BitConverter.ToUInt32(inventoryslice, i * 8);
                amount = BitConverter.ToInt32(inventoryslice, i*8+4);
                _SlotList.Add(new InventorySlot(type, amount, itemcode));
            }
            SlotList = (ListCollectionView) new CollectionViewSource {Source = _SlotList}.View;
        }

        public List<byte> Serialize()
        {
            List<byte> result = new List<byte>();
            foreach (InventorySlot inv in SlotList)
            {
                result.AddRange(inv.Serialize());
            }
            return result;
        }
        
        //Type: 0=Item, 1=Material, 2=Account Item, 3=Ammo/Coating, 4=Decoration
        public static (int localoffset, int count, int type, string area)[] AreaSet = new[]
        {
            (0xa2c79, 24, 0, "Item Pouch"),
            (0xa2d39, 16, 3, "Ammo Pouch"),
            (0xa2ed9, 200, 0, "Item Box"),
            (0xa3519, 200, 3, "Ammo Box"),
            (0xa3b59, 800, 1, "Material Box"),
            (0xa5459, 200, 4, "Deco Box")
        };

    }
    
}