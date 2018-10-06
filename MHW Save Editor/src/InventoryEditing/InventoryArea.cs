using System;
using System.Collections.ObjectModel;

namespace MHW_Save_Editor.InventoryEditing
{
    public class InventoryArea
    {
        public ObservableCollection<InventorySlot> SlotList;
        private int _type;
        private string _areaname;
        public override string ToString()
        {
            return _areaname;
        }
        public InventoryArea(byte[] inventoryslice, int type, string areaname)
        {
            _type = 0;
            _areaname = areaname;
            int size = inventoryslice.Length / 8;
            uint itemcode;
            int amount;
            SlotList = new ObservableCollection<InventorySlot>();
            for (int i = 0; i < size; i++)
            {
                itemcode = BitConverter.ToUInt32(inventoryslice, i * 8);
                amount = BitConverter.ToInt32(inventoryslice, i*8+4);
                SlotList.Add(new InventorySlot(type, amount, itemcode));
            }
        }
        
        //Type: 0=Item, 1=Material, 2=Account Item, 3=Ammo/Coating, 4=Decoration
        public static (int localoffset, int count, int type, string area)[] AreaSet = new[]
        {
            (0xa2c79, 16, 0, "Item Pouch"),
            (0xa2d39, 24, 3, "Ammo Pouch"),
            (0xa2ed9, 200, 0, "Item Box"),
            (0xa3519, 200, 3, "Ammo Box"),
            (0xa3b59, 200, 1, "Material Box"),
            (0xa5459, 200, 4, "Deco Box")
        };

    }
    
}