using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public object PopulateInventory(byte[] savefile)
        {
            List<InventoryArea> inv = GetInventoryAreas(
                savefile.Slice(PlayerOffset, PlayerOffsetEnd));
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

        public void CommitInventory()
        {
            IList<InventoryArea> source = (IList<InventoryArea>) ((ListCollectionView) Application.Current.Resources["InventoryAreaCollectionView"])
                .SourceCollection;
            foreach ((var Area, var Properties) in source.Zip(InventoryArea.AreaSet, (i1, i2) => (i1, i2)))
            {
                Area.Serialize().ToArray().CopyTo(saveFile.data, PlayerOffset+Properties.localoffset);
            }
        }
        
    }
}