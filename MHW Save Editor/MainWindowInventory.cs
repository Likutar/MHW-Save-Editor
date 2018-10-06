using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using MHW_Save_Editor.InventoryEditing;
using MHW_Save_Editor.InvestigationEditing;
using MHW_Save_Editor.Tabs;

namespace MHW_Save_Editor
{
    public partial class MainWindow
    {
        private int PlayerOffset = 0x3004DC;
        private int PlayerOffsetEnd = 0x3F65EB;
        public object PopulateInventory(byte[] newdata)
        {
            List<InventoryArea> inv = GetInventoryAreas(
                newdata.Slice(PlayerOffset, PlayerOffsetEnd));
            ObservableCollection<InventoryArea> inventory = new ObservableCollection<InventoryArea>(inv);
            ListCollectionView inventoryAreaCollectionView = (ListCollectionView)new CollectionViewSource { Source = inventory }.View;
            Application.Current.Resources["InventoryAreaCollectionView"] = inventoryAreaCollectionView;
            return new InventoryTab();
        }
        
        public List<InventoryArea> GetInventoryAreas(byte[] newdata)
        {
            List<InventoryArea> inv = new List<InventoryArea>();
            foreach ( (int localoffset, int count, int type, string label) keyset in InventoryArea.AreaSet )
            inv.Add(new InventoryArea(newdata.Slice(keyset.localoffset, keyset.localoffset+8*keyset.count), keyset.type, keyset.label));
            
            return inv;
        }
    }
}