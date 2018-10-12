using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using MHW_Save_Editor.Data;

namespace MHW_Save_Editor.InventoryEditing
{
    public class InventoryAreasViewModel : NotifyUIBase
    {
        public List<InventoryContentsView> InventoryAreaUIView {get; set;} 
        public ListCollectionView InventoryAreaCollectionView {get; set;}

        public InventoryAreasViewModel()
        {
            InventoryAreaCollectionView = Application.Current.Resources["InventoryAreaCollectionView"] as ListCollectionView;
            InventoryAreaCollectionView.MoveCurrentToPosition(0);
            //InventoryAreaUIView = new List<InventoryContentsView>(); 
            //foreach (InventoryArea box in InventoryAreaCollectionView.SourceCollection as IList<InventoryArea>)
            //{
            //    InventoryContentsView boxview = new InventoryContentsView();
            //    boxview.DataContext = box;
            //    InventoryAreaUIView.Add(boxview);
            //}
        }
    }
}