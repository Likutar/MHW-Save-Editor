using System.Windows;
using System.Windows.Data;
using MHW_Save_Editor.Data;

namespace MHW_Save_Editor.InventoryEditing
{
    public class InventoryContentsViewModel : NotifyUIBase
    {
        public ListCollectionView InventoryAreaCollectionView { get; set; }
        
        private InventoryArea CurrentArea
        {
            get { return InventoryAreaCollectionView.CurrentItem as InventoryArea; }
            set
            {
                InventoryAreaCollectionView.MoveCurrentTo(value);
                RaisePropertyChanged();
            }
        }
        public InventoryContentsViewModel()
        {
            InventoryAreaCollectionView = Application.Current.Resources["InventoryAreaCollectionView"] as ListCollectionView;
        }
        
    }
}