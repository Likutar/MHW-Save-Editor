using System;
using System.Collections.ObjectModel;

namespace MHW_Save_Editor.InventoryEditing
{
    public class InventoryArea
    {
        public ObservableCollection<InventorySlot> SlotList;

        public InventoryArea(byte[] inventoryslice, int type)
        {
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
    }
    
}